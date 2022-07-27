using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ProjectileThrower : MonoBehaviour
{

    int ProjectilePoolSize = 40;

    List<Projectile> ProjectilePool = new List<Projectile>();

    public Projectile ProjectilePrefab;

    public GameObject Target;

    public GameObject cannonFrame;

    public GameObject cannonBarrel;

    public AudioSource cannonSound;

    public Rigidbody targetRbody;

    public float StartTime = 0f;

    public float CoolOffTime = 0.50f;

    float LastShotTime = 0f;

    public Vector3 LaunchPos = new Vector3(0f, 1f, 0f);

    public float ShotSpeed = 20f;
    float OrigShotSpeed = 20f;

    //public GameObject LaunchSpotIndicator;

    public Vector2 LaunchHeightRange = new Vector2(1f, 15f);


    public float MaxAbsLauncherHeightChangeVel = 3f;
    public float LaunchHeightRandomAccel = 0.5f;

    float launcherVel = 0f;

    private ParticleSystem cannonFire;

    public float DB_launchAngle = 0f;

    float maxAllowedErrorDist = (0.25f + 0.5f) * 0.99f; //projectile radius + target radius 

    private void Awake()
    {
        if(ProjectilePrefab == null)
        {
            Debug.LogError("No projectile prefab");
        }

        if(Target == null)
        {
            Debug.LogError("No target");
        }

        /*if(LaunchSpotIndicator == null)
        {
            Debug.LogError("launch spot indicator is null");
        }*/

        for (int i = 0; i < ProjectilePoolSize; ++i)
        {

            var prj = Instantiate<Projectile>(ProjectilePrefab);
            prj.gameObject.SetActive(false);
            prj.mgr = this;

            ProjectilePool.Add(prj);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (Target.GetComponent<Rigidbody>() != null)
        {
            targetRbody = Target.GetComponent<Rigidbody>();
        } else {
            Debug.Log("No rigid body");
        }
        OrigShotSpeed = ShotSpeed;

        if (cannonBarrel.GetComponent<ParticleSystem>() != null)
        {
            cannonFire = cannonBarrel.GetComponent<ParticleSystem>();
        } else {
            Debug.Log("No cannon fire");
        }
    }


    public void Recycle(Projectile p)
    {
        ProjectilePool.Add(p);
    }


    private void Update()
    {
        LaunchPos = cannonBarrel.transform.position;
        cannonFrame.transform.LookAt(Target.transform);
    }


    void FixedUpdate()
    {

        // The launcher randomly moves up and down for variety of shot angles
        var h = LaunchPos.y;

        var r = Random.Range(-1f, 1f) * Random.Range(-1f, 1f);

        r *= Time.deltaTime * LaunchHeightRandomAccel;

        //launcherVel = Mathf.Clamp(launcherVel + r, -MaxAbsLauncherHeightChangeVel, MaxAbsLauncherHeightChangeVel);

        //h += launcherVel;

        h = Mathf.Clamp(h, LaunchHeightRange.x, LaunchHeightRange.y);

        if (h == LaunchHeightRange.x || h == LaunchHeightRange.y)
            launcherVel = 0f;

        LaunchPos.y = h;

        //LaunchSpotIndicator.transform.position = LaunchPos;

        if (ProjectilePool.Count > 0 && (Time.timeSinceLevelLoad > (LastShotTime + CoolOffTime)))
        {
            
            // Calc the shot trajectory, if possible
            if(MyMinionThrow(LaunchPos, ShotSpeed, Physics.gravity,
                Target.transform.position, targetRbody.velocity, Target.transform.forward,
                maxAllowedErrorDist,
                out var projectileDir, out var projectileSpeed, out var t, out var altT))
      
            {


                // Don't fire if the projectile is going to bounce off the imaginary wall before shot
                // can get there
                var intercept = Target.transform.position + targetRbody.velocity * t;

               
                // FIRE!

                var xzDir = (new Vector3(projectileDir.x, 0f, projectileDir.z)).normalized;
                DB_launchAngle = Vector3.Angle(xzDir, projectileDir);
                // The following uses object recycling
                Projectile p = ProjectilePool[0];

                ProjectilePool.RemoveAt(0);

                p.gameObject.SetActive(true);

                p.gameObject.transform.position = LaunchPos;
                p.gameObject.transform.rotation = Quaternion.identity;

                p.rbody.ResetInertiaTensor();
                p.rbody.velocity = Vector3.zero;
                p.rbody.angularVelocity = Vector3.zero;

                var throwVec = projectileDir * projectileSpeed;

                p.rbody.AddForce(throwVec, ForceMode.VelocityChange);
                cannonBarrel.SetActive(true);
                LastShotTime = Time.timeSinceLevelLoad;
                //cannonBarrel.SetActive(false);
                Invoke("StopShoot", 3f);
            
            }

        }

    }

    void StopShoot()
    {
        cannonBarrel.SetActive(false);
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
