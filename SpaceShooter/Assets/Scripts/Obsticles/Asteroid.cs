using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    [Range(10f, 100000f)]
    private float health = 100f;

    [SerializeField]
    [Range(.3f, 25f)]
    private float minScale = .6f;

    [SerializeField]
    [Range(1f, 50f)]
    private float maxScale = 1.2f;

    private Vector3 asteroidScale;
    private Vector3 asteroidRotation;

    private Rigidbody rb;

    [SerializeField]
    [Range(1f, 1000f)]
    private float rotationSpeed = 100f;
    [SerializeField]
    [Range(1f, 1000f)]
    private float accelerationForce = 150f;

    public GameObject destroyEffect;
    [Header("S F X")]
    [Space(10)]
    [SerializeField] private AudioClip destroySoundEffect;
    [Space(5)]
    [SerializeField] private AudioClip hitSoundEffect;
    private AudioSource asteroidSoundSource;

    private int ImpactDamageForce = 10;

    [SerializeField]
    [Range(1f, 1000f)]
    private int hitPoints = 50;
    [SerializeField]
    [Range(1f, 1000f)]
    private int destroyPoints = 200;

    private bool damageOnce;

    void Awake() { asteroidSoundSource = GetComponent<AudioSource>(); rb = GetComponent<Rigidbody>(); }

    private void Start()
    {
        CreateAsteroid();
    }

    private void Update()
    {
        RotateAsteroid();
        rb.AddForce(transform.forward * accelerationForce);        
    }

    void OnTriggerEnter(Collider other)
    {
        SpaceShipHealth shipHealth = other.gameObject.GetComponent<SpaceShipHealth>();
        if (shipHealth != null && !damageOnce)
        {
            Explode();
            shipHealth.DamageShuttle(ImpactDamageForce);
            damageOnce = false;
        }
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

    public void DamageAsteroid(int _damage)
    {
        bool _isStable = health <= 0 ? true : false;
        if (_isStable) { return; }

        health -= _damage;
        ScoreManager.Instance.UPDATE_SCORE(hitPoints);
        ScoreManager.Instance.PLAY_RANDOM_POP_UP(hitPoints.ToString());

        Debug.Log("Asteroid damaged by: " + _damage);
        if (health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(explosion, 1.5f);
        PLAY_SOUND(true);
        GetComponent<MeshRenderer>().enabled = false;
        ScoreManager.Instance.UPDATE_SCORE(destroyPoints);
        ScoreManager.Instance.PLAY_RANDOM_POP_UP(destroyPoints.ToString());

        KillManager.Instance.IncreaseDestructionRage(10);
        Destroy(gameObject, 3f);
    }

    public void PLAY_SOUND(bool destroy)
    {
        AudioClip clip = destroy ? destroySoundEffect : hitSoundEffect;
        asteroidSoundSource.PlayOneShot(clip);
    }

    private Vector3 GenerateRandomVector(float min, float max)
    {
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);
        float z = Random.Range(min, max);

        return new Vector3(x, y, z);
    }
}