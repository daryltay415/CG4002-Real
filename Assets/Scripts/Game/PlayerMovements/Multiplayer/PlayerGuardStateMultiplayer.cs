using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuardStateMultiplayer : PlayerBaseStateMultiplayer
{
    public PlayerGuardStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
    :base(curContext, playerStateFactory){ }
    public override void EnterState()
    {
        Ctx._animator.SetBool(Ctx._isGuardingHash, true);
    }
    public override void UpdateState()
    {
        
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Ctx._animator.SetBool(Ctx._isGuardingHash, false);
    }
    public override void CheckSwitchStates()
    {
        if (Ctx._camIsMoving && !Ctx._isGuardingPressed && !Ctx._isAttackPressed)
        {
            SwitchState(Factory.Walk());
        } else if(Ctx._isAttackPressed && !Ctx._isGuardingPressed)
        {
            SwitchState(Factory.Attack());
        } else if(!Ctx._isAttackPressed && !Ctx._camIsMoving && !Ctx._isGuardingPressed)
        {
            SwitchState(Factory.Idle());
        }
    }
    public override void InitializeSubState() { }
}
