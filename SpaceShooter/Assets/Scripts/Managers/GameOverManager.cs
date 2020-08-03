using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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





}
