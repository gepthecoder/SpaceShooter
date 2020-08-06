using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PopUpController))]
public class EnemyHealth : MonoBehaviour
{
    public event Action<float> OnHealthPctChanged = delegate { };

    [Range(10, 500)]
    [SerializeField] private int maxHealth = 100;
    [Range(10, 500)]
    [SerializeField]
    private int killPoints = 100;
    [Range(1, 100)]
    [SerializeField]
    private int hitPoints = 10;
    [SerializeField]
    private GameObject explosionEffect;

    public int currentHealth;
    public bool isDead;

    private Animator anime;
    #region Animations
    private void GetHit()   { anime.SetTrigger("GetHit");   }
    private void Death()    { anime.SetTrigger("Dead");     }
    #endregion

    private PopUpController popUp; // damage effect
    private Rigidbody rb;

    private void Start()
    {
        currentHealth = maxHealth;

        anime = GetComponent<Animator>();
        popUp = GetComponent<PopUpController>();
        rb = GetComponent<Rigidbody>();
    }

    public void DamageEnemy(int _damage)
    {
        bool _isDead = currentHealth <= 0 ? true : false;
        if (_isDead) { return; }

        popUp.PlayPopUpAnimation(_damage.ToString());
        ConfigureHealth(_damage);
        GetHit();

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
        Death();
        ScoreManager.Instance.UPDATE_SCORE(killPoints);
        ScoreManager.Instance.PLAY_RANDOM_POP_UP(killPoints.ToString());
        StartCoroutine(DeathEffect());
    }

    private IEnumerator DeathEffect()
    {
        rb.isKinematic = false;
        yield return new WaitForSeconds(2f);
        Instantiate(explosionEffect, transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(1f);
        KillManager.Instance.IncreaseDestructionRage(10);
        CollectableManager.Instance.SPAWN_RANDOM_COLLECTABLE(transform);
        Destroy(gameObject);
    }

}
