using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
