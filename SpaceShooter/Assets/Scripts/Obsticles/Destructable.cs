using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyEffect;
    [SerializeField] [Range(1, 1000000)]
    private int maxHealth = 1000;
    public int health;

    [SerializeField]
    private int damage = 1000;


    void Start()
    {
        health = maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            SpaceShipHealth spaceShip = other.transform.GetComponent<SpaceShipHealth>();
            spaceShip.DamageShuttle(damage);
        }
    }

    public void DamagePlanet(int _damage)
    {
        bool _isStable = health <= 0 ? true : false;
        if (_isStable) { return; }

        health -= _damage;

        if (health <= 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(explosion, 3f);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(.0000000001f, .0000000001f, .0000000001f), 4f);
        Destroy(gameObject, 4f);
    }
}
