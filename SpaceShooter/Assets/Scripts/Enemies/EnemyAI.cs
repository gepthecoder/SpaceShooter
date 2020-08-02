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

    private Transform       player_target;
    private SpaceShipHealth player_health;
    private Animator        anime;
    private EnemyHealth enemyHealth;

    private float attackCoolDown;

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
