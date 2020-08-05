using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    #region Singleton
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null) { instance = FindObjectOfType(typeof(ScoreManager)) as ScoreManager; }
            return instance;
        }
        set { instance = value; }
    }
    #endregion

    public enum SCORE_EFFECTS { Yellow, Purple, Blue, Orange }

    [SerializeField] private Text scoreText;

    public Animator[] scorePopUps;
    public int SCORE = 0;
    public int BEST_SCORE;

    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("BEST_SCORE"))
        {
            BEST_SCORE = PlayerPrefs.GetInt("BEST_SCORE", 0);
        }
        else
        {
            SAVE_SCORE();
        }

        SCORE = 0;
    }

    void Start()
    {
        scoreText.text = "SCORE: " + SCORE;
    }

    public void SAVE_SCORE()
    {
        PlayerPrefs.SetInt("SCORE", BEST_SCORE);
    }

    public void UPDATE_SCORE(int byAmount)
    {
        StartCoroutine(scoreUpdater(byAmount));
    }

    private IEnumerator scoreUpdater(int byAmount)
    {
        int desiredScore = SCORE + byAmount;

        while(SCORE != desiredScore)
        {
            if(SCORE < desiredScore)
            {
                SCORE += 5;
                scoreText.text = "SCORE: " + SCORE;
            }
            yield return new WaitForSeconds(.01f);
        }
    }

    public void PLAY_RANDOM_POP_UP(string amount)
    {
        /*
            Yellow = 0, Purple, Blue, Orange
        */
        int randomPopUp = Random.Range(0, scorePopUps.Length);
        Text getPointsText = scorePopUps[randomPopUp].GetComponent<Text>();
        if (getPointsText != null) { getPointsText.text = amount; }

        switch (randomPopUp)
        {
            case (int)SCORE_EFFECTS.Yellow:
                scorePopUps[randomPopUp].SetTrigger("Y");
                break;
            case (int)SCORE_EFFECTS.Purple:
                scorePopUps[randomPopUp].SetTrigger("P");
                break;
            case (int)SCORE_EFFECTS.Blue:
                scorePopUps[randomPopUp].SetTrigger("B");
                break;
            case (int)SCORE_EFFECTS.Orange:
                scorePopUps[randomPopUp].SetTrigger("O");
                break;
            default:
                scorePopUps[0].SetTrigger("Y");
                break;
        }   
    }

    private void OnDisable()
    {
        SAVE_SCORE();
    }
}
