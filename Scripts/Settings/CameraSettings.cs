using UnityEngine;

// Camera settings
public class CameraSettings : MonoBehaviour
{
    // Singleton
    public static CameraSettings instance;

    // Camera
    public GameObject mainCamera;
    public GameObject pointer;

    // Settings
    [Range(0, 20)]
    public float aimSensitivity = 10f;
    [Range(0, 10)]
    public float zoomSensitivity = 2f;
    [Range(0, 1)]
    public float zoomSpeed = 0.5f;
    [Range(0, 1)]
    public float zoomDecay = 0.35f;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        this.mainCamera = this.gameObject;
        this.pointer = GameObject.Find("Pointer");
    }
}
