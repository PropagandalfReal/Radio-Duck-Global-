using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerCamera : MonoBehaviour
{
    public Camera Cameralower;
    float PreviousSize;
    // Start is called before the first frame update
    void Start()
    {
        PreviousSize = transform.localScale.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * PreviousSize * Mathf.Sqrt(Cameralower.orthographicSize/70); ;
    }
}
