using Cinemachine;

using UnityEngine;

// Controls the camera
public static class CameraController
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;
    // Camera Settings reference
    private static readonly CameraSettings camSettings = CameraSettings.instance;

    // GameObject references
    private static GameObject pointer;
    private static CinemachineVirtualCamera vCamera;
    private static Camera camera;
    
    // Camera default position, rotation, and zoom levels
    private static readonly Vector3 pointerDefaultPosition = new Vector3(0, 0, 0);
    private static readonly float cameraDefaultZoomLevel = 40f;

    // Zoom stuff
    private static float zoomTotal = 0f;

    // Mouse position in world
    public static Vector3 mousePositionInWorld = new Vector3();


    // Setup camera
    public static void SetupCamera()
    {
        pointer = camSettings.pointer;
        pointer.transform.position = pointerDefaultPosition;
        camera = camSettings.mainCamera.GetComponent<Camera>();
        vCamera = camSettings.mainCamera.GetComponent<CinemachineVirtualCamera>();
        vCamera.m_Lens.OrthographicSize = cameraDefaultZoomLevel;
    }

    // Moves the camera according to inputs
    public static void UpdateCameraPosition(Vector2 _aim)
    {
        // Update aim
        if(_aim.magnitude > 0)
        {
            pointer.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(_aim.x, 0, _aim.y) * (vCamera.m_Lens.OrthographicSize / 10) * camSettings.aimSensitivity);
        }
    }

    // Zooms the camera in and out
    public static void UpdateCameraZoom(float _zoom)
    {
        // Update zoom
        zoomTotal -= _zoom;
        if(zoomTotal != 0)
        {
            float newSize = Mathf.Clamp(Mathf.Lerp(vCamera.m_Lens.OrthographicSize, vCamera.m_Lens.OrthographicSize + (zoomTotal * camSettings.zoomSensitivity), camSettings.zoomSpeed), 1, 100);
            vCamera.m_Lens.OrthographicSize = newSize;
            zoomTotal = Mathf.Lerp(zoomTotal, 0, camSettings.zoomDecay);
        }
    }

    // Reset camera to default position
    public static void ResetCameraPosition()
    {
        pointer.transform.position = pointerDefaultPosition;
        vCamera.m_Lens.OrthographicSize = cameraDefaultZoomLevel;
    }

    // Get mouse point in world space
    public static void GetMousePositionInWorld()
    {
        mousePositionInWorld = camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
