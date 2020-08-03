using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlacer : MonoBehaviour
{
    [SerializeField] private GameObject[] ASTEROIDS;

    [SerializeField] [Range(1, 50)] private int numOfAsteroids = 10; //X^^3 asteroids
    [SerializeField] [Range(1, 10000)] private int spacingBetweenAsteroids = 5;


    private void Start()
    {
        SpawnAsteroids();
    }

    private void SpawnAsteroids()
    {
        for (int X = 0; X < numOfAsteroids; X++)
        {
            for (int Y = 0; Y < numOfAsteroids; Y++)
            {
                for (int Z = 0; Z < numOfAsteroids; Z++)
                {
                    InstantiateAsteroid(X,Y,Z);
                }
            }
        }
    }

    private void InstantiateAsteroid(int _x, int _y, int _z)
    {
        Vector3 pos = transform.position;
        Vector3 offset = new Vector3(pos.x + (_x * spacingBetweenAsteroids) + AsteroidOffset(),
                                        pos.y + (_y * spacingBetweenAsteroids) + AsteroidOffset(),
                                            pos.z + (_z * spacingBetweenAsteroids) + AsteroidOffset());
        Instantiate(RANDOM_ASTEROID(), offset, Quaternion.identity, transform); //clones are under parent
    }

    GameObject RANDOM_ASTEROID()
    {
        int r = Random.Range(0, ASTEROIDS.Length);
        return ASTEROIDS[r];
    }

    float AsteroidOffset()
    {
        float r = Random.Range(-spacingBetweenAsteroids / 2f, spacingBetweenAsteroids / 2f);
        return r;
    }


}
