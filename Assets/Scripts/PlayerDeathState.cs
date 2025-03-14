using System.Collections;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    // What happens when a death is triggered
    public override void EnterState(PlayerStateManager player)
    {
        player.setCanDie(false);

        player.triggerAnimator("DeathTrigger");
        player.playDeath();
        player.setVelocity(new Vector2(0, 0));
        player.setGravity(0f);

        // starts the respawn sequence
        player.StartCoroutine(respawn(player));
    }

    // coroutine which switches back to gameplay again
    public IEnumerator respawn(PlayerStateManager player)
    {
        // waits for the animation to play, then moves player
        yield return new WaitForSeconds(2f);
        player.setGravity(8f);
        player.setWhite();
        player.ResetToCheckpoint();
        player.setCanDie(true);

        // waits briefly before enabling gameplay again
        yield return new WaitForSeconds(0.1f);
        player.SwitchState(player.IdleState);
    }

    // unused inherited methods
    public override void UpdateState(PlayerStateManager player)
    {
    
    }

    public override void UpdatePhysics(PlayerStateManager player)
    {
       
    }
}
