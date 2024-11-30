using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            float finalTime = PlayerUI.Instance.GetFinalTime();
            MenuController.Instance.OnLevelComplete(finalTime);
        }
    }
}
