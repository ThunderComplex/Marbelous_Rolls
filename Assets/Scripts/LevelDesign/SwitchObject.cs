using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    [SerializeField] private bool activ;

    private Collider objCollider;
    private Material objMaterial;

    private Color activColor;
    private Color inactivColor;
    void Awake()
    {
        objCollider = GetComponent<Collider>();
        objMaterial = gameObject.GetComponent<Renderer>().material;

        activColor = objMaterial.color;

        Color transperantColor = activColor;
        transperantColor.a = 0.4f;
        inactivColor = transperantColor;

        if (activ == false)
        {
            objCollider.isTrigger = true;
            objMaterial.color = inactivColor;
        }
    }

    public void ChangeColorState()
    {
        if (activ)
        {
            objCollider.isTrigger = true;
            objMaterial.color = inactivColor;
        }
        else
        {
            objCollider.isTrigger = false;
            objMaterial.color = activColor;
        }
        activ = !activ;
    }
}
