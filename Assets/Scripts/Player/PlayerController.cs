using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float rotationSpeed;
    public float jumpForce;
    public float cameraSpeed;
    public Camera playerCamera;
    public CinemachineCamera cinemaCamera;
    private Rigidbody _rigidbody;
    private Vector3 speed;
    private Controls controls;
    private bool performJump;
    private bool isGrounded;
    //private PowerupType? currentPowerup = null;
    //private int currentPowerupAmount = 0;
    private bool canSpeedBoost = false;
    private Vector3 steeringVector = Vector3.zero;
    private CinemachineOrbitalFollow orbitalFollow;
    private bool didDoubleJump = false;
    private Coroutine powerUpCoroutine;

    private int boostCharges;
    [NonSerialized] public bool[] powerUpCooldowns = new bool [2];

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        controls = Keybindinputmanager.inputActions;

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
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        TryGetComponent(out _rigidbody);
        cinemaCamera.TryGetComponent(out orbitalFollow);
    }

    void Update()
    {
        var move = controls.Player.Move.ReadValue<Vector2>();
        var move3d = new Vector3(0, 0, move.y);
        var moveAngles = playerCamera.transform.rotation.eulerAngles;
        // Ignore vertical camera rotation
        moveAngles.x = 0f;
        steeringVector.y += move.x * 5;
        var q = Quaternion.Euler(steeringVector);
        speed = q * move3d * rotationSpeed;
        // Debug.DrawRay(transform.position, speed, Color.red, 0.1f, false);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (isGrounded)
        {
            didDoubleJump = false;
        }

        if (controls.Player.Jump.WasPerformedThisFrame())
        {
            performJump = true;
        }

        if (controls.Player.Boost.WasPerformedThisFrame() && !powerUpCooldowns[0] && boostCharges > 0)
        {
            boostCharges -= 1;
            powerUpCooldowns[0] = true;
            canSpeedBoost = true;
            PlayerUI.Instance.StartCooldown(1, 0, boostCharges);
        }
        //if (currentPowerup != null && controls.Player.Boost.WasPerformedThisFrame())
        //{
        //    if (powerUpCoroutine == null)
        //    {
        //        powerUpCoroutine = StartCoroutine(ExecutePowerUp());
        //    }
        //}

    }

    //IEnumerator ExecutePowerUp()
    //{
    //    if (currentPowerupAmount > 0)
    //    {
    //        switch (currentPowerup)
    //        {
    //            case PowerupType.SpeedBoost:
    //                canSpeedBoost = true;
    //                break;
    //        }

    //        currentPowerupAmount--;
    //    }

    //    if (currentPowerupAmount == 0)
    //    {
    //        currentPowerup = null;
    //    }

    //    yield return new WaitForSeconds(0.6f);
    //    powerUpCoroutine = null;
    //}

    void FixedUpdate()
    {
        orbitalFollow.HorizontalAxis.Value = Mathf.Lerp(
            orbitalFollow.HorizontalAxis.Value,
            steeringVector.y,
            Time.fixedDeltaTime * cameraSpeed
        );
        orbitalFollow.VerticalAxis.Value = 25;

        var timesteppedSpeed = speed * Time.fixedDeltaTime;
        // _rigidbody.AddTorque(
        //     new Vector3(timesteppedSpeed.z, 0, timesteppedSpeed.x * -1),
        //     ForceMode.Acceleration
        // );
        _rigidbody.AddForce(timesteppedSpeed, ForceMode.Acceleration);

        var cameraDiff = Mathf.Abs(
            Vector3.Dot(_rigidbody.linearVelocity.normalized, playerCamera.transform.forward.normalized)
        );
        if (cameraDiff < 0.8 && speed.magnitude < 0.1f)
        {
            _rigidbody.AddForce(_rigidbody.linearVelocity * -1.5f, ForceMode.Acceleration);
        }

        if (performJump)
        {
            if (isGrounded || !didDoubleJump)
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            performJump = false;
            isGrounded = false;

            if (!didDoubleJump)
            {
                didDoubleJump = true;
            }
        }

        if (canSpeedBoost)
        {
            _rigidbody.AddForce(speed * 1.5f, ForceMode.Impulse);
            canSpeedBoost = false;
        }

        // Air control
        if (!isGrounded)
        {
            _rigidbody.AddForce(timesteppedSpeed * 2);
        }
    }

    public void GivePowerUp(PowerupType powerupType, int amount)
    {
        if (powerupType == PowerupType.SpeedBoost) boostCharges = amount;

        //currentPowerup = powerupType;
        //currentPowerupAmount = amount;
        PlayerUI.Instance.ActivateCooldownIcon(amount, PlayerUI.Cooldown.boostCD);
    }
}
