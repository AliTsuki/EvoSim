// Controls the state of the game
public static class GameController
{
    // Version
    private static readonly string version = "0.0.2f";

    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // Random number generator
    public static System.Random random = new System.Random();


    // Start is called before the first frame update
    public static void Start()
    {
        Logger.InitializeLog();
        CameraController.SetupCamera();
        UIController.Start();
        InputController.Start();
        World.Start();
        Lifeforms.Start();
    }

    // Update is called once per frame
    public static void Update()
    {
        UIController.Update();
        InputController.Update();
        World.Update();
        Lifeforms.Update();
    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {
        UIController.FixedUpdate();
        InputController.FixedUpdate();
        World.FixedUpdate();
        Lifeforms.FixedUpdate();
    }
}
