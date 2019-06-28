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
    private static CinemachineVirtualCamera camera;
    
    // Camera default position, rotation, and zoom levels
    private static readonly Vector3 pointerDefaultPosition = new Vector3(0, 0, 0);
    private static readonly float cameraDefaultZoomLevel = 40f;

    // Zoom stuff
    private static float zoomTotal = 0f;


    // Setup camera
    public static void SetupCamera()
    {
        pointer = camSettings.pointer;
        pointer.transform.position = pointerDefaultPosition;
        camera = camSettings.mainCamera.GetComponent<CinemachineVirtualCamera>();
        camera.m_Lens.OrthographicSize = cameraDefaultZoomLevel;
    }

    // Moves the camera according to inputs
    public static void UpdateCameraPosition(Vector2 _aim, float _zoom)
    {
        // Update aim
        if(_aim.magnitude > 0)
        {
            pointer.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(_aim.x, 0, _aim.y) * (camera.m_Lens.OrthographicSize / 10) * camSettings.aimSensitivity);
        }
        // Update zoom
        zoomTotal -= _zoom;
        if(zoomTotal != 0)
        {
            float newSize = Mathf.Clamp(Mathf.Lerp(camera.m_Lens.OrthographicSize, camera.m_Lens.OrthographicSize + (zoomTotal * camSettings.zoomSensitivity), camSettings.zoomSpeed), 1, 100);
            camera.m_Lens.OrthographicSize = newSize;
            zoomTotal = Mathf.Lerp(zoomTotal, 0, camSettings.zoomDecay);
        }
    }

    // Reset camera to default position
    public static void ResetCameraPosition()
    {
        pointer.transform.position = pointerDefaultPosition;
        camera.m_Lens.OrthographicSize = cameraDefaultZoomLevel;
    }
}
