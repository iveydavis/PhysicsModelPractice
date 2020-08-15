using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceController : MonoBehaviour
{
    public enum TimeScale{
        second,
        minute,
        hour,
        day
    }
    [SerializeField]
    public TimeScale timeScale;

    public enum DistanceScale{
        JupiterRadius,
        SolarRadius,
        AU,
        oneToOne
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
    public void SetTimeScale(TimeScale scale)
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
            default:
                throw new System.Exception("Unrecognized distance scale: "+ distance);
        }
    }
    
}
