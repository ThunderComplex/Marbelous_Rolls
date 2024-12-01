using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

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
    }
    void Start()
    {
        if(playerController != null) playerController.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        countdownObj.SetActive(true);
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
