using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipHealth : MonoBehaviour
{
    #region Singleton
    private static SpaceShipHealth instance;
    public static SpaceShipHealth Instance
    {
        get
        {
            if (instance == null) { instance = FindObjectOfType(typeof(SpaceShipHealth)) as SpaceShipHealth; }
            return instance;
        }
        set { instance = value; }
    }
    #endregion

    [Header("H E A L T H")]
    [Space(5)]
    [SerializeField] private int maxHealth = 200;
    public int currentHealth;
    public bool isDead = false;
    private bool _damaged;
    [Header("G U I")]
    [Space(5)]
    [SerializeField] private Image health_slider;
    [SerializeField] private Text health_text;
    [SerializeField] private float fillSpeedInSec = .5f;
    [Header("E F F E C T")]
    [Space(5)]
    [SerializeField]
    private Image damageImage;
    [SerializeField]
    private float flashSpeed = .5f;
    [SerializeField]
    private Color flashColor = new Color(1f, 0f, 0f, .5f);
    [SerializeField]
    private GameObject damageExplosion;
    [SerializeField]
    private AudioSource explosionSource;
    [SerializeField]
    private AudioClip explosionClip;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthInfo();
    }

    private void Update()
    {
        if (_damaged) { damageImage.color = flashColor; }
        else { damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime); }
        _damaged = false;
    }

    public void DamageShuttle(int amount)
    {
        bool _isdead = currentHealth <= 0;

        if (_isdead) { return; }

        currentHealth -= amount;
        StartCoroutine(ChangeToPct((float)currentHealth / (float)maxHealth));
        UpdateHealthInfo();
        AddExplosionEffect();
        //Vibrate.VibrateDevice(200);

        if (currentHealth <= 0)
        {
            AddExplosionEffect();
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
        GameOverManager.Instance.GAME_OVER();
    }

    private void AddExplosionEffect()
    {
        GameObject exPlosion = Instantiate(damageExplosion, transform.position, Quaternion.identity, transform);
        Destroy(exPlosion, 1.5f);
        explosionSource.PlayOneShot(explosionClip);
    }

}
