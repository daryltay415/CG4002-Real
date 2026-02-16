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
        if (Ctx._takingDmg == 1)
        {
            SwitchState(Factory.Damaged());
        }
        else if (Ctx._camIsMoving && !Ctx._isAttackPressed)
        {
            SwitchState(Factory.Walk());
        }
        else if (Ctx._isAttackPressed)
        {
            Debug.Log($"[Idle State] Condition Met: isAttackPressed is TRUE. Switching to Attack. Frame: {Time.frameCount}");
            SwitchState(Factory.Attack());
        }
        else if (Ctx._isGuardingPressed)
        {
            SwitchState(Factory.Guard());
        }
        
    }
    public override void InitializeSubState() { }
}
