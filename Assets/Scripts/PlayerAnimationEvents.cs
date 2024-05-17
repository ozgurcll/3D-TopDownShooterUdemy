using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver()
    {
        visualController.ReturnRigWeightToOne();
    }
    public void WeaponGrabIsOver()
    {
        visualController.ReturnRigWeightToOne();
        visualController.ReturnWeightToLeftHandIK();
        visualController.SetBusyGrabbingWeaponTo(false);
    }
}
