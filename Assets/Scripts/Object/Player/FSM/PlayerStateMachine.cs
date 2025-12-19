using UnityEngine;

public class PlayerStateMachine
{
    public StateBase CurrentState { get; private set; }
    public void Initialize(StateBase startState)
    {
        CurrentState.Exit();
        CurrentState = startState;
        CurrentState.Enter();
    }
    public void Tick()
    {
        CurrentState.Tick();
    }
}
