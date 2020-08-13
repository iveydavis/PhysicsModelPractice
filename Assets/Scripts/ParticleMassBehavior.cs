using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleMassBehavior : MonoBehaviour
{
    [SerializeField]
    float mass;
    Vector3 velocity;

    [SerializeField]
    Vector3 initialVelocity = new Vector3(0.0f,0.0f,0.0f);
    Vector3 previousVelocity;

    GameObject[] particles;
    Rigidbody rigidBody3D;
    enum SystemScale{
        SolarSystem,
        Stellar
    }
    [SerializeField]
    SystemScale systemScale;
    float distanceMultiplier;
    
    void Start()
    {
        previousVelocity = initialVelocity;
        particles = GameObject.FindGameObjectsWithTag("Particle");
        rigidBody3D = GetComponent<Rigidbody>();
        SetSystemScale();
    }

    // Update is called once per frame
    //Maybe I should do GetComponent from the beginning? And have position as a variable?
    void FixedUpdate()
    {
        Vector3 totalAcceleration = new Vector3(0.0f,0.0f,0.0f);
        foreach (GameObject particle in particles){
            Vector3 dist = FindDistance(particle);
            if (dist.magnitude!= 0){
                dist = dist*distanceMultiplier;
                ParticleMassBehavior otherParticle = particle.GetComponent<ParticleMassBehavior>();
                float otherMass = otherParticle.mass;
                Vector3 acceleration = AccelerationGravity(dist,otherMass); 
                totalAcceleration += acceleration; 
            }
        velocity = previousVelocity + totalAcceleration*Time.deltaTime;
        previousVelocity = velocity;
        Debug.Log(velocity.magnitude);
        rigidBody3D.AddForce(velocity,ForceMode.VelocityChange);
        }
    }

    void SetSystemScale()
    {
        if (systemScale == SystemScale.SolarSystem){
            distanceMultiplier = 69.911f*Mathf.Pow(10f,6f);}
        if (systemScale == SystemScale.Stellar){
            distanceMultiplier = 696.34f * Mathf.Pow(10f, 6f);}
    }
    Vector3 FindDistance(GameObject other)
    {
        Vector3 dist = transform.position - other.transform.position;
        return dist;   
    }
    Vector3 AccelerationGravity(Vector3 distance,float otherMass)
    {
        float distMag = distance.magnitude;
        float G = 6.67f*Mathf.Pow(10f,-11f);
        Vector3 acceleration = -G*otherMass*distance/Mathf.Pow(distMag,3f);
        return acceleration;
    }
}
