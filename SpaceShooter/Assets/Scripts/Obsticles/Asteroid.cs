using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    [Range(.3f, 25f)]
    private float minScale = .6f;

    [SerializeField]
    [Range(1f, 50f)]
    private float maxScale = 1.2f;

    [SerializeField]
    [Range(1f, 1000f)]
    private float rotationSpeed = 100f;

    private Vector3 asteroidScale;
    private Vector3 asteroidRotation;

    private void Start()
    {
        CreateAsteroid();
    }

    private void Update()
    {
        RotateAsteroid();
    }

    private void CreateAsteroid()
    {
        SetRandomLocalScale();
        SetRandomRotation();
    }

    private void SetRandomLocalScale()
    {//set random local scale of asteroid

        asteroidScale = Vector3.one; // (1,1,1)
        float scaleX = Random.Range(minScale, maxScale);
        float scaleY = Random.Range(minScale, maxScale);
        float scaleZ = Random.Range(minScale, maxScale);
        asteroidScale.x = scaleX;
        asteroidScale.y = scaleY;
        asteroidScale.z = scaleZ;
        transform.localScale = asteroidScale;
    }

    private void SetRandomRotation()
    {//set random rotation of asteroid

        float rotationX = Random.Range(-rotationSpeed, rotationSpeed);
        float rotationY = Random.Range(-rotationSpeed, rotationSpeed);
        float rotationZ = Random.Range(-rotationSpeed, rotationSpeed);

        asteroidRotation.x = rotationX;
        asteroidRotation.y = rotationY;
        asteroidRotation.z = rotationZ;
    }

    private void RotateAsteroid()
    {
        transform.Rotate(asteroidRotation * Time.deltaTime);
    }
}