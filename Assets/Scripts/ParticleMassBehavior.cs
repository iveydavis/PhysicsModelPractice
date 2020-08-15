using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleMassBehavior : MonoBehaviour
{
    //[SerializeField]
    public float mass = Mathf.Pow(10f,20f);
    Vector3 velocity;
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
    Rigidbody rigidBody3D;
    PaceController environControl;
    float distanceMultiplier;
    
    void Awake()
    {
        environControl = GameObject.Find("EnvironmentControls").GetComponent<PaceController>();
        distanceMultiplier = environControl.SetDistanceScale(environControl.distanceScale);
        particles = GameObject.FindGameObjectsWithTag("Particle");
        rigidBody3D = GetComponent<Rigidbody>();
        rigidBody3D.velocity = initialVelocity;
    }

    void FixedUpdate()
    {
        Vector3 totalAcceleration = Vector3.zero;
        foreach (GameObject particle in particles){
            if (particle!= this.gameObject){
                Vector3 dist = FindDistance(particle);
                dist = dist*distanceMultiplier;
                ParticleMassBehavior otherParticle = particle.GetComponent<ParticleMassBehavior>();
                float otherMass = otherParticle.mass;
                float otherCharge = otherParticle.charge;
                Vector3 accelerationGravity = AccelerationGravity(dist,otherMass);
                Vector3 accelerationCharge = AccelerationElectric(dist,otherCharge);
                totalAcceleration += accelerationGravity;
                totalAcceleration += accelerationCharge;
            }
        
        velocity = totalAcceleration*Time.deltaTime;
        rigidBody3D.AddForce(velocity,ForceMode.VelocityChange);
        }
    }

    Vector3 FindDistance(GameObject other)
    {
        Vector3 dist =  other.transform.position - transform.position;
        return dist;   
    }
    Vector3 AccelerationGravity(Vector3 distance, float otherMass)
    {
        float distMag = distance.magnitude;
        float G = 6.67f*Mathf.Pow(10f,-11f);
        Vector3 acceleration = G*otherMass*distance/Mathf.Pow(distMag,3f);
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
        float signFactor = Mathf.Sign(charge);
        Vector3 acceleration = -signFactor*otherCharge*distance/(4*pi*eps0*Mathf.Pow(distMag,3f));
        return acceleration;
    }
}
