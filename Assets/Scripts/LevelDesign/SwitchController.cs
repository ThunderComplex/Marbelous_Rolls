using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private Controls controls;

    private List<SwitchObject> switchObjList = new List<SwitchObject>();

    private void Awake()
    {
        controls = Keybindinputmanager.inputActions;

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out SwitchObject switchObject))
            {
                switchObjList.Add(switchObject);
            }
        }
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    void Update()
    {
        if(controls.Player.Switch.WasPerformedThisFrame() && MenuController.Instance.gameIsPaused == false)
        {
            foreach (SwitchObject obj in switchObjList)
            {
                obj.ChangeColorState();
            }
        }
    }
}
