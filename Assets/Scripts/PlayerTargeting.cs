using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{

    public Transform target;
    public bool wantsToTarget = false;
    public float visionDistance = 10;

    private List<TargatableThing> potentialTargets = new List<TargatableThing>();

    float cooldownScan = 0;
    float cooldownPick = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");

        cooldownScan -= Time.deltaTime; // counting down
        if (cooldownScan <= 0) ScanForTargets(); // do this when countdown is finished 

        cooldownPick -= Time.deltaTime;
        if (cooldownPick <= 0) PickATarget();
    }

    private void ScanForTargets()
    {
        cooldownScan = 1; // do the next scan in 2 seconds 

        // empty the list

        potentialTargets.Clear(); 

        // refil the list:

        TargatableThing[] things = GameObject.FindObjectsOfType<TargatableThing>();
        foreach (TargatableThing thing in things)
        {
            // check how far away thing is 

            Vector3 disToThing = thing.transform.position - transform.position;

            if(disToThing.sqrMagnitude < visionDistance * visionDistance)
            {
                if(Vector3.Angle(transform.forward, disToThing) < 45) {
                    potentialTargets.Add(thing);
                }
            }

            // check what direction it is in
        }

    }

    void PickATarget()
    {
        cooldownPick = .25f;

        if (target) return; // we already have a target 

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
