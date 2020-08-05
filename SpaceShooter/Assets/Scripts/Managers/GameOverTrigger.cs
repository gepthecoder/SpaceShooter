using UnityEngine;
using UnityEngine.UI;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] private Animator gameOverGuiAnime;
    [SerializeField] private Text highScoreText;

    public void TRIGGER_GAME_OVER()
    {
        SetHighScoreText();
        gameOverGuiAnime.SetTrigger("gameOver");
    }

    private void SetHighScoreText()
    {
        if(ScoreManager.Instance.SCORE > ScoreManager.Instance.BEST_SCORE)
        {
            Debug.Log("New Best Score!! :)");
            ScoreManager.Instance.BEST_SCORE = ScoreManager.Instance.SCORE;
            ScoreManager.Instance.SAVE_SCORE();
        }
        highScoreText.text = PlayerPrefs.GetInt("BEST_SCORE", 0).ToString();
    }
}
