using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    #region Singleton
    private static GameOverManager instance;
    public static GameOverManager Instance
    {
        get
        {
            if (instance == null) { instance = FindObjectOfType(typeof(GameOverManager)) as GameOverManager; }
            return instance;
        }
        set { instance = value; }
    }

    void Awake() { instance = this; }
    #endregion

    [SerializeField] private Animator darkenScreenAnime;

    public void GAME_OVER()
    {
        darkenScreenAnime.SetTrigger("darken");
    }

    public void PLAY_AGAIN()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void MAIN_MENU()
    {
        SceneManager.LoadScene("MainMenu");
    }





}
