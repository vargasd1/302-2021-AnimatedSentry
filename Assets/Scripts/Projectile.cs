using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float bullCD = 0;


    private void Update()
    {
        bullCD += Time.deltaTime;
        if (bullCD >= 5) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        

        if (player) // overlapping a player object
        {
            
            HealthSystem playerHealth =  player.GetComponent<HealthSystem>();
            if (playerHealth)
            {
                playerHealth.TakeDamage(10); // do damage to player
            }
            Destroy(gameObject); // remove projectile from the game 
        }

    }
}
