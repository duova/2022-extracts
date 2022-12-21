using System;
using System.IO;
using Game.Scripts.Server.Structs;
using UnityEngine;
using UnityEditor;

namespace Game.Scripts.Tooling.Editor
{
    [Serializable]
    public class SlotableInfoEditor : EditorWindow
    {
        private SerializedObject _serializedSlotableList;
        private string _selectedPropertyPath;
        private SerializedProperty _selectedProperty;
        private Vector2 _slotableScrollPosition;
        private Vector2 _typeScrollPosition;
        private SlotableInfoList _editableList;

        public static SlotableInfoEditor Open(SlotableInfoList list)
        {
            var window = GetWindow<SlotableInfoEditor>("Slotable Editor");
            list.SyncEnums();
            window._serializedSlotableList = new SerializedObject(list);
            window._editableList = list;
            return window;
        }

        private void OnGUI()
        {

            #region JSON features
            
            var jsonPathProperty = _serializedSlotableList.FindProperty("jsonPath");
            EditorGUILayout.PropertyField(jsonPathProperty, false);
            
            if (GUILayout.Button("Extract from Json"))
            {
                ConvertFromJson(_editableList.jsonPath, ref _editableList);
            }
            if (GUILayout.Button("Write to Json"))
            {
                ConvertToJson(_editableList.jsonPath, _editableList);
            }
            
            #endregion

            DrawEditor(_serializedSlotableList);
            
            if (!GUI.changed) return;
            _serializedSlotableList.ApplyModifiedProperties();
            _serializedSlotableList.Update();
        }
        
        protected static void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            EditorGUILayout.PropertyField(prop, drawChildren);
        }

        private void DrawSidebar(SerializedProperty property)
        {
            foreach (SerializedProperty p in property)
            {
                if (GUILayout.Button(p.displayName))
                {
                    _selectedPropertyPath = p.propertyPath;
                }

                if (!string.IsNullOrEmpty(_selectedPropertyPath))
                {
                    _selectedProperty = _serializedSlotableList.FindProperty(_selectedPropertyPath);
                }
            }
        }

        public static void ConvertToJson(string path, SlotableInfoList list)
        {
            File.WriteAllText( path, JsonUtility.ToJson(list, true));
        }

        public static void ConvertFromJson(string path, ref SlotableInfoList list)
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), list);
        }

        private void DrawEditor(SerializedObject source)
        {
            var abilityProperty = source.FindProperty("slotableAbilityInfos");
            var skillProperty = source.FindProperty("slotableSkillInfos");
            var itemProperty = source.FindProperty("slotableItemInfos");
            var effectProperty = source.FindProperty("slotableEffectInfos");
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            _slotableScrollPosition = EditorGUILayout.BeginScrollView(_slotableScrollPosition);
            GUILayout.Label ("Abilities", EditorStyles.boldLabel);
            DrawSidebar(abilityProperty);

            GUILayout.Label ("Skills", EditorStyles.boldLabel);
            DrawSidebar(skillProperty);

            GUILayout.Label ("Items", EditorStyles.boldLabel);
            DrawSidebar(itemProperty);

            GUILayout.Label ("Effects", EditorStyles.boldLabel);
            DrawSidebar(effectProperty);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            _typeScrollPosition = EditorGUILayout.BeginScrollView(_typeScrollPosition);
            if (_selectedProperty != null) DrawProperties(_selectedProperty, true);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
    }
}
