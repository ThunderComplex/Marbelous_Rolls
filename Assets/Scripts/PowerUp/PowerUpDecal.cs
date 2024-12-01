using UnityEngine;

[ExecuteInEditMode]
public class PowerUpDecal : MonoBehaviour
{
    public Material speedBoostMaterial;
    public Material doubleJumpMaterial;

    void Update()
    {
        var powerupType = transform.parent.GetComponent<PowerUp>().powerupType;
        var crenderer = GetComponent<Renderer>();

        switch (powerupType)
        {
            case PowerupType.SpeedBoost:
                crenderer.material = speedBoostMaterial;
                break;
        }
    }
}
