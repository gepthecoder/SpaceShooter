using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class SpaceShipMovement : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private float fSpeedOfShip = 1.3f;
    [SerializeField]
    [Range(1, 1000)]
    private float fHyperSpeedOfShip = 20f;
    [SerializeField]
    [Range(1, 100)]
    private float fTurnSpeedOfShip = 40f;
    [SerializeField]
    private ParticleSystem space_effect;
    [SerializeField]
    private Text spaceShipSpeedTxt;

    private float verticalAxisInput;
    private float horizontalAxisInput;
    private float pitchInput;
    private float rollInput;

    private bool bHyperJump;

    public bool bIsAccelerating;
    public bool bIsAcceleratingToSpeedOfLight;

    private SpaceShipHealth health;

    private void Start() { health = GetComponent<SpaceShipHealth>(); }

    private void Update()
    {
        //INPUTS
        IS_DEAD();
        // MOVEMENT
        verticalAxisInput = CrossPlatformInputManager.GetAxis("Vertical");
        horizontalAxisInput = CrossPlatformInputManager.GetAxis("Horizontal");
        // TURN
        pitchInput = CrossPlatformInputManager.GetAxis("Pitch");
        // ROLL
        rollInput = CrossPlatformInputManager.GetAxis("Roll");
        // HYPER JUMP
        bHyperJump = KillManager.Instance.bActivateHyperJump;

        HandleSpeedGUI();
    }

    private void FixedUpdate()
    {
        IS_DEAD();

        EngineThrust();
        RotateShip();
    }

    private void EngineThrust()
    {
        if (!bHyperJump)
        {
            bIsAcceleratingToSpeedOfLight = false;

            if (verticalAxisInput > 0.1f)
            {
                transform.position += transform.forward * fSpeedOfShip * verticalAxisInput;
                bIsAccelerating = true;
                if(verticalAxisInput > 0.3f) { space_effect.Play(); }
            }
            else { bIsAccelerating = false; space_effect.Stop(); }
        }
        else
        {
            // HYPER JUMP
            transform.position += transform.forward * fHyperSpeedOfShip;
            bIsAcceleratingToSpeedOfLight = true;
        }
       
    }

    private void RotateShip()
    {
        float fRotShipY = fTurnSpeedOfShip * horizontalAxisInput * Time.deltaTime;
        float fRotPitchX = fTurnSpeedOfShip * pitchInput * Time.deltaTime;
        float fRotRollZ = fTurnSpeedOfShip * rollInput * Time.deltaTime;

        transform.Rotate(-fRotPitchX, fRotShipY, fRotRollZ);
    }

    private void IS_DEAD()
    {
        if (health.isDead) { return; }
    }

    private void HandleSpeedGUI()
    {
        float multiplier = 10000f;
        float baseSpeed = bIsAcceleratingToSpeedOfLight ? fHyperSpeedOfShip : fSpeedOfShip;
        float inputMultiplier = bIsAcceleratingToSpeedOfLight ? 1f : verticalAxisInput;
        float temp = baseSpeed * multiplier * inputMultiplier;
        if(temp < 0) { temp = 0; }
        spaceShipSpeedTxt.text = temp.ToString() + " mph";
    }
}
