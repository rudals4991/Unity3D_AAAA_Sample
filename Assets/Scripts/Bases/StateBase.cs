using UnityEngine;

public abstract class StateBase
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    public StateBase(Player player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
}
