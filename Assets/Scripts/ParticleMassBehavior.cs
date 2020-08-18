using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleMassBehavior : MonoBehaviour
{
    //[SerializeField]
    public float mass = Mathf.Pow(10f,20f);
    public Vector3 velocity;
    public Vector3 initialVelocity = new Vector3(0.0f,0.0f,0.0f);

    [SerializeField]
    float charge;
    GameObject[] particles;
    PaceController environControl;
    float distanceMultiplier;
    float velocityMultiplier;
    
    void Awake()
    {
        environControl = GameObject.Find("EnvironmentControls").GetComponent<PaceController>();
        distanceMultiplier = environControl.SetDistanceScale(environControl.distanceScale);
        velocityMultiplier = environControl.SetTimeScale(environControl.timeScale);
        particles = GameObject.FindGameObjectsWithTag("Particle");
        velocity = initialVelocity*velocityMultiplier;
    }

    void FixedUpdate()
    {
        // Vector3 totalAcceleration = Vector3.zero;
        Vector3 totalVelocity = Vector3.zero;
        Vector3 collisionVelocity = Vector3.zero;
        float accMultiplier = Mathf.Pow(velocityMultiplier,2f);
        foreach (GameObject particle in particles){
            if (particle!= this.gameObject){
                Vector3 dist = FindDistance(particle);
                ParticleMassBehavior otherParticle = particle.GetComponent<ParticleMassBehavior>();
                float otherMass = otherParticle.mass;
                float otherCharge = otherParticle.charge;
                if (dist.magnitude < 0.1f){
                    Vector3 x1 = transform.position;
                    Vector3 x2 = particle.transform.position;
                    Vector3 v1 = this.velocity;
                    Vector3 v2 = otherParticle.velocity;
                    collisionVelocity = Collision(x1,x2,v1,v2, otherMass);
                }
                dist = dist*distanceMultiplier;
                Vector3 velocityGravity = AccelerationGravity(dist,otherMass)*accMultiplier*Time.fixedDeltaTime;
                Vector3 velocityCharge = AccelerationElectric(dist,otherCharge)*accMultiplier*Time.fixedDeltaTime;
                totalVelocity += velocityGravity;
                totalVelocity += velocityCharge;
                totalVelocity += collisionVelocity;
            }
        
        velocity += totalVelocity;
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

    Vector3 AccelerationElectric(Vector3 distance, float otherCharge)
    {
        float distMag = distance.magnitude;
        float eps0 = 8.854f*Mathf.Pow(10f,-12f);
        float pi = Mathf.PI;
        //float signFactor = Mathf.Sign(charge);
        Vector3 acceleration = charge*otherCharge*distance/(mass*4*pi*eps0*Mathf.Pow(distMag,3f));
        return acceleration;
    }

    Vector3 Collision(Vector3 x1, Vector3 x2, Vector3 v1, Vector3 v2, float otherMass)
    {
        Vector3 deltaX = x1 - x2;
        Vector3 deltaV = v1 - v2;
        float massFrac = 2*otherMass/(mass + otherMass);
        Vector3 newV = v1 - deltaX*massFrac*Vector3.Dot(deltaV,deltaX)/Mathf.Pow(deltaX.magnitude,2f);
        if (deltaX.magnitude == 0){
            newV = Vector3.zero;
        }
        return newV;
    }
}
