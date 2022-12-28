using System;
using System.Linq;
using Entity.Enemy;
using Terrain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Management
{
    public enum TerrainDifficulty
    {
        Start,
        Easy,
        Medium,
        Hard,
        Insane,
        EasyBoss,
        MediumBoss,
        HardBoss
    }

    [Serializable]
    public struct SectionData
    {
        public GameObject terrainObject;
        public TerrainDifficulty difficulty;
    }

    public class TerrainSpawner : MonoBehaviour
    {
        public static TerrainSpawner Instance { get; private set; }

        [SerializeField] private SectionData[]
            sections; //Section prefabs contain child objects with SectionEndMarker script and SectionSpawnMarker script

        [SerializeField] private DifficultyData difficultyData;

        public int SectionsTraveled { get; set; }

        public GameObject CurrentSectionObject { get; private set; }
        public int CurrentSectionIndex { get; private set; }

        public int BossHeatRaw { get; set; }

        public float BossHeat { get; private set; }

        /// <summary>
        /// Multiplier * Sections travelled = Percentage chance for next section to include a boss.
        /// </summary>
        [SerializeField] private float bossHeatMultiplier;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of TerrainSpawner should not exist");
            }

            CreateTerrainRecursively(5);
        }

        private void Update()
        {
            if (SectionsTraveled + 3 > CurrentSectionIndex)
            {
                CreateTerrainRecursively(1);
            }

            BossHeat = bossHeatMultiplier * BossHeatRaw;
        }

        private void CreateTerrainRecursively(int amount = 1)
        {
            GameObject spawnedObject;
            // this needs to pick randomly based on parameters instead of iteratively.
            if (CurrentSectionObject == null)
            {
                spawnedObject = sections.First(section => section.difficulty == TerrainDifficulty.Start).terrainObject;
                CurrentSectionObject = Instantiate(spawnedObject, Vector3.zero, Quaternion.identity);
                amount -= 1;
            }

            for (var i = 0; i < amount; i++)
            {
                spawnedObject = DetermineSpawnedTerrain(out var difficulty);
                CurrentSectionObject = Instantiate(spawnedObject,
                    CurrentSectionObject.GetComponentInChildren<SectionEndMarker>().transform.position,
                    CurrentSectionObject.GetComponentInChildren<SectionEndMarker>().transform.rotation);
                SpawnTerrainEnemies(CurrentSectionObject.GetComponentsInChildren<SectionSpawnMarker>(), difficulty);
                CurrentSectionIndex++;
            }
        }

        private void SpawnTerrainEnemies(SectionSpawnMarker[] spawnMarkers, TerrainDifficulty difficulty)
        {
            foreach (var marker in spawnMarkers)
            {
                var enemy = EnemySpawner.Instance.Spawn(marker.transform.position, difficulty);
                enemy.GetComponent<IEnemyBase>().SectionObject = marker.transform.parent.gameObject;
            }
        }

        private GameObject DetermineSpawnedTerrain(out TerrainDifficulty difficulty)
        {
            var level = DifficultyManager.Instance.DifficultyLevel;
            if (BossCheck(BossHeat))
            {
                if (CheckLevelRangeInclusive(level, difficultyData.easyBossLevels))
                {
                    difficulty = TerrainDifficulty.EasyBoss;
                    return RandomlySelectTerrain(TerrainDifficulty.EasyBoss);
                }

                if (CheckLevelRangeInclusive(level, difficultyData.mediumBossLevels))
                {
                    difficulty = TerrainDifficulty.MediumBoss;
                    return RandomlySelectTerrain(TerrainDifficulty.MediumBoss);
                }

                if (CheckLevelRangeInclusive(level, difficultyData.hardBossLevels))
                {
                    difficulty = TerrainDifficulty.HardBoss;
                    return RandomlySelectTerrain(TerrainDifficulty.HardBoss);
                }

                throw new Exception("Level out of range.");
            }

            if (CheckLevelRangeInclusive(level, difficultyData.easyLevels))
            {
                difficulty = TerrainDifficulty.Easy;
                return RandomlySelectTerrain(TerrainDifficulty.Easy);
            }

            if (CheckLevelRangeInclusive(level, difficultyData.mediumLevels))
            {
                difficulty = TerrainDifficulty.Medium;
                return RandomlySelectTerrain(TerrainDifficulty.Medium);
            }

            if (CheckLevelRangeInclusive(level, difficultyData.hardLevels))
            {
                difficulty = TerrainDifficulty.Hard;
                return RandomlySelectTerrain(TerrainDifficulty.Hard);
            }

            if (CheckLevelRangeInclusive(level, difficultyData.insaneLevels))
            {
                difficulty = TerrainDifficulty.Insane;
                return RandomlySelectTerrain(TerrainDifficulty.Insane);
            }

            throw new Exception("Level out of range.");
        }

        private bool BossCheck(float bossHeat)
        {
            return Random.Range(0f, 100f) < bossHeat;
        }

        private bool CheckLevelRangeInclusive(int value, IntRange range)
        {
            return value >= range.min && value <= range.max;
        }

        private GameObject RandomlySelectTerrain(TerrainDifficulty difficulty)
        {
            var sectionDatas = sections.Where(section => section.difficulty == difficulty).ToList();
            return sectionDatas[Random.Range(0, sectionDatas.Count)].terrainObject;
        }
    }
}