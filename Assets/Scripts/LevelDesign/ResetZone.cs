using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetZone : MonoBehaviour
{
    private int resetTime = 2;
    private bool resetStarted;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(resetStarted == false)
            {
                resetStarted = true;
                StartCoroutine(StartReset());
            }
        }
    }
    private IEnumerator StartReset()
    {
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.levelFail);
        yield return new WaitForSeconds(resetTime);

        //MenuController.Instance.ControlsDisable();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
