using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class KillManager : MonoBehaviour
{
    /*
     Once you reach a certain amount of destruction rage you can perform hyper jump
         */

    #region Singleton
    private static KillManager instance;
    public static KillManager Instance
    {
        get
        {
            if (instance == null) { instance = FindObjectOfType(typeof(KillManager)) as KillManager; }
            return instance;
        }
        set { instance = value; }
    }

    #endregion

    [SerializeField] private Animator[] popUpAnimators;
    [SerializeField] private GameObject hyperJumpButtonObj;
    [SerializeField] private Image destructionFillImage;
    [SerializeField] private float fillSpeedInSec = .5f;
    public int destructionCounter;

    //BOOSTER
    [SerializeField] private Text numOfBoostersText;
    public int numOfBoosters;


    private int maxRage = 100;
    private bool bCanPerformHyperJump;
    public bool bActivateHyperJump;

    private Button hyperButton;
    private Animator fillImageAnime;


    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("BOOSTERS"))
        {
            // WE HAD A PREVIOUS SESSION
            numOfBoosters = PlayerPrefs.GetInt("BOOSTERS", 0);
        }
        else
        {
            SAVE_PREFS();
        }
    }


    void Start()
    {
        destructionFillImage.fillAmount = 1f;
        destructionCounter = 100;
        bCanPerformHyperJump = true;

        hyperButton = hyperJumpButtonObj.GetComponent<Button>();
        fillImageAnime = destructionFillImage.GetComponent<Animator>();

        hyperJumpButtonObj.SetActive(false);
        update_NumOfBoostersText();
    }

    void Update()
    {
        if (bCanPerformHyperJump)
        {
            hyperButton.interactable = true;
            hyperJumpButtonObj.SetActive(true);
            fillImageAnime.SetBool("effect", true);
            //activate slider effect
            if (CrossPlatformInputManager.GetButtonDown("HyperJump"))
            {
                //SlowTime();
                bCanPerformHyperJump = false;
                hyperButton.interactable = false;
                //perform hyper jump
                bActivateHyperJump = true;
                //activate warp time effect
                GameHandler.Instance.WARP_SPACE.Play();
                StartCoroutine(HYPER_EFFECT_GUI());
            }
        }
    }

    private void SlowTime() { Time.timeScale = .5f; }
    private void DefaultTime() { Time.timeScale = 1f; }

    public void SAVE_PREFS()
    {
        PlayerPrefs.SetInt("BOOSTERS", numOfBoosters);

    }
    
    public void update_NumOfBoostersText()
    {
        numOfBoostersText.text = numOfBoosters.ToString();
    }

    public void USE_BOOSTER()
    {
        if(numOfBoosters > 0)
        {
            numOfBoosters--;
            update_NumOfBoostersText();
            SAVE_PREFS();

            destructionCounter += 20;
        }
    }

    private IEnumerator HYPER_EFFECT_GUI()
    {
        while(destructionCounter > 0)
        {
            destructionCounter -= 1;
            destructionFillImage.fillAmount = (float)destructionCounter / (float)maxRage;
            yield return new WaitForSeconds(.05f);
        }

        if(destructionCounter <= 0)
        {
            bActivateHyperJump = false;
            hyperJumpButtonObj.SetActive(false);
            fillImageAnime.SetBool("effect", false);
            //DefaultTime();
            GameHandler.Instance.WARP_SPACE.Stop();
        }
    }

    public void IncreaseDestructionRage(int amount)
    {
        bool _filled = destructionFillImage.fillAmount >= 1;
        if (_filled) { return; }

        destructionCounter += amount;
        Debug.Log("Destruction timer: " + destructionCounter);
        StartCoroutine(ChangeToPct((float)destructionCounter / (float)maxRage));
        PLAY_RANDOM_POPUP(amount);
        if (destructionCounter >= maxRage)
        {
            bCanPerformHyperJump = true;
        }
    }


    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = destructionFillImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < fillSpeedInSec)
        {
            elapsed += Time.deltaTime;
            destructionFillImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / fillSpeedInSec);
            yield return null;
        }
        destructionFillImage.fillAmount = pct;
    }

    private void PLAY_RANDOM_POPUP(int amount)
    {
        /*
         Good=0, Great, Nice, Wow

         */

        bool better = amount > 10;
        int randomTemp = Random.Range(0, 100);
        if (better)
        {
            if(randomTemp <= 50)
            {
                popUpAnimators[1].SetTrigger("great");
            }
            else
            {
                popUpAnimators[3].SetTrigger("wow");
            }
        }
        else
        {
            if (randomTemp <= 50)
            {
                popUpAnimators[0].SetTrigger("good");
            }
            else
            {
                popUpAnimators[2].SetTrigger("nice");
            }
        }
    }

}
