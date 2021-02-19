using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public Transform target;
    public bool wantsToTarget = false;
    public float visionDistance = 10;
    public float visionAngle = 45;

    private List<TargatableThing> potentialTargets = new List<TargatableThing>();

    float cooldownScan = 0;
    float cooldownPick = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  
    }

    // Update is called once per frame
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");

        if (!wantsToTarget) target = null;

        cooldownScan -= Time.deltaTime; // counting down
        if (cooldownScan <= 0 || (target == null && wantsToTarget)) ScanForTargets(); // do this when countdown is finished 

        cooldownPick -= Time.deltaTime;
        if (cooldownPick <= 0) PickATarget();

        // if we have target and we can't see it, set target = null
        if (target && !CanSeeThing(target)) target = null;
    }

    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false; // error

        Vector3 vToThing = thing.position - transform.position;

        //checks distance 
        if (vToThing.sqrMagnitude > visionDistance * visionDistance) return false; // it's too far away to see

        //checks direction
        if (Vector3.Angle(transform.forward, vToThing) > visionAngle) return false; // out of the vision cone

        // TODO check occlusion (something in the way

        return true;
    }

    private void ScanForTargets()
    {
        cooldownScan = 1; // do the next scan in 1 second

        // empty the list

        potentialTargets.Clear(); 

        // refil the list:

        TargatableThing[] things = GameObject.FindObjectsOfType<TargatableThing>();
        foreach (TargatableThing thing in things)
        {
            

            // if we can see it 
            // add targets to potential targest 

            if (CanSeeThing(thing.transform)) { 
                    potentialTargets.Add(thing);
                
            }

            
        }

    }

    void PickATarget()
    {
        cooldownPick = .25f;

        //if (target) return; // we already have a target 
        target = null;

        float closestDistanceSoFar = 0;

        // finds closest targetable thing and sets it as our target :
        foreach(TargatableThing pt in potentialTargets)
        {
            float dd = (pt.transform.position - transform.position).sqrMagnitude;

            if(dd < closestDistanceSoFar || target == null)
            {
                target = pt.transform;
                closestDistanceSoFar = dd;
            }

            
        }
    }
}
