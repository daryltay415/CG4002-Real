using Unity.Netcode;
using UnityEngine;

public class PlayerAttackStateMultiplayer : PlayerBaseStateMultiplayer
{
    public PlayerAttackStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
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
