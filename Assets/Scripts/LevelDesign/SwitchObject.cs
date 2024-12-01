using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    [SerializeField] private bool activ;

    //[SerializeField] private Collider collider = new Collider();

    private Color activColor;
    private Color DisableColor;
    void Awake()
    {
        //activColor = 
    }

    public void ChangeColorState()
    {
        if (activ)
        {

        }
        else
        {

        }
        activ = !activ;
    }
}
