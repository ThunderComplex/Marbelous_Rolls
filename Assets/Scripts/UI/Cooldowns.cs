using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Cooldowns : MonoBehaviour
{
    [SerializeField] private Image cooldownIcon;
    [SerializeField] private Image cooldownTimeIcon;
    [SerializeField] private TextMeshProUGUI cooldownCharges;

    private float time;

    public void SetCooldown(int charges)
    {
        cooldownIcon.color = Color.green;
        cooldownCharges.text = charges.ToString();
        cooldownTimeIcon.fillAmount = 0;
    }

    public IEnumerator Cooldown(float cooldownTime, int abilityNumber, int charges)
    {
        cooldownCharges.text = charges.ToString();
        time = cooldownTime;
        cooldownIcon.color = Color.red;
        cooldownTimeIcon.fillAmount = 1;
        while (time > 0)
        {
            time -= Time.deltaTime;
            cooldownTimeIcon.fillAmount = time / cooldownTime;
            yield return null;
        }

        cooldownIcon.color = Color.green;
        PlayerController.instance.powerUpCooldowns[abilityNumber] = false;
    }
}
