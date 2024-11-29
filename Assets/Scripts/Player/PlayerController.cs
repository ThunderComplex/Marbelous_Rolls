using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotSpeed;
    public Camera playerCamera;
    private Rigidbody _rigidbody;
    private Vector2 speed;
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
        var ee = playerCamera.transform.rotation.eulerAngles;
        ee.x = 0f;
        var q = Quaternion.Euler(ee);
        speed = q * move * rotSpeed * -1;
    }

    void FixedUpdate()
    {
        _rigidbody.AddTorque(speed * Time.fixedDeltaTime);
    }
}
