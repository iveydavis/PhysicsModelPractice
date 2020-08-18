using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCloud : MonoBehaviour
{
    Vector3 origin;
    // Rigidbody rigidbody;
    public GameObject electronPrefab;
    public GameObject protonPrefab;
    public GameObject neutronPrefab;

    [Min(1)]
    [SerializeField]
    int nParticles;
    int nElectrons = 0;
    int nProtons = 0;
    int nNeutrons =  0;

    enum DistributionType{
        Uniform,
        Random
    }

    enum CloudComposition{
        Electrons,
        Protons,
        Neutrons,
        Mixed
    }
    [SerializeField]
    CloudComposition cloudComposition;

    [Range(1f,99f)]
    [SerializeField]
    float percentElectron;

    [Min(1f)]
    [SerializeField]
    float innerRadius;
    [Min(1f)]
    [SerializeField]
    float outerRadius;
    PaceController environmentController;
    ParticleMassBehavior coreController;
    float coreMass;
    float distanceScale;

    void Awake()
    {
        environmentController = GameObject.Find("EnvironmentControls").GetComponent<PaceController>();
        distanceScale = environmentController.SetDistanceScale(environmentController.distanceScale);
        // rigidbody = GetComponent<Rigidbody>();
        // origin = rigidbody.position;
        origin = transform.position;
        coreController = GetComponent<ParticleMassBehavior>();
        coreMass = coreController.mass;
        DeterminePopulation();
        InstantiateParticles(nElectrons,electronPrefab);
        InstantiateParticles(nProtons,protonPrefab);
        InstantiateParticles(nNeutrons,neutronPrefab);
    }

    Vector3 DeterminePosition()
    {
        float pi = Mathf.PI;
        float theta = Random.Range(0f,2*pi);
        float phi = Random.Range(0f,pi);
        float radius = Random.Range(innerRadius,outerRadius);
        float xval = Mathf.Sin(phi)*Mathf.Cos(theta)*radius;
        float yval = Mathf.Sin(phi)*Mathf.Sin(theta)*radius;
        float zval = Mathf.Cos(phi)*radius;
        Vector3 pos = new Vector3(xval,yval,zval);
        return pos;
    }

    Vector3 DetermineInitialVelocity(Vector3 pos)
    {
        float xval = -pos.y;
        float yval = pos.x;
        Vector3 vel = new Vector3(xval,yval);
        vel = vel/vel.magnitude;
        float radius = pos.magnitude;
        //Debug.Log(radius);
        Vector3 zvec = new Vector3(0,0,pos.z);
        float G = 6.67f*Mathf.Pow(10f,-11f);
        float theta = Mathf.Acos(Vector3.Dot(pos,zvec)/(pos.z*radius)); //angle from z axis
        float velMag = Mathf.Sqrt(G*coreMass/radius);
        vel = vel*velMag*Mathf.Sin(theta);
        return vel;
    }

    void DeterminePopulation()
    {
        switch(cloudComposition){
            case CloudComposition.Electrons:
                nElectrons = nParticles;
                return;
            case CloudComposition.Protons:
                nProtons = nParticles;
                return;
            case CloudComposition.Neutrons:
                nNeutrons = nParticles;
                return;
            case CloudComposition.Mixed:
                nElectrons = Mathf.CeilToInt(percentElectron*nParticles/100);
                nProtons = nParticles - nElectrons;
                return;
            default:
                throw new System.Exception("Uhhhhhhh");
        }
    }
    void InstantiateParticles(int N, GameObject prefab)
    {
        for (int i = 0; i < N; i++){
            Vector3 pos = DeterminePosition();
            Vector3 velocity = DetermineInitialVelocity(pos*distanceScale);
            Vector3 truePos = origin + pos;
            GameObject particle = Instantiate(prefab,truePos,Quaternion.identity);
            ParticleMassBehavior particleBehavior = particle.GetComponent<ParticleMassBehavior>();
            particleBehavior.initialVelocity = velocity; // i don't think this actually does anything
            particleBehavior.velocity = velocity; // this is way too fast
        }
    }
}
