using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SpaceShipMovement : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private float fSpeedOfShip = 20f;

    [SerializeField]
    [Range(1, 50)]
    private float fTurnSpeedOfShip = 40f;

    private float verticalAxisInput;
    private float horizontalAxisInput;
    private float pitchInput;
    private float rollInput;

    public bool bIsAccelerating;
    
    private void Update()
    {
        //INPUTS

        // MOVEMENT
        verticalAxisInput = CrossPlatformInputManager.GetAxis("Vertical");
        horizontalAxisInput = CrossPlatformInputManager.GetAxis("Horizontal");
        // TURN
        pitchInput = CrossPlatformInputManager.GetAxis("Pitch");
        // ROLL
        rollInput = CrossPlatformInputManager.GetAxis("Roll");
    }

    private void FixedUpdate()
    {
        EngineThrust();
        RotateShip();
    }


    private void EngineThrust()
    {
        if(verticalAxisInput > 0.1f)
        {
            transform.position += transform.forward * fSpeedOfShip * verticalAxisInput;
            bIsAccelerating = true;
        }
        else { bIsAccelerating = false; }
    }

    private void RotateShip()
    {
        float fRotShipY = fTurnSpeedOfShip * horizontalAxisInput * Time.deltaTime;
        float fRotPitchX = fTurnSpeedOfShip * pitchInput * Time.deltaTime;
        float fRotRollZ = fTurnSpeedOfShip * rollInput * Time.deltaTime;

        transform.Rotate(-fRotPitchX, fRotShipY, fRotRollZ);
    }
}
