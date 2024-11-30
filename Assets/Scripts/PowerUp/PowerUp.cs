using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerupType powerupType;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GivePowerUp(powerupType);
        }
    }
}

public enum PowerupType
{
    DoubleJump,
    SpeedBoost
}
