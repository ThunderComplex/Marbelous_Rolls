using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    private Controls controls;

    [SerializeField] private GameObject countdownObj;
    public GameObject timerObj;

    [SerializeField] private Cooldowns boostCooldown;
    [SerializeField] private Cooldowns gravityCooldown;

    [NonSerialized] public Cooldown cooldown;
    public enum Cooldown
    {
        boostCD,
        gravityCD,
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        controls = Keybindinputmanager.inputActions;
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    void Start()
    {
        if(PlayerController.instance != null) PlayerController.instance.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        countdownObj.SetActive(true);
    }
    private void Update()
    {
        if (controls.Player.Reset.WasPerformedThisFrame() && MenuController.Instance.gameIsPaused == false)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void SwitchPlayerRigidbody()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    public float GetFinalTime()
    {
        timerObj.SetActive(false);
        return timerObj.GetComponent<Timer>().time;
    }

    public void ActivateCooldownIcon(int charges)
    {
        switch (cooldown)
        {
            case Cooldown.boostCD:
                CooldownIcon(boostCooldown, charges);
                break;
            case Cooldown.gravityCD:
                CooldownIcon(gravityCooldown, charges);
                break;

        }
    }
    private void CooldownIcon(Cooldowns cd, int charges)
    {
        cd.gameObject.SetActive(true);
        cd.SetCooldown(charges);
    }

    public void StartCooldown(float cooldownTime, bool cdBool, int charges)
    {
        switch (cooldown)
        {
            case Cooldown.boostCD:
                StartCD(boostCooldown,cooldownTime, cdBool, charges);
                break;
            case Cooldown.gravityCD:
                StartCD(gravityCooldown, cooldownTime, cdBool, charges);
                break;
        }
    }
    private void StartCD(Cooldowns cd, float cooldownTime, bool cdBool,  int charges)
    {
        if (charges <= 0) cd.gameObject.SetActive(false);
        else StartCoroutine(cd.Cooldown(cooldownTime, cdBool));
    }
}
