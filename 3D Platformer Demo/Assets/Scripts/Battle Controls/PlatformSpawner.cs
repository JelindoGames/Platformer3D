using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour //This script is the second in the series of two spawners. After this is the enemy spawning.
{
    [SerializeField] EnemySpawner enemySpawner = null;

    public void SpawnPlatforms(BattleEnemySpawnInfo enemyInfo)
    {
        enemySpawner.SpawnEnemies(enemyInfo);
    }
}
