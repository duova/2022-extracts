using System;
using System.Linq;
using Entity.Enemy;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Management
{
    public enum EnemyDifficulty
    {
        Static,
        Easy,
        Medium,
        Hard,
        Insane
    }

    public enum EnemyLocation
    {
        Top,
        Side
    }

    [Serializable]
    public struct EnemyData
    {
        public GameObject enemyPrefab;
        public EnemyDifficulty[] enemyDifficulty;
        public EnemyLocation enemyLocation;
    }

    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [SerializeField]
        private EnemyData[] enemies;

        [SerializeField]
        private GameObject[] easyBosses, mediumBosses, hardBosses;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of EnemySpawner should not exist");
            }
        }

        public GameObject Spawn(Vector3 position, TerrainDifficulty difficulty)
        {
            //Spawn a enemy at the location based on the current difficulty and terrain difficulty and returns the enemy.
            var spawnableEnemyList = enemies.Where(enemy => enemy.enemyDifficulty.Contains((EnemyDifficulty)(int)difficulty)).ToArray();
            var spawnedEnemy = spawnableEnemyList[Random.Range(0, spawnableEnemyList.Length)];
            var enemyObject = Instantiate(spawnedEnemy.enemyPrefab, position, Quaternion.identity);
            ApplyDifficultyModifications(enemyObject.GetComponent<IEnemyBase>());
            return enemyObject;
        }

        /// <summary>
        /// Calculate multipliers for enemy stats based on damage.
        /// </summary>
        /// <param name="enemy"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ApplyDifficultyModifications(IEnemyBase enemy)
        {
            throw new NotImplementedException();
        }
    }
}