using UnityEngine;

// Controls the camera
public static class CameraController
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // Main camera reference
    public static GameObject cameraObject;
    public static Camera camera;
    
    // Camera default position, rotation, and zoom levels
    public static readonly Vector3 cameraDefaultPosition = new Vector3(0, 0, -10);
    public static readonly Quaternion cameraDefaultRotation = Quaternion.Euler(0, 0, 0);
    public static readonly float cameraDefaultZoomLevel = 40f;

    // Zoom stuff
    public static Vector2 aimTotal = new Vector2(0, 0);
    public static float zoomTotal = 0f;


    // Setup camera
    public static void SetupCamera()
    {
        if(gm.mainCamera == null)
        {
            gm.mainCamera = GameObject.Find("Main Camera");
        }
        cameraObject = gm.mainCamera;
        camera = gm.mainCamera.GetComponent<Camera>();
    }

    // Moves the camera according to inputs
    public static void UpdateCameraPosition(Vector2 _aim, float _zoom)
    {
        // Update aim
        aimTotal += _aim;
        if(aimTotal.magnitude > 0)
        {
            Vector3 newPosition = Vector3.Lerp(cameraObject.transform.position, cameraObject.transform.position + (new Vector3(aimTotal.x, aimTotal.y, 0) * (camera.orthographicSize / 10) * gm.aimSensitivity), gm.aimSpeed);
            cameraObject.transform.position = newPosition;
            aimTotal = Vector2.Lerp(aimTotal, new Vector2(0, 0), gm.aimDecay);
        }
        // Update zoom
        zoomTotal -= _zoom;
        if(zoomTotal != 0)
        {
            float newSize = Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, camera.orthographicSize + (zoomTotal * gm.zoomSensitivity), gm.zoomSpeed), 1, 100);
            camera.orthographicSize = newSize;
            zoomTotal = Mathf.Lerp(zoomTotal, 0, gm.zoomDecay);
        }
    }

    // Reset camera to default position
    public static void ResetCameraPosition()
    {
        cameraObject.transform.position = cameraDefaultPosition;
        cameraObject.transform.rotation = cameraDefaultRotation;
        camera.orthographicSize = cameraDefaultZoomLevel;
    }
}
