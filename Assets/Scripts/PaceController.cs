using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceController : MonoBehaviour
{
    enum TimeScale{
        second,
        minute,
        hour,
        day
    }
    [SerializeField]
    TimeScale timeScale;

    enum DistanceScale{
        JupiterRadius,
        SolarRadius,
        AU
    }
    [SerializeField]
    DistanceScale distanceScale;
    public float dist;
    float million = Mathf.Pow(10f,6f);
    float billion = Mathf.Pow(10f,9f);

    void Start()
    {
        SetTimeScale(timeScale);
        SetDistanceScale(distanceScale);
    }
    void SetTimeScale(TimeScale scale)
    {
        switch(scale){
            case TimeScale.second:
                Time.timeScale = 1.0f;
                return;
            case TimeScale.minute:
                Time.timeScale = 60f;
                return;
            case TimeScale.hour:
                Time.timeScale= 3600f;
                return;
            case TimeScale.day:
                Time.timeScale = 3600f*24f;
                return;
            default:
                throw new System.Exception("Unrecognized time scale: "+scale);
        }

    }
    void SetDistanceScale(DistanceScale distance)
    {
        switch(distance){
            case DistanceScale.JupiterRadius:
                dist = 69.911f*million;
                return;
            case DistanceScale.SolarRadius:
                dist = 693.34f*million;
                return;
            case DistanceScale.AU:
                dist = 149.6f*billion;
                return;
            default:
                throw new System.Exception("Unrecognized distance scale: "+ distance);
        }
    }
    
}
