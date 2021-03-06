using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public ParticleSystem prefabExplosion;
    public float health { get; private set; } // public for unity, private for other scripts 

    public float healthMax = 100;

    private void Start()
    {
        health = healthMax;
    }

    public void TakeDamage(float amt)
    {
        if (amt <= 0) return;


        health -= amt;

        if (health <= 0) Die();
    }

    public void Die()
    {
        // removes this gameobject from the game
        Destroy(gameObject);
        Instantiate(prefabExplosion, gameObject.transform.position, gameObject.transform.rotation);
        

    }
}
