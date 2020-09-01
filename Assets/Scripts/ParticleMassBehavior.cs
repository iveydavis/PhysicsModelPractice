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
    
    public Vector3 magneticMoment = Vector3.zero;
    PaceController environControl;
    float distanceMultiplier;
    float velocityMultiplier;
    
    void Awake()
    {
        environControl = GameObject.Find("EnvironmentControls").GetComponent<PaceController>();
        distanceMultiplier = environControl.SetDistanceScale(environControl.distanceScale);
        velocityMultiplier = environControl.SetTimeScale(environControl.timeScale);
        velocity = initialVelocity*velocityMultiplier;
    }

    void FixedUpdate()
    {
        particles = GameObject.FindGameObjectsWithTag("Particle");
        Vector3 newVelSum = Vector3.zero;
        Vector3 collisionVelocity = Vector3.zero;
        Vector3 magneticFieldVelocity = Vector3.zero;
        float accMultiplier = Mathf.Pow(velocityMultiplier,2f);
        foreach (GameObject particle in particles){
            if (particle!= this.gameObject){
                Vector3 dist = FindDistance(particle);
                ParticleMassBehavior otherParticle = particle.GetComponent<ParticleMassBehavior>();
                float otherMass = otherParticle.mass;
                float otherCharge = otherParticle.charge;
                float deltaR = (particle.transform.localScale.x - this.transform.localScale.x)/2;
                if (dist.magnitude < Mathf.Abs(deltaR)){
                    Vector3 x1 = transform.position;
                    Vector3 x2 = particle.transform.position;
                    Vector3 v1 = this.velocity;
                    Vector3 v2 = otherParticle.velocity;
                    collisionVelocity = Collision(x1,x2,v1,v2, otherMass);
                }
                dist = dist*distanceMultiplier;
                Vector3 velocityGravity = AccelerationGravity(dist,otherMass)*accMultiplier*Time.fixedDeltaTime;
                Vector3 velocityCharge = AccelerationElectric(dist,otherCharge)*accMultiplier*Time.fixedDeltaTime;
                if (otherParticle.magneticMoment != Vector3.zero){
                    Vector3 magneticFieldAcceleration = AccelerationMagneticDipole(dist,otherParticle.magneticMoment);
                    magneticFieldVelocity = magneticFieldAcceleration*accMultiplier*Time.fixedDeltaTime;
                }
                newVelSum += velocityGravity;
                newVelSum += velocityCharge;
                newVelSum += collisionVelocity;
                newVelSum += magneticFieldVelocity;
            }
        
        velocity += newVelSum/distanceMultiplier;
        // Debug.Log(velocity.magnitude);
        Vector3 deltaPos = 0.5f*velocity*Time.fixedDeltaTime;
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

    Vector3 AccelerationMagneticDipole(Vector3 dist, Vector3 magMoment)
    {
        float distMag = dist.magnitude;
        Vector3 rUnit = dist/distMag;
        float pi = Mathf.PI;
        float mu0 = 4f*pi*Mathf.Pow(10f,-7f);
        Vector3 magField = (charge*mu0*(3*rUnit*Vector3.Dot(magMoment,rUnit) -magMoment))/(mass*4f*pi*Mathf.Pow(distMag,3f));
        Vector3 magneticFieldAcceleration = Vector3.Cross(velocity,magField);
        Debug.Log(magField);
        return magneticFieldAcceleration;
    }
}
