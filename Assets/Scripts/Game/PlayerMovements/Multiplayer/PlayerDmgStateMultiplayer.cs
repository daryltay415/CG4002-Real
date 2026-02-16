using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerDmgStateMultiplayer : PlayerBaseStateMultiplayer
{
    public PlayerDmgStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
    :base(curContext, playerStateFactory){ }
    
    public override void EnterState()
    {
        //Debug.Log($"<color=red>[Attack State]</color> Enter. Animator integer set to 1. Frame: {Time.frameCount}");
        Ctx._animator.SetBool(Ctx._isDamagedHash, true);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        //Debug.Log($"<color=red>[Attack State]</color> Exit. Animator integer set to 0. Frame: {Time.frameCount}");
        Ctx._animator.SetBool(Ctx._isDamagedHash, false);
    }
    public override void CheckSwitchStates()
    {
        
        if (Ctx._camIsMoving && Ctx._takingDmg == 0)
        {
            SwitchState(Factory.Walk());
        }
        else if(!Ctx._camIsMoving && Ctx._takingDmg == 0 && !Ctx._isGuardingPressed)
        {
            SwitchState(Factory.Idle());
        } else if(Ctx._isGuardingPressed && Ctx._takingDmg == 0)
        {
            SwitchState(Factory.Guard());
        }

    }
    public override void InitializeSubState() { }
}
