using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    private Controls controls;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject countdownObj;
    public GameObject timerObj;

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
        if(playerController != null) playerController.gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
        if (playerController != null) playerController.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    public float GetFinalTime()
    {
        timerObj.SetActive(false);
        return timerObj.GetComponent<Timer>().time;
    }
}
