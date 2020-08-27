using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour //This script is the second in the series of two spawners. Before this is the platform spawning.
{
    [SerializeField] GameObject ringShooter = null;
    [HideInInspector] public bool enemySpawnComplete = false;

    public void SpawnEnemies(BattleEnemySpawnInfo enemyInfo)
    {
        GameObject enemyToSpawn = GetProperEnemyType(enemyInfo);

        GameObject enemyA = Instantiate(enemyToSpawn, new Vector3(-10, 0, 20), Quaternion.Euler(0, 0, 0));
        enemyA.transform.parent = transform;
        GameObject enemyB = Instantiate(enemyToSpawn, new Vector3(0, 0, 20), Quaternion.Euler(0, 0, 0));
        enemyB.transform.parent = transform;
        GameObject enemyC = Instantiate(enemyToSpawn, new Vector3(10, 0, 20), Quaternion.Euler(0, 0, 0));
        enemyC.transform.parent = transform;

        enemySpawnComplete = true; //Battle Controller will now immediately set as false for the next battle
    }

    GameObject GetProperEnemyType(BattleEnemySpawnInfo enemyInfo)
    {
        switch (enemyInfo.typeOfEnemy)
        {
            case BattleEnemySpawnInfo.TypeOfEnemy.RingShooter:
                return ringShooter;
            default:
                Debug.LogError("The type of enemy you hit is not registered by the enemy spawner. Check the overworld enemy's SpawnInfo and the arena's EnemySpawner script.");
                return null;
        }
    }
}
