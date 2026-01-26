using UnityEngine;

public class PlayerNormalState : PlayerBaseState
{
    public PlayerNormalState(PlayerStateMachine curContext, PlayerStateFactory playerStateFactory)
    : base(curContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        //todo for god mode
    }
    public override void InitializeSubState()
    {
        SetSubState(Factory.Idle());
    }

}
