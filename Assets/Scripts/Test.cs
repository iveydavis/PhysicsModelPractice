using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector3 pos;
    Vector3 posSave;
    // Start is called before the first frame update
    void Start()
    {
        posSave = transform.position;
        // Debug.Log(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        if (pos.z > 0 & posSave.z < 0){
            Debug.Log(Time.time);
        }
        if (pos.z< 0 & posSave.z > 0){
            Debug.Log(Time.time);
        }
        posSave = pos;
    }
}
