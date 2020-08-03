using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject Missile;
    [SerializeField] private Transform shootPosL;
    [SerializeField] private Transform shootPosR;

    [SerializeField] private Laser[] LASERS;

    private float fireRate = .2f;
    private float fireTimer;

    private float laserFireRate = .1f;
    private float laserfireTimer;

    private void Update()
    {
        fireTimer += Time.deltaTime;
        bool bCanFire = fireTimer > fireRate;

        if (CrossPlatformInputManager.GetButtonDown("Fire") && bCanFire)
        {
            ShootProjectiles();
            fireTimer = 0;
        }

        laserfireTimer += Time.deltaTime;
        bool bCanFireLaser = laserfireTimer > laserFireRate;

        if (CrossPlatformInputManager.GetButtonDown("FireLaser") && bCanFireLaser)
        {
            foreach(Laser laser in LASERS)
            {
                laser.canFireLaser = true;
                laser.FireSuperSonicLaser();
            }
        }
    }
    
    private void ShootProjectiles()
    {
        Instantiate(Missile, shootPosL.position, shootPosL.rotation);
        Instantiate(Missile, shootPosR.position, shootPosR.rotation);
    }

}
