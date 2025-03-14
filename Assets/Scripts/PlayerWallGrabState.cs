using UnityEngine;

public class PlayerWallGrabState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.triggerAnimator("HoldTrigger");

        // set gravity to zero to prevent sliding
        player.setGravity(0f);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // when shift is released, a new state is entered
        if(!Input.GetKey(KeyCode.Mouse0) || !(player.getCurrentColor() == "orange"))
        {
            // choosing which state to enter
            if (Mathf.Abs(player.getInputX()) > Mathf.Epsilon && player.getTouchingDown())
            {
                // switches player to run state
                player.SwitchState(player.RunState);
            }
            else if (player.getTouchingDown())
            {
                // switches to idle state
                player.SwitchState(player.IdleState);
            }
            else
            {
                // switches player to falling state
                player.SwitchState(player.FallingState);
            }
        }

        // initiates a dash from wall hold
        if (player.getCanDash() && (Input.GetKey(KeyCode.Mouse0)) && (player.getCurrentColor() == "yellow"))
        {
            player.SwitchState(player.DashState);
        }

        // switches to wall climb state if vertical input is detected
        if(Mathf.Abs(player.getInputY()) > Mathf.Epsilon)
        {
            player.SwitchState(player.WallClimbState);
        }

        // detects if a jump request is active
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // switches to wall jump state
            player.setJumpRequest(false);
            player.SwitchState(player.WallJumpState);
        }
    }

    public override void UpdatePhysics(PlayerStateManager player)
    {
        // maintains the player at the same position
        player.setVelocity(new Vector2(0, 0));

        if (player.getTouchingLeft())
        {
            player.setSpriteDirection(true);
        }
        else if (player.getTouchingRight())
        {
            player.setSpriteDirection(false);
        }
    }
}
