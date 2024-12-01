using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed;
    public float jumpForce;
    public Camera playerCamera;
    private Rigidbody _rigidbody;
    private Vector3 speed;
    private Controls controls;
    private bool performJump;
    private bool isGrounded;
    private PowerupType? currentPowerup = null;
    private bool canDoubleJump = false;
    private bool canSpeedBoost = false;
    private Vector3 steeringVector = Vector3.zero;

    void Awake()
    {
        controls = new Controls();
        controls.Enable();

        var debugPlayerStart = GameObject.Find("DebugPlayerStart");

        if (debugPlayerStart != null)
        {
            transform.position = debugPlayerStart.transform.position;
        }

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
        var move3d = new Vector3(0, 0, move.y);
        var moveAngles = playerCamera.transform.rotation.eulerAngles;
        // Ignore vertical camera rotation
        moveAngles.x = 0f;
        steeringVector.y += move.x * 2;
        var q = Quaternion.Euler(steeringVector);
        speed = q * move3d * rotationSpeed;
        Debug.DrawRay(transform.position, speed, Color.red, 0.1f, false);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (isGrounded && controls.Player.Jump.WasPerformedThisFrame())
        {
            performJump = true;
        }

        if (currentPowerup != null && controls.Player.Switch.WasPerformedThisFrame())
        {
            ExecutePowerUp();
        }

    }

    void ExecutePowerUp()
    {
        switch (currentPowerup)
        {
            case PowerupType.DoubleJump:
                canDoubleJump = true;
                break;
            case PowerupType.SpeedBoost:
                canSpeedBoost = true;
                break;
        }

        currentPowerup = null;
    }

    void FixedUpdate()
    {
        var timesteppedSpeed = speed * Time.fixedDeltaTime;
        _rigidbody.AddTorque(
            new Vector3(timesteppedSpeed.z, 0, timesteppedSpeed.x * -4),
            ForceMode.Acceleration
        );

        if (performJump && isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            performJump = false;
            isGrounded = false;
        }

        if (canDoubleJump)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canDoubleJump = false;
        }

        if (canSpeedBoost)
        {
            _rigidbody.AddForce(speed * 0.5f, ForceMode.Impulse);
            canSpeedBoost = false;
        }

        // Air control
        if (!isGrounded)
        {
            _rigidbody.AddForce(timesteppedSpeed * 2);
        }
    }

    public void GivePowerUp(PowerupType powerupType)
    {
        currentPowerup = powerupType;
    }
}
