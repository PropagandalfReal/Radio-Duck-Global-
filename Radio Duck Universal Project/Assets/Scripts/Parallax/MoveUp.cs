using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp: MonoBehaviour
{
    public Camera MovingCamera;
    float InitialSize;
    float InitialPositionY;
    // Start is called before the first frame update
    void Start()
    {
        InitialSize = MovingCamera.orthographicSize;
        InitialPositionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, InitialPositionY + (InitialSize - MovingCamera.orthographicSize));
    }
}
