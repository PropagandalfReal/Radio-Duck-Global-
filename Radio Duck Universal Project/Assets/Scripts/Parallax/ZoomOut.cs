using UnityEngine;

public class ZoomOut: MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 InitialZoom;
    public Camera MovingCamera;
    float InitialCameraZoom;
    void Start()
    {
        InitialCameraZoom = MovingCamera.orthographicSize;
        InitialZoom = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * Mathf.Clamp(MovingCamera.orthographicSize, 24, 58);
    }
}
