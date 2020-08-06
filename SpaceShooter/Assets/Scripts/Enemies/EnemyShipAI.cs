using System;
using System.Collections;
using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    [Header("M O V E M E N T")]
    [Space(10)]
    [SerializeField] private float rotationalDamp = .5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] [Range(5, 300)] private float avoidenceRadius = 10f;
    [Header("A T T A C K")]
    [SerializeField]
    private Laser[] LASERS;
    [SerializeField]
    [Range(5, 1500)]
    private float maxDistance = 50f;
    [SerializeField]
    private float fireRate = 2f;
    [SerializeField]
    private GameObject Missile;
    [SerializeField]
    private Transform shootPosL;
    [SerializeField]
    private Transform shootPosR;
    [Header("H E A L T H")]
    [Range(10, 500)]
    [SerializeField]
    private int maxHealth = 50;
    [SerializeField]
    private GameObject explosionEffect;
    public int currentHealth;
    [Header("S C O R I N G")]
    [Range(10, 1000)]
    [SerializeField]
    private int killPoints = 200;
    [Range(1, 200)]
    [SerializeField]
    private int hitPoints = 50;
    [Range(1, 100)]
    [SerializeField]
    private float deathExplosionRadius = 4f;
    [Range(1, 100)]
    [SerializeField]
    private float deathExplosionForce = 30f;
    [Header("S O U N D  F X")]
    [SerializeField]
    private AudioSource deathExplosionSource;
    [SerializeField]
    private AudioClip deathExplosionSoundFX;

    private float fireTimer;
    private Vector3 hitPos;

    private Transform target;

    private PopUpController popUp; // damage effect
    private Rigidbody rb;
    
    public bool isDead;
    public GameObject healthBar;

    private Vector3 avoidenceOffsetVect = new Vector3(3,3,3);


    public event Action<float> OnHealthPctChanged = delegate { };

    private void Start()
    {
        target = GameHandler.Instance.PLAYER;

        popUp = GetComponent<PopUpController>();
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        DetectNearbyObj(avoidenceRadius);

        Turning();
        Moving();

        fireTimer += Time.deltaTime;
        bool bCanFire = fireTimer >= fireRate;

        if (isPlayerUpFront() && InLineOfSight() && bCanFire)
        {
            #region LASER
            //fire lasers
            //FIRE_LASER();
            #endregion

            #region PROJECTILES
            FIRE_PROJECTILES();
            fireTimer = 0;
            #endregion
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Ship")
        {
            rb.AddForceAtPosition(Vector3.up * 10f, transform.position);
        }
    }

    // MOVEMENT

    private void Turning()
    {
        Vector3 position = target.position - transform.position;
        Vector3 choosenPos = bTurnOffset ? position + avoidenceOffsetVect : position;
        Quaternion rotation = Quaternion.LookRotation(position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
    }

    private void Moving()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    // ATTACK

    private bool isPlayerUpFront()
    {
        Vector3 dirToTarget = transform.position - target.position;
        float angle = Vector3.Angle(transform.forward, dirToTarget);

        //if in range
        if(Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            //Debug.DrawLine(transform.position, target.position, Color.green);
            return true;
        }
        //Debug.DrawLine(transform.position, target.position, Color.yellow);
        return false;
    }

    private bool InLineOfSight()
    {
        RaycastHit hitInfo;
        Vector3 direction = target.position - transform.position;

        #region LASER
        //foreach (Laser laser in LASERS)
        //{
        //    //Debug.DrawRay(laser.transform.position, direction, Color.red);
        //    if(Physics.Raycast(laser.transform.position, direction, out hitInfo, laser.maxDistance))
        //    {
        //        if (hitInfo.transform.root.CompareTag("Player"))
        //        {
        //            Debug.DrawRay(laser.transform.position, direction, Color.green);
        //            hitPos = hitInfo.transform.position;
        //            return true;
        //        }
        //    }
        //}
        #endregion

        #region PROJECTILES
        if (Physics.Raycast(shootPosL.transform.position, direction, out hitInfo, maxDistance))
        {
            if (hitInfo.transform.root.CompareTag("Player"))
            {
                Debug.DrawRay(shootPosL.transform.position, direction, Color.green);
                hitPos = hitInfo.transform.position;
                return true;
            }
        }else if(Physics.Raycast(shootPosR.transform.position, direction, out hitInfo, maxDistance))
        {
            if (hitInfo.transform.root.CompareTag("Player"))
            {
                Debug.DrawRay(shootPosR.transform.position, direction, Color.green);
                hitPos = hitInfo.transform.position;
                return true;
            }
        }
        #endregion

        return false;
    }

    private void FIRE_LASER()
    {
        foreach(Laser laser in LASERS)
        {
            laser.FireSuperSonicLaser(hitPos);
        }
    }

    private void FIRE_PROJECTILES()
    {
        Instantiate(Missile, shootPosL.position, shootPosL.rotation);
        Instantiate(Missile, shootPosR.position, shootPosR.rotation);
    }

    // HEALTH

    public void DamageEnemy(int _damage)
    {
        bool _isDead = currentHealth <= 0 ? true : false;
        if (_isDead) { return; }

        popUp.PlayPopUpAnimation(_damage.ToString());
        ConfigureHealth(_damage);

        ScoreManager.Instance.UPDATE_SCORE(hitPoints);
        ScoreManager.Instance.PLAY_RANDOM_POP_UP(hitPoints.ToString());

        Debug.Log("Enemy damaged by: " + _damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ConfigureHealth(int byAmount)
    {
        currentHealth -= byAmount;
        float curentPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(curentPct);
    }

    private void Die()
    {
        isDead = true;
        ScoreManager.Instance.UPDATE_SCORE(killPoints);
        ScoreManager.Instance.PLAY_RANDOM_POP_UP(killPoints.ToString());
        KillManager.Instance.IncreaseDestructionRage(30);
        Destroy(healthBar);
        ExplosionEffect();
    }

    private void ExplosionEffect()
    {
        GameObject go = Instantiate(explosionEffect, transform.position, transform.rotation);
        deathExplosionSource.PlayOneShot(deathExplosionSoundFX);
        Destroy(go, 3f);
        #region Snippet
        Collider[] PARTS = Physics.OverlapSphere(transform.position, deathExplosionRadius);

        foreach(Collider part in PARTS)
        {
            Debug.Log(part.name);
            part.transform.parent = null;
            part.transform.tag = "cleanUp";

            Rigidbody rb = part.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(deathExplosionForce, transform.position, deathExplosionRadius);
            }
        }

        GameObject[] waste = GameObject.FindGameObjectsWithTag("cleanUp");
        foreach(GameObject part in waste) { Destroy(part, 4f); }
        #endregion
        StartCoroutine(REWARD());
    }

    IEnumerator REWARD()
    {
        yield return new WaitForSeconds(1.5f);
        CollectableManager.Instance.SPAWN_RANDOM_COLLECTABLE(transform);
        Destroy(gameObject);
    }

    private bool bTurnOffset;
    private void DetectNearbyObj(float radius)
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, radius);
        if(objects != null)
        {
            foreach (Collider obj in objects)
            {
                if (obj.transform.tag == "Ship")
                {
                    bTurnOffset = true;
                }
            }
        }
        else
        {
            bTurnOffset = false;
        }

    }


    #region Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidenceRadius);
    }
    #endregion
}
