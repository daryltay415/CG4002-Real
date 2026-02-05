using System;
using Unity.Netcode;
using UnityEngine;

public abstract class PlayerBaseStateMultiplayer
{
    private bool isRootState = false;
    private PlayerStateMachineMultiplayer _ctx;
    private PlayerStateFactoryMultiplayer _factory;
    private PlayerBaseStateMultiplayer _currentSubState;
    private PlayerBaseStateMultiplayer _currentSuperState;
    
    protected bool IsRootState {set{ isRootState = value; }}
    protected PlayerStateMachineMultiplayer Ctx {get { return _ctx; }}
    protected PlayerStateFactoryMultiplayer Factory {get { return _factory; }} 


    public PlayerBaseStateMultiplayer(PlayerStateMachineMultiplayer curContext, PlayerStateFactoryMultiplayer playerStateFactory)
    {
        _ctx = curContext;
        _factory = playerStateFactory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }
    protected void SwitchState(PlayerBaseStateMultiplayer newState)
    {
        
        ExitState();
        newState.EnterState();
        _ctx.currentState = newState;

        if (isRootState)
        {
            _ctx.currentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
        
    }
    protected void SetSuperState(PlayerBaseStateMultiplayer newSuperState)
    {
        _currentSuperState = newSuperState;    
    }
    protected void SetSubState(PlayerBaseStateMultiplayer newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

}
