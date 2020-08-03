using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] private Animator gameOverGuiAnime;

    public void TRIGGER_GAME_OVER()
    {
        gameOverGuiAnime.SetTrigger("gameOver");
    }
}
