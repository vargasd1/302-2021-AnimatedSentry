using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public Transform target;
    public bool wantsToTarget = false;
    public bool wantsToAttack = false;
    public float visionDistance = 10;
    public float visionAngle = 45;

    private List<TargatableThing> potentialTargets = new List<TargatableThing>();

    float cooldownScan = 0;
    float cooldownPick = 0;
    float cooldownShoot = 0;

    public float roundsPerSecond = 10;

    // references to the player's arm "bones"
    public Transform armL;
    public Transform armR;

    private Vector3 startPosArmL;
    private Vector3 startPosArmR;

    /// <summary>
    /// a reference to the particle system prefab to spawn when the gun shoots 
    /// </summary>

    public ParticleSystem prefabMuzzleFlash;
    public Transform handR;
    public Transform handL;

    CameraOrbit camOrbit;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        startPosArmL = armL.localPosition;
        startPosArmR = armR.localPosition;

        camOrbit = Camera.main.GetComponentInParent<CameraOrbit>();

    }

    // Update is called once per frame
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");
        wantsToAttack = Input.GetButton("Fire1");

        if (!wantsToTarget) target = null;

        cooldownScan -= Time.deltaTime; // counting down
        if (cooldownScan <= 0 || (target == null && wantsToTarget)) ScanForTargets(); // do this when countdown is finished 

        cooldownPick -= Time.deltaTime;
        if (cooldownPick <= 0) PickATarget();

        if(cooldownShoot > 0 ) cooldownShoot -= Time.deltaTime; 

        // if we have target and we can't see it, set target = null
        if (target && !CanSeeThing(target)) target = null;

        SlideArmsHome();

        DoAttack();

    }
    private void SlideArmsHome()
    {
        armL.localPosition = AnimMath.Slide(armL.localPosition, startPosArmL, .01f);
        armR.localPosition = AnimMath.Slide(armR.localPosition, startPosArmR, .01f);
    }


    private void DoAttack()
    {
        if (cooldownShoot > 0) return; // too soon
        if (!wantsToTarget) return; // player not targeting 
        if (!wantsToAttack) return; // player not shooting 
        if (target == null) return; // no target
        if (!CanSeeThing(target)) return; // target can't be seen


        HealthSystem targetHealth = target.GetComponent<HealthSystem>();

        if (targetHealth)
        {
            targetHealth.TakeDamage(20);
        }

        cooldownShoot = 1 / roundsPerSecond;
        // attack!

        camOrbit.Shake(.5f);

        if(handL) Instantiate(prefabMuzzleFlash, handL.position, handL.rotation);
        if(handR) Instantiate(prefabMuzzleFlash, handR.position, handR.rotation);

        // trigger arm animation

        // rotates arms up
        armL.localEulerAngles += new Vector3 (-20, 0, 0);
        armR.localEulerAngles += new Vector3 (-20, 0, 0);

        // moves arms backwards (kickback)
        armL.position += -armL.forward * .1f;
        armR.position += -armR.forward * .1f;
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
