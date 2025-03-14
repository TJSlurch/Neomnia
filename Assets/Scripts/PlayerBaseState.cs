public abstract class PlayerBaseState
{
    // abstract methods which each state inherits and defines
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void UpdatePhysics(PlayerStateManager player);
}
