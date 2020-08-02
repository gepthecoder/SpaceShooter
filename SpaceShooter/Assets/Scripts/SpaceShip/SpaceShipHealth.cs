using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipHealth : MonoBehaviour
{
    [Header("H E A L T H")]
    [Space(5)]
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    [Header("G U I")]
    [Space(5)]
    [SerializeField] private Image health_slider;
    [SerializeField] private Text health_text;
    [SerializeField] private float fillSpeedInSec = .5f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthInfo();
    }

    public void DamageShuttle(int amount)
    {
        bool _isdead = currentHealth <= 0;

        if (_isdead) { return; }

        currentHealth -= amount;
        StartCoroutine(ChangeToPct((float)currentHealth / (float)maxHealth));
        UpdateHealthInfo();

        if(currentHealth <= 0)
        {
            // PLAYER IS DEAD -> DISPLAY GAME OVER EFFECT
            Dead();
        }
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = health_slider.fillAmount;
        float elapsed = 0f;

        while(elapsed < fillSpeedInSec)
        {
            elapsed += Time.deltaTime;
            health_slider.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / fillSpeedInSec);
            yield return null;
        }
        health_slider.fillAmount = pct;
    }

    private void UpdateHealthInfo() { health_text.text = currentHealth.ToString() + "/" + maxHealth.ToString(); }

    private void Dead()
    {
        isDead = true;
    }

}
