using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    public float time;
    private float currentGameTime;

    private int milliseconds;
    private int seconds;
    private int minutes;
    private void Awake()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        time = 0;
    }
    void Update()
    {
        time += Time.deltaTime;

        milliseconds = Mathf.FloorToInt(time * 100);
        milliseconds = Mathf.FloorToInt(milliseconds % 100);
        seconds = Mathf.FloorToInt(time % 60);
        minutes = Mathf.FloorToInt(time / 60 % 60);
        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        //time += Time.deltaTime;
        //timerText.text = string.Format("{00:00.00}", time);
    }

}
