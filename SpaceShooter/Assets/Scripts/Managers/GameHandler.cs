using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    #region Singleton
    private static GameHandler instance;
    public static GameHandler Instance
    {
        get
        {
            if(instance == null) { instance = FindObjectOfType(typeof(GameHandler)) as GameHandler; }
            return instance;
        }
        set { instance = value; }
    }

    void Awake() { instance = this; }
    #endregion


    public Transform PLAYER;
    public SpaceShipHealth SHIP_HEALTH;

    public ParticleSystem WARP_SPACE;


    private bool pauseMenuOpened;
    public Animator darkenScreenAnime;
    public Animator pauseMenuGuiAnime;

    void Start()
    {
        pauseMenuOpened = false;
    }

    void Update()
    {
        if (pauseMenuOpened)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    // PAUSE MANAGER
    public void OPEN_PAUSE_MENU()
    {
        if (!pauseMenuOpened)
        {
            //open
            darkenScreenAnime.SetBool("darkenScreen", true);
            pauseMenuGuiAnime.SetBool("showPauseMenu", true);
            pauseMenuOpened = true;
        }
    }

    public void RESUME()
    {
        pauseMenuOpened = false;
        darkenScreenAnime.SetBool("darkenScreen", false);
        pauseMenuGuiAnime.SetBool("showPauseMenu", false);
    }

    public void OPEN_MAIN_MENU_FROM_PAUSE()
    {
        RESUME();
        SceneManager.LoadScene("MainMenu");
    }

}
