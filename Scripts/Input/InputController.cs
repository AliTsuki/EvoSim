using UnityEngine;

// Gets inputs and modifies game objects accordingly
public static class InputController
{
    public static Vector2 aim;
    public static float zoom;


    // Start is called before the first frame update
    public static void Start()
    {

    }

    // Update is called once per frame
    public static void Update()
    {
        GetInput();
        CameraController.UpdateCameraZoom(zoom);
        CameraController.GetMousePositionInWorld();
    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {
        GetInput();
        CameraController.UpdateCameraPosition(aim);
    }

    // Get inputs from keyboard/mouse
    public static void GetInput()
    {
        aim = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        zoom = Input.GetAxis("Mouse ScrollWheel");
    }
}
