using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttackStateMultiplayer : PlayerBaseStateMultiplayer
{
    public PlayerAttackStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
    :base(curContext, playerStateFactory){ }
    //public static event Action<bool> hitBoxIsActive;
    
    public override void EnterState()
    {
        Debug.Log($"<color=red>[Attack State]</color> Enter. Animator integer set to 1. Frame: {Time.frameCount}");
        Ctx._animator.SetInteger(Ctx._isAttackingHash, (int)Ctx.atktype);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Debug.Log($"<color=red>[Attack State]</color> Exit. Animator integer set to 0. Frame: {Time.frameCount}");
        Ctx._animator.SetInteger(Ctx._isAttackingHash, 0);
    }
    public override void CheckSwitchStates()
    {
        //if (!Ctx._isAttackPressed && !Ctx._camIsMoving)
        //{
        //    SwitchState(Factory.Idle());
        //}
        //else if (Ctx._camIsMoving && !Ctx._isAttackPressed)
        //{
        //    SwitchState(Factory.Walk());
        //}

        if (Ctx._takingDmg== 1)
        {
            SwitchState(Factory.Damaged());
        }
        else if (Ctx._camIsMoving && Ctx._stillAttacking == 0)
        {
            SwitchState(Factory.Walk());
        }
        else if(!Ctx._camIsMoving && Ctx._stillAttacking == 0 && !Ctx._isGuardingPressed)
        {
            SwitchState(Factory.Idle());
        } else if(Ctx._isGuardingPressed && Ctx._stillAttacking == 0)
        {
            SwitchState(Factory.Guard());
        }
        //if(Ctx._stillAttacking == 0)
        //{
        //    SwitchState(Factory.Idle());
        //}

    }
    public override void InitializeSubState() { }
}
