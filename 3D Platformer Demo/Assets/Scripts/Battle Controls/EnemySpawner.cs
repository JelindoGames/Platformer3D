using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour //This script is the second in the series of two spawners. Before this is the platform spawning.
{
    [SerializeField] GameObject ringShooter = null;
    [HideInInspector] public bool enemySpawnComplete = false;

    public void SpawnEnemies(BattleInfo battleInfo) //Called by PlatformSpawner
    {
        for (int i = 0; i < battleInfo.opponents.Length; i++)
        {
            GameObject enemyType = GetEnemyType(battleInfo.opponents[i].type);
            GameObject enemy = Instantiate(enemyType, battleInfo.opponents[i].position, Quaternion.Euler(0, 0, 0));
            enemy.transform.parent = transform;
        }

        enemySpawnComplete = true; //Battle Controller will now immediately set as false for the next battle
    }

    GameObject GetEnemyType(Opponent.Type type)
    {
        switch (type)
        {
            case Opponent.Type.RingShooter:
                return ringShooter;
            default:
                Debug.LogError("The type of enemy you hit is not registered by the enemy spawner. Check the overworld enemy's SpawnInfo and the arena's EnemySpawner script.");
                return null;
        }
    }
}
