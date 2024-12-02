using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerupType powerupType;
    [Range(1, 10)]
    public int amount;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GivePowerUp(powerupType, amount);
            PlayerUI.Instance.GainAbility(powerupType);
        }
    }
}

public enum PowerupType
{
    SpeedBoost
}
