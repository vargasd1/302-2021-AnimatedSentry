using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    public Transform target;
    
    public float visDistance = 10;
    public float visionAngle = 45;
    public float bulletSpeed = 10;
    

    public GameObject Player;

   
    float cooldownShoot = 0;

    public float rPS = 1;

    public Transform tRotator;

    private Vector3 startPosTRotator;

    public ParticleSystem prefabMuzzleFlash;
    public GameObject prefabBullet;
    public Transform tBarrel;


    private void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
        
    }

    
    void Update()
    {
        if (cooldownShoot > 0) cooldownShoot -= Time.deltaTime;       

        DoAttack();

    }

    private void DoAttack()
    {
        if (cooldownShoot > 0) return;
        if (Player == null) return;
        if (!CanSeeThing(Player.transform)) return;

        if (tBarrel) Instantiate(prefabMuzzleFlash, tBarrel.position, tBarrel.rotation);
        var bullet = Instantiate(prefabBullet, tBarrel.position, tBarrel.rotation);
        bullet.GetComponent<Rigidbody>().velocity = (Player.transform.position - tBarrel.position).normalized * bulletSpeed;
        cooldownShoot = 1 / rPS;

        

        

    }

    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false;

        Vector3 vToThing = thing.position - transform.position;

        if (vToThing.sqrMagnitude > visDistance * visDistance) return false;
        

        return true;
    }
}
