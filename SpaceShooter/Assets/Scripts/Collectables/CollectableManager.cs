using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    #region Singleton
    private static CollectableManager instance;
    public static CollectableManager Instance
    {
        get
        {
            if (instance == null) { instance = FindObjectOfType(typeof(CollectableManager)) as CollectableManager; }
            return instance;
        }
        set { instance = value; }
    }
    #endregion

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject HEART;
    [SerializeField] private GameObject DIAMOND;
    [SerializeField] private GameObject CRYSTAL;
    [SerializeField] private GameObject FUEL;

    // SPAWN OBJECTS
    public void SPAWN_FUEL(Transform spawnPos) { Instantiate(FUEL, spawnPos.position, Quaternion.identity); }

    public void SPAWN_CRYSTAL(Transform spawnPos) { Instantiate(CRYSTAL, spawnPos.position, Quaternion.identity); }

    public void SPAWN_DIAMOND(Transform spawnPos) { Instantiate(DIAMOND, spawnPos.position, Quaternion.identity); }

    public void SPAWN_HEART(Transform spawnPos) { Instantiate(HEART, spawnPos.position, Quaternion.identity); }

    public void SPAWN_RANDOM_COLLECTABLE(Transform spawnPos)
    {
        int random = Random.Range(0, 200);

        if (random >= 0 && random < 50) { SPAWN_FUEL(spawnPos); }
        else if (random >= 50 && random < 100) { SPAWN_DIAMOND(spawnPos); }
        else if (random >= 100 && random < 150) { SPAWN_HEART(spawnPos); }
        else if (random >= 150 && random < 200) { SPAWN_CRYSTAL(spawnPos); }
    }
}
