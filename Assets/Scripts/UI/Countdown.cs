using UnityEngine;
using TMPro;
using System.Collections;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private void OnEnable()
    {
        StartCoroutine(CountdownStart());
    }
    private IEnumerator CountdownStart()
    {
        int remainingTime = 4;
        while (remainingTime > 1)
        {
            remainingTime -= 1;
            countdownText.color = Color.red;
            countdownText.text = remainingTime.ToString();
            yield return new WaitForSeconds(0.7f);
        }
        PlayerUI.Instance.timerObj.SetActive(true);
        countdownText.color = Color.green;
        countdownText.text = "Go!";
        StartCoroutine(CountdownDisable());
    }
    private IEnumerator CountdownDisable()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}