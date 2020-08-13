using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleMassBehavior : MonoBehaviour
{
    [SerializeField]
    float mass = Mathf.Pow(10f,20f);
    Vector3 velocity;

    [SerializeField]
    Vector3 initialVelocity = new Vector3(0.0f,0.0f,0.0f);
    
    GameObject[] particles;
    Rigidbody rigidBody3D;
    PaceController environControl;
    float distanceMultiplier;
    
    void Start()
    {
        environControl = GameObject.Find("EnvironmentControls").GetComponent<PaceController>();
        distanceMultiplier = environControl.dist;
        particles = GameObject.FindGameObjectsWithTag("Particle");
        rigidBody3D = GetComponent<Rigidbody>();
        rigidBody3D.velocity = initialVelocity;
    }

    // Update is called once per frame
    //Maybe I should do GetComponent from the beginning? And have position as a variable?
    void FixedUpdate()
    {
        Vector3 totalAcceleration = Vector3.zero;
        foreach (GameObject particle in particles){
            if (particle!= this.gameObject){
                Vector3 dist = FindDistance(particle);
                dist = dist*distanceMultiplier;
                ParticleMassBehavior otherParticle = particle.GetComponent<ParticleMassBehavior>();
                float otherMass = otherParticle.mass;
                Vector3 acceleration = AccelerationGravity(dist,otherMass); 
                totalAcceleration += acceleration; 
            }
        velocity = totalAcceleration*Time.deltaTime;
        rigidBody3D.AddForce(velocity,ForceMode.VelocityChange);
        }
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
