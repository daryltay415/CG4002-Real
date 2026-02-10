public class PlayerStateFactoryMultiplayer
{
    PlayerStateMachineMultiplayer _context;

    public PlayerStateFactoryMultiplayer(PlayerStateMachineMultiplayer curContext)
    {
        _context = curContext;
    }

    public PlayerBaseStateMultiplayer Idle()
    {
        return new PlayerIdleStateMultiplayer(_context, this);
    }
    //public PlayerBaseState Jump()
    //{
    //    return new PlayerJumpState(_context, this);
    //}
    public PlayerBaseStateMultiplayer Walk()
    {
        return new PlayerWalkStateMultiplayer(_context, this);
    }
    //public PlayerBaseState Run()
    //{
    //    return new PlayerRunState(_context, this);
    //}
    //public PlayerBaseState Grounded()
    //{
    //    return new PlayerGroundedState(_context, this);
    //}
//
    //public PlayerBaseState Fall()
    //{
    //    return new PlayerFallState(_context, this);
    //}

    public PlayerGuardStateMultiplayer Guard()
    {
        return new PlayerGuardStateMultiplayer(_context, this);
    }

    public PlayerBaseStateMultiplayer Attack()
    {
        return new PlayerAttackStateMultiplayer(_context, this);
    }
}
