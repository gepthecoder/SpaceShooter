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
        highScoreText.text = PlayerPrefs.GetInt("BEST_SCORE", 0).ToString();
    }
}
