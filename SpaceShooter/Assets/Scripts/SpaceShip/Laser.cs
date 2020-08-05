using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] private float laserDuration = .5f;
    [SerializeField] private int laserDamage = 5;
    [Range(10, 10000)] [SerializeField] private float hitForce = 5f;
    [Range(1, 100)] [SerializeField] private float explosionRadius = 10f;


    public Light shootLight;
    [SerializeField] private float laserLightDuration = .2f;

    public GameObject hitEffect;

    [Range(100, 10000)]public float maxDistance = 100f;

    private LineRenderer laser;
    public bool canFireLaser = false;

    [SerializeField]
    private AudioClip laserSound;
    private AudioSource aSource;


    void Awake() { laser = GetComponent<LineRenderer>(); aSource = GetComponent<AudioSource>(); }

    void Start() { laser.enabled = false; shootLight.enabled = false; }

    Vector3 CastLaserRay()
    {
        RaycastHit hitInfo;
        Vector3 fwd = transform.TransformDirection(Vector3.forward) * maxDistance;
        Vector3 origin = transform.position;
        if (Physics.Raycast(origin, fwd, out hitInfo))
        {
            Debug.Log("Hit info: " + hitInfo.transform.tag);
            if (hitInfo.transform.tag == "Enemy")
            {
                if (hitInfo.transform.name.Contains("Ship"))
                {
                    EnemyShipAI health = hitInfo.transform.GetComponent<EnemyShipAI>();
                    if (health != null)
                        health.DamageEnemy(laserDamage);
                }
                else
                {
                    EnemyHealth health = hitInfo.transform.GetComponent<EnemyHealth>();
                    if (health != null)
                        health.DamageEnemy(laserDamage);
                }
            
                Rigidbody rb = hitInfo.transform.GetComponent<Rigidbody>();
                if(rb != null)
                    rb.AddExplosionForce(hitForce, hitInfo.point, explosionRadius, 4f, ForceMode.Impulse);
            }
            else if (hitInfo.transform.tag == "Asteroid")
            {
                Asteroid health = hitInfo.transform.name == "Collider" ? hitInfo.transform.parent.GetComponent<Asteroid>() : hitInfo.transform.GetComponent<Asteroid>();
                if(health != null)
                    health.DamageAsteroid(laserDamage);

                Rigidbody rb = hitInfo.transform.name == "Collider" ? hitInfo.transform.parent.GetComponent<Rigidbody>() : hitInfo.transform.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(hitForce + 20f, hitInfo.point, explosionRadius, 4f, ForceMode.Impulse);
            }
            else { Debug.Log("You have missed!"); }

            ExplosionHitEffectOnPosition(hitInfo.point);
            return hitInfo.point;
        }
        // we missed
        Debug.Log("WeMissed");
        Vector3 maxDistanceVect = transform.position + (transform.forward * maxDistance);
        return maxDistanceVect;
    }

    public void FireSuperSonicLaser()
    {
        Vector3 pos = CastLaserRay();
        FireSuperSonicLaser(pos);
    }

    // overload function
    public void FireSuperSonicLaser(Vector3 targetPos)
    {
        if (canFireLaser)
        {
            PLAY_LASER_SOUND();
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, targetPos);
            laser.enabled = true;
            shootLight.enabled = true;
            canFireLaser = false;
            Invoke("DisableLaser", laserDuration);
        }
    }

    private void DisableLaser()
    {
        laser.enabled = false;
        shootLight.enabled = false;
    }


    public void ExplosionHitEffectOnPosition(Vector3 hitPoint)
    {
        GameObject temp = Instantiate(hitEffect, hitPoint, Quaternion.identity, transform);
        Destroy(temp, 3f);
    }

    private void PLAY_LASER_SOUND()
    {
        aSource.PlayOneShot(laserSound);
    }
}
