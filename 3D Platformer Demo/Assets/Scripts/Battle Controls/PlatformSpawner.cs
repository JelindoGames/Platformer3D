using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour //This script is the second in the series of two spawners. After this is the enemy spawning.
{
    [SerializeField] EnemySpawner enemySpawner = null;
    [SerializeField] GameObject platformPrefab;

    public void SpawnPlatforms(BattleInfo battleInfo) //Called by BattleController
    {
        ClearExistingPlatforms();

            for (int i = 0; i < battleInfo.platforms.Length; i++)
            {
                GameObject platform = Instantiate(platformPrefab, battleInfo.platforms[i].position, Quaternion.Euler(0, 0, 0));
                platform.transform.localScale = battleInfo.platforms[i].scale;
                platform.transform.parent = transform;
                MovingPlatform platformScript = platform.GetComponent<MovingPlatform>();
                platformScript.period = battleInfo.platforms[i].period;
                platformScript.positionChangeFromCenter = battleInfo.platforms[i].posChangeFromCenter;
            }

        enemySpawner.SpawnEnemies(battleInfo);
    }

    void ClearExistingPlatforms()
    {
        foreach (Transform platform in transform)
        {
            Destroy(platform.gameObject);
        }
    }
}
