using System;
using UnityEngine;

namespace Management
{
    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager Instance { get; private set; }
        public int DifficultyLevel { get; private set; }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of DifficultyManager should not exist");
            }
        }

        private void Update()
        {
            DifficultyLevel = CalculateDifficulty(TerrainSpawner.Instance.SectionsTraveled);
        }

        private int CalculateDifficulty(int sectionsTravelled)
        {
            //DifficultyLevel is equal to sections travelled except it is adjusted by the following factors.
            //Recent damage taken, whether a boss battle has just happened.
            throw new NotImplementedException();
        }
    }
}
