using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThrower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        // Note: You have to implement the following method with prediction:
    // Either directly solved (e.g. Law of Cosines or similar) or iterative.
    // You cannot modify the method signature. However, if you want to do more advanced
    // prediction (such as analysis of the navmesh) then you can make another method that calls
    // this one. 
    // Be sure to run the editor mode unit test to confirm that this method runs without
    // any gamemode-only logic
    public static bool PredictThrow(
        // The initial launch position of the projectile
        Vector3 projectilePos,
        // The initial ballistic speed of the projectile
        float maxProjectileSpeed,
        // The gravity vector affecting the projectile (likely passed as Physics.gravity)
        Vector3 projectileGravity,
        // The initial position of the target
        Vector3 targetInitPos,
        // The constant velocity of the target (zero acceleration assumed)
        Vector3 targetConstVel,
        // The forward facing direction of the target. Possibly of use if the target
        // velocity is zero
        Vector3 targetForwardDir,
        // For algorithms that approximate the solution, this sets a limit for how far
        // the target and projectile can be from each other at the interceptT time
        // and still count as a successful prediction
        float maxAllowedErrorDist,
        // Output param: The solved projectileDir for ballistic trajectory that intercepts target
        out Vector3 projectileDir,
        // Output param: The speed the projectile is launched at in projectileDir such that
        // there is a collision with target. projectileSpeed must be <= maxProjectileSpeed
        out float projectileSpeed,
        // Output param: The time at which the projectile and target collide
        out float interceptT,
        // Output param: An alternate time at which the projectile and target collide
        // Note that this is optional to use and does NOT coincide with the solved projectileDir
        // and projectileSpeed. It is possibly useful to pass on to an incremental solver.
        // It only exists to simplify compatibility with the ShootingRange
        out float altT)
    {
        // TODO implement an accurate throw with prediction. This is just a placeholder

        // FYI, if Minion.transform.position is sent via param targetPos,
        // be aware that this is the midpoint of Minion's capsuleCollider
        // (Might not be true of other agents in Unity though. Just keep in mind for future game dev)

        projectileDir = Vector3.zero;
        projectileSpeed = -1f;
        interceptT = -1f;
        altT = -1f;

        Vector3 delta;
        /*
        if (Vector3.Magnitude(targetConstVel) == 0) {
            // If target is standing still throw slighty in front incase it moves
            delta = (targetInitPos + (targetForwardDir * maxAllowedErrorDist)) - projectilePos;
        } else {
            
        }*/

        delta = targetInitPos - projectilePos;

        // Calculate the vector from the target to start pos
        //Vector3 delta = ( targetInitPos +( targetConstVel * (Vector3.Distance(targetInitPos,projectilePos)/maxProjectileSpeed)) )- projectilePos;
        

        float a = projectileGravity.sqrMagnitude;
        float b = -4f * (Vector3.Dot(projectileGravity, delta) + maxProjectileSpeed * maxProjectileSpeed);
        float c = 4f * delta.sqrMagnitude;

        // check for no real solutions (ie shot is impossible)
        float b2minus4ac = (b*b) - (4*a*c);
        if (b2minus4ac < 0f){return false;}

        // Find the candidate times
        float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b2minus4ac)) / (2*a));
        float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b2minus4ac)) / (2*a));

        // Find time to target
        if (time0 < 0f) // If time0 is not valid check time1
        {
            if (time1 < 0f) // Next check if the solved time1 is valid
            {// No valid times
                return false;
            } else {
                interceptT = time1;
                altT = -1f;
            }
        } else {
            if (time1 < 0f) 
            {
                interceptT = time0;
                altT = -1f;
            } else {
                interceptT = Mathf.Min(time0,time1);
                altT = Mathf.Max(time0,time1);
            }
        }

        // You'll probably want to leave this as is. For advanced prediction you can slow your throw down
        // You don't need to predict the speed of your throw. Only the direction assuming full speed
        projectileSpeed = maxProjectileSpeed;

        Vector3 fireVec = ((delta * 2) - (projectileGravity * (interceptT * interceptT))) / (2 * projectileSpeed * interceptT);
        // Make sure this is normalized! (The direction of your throw)
        projectileDir = Vector3.Normalize(fireVec);

        

        // TODO return true or false based on whether target can actually be hit
        // This implementation just thinks, "I guess so?", and returns true
        // Implementations that don't exactly solve intercepts will need to test the approximate
        // solution with maxAllowedErrorDist. If your solution does solve exactly, you will
        // probably want to add a debug assertion to check your solution against it.
        return true;

    }
    

    public static bool MyMinionThrow(
        Vector3 projectilePos,
        float maxProjectileSpeed,
        Vector3 projectileGravity,
        Vector3 targetInitPos,
        Vector3 targetConstVel,
        Vector3 targetForwardDir,
        float maxAllowedErrorDist,
        out Vector3 projectileDir,
        out float projectileSpeed,
        out float interceptT,
        out float altT)
    {
        projectileDir = Vector3.zero;
        projectileSpeed = -1f;
        interceptT = -1f;
        altT = -1f;

        
        // First see if we can make a throw to target at all as a static target
        var canThrow = PredictThrow(projectilePos, maxProjectileSpeed, projectileGravity,
                targetInitPos, targetConstVel, targetForwardDir, maxAllowedErrorDist,
                out var testDir, out var testSpeed, out var testInterceptT, out var testAltT);
       
        // If impossible return false
        if (!canThrow){return false;}

        //Vector3 oldTarget = targetInitPos;
        Vector3 oldTarget = targetInitPos + (targetConstVel * testInterceptT);


        int iter = 0;
        float distance = Mathf.Infinity;
        float oldT = testInterceptT;
        while (Mathf.Abs(distance) >= maxAllowedErrorDist && iter < 6)
        {
            iter++;
            canThrow = PredictThrow(projectilePos, maxProjectileSpeed, projectileGravity,
                oldTarget, targetConstVel, targetForwardDir, maxAllowedErrorDist,
                out testDir, out testSpeed, out testInterceptT, out testAltT);

            Vector3 newTarget = targetInitPos + (targetConstVel * testInterceptT);

            distance = Vector3.Distance(oldTarget, newTarget);
            
            if (distance < maxAllowedErrorDist) 
            {
                Vector3 landingLocation = projectilePos + (testDir * testSpeed * testInterceptT) + ((projectileGravity * (testInterceptT*testInterceptT))/2);
                if (Vector3.Distance(landingLocation, newTarget) < maxAllowedErrorDist)
                {
                    projectileDir = testDir;
                    projectileSpeed = testSpeed;
                    interceptT = testInterceptT;
                    altT = testAltT;
                    return true;
                } 
            } 
            oldTarget = newTarget;
        }

        Vector3 landingLocation2 = projectilePos + (testDir * testSpeed* testInterceptT) + ((projectileGravity * (testInterceptT*testInterceptT))/2);
        if (Vector3.Distance(landingLocation2, oldTarget) < maxAllowedErrorDist)
        {
            projectileDir = testDir;
            projectileSpeed = testSpeed;
            interceptT = testInterceptT;
            altT = testAltT;
            return true;
        } 
        return false;
    }
}
