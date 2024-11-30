using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

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
        countdownObj.SetActive(true);
    }
    public float GetFinalTime()
    {
        timerObj.SetActive(false);
        return timerObj.GetComponent<Timer>().time;
    }
}
