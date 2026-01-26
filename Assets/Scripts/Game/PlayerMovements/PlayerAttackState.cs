using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine curContext, PlayerStateFactory playerStateFactory)
    :base(curContext, playerStateFactory){ }
    public override void EnterState()
    {
        Ctx._animator.SetInteger(Ctx._isAttackingHash, 1);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Ctx._animator.SetInteger(Ctx._isAttackingHash, 0);
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx._isAttackPressed)
        {
            SwitchState(Factory.Idle());
        }
    }
    public override void InitializeSubState() { }
}
