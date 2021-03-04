using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointAt : MonoBehaviour
{
    private EnemyTargeting enemyTargeting;

    public float visionDistance = 10;

    public bool lockRotationX;
    public bool lockRotationY;
    public bool lockRotationZ;

    public GameObject Player;

    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeeThing(Player.transform)) AimAtPlayer();
    }

    private void AimAtPlayer()
    {
        
        
            Vector3 disToTarget = Player.transform.position - transform.position;

            Quaternion targetRot = Quaternion.LookRotation(disToTarget, Vector3.up);

            Vector3 euler1 = transform.localEulerAngles;
            Quaternion prevRot = transform.rotation;
            transform.rotation = targetRot;
            Vector3 euler2 = transform.localEulerAngles;

            if (lockRotationX) euler2.x = euler1.x; 
            if (lockRotationY) euler2.y = euler1.y; 
            if (lockRotationZ) euler2.z = euler1.z;

            transform.rotation = prevRot;

            transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), .01f);

        
        

    }

    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false; // error

        Vector3 vToThing = thing.position - transform.position;

        //checks distance 
        if (vToThing.sqrMagnitude > visionDistance * visionDistance) return false; // it's too far away to see

        //checks direction
        

        // TODO check occlusion (something in the way

        return true;
    }
}
