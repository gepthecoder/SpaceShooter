using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ALIEN_ENEMIES;
    [SerializeField] private GameObject[] SPACE_SHIP_ENEMIES;

    [SerializeField] private Transform[] CLUSTER_POINTS;

    [SerializeField] [Range(1, 50)]
    private int numOfEnemies = 3; //X^^3 enemies
    [SerializeField] [Range(1, 10000)]
    private int spacingBetweenEnemies = 5;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int X = 0; X < numOfEnemies; X++)
        {
            for (int Y = 0; Y < numOfEnemies; Y++)
            {
                for (int Z = 0; Z < numOfEnemies; Z++)
                {
                    InstantiateEnemy(X, Y, Z);
                }
            }
        }
    }

    private void InstantiateEnemy(int _x, int _y, int _z)
    {
        Vector3 pos = RANDOM_SPAWN_POS();
        Vector3 offset = new Vector3(pos.x + (_x * spacingBetweenEnemies) + EnemyOffset(),
                                        pos.y + (_y * spacingBetweenEnemies) + EnemyOffset(),
                                            pos.z + (_z * spacingBetweenEnemies) + EnemyOffset());


        GameObject enemy_to_spawn = RandomNumberGen(100) > 50 ? RANDOM_ENEMY() : RANDOM_SPACE_SHIP_ENEMY();
        Instantiate(enemy_to_spawn, offset, Quaternion.identity, transform); //clones are under parent
    }

    int RandomNumberGen(int max)
    {
        return Random.Range(0, max);
    }

    GameObject RANDOM_ENEMY()
    {
        int r = Random.Range(0, ALIEN_ENEMIES.Length);
        return ALIEN_ENEMIES[r];
    }

    GameObject RANDOM_SPACE_SHIP_ENEMY()
    {
        int r = Random.Range(0, SPACE_SHIP_ENEMIES.Length);
        return SPACE_SHIP_ENEMIES[r];
    }

    Vector3 RANDOM_SPAWN_POS()
    {
        int r = Random.Range(0, CLUSTER_POINTS.Length);
        return CLUSTER_POINTS[r].position;
    }

    float EnemyOffset()
    {
        float r = Random.Range(-spacingBetweenEnemies / 2f, spacingBetweenEnemies / 2f);
        return r;
    }

}
