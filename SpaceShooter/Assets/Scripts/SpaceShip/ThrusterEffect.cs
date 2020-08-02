using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterEffect : MonoBehaviour
{
    [Header("T H R U S T E R")]
    [Space(10)]
    [SerializeField] private ParticleSystem lThruster; //left small thruster effect
    [Space(5)]
    [SerializeField] private ParticleSystem rThruster; //right small thruster effect
    [Space(5)]
    [SerializeField] private ParticleSystem mThruster; //main thruster effect
    [Space(10)]
    [Header("S O U N D  E F F E C T S")]
    [SerializeField] private AudioSource asThrusterSource_M; //main thruster sound source
    [SerializeField] private AudioSource asThrusterSource_R; //R thruster sound source
    [SerializeField] private AudioSource asThrusterSource_L; //L thruster sound source

    private SpaceShipMovement shipControls;

    void Start() { shipControls = GetComponent<SpaceShipMovement>(); }

    private void Update()
    {
        ManageThrusters();
    }

    public void ManageThrusters()
    {
        if (shipControls.bIsAccelerating)
        {
            // increase start lifetime
            // L submodule
            ParticleSystem.MainModule lPs = lThruster.main;
            lPs.startLifetime = Mathf.Lerp(.6f, 1.5f, 1f);
            // increase voulume
            asThrusterSource_L.volume = Mathf.Lerp(.15f, .65f, 2f);
            // M submodule
            ParticleSystem.MainModule mPs = mThruster.main;
            mPs.startLifetime = Mathf.Lerp(.6f, 2.1f, 1f);
            // increase voulume
            asThrusterSource_M.volume = Mathf.Lerp(.15f, .8f, 2f);
            // R submodule
            ParticleSystem.MainModule rPs = rThruster.main;
            rPs.startLifetime = Mathf.Lerp(.6f, 1.51f, 1f);
            // increase voulume
            asThrusterSource_R.volume = Mathf.Lerp(.15f, .65f, 2f);
        }
        else
        {
            // decrease start lifetime
            // L submodule
            ParticleSystem.MainModule lPs = lThruster.main;
            lPs.startLifetime = Mathf.Lerp(1.5f, .6f, 1f);
            // decrease voulume
            asThrusterSource_L.volume = Mathf.Lerp(.65f, .15f, 2f);
            // M submodule
            ParticleSystem.MainModule mPs = mThruster.main;
            mPs.startLifetime = Mathf.Lerp(2.1f, .6f, 1);
            // decrease voulume
            asThrusterSource_M.volume = Mathf.Lerp(.8f, .15f, 2f);
            // R submodule
            ParticleSystem.MainModule rPs = rThruster.main;
            rPs.startLifetime = Mathf.Lerp(1.5f, .6f, 1f);
            // decrease voulume
            asThrusterSource_R.volume = Mathf.Lerp(.65f, .15f, 2f);
        }
    }
}
