using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ENEMIES;
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
        Instantiate(RANDOM_ENEMY(), offset, Quaternion.identity, transform); //clones are under parent
    }

    GameObject RANDOM_ENEMY()
    {
        int r = Random.Range(0, ENEMIES.Length);
        return ENEMIES[r];
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
