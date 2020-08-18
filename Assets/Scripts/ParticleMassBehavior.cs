using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleMassBehavior : MonoBehaviour
{
    //[SerializeField]
    public float mass = Mathf.Pow(10f,20f);
    public Vector3 velocity;
    public Vector3 initialVelocity = new Vector3(0.0f,0.0f,0.0f);
    
    // enum ChargeQuality{
    //     Neutral,
    //     Positive,
    //     Negative
    // }

    // [SerializeField]
    // ChargeQuality chargeQuality;
    [SerializeField]
    float charge;
    // float signFactor;

    GameObject[] particles;
    // Rigidbody rigidBody3D;
    PaceController environControl;
    float distanceMultiplier;
    float velocityMultiplier;
    
    void Awake()
    {
        environControl = GameObject.Find("EnvironmentControls").GetComponent<PaceController>();
        distanceMultiplier = environControl.SetDistanceScale(environControl.distanceScale);
        velocityMultiplier = environControl.SetTimeScale(environControl.timeScale);
        particles = GameObject.FindGameObjectsWithTag("Particle");
        // rigidBody3D = GetComponent<Rigidbody>();
        velocity = initialVelocity*velocityMultiplier;
        // rigidBody3D.AddForce(velocity,ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
        Vector3 totalAcceleration = Vector3.zero;
        float accMultiplier = Mathf.Pow(velocityMultiplier,2f);
        foreach (GameObject particle in particles){
            if (particle!= this.gameObject){
                Vector3 dist = FindDistance(particle);
                dist = dist*distanceMultiplier;
                ParticleMassBehavior otherParticle = particle.GetComponent<ParticleMassBehavior>();
                float otherMass = otherParticle.mass;
                float otherCharge = otherParticle.charge;
                Vector3 accelerationGravity = AccelerationGravity(dist,otherMass)*accMultiplier;
                Vector3 accelerationCharge = AccelerationElectric(dist,otherCharge)*accMultiplier;
                totalAcceleration += accelerationGravity;
                totalAcceleration += accelerationCharge;
            }
        
        velocity += totalAcceleration*Time.fixedDeltaTime;
        Vector3 deltaPos = 0.5f*velocity*Time.fixedDeltaTime;
        // rigidBody3D.AddForce(velocity,ForceMode.VelocityChange);
        //Debug.Log(deltaPos);
        transform.position += deltaPos;
        }
    }

    Vector3 FindDistance(GameObject other)
    {
        Vector3 dist =  transform.position - other.transform.position ;
        return dist;   
    }
    Vector3 AccelerationGravity(Vector3 distance, float otherMass)
    {
        float distMag = distance.magnitude;
        float G = 6.67f*Mathf.Pow(10f,-11f);
        Vector3 acceleration = -G*otherMass*distance/Mathf.Pow(distMag,3f);
        return acceleration;
    }

    // static float AssignChargeSign(ChargeQuality charge)
    // {
    //     switch(charge){
    //         case ChargeQuality.Negative:
    //             signFactor = -1f;
    //             return Mathf.Abs(charge);
    //         case ChargeQuality.Positive:
    //             signFactor = 1f;
    //             return;
    //         case ChargeQuality.Neutral:
    //             signFactor = 0f;
    //             return;
    //     }
    // }
    Vector3 AccelerationElectric(Vector3 distance, float otherCharge)
    {
        float distMag = distance.magnitude;
        float eps0 = 8.854f*Mathf.Pow(10f,-12f);
        float pi = Mathf.PI;
        //float signFactor = Mathf.Sign(charge);
        Vector3 acceleration = charge*otherCharge*distance/(mass*4*pi*eps0*Mathf.Pow(distMag,3f));
        return acceleration;
    }
}
