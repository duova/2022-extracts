using System;
using UnityEngine;

namespace Management
{
    public enum PooledObject
    {
        TurtleBlast
    }

    [Serializable]
    public struct PooledObjectData
    {
        public PooledObject enumId;
        public GameObject gameObject;
    }
    
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        [SerializeField]
        private PooledObjectData[] objectDatas;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of ObjectPool should not exist");
            }
        }

        //Use unity pooling.
        public GameObject Create(PooledObject enumId)
        {
            throw new NotImplementedException();
        }

        public GameObject Remove(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}