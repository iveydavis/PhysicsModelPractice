using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceController : MonoBehaviour
{
    public enum TimeScale{
        defaultScale,
        second,
        minute,
        hour,
        day
    }
    //[SerializeField]
    public TimeScale timeScale;


    public enum DistanceScale{
        JupiterRadius,
        SolarRadius,
        AU,
        oneToOne,
        Angstrom
    }
    
    public DistanceScale distanceScale;
    float dist;
    float million = Mathf.Pow(10f,6f);
    float billion = Mathf.Pow(10f,9f);

    // void Start()
    // {
    //     SetTimeScale(timeScale);
    //     SetDistanceScale(distanceScale);
    // }
    public float SetTimeScale(TimeScale scale)
    {
        switch(scale){
            case TimeScale.defaultScale:
                Time.fixedDeltaTime = 0.02f;
                return 1.0f;
            case TimeScale.second:
                //Time.fixedDeltaTime = 1f;
                return 1.0f;
            case TimeScale.minute:
                //Time.fixedDeltaTime = 1f;
                return 60f;
            case TimeScale.hour:
                //Time.fixedDeltaTime = 1f;
                return 3600f;
            case TimeScale.day:
                //Time.fixedDeltaTime = 1f;
                return 3600f*24f;
            default:
                throw new System.Exception("Unrecognized time scale: "+scale);
        }

    }
    public float SetDistanceScale(DistanceScale distance)
    {
        switch(distance){
            case DistanceScale.JupiterRadius:
                return 69.911f*million;
            case DistanceScale.SolarRadius:
                return 693.34f*million;
            case DistanceScale.AU:
                return 149.6f*billion;
            case DistanceScale.oneToOne:
                return 1f;
            case DistanceScale.Angstrom:
                return Mathf.Pow(10f,-10f);
            default:
                throw new System.Exception("Unrecognized distance scale: "+ distance);
        }
    }
    
}
