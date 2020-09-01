using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{
    float time = 0;
    TextMeshPro textMesh;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        textMesh.SetText("Time: {0}",time);
    }
}
