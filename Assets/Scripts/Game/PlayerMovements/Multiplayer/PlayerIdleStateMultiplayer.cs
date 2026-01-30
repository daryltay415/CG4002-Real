using UnityEngine;

public class PlayerIdleStateMultiplayer : PlayerBaseStateMultiplayer
{
    public PlayerIdleStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
    :base(curContext, playerStateFactory){ }
    public override void EnterState()
    {
        
    }
    public override void UpdateState()
    {
        
        CheckSwitchStates();
    }
    public override void ExitState()
    { 
    }
    public override void CheckSwitchStates()
    {
        if (Ctx._camIsMoving)
        {
            SwitchState(Factory.Walk());
        }
        else if (Ctx._isAttackPressed)
        {
            SwitchState(Factory.Attack());
        }
    }
    public override void InitializeSubState() { }
}
