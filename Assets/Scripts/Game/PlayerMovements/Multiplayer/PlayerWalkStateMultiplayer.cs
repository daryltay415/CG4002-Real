using UnityEngine;

public class PlayerWalkStateMultiplayer : PlayerBaseStateMultiplayer
{
    public PlayerWalkStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
    :base(curContext, playerStateFactory){ }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void EnterState()
    {
        Ctx._animator.SetBool(Ctx._isWalkingHash, true);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Ctx._animator.SetBool(Ctx._isWalkingHash, false);
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx._camIsMoving)
        {
            SwitchState(Factory.Idle());
        }
    }
    public override void InitializeSubState() { }
}
