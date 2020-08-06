using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyAI : MonoBehaviour
{
    [Space(5)]
    [Header("M O V E M E N T")]
    [SerializeField] [Range(5, 50)]     private float movementSpeed = 10f;
    [SerializeField] [Range(0, 100)]    private float rotationSpeed = .5f;
    [Header("A T T A C K")]
    [Space(5)]
    [SerializeField] private float attackSpeed = 2f;
    [Space(5)]
    [Range(1, 10)]
    [SerializeField] private float stoppingDistance = 4f;
    [Space(5)]
    [Range(1, 50)]
    [SerializeField] private int damage = 8;
    [Space(5)]
    [Range(5,10000)]
    [SerializeField] private float lookRadius = 100f;
    [Space(5)]
    [Range(5, 10000)]
    [SerializeField] private float chaseRadius = 50f;
    [Space(5)]
    [SerializeField]
    private GameObject explosiveDetonationEffect;


    private Transform       player_target;
    private SpaceShipHealth player_health;
    private Animator        anime;
    private EnemyHealth enemyHealth;

    private float attackCoolDown;
    private float detonationTime = 2f;

    private void Start()
    {
        player_target = GameHandler.Instance.PLAYER;
        player_health = GameHandler.Instance.SHIP_HEALTH;

        anime = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }


    private void Update()
    {
        if(player_target != null && !enemyHealth.isDead)
        {
            if (player_target.GetComponent<SpaceShipHealth>().isDead) { return; }

            float playerDistance = Vector3.Distance(player_target.position, transform.position);
            attackCoolDown -= Time.deltaTime;

            if (playerDistance <= lookRadius)
            {
                LOOK_FOR_PLAYER();

                if(playerDistance <= chaseRadius)
                {
                    CHASE_PLAYER();
                    anime.SetTrigger("Chase");

                    if (playerDistance <= stoppingDistance)
                    {
                        if (attackCoolDown <= 0)
                        {
                            if (player_health != null)
                            {
                                anime.SetTrigger("Attack");
                                StartCoroutine(DetonationTriggered());
                            }
                            attackCoolDown = attackSpeed;
                        }
                    }
                }
            }
            else
            {
                anime.SetTrigger("Idle");
            }
        }
    }

    private void LOOK_FOR_PLAYER()
    {
        Vector3 posToLook = player_target.position - transform.position;
        Quaternion enemyRotation = Quaternion.LookRotation(posToLook);
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, enemyRotation, step);
    }

    private void CHASE_PLAYER()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    // Call in animation event => "Attack"
    public void DAMAGE_PLAYER()
    {
        player_health.DamageShuttle(damage);
    }

    private IEnumerator DetonationTriggered()
    {
        float t = 0f;
        while (t <= detonationTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if(t >= detonationTime) { Detonate(30); }
    }

    private void Detonate(int additionalDamage)
    {
        GameObject temp = Instantiate(explosiveDetonationEffect, transform.position, transform.rotation);
        Destroy(temp, 1.5f);
        StartCoroutine(ShrinkToSingularity());
    }

    private IEnumerator ShrinkToSingularity()
    {
        float t = 0f;

        while (t <= 2f)
        {
            t += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x - .1f, transform.localScale.y - .1f, transform.localScale.z - .1f);
            yield return null;
        }

        if (t >= 2f) { Destroy(gameObject); }
    }
    #region Gizmos
    void OnDrawGizmosSelected()
    {
        // lookAt
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        // chase
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
    #endregion
}
