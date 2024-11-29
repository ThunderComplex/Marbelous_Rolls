using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotSpeed;
    public Camera playerCamera;
    private Rigidbody _rigidbody;
    private Vector3 speed;
    private Controls controls;

    void Awake()
    {
        controls = new Controls();
        controls.Enable();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TryGetComponent(out _rigidbody);
    }

    void Update()
    {
        var move = controls.Player.Move.ReadValue<Vector2>();
        var move3d = new Vector3(move.x, 0, move.y);
        var moveAngles = playerCamera.transform.rotation.eulerAngles;
        // Ignore vertical camera rotation
        moveAngles.x = 0f;
        var q = Quaternion.Euler(moveAngles);
        speed = q * move3d * rotSpeed;
    }

    void FixedUpdate()
    {
        var timesteppedSpeed = speed * Time.fixedDeltaTime;
        _rigidbody.AddTorque(
            new Vector3(timesteppedSpeed.z, 0, timesteppedSpeed.x * -1),
            ForceMode.Acceleration
        );
    }
}
