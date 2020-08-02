using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PopUpController))]
public class EnemyHealth : MonoBehaviour
{
    public event Action<float> OnHealthPctChanged = delegate { };

    [Range(10, 500)]
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    public bool isDead;

    private Animator anime;
    #region Animations
    private void GetHit()   { anime.SetTrigger("GetHit");   }
    private void Death()    { anime.SetTrigger("Dead");     }
    #endregion

    private PopUpController popUp; // damage effect

    private void Start()
    {
        currentHealth = maxHealth;

        anime = GetComponent<Animator>();
        popUp = GetComponent<PopUpController>();
    }

    public void DamageEnemy(int _damage)
    {
        bool _isDead = currentHealth <= 0 ? true : false;
        if (_isDead) { return; }

        popUp.PlayPopUpAnimation(_damage.ToString());
        ConfigureHealth(_damage);
        GetHit();
        //TO:DO -> score for player
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
        //TO:DO -> extra score for player
        StartCoroutine(DeathEffect());
    }

    private IEnumerator DeathEffect()
    {
        yield return new WaitForSeconds(2f);
        //TO:DO -> instantiate smoke effect
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        //TO:DO -> kills++
    }

}
