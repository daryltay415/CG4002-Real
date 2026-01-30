using UnityEngine;

public class PlayerNormalStateMultiplayer : PlayerBaseStateMultiplayer
{
    public PlayerNormalStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
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
