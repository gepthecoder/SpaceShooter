using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Collectable : MonoBehaviour
{
    public string Name;
    // AUDIO CLIPS
    public AudioClip pickUpSound;
    //
    private AudioSource aSource;

    private float destroyTime;

    bool rewardGiven;

    void Awake()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag == "Player")
        {
            if (!rewardGiven)
            {
                PLAY(pickUpSound);
                COLLECT_REWARD(Name);
            }
            Destroy(gameObject, destroyTime);
        }
    }

    void PLAY(AudioClip clip)
    {
        destroyTime = clip.length;
        aSource.PlayOneShot(clip);
    }

    void COLLECT_REWARD(string rewardName)
    {
        switch (rewardName)
        {
            case "Diamond":
                CurrencyManager.Instance.numOfDiamonds++;
                CurrencyManager.Instance.SAVE_PREFS();

                CurrencyManager.Instance.updateText_Diamonds();
                break;
            case "Crystal":
                CurrencyManager.Instance.numOfCrystals++;
                CurrencyManager.Instance.SAVE_PREFS();

                CurrencyManager.Instance.updateText_Crystals();
                break;
            case "Fuel":
                KillManager.Instance.destructionCounter += 20;
                break;
            case "Heart":
                SpaceShipHealth.Instance.currentHealth += 20;
                break;
        }

        rewardGiven = true;
    }

}
