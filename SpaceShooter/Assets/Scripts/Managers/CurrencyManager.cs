using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    #region Singleton
    private static CurrencyManager instance;
    public static CurrencyManager Instance
    {
        get
        {
            if (instance == null) { instance = FindObjectOfType(typeof(CurrencyManager)) as CurrencyManager; }
            return instance;
        }
        set { instance = value; }
    }
    #endregion

    [SerializeField]
    private Text textDiamonds;
    [SerializeField]
    private Text textCrystals;

    public int numOfDiamonds;
    public int numOfCrystals;


    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("DIAMONDS") || PlayerPrefs.HasKey("CRYSTALS"))
        {
            // WE HAD A PREVIOUS SESSION
            numOfCrystals = PlayerPrefs.GetInt("CRYSTALS", 0);
            numOfDiamonds = PlayerPrefs.GetInt("DIAMONDS", 0);
        }
        else
        {
            SAVE_PREFS();
        }
    }

    void Start()
    {
        updateText_Diamonds();
        updateText_Crystals();
    }

    public void SAVE_PREFS()
    {
        PlayerPrefs.SetInt("CRYSTALS", numOfCrystals);
        PlayerPrefs.SetInt("DIAMONDS", numOfDiamonds);
    }

    public void updateText_Diamonds()
    {
        textDiamonds.text = numOfDiamonds.ToString();
    }

    public void updateText_Crystals()
    {
        textCrystals.text = numOfCrystals.ToString();
    }
}
