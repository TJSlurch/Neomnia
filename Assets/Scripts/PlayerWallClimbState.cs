using UnityEngine;

public class PlayerWallClimbState : PlayerBaseState
{
    private int previousClimbDirection;

    public override void EnterState(PlayerStateManager player)
    {
        player.triggerAnimator("ClimbTrigger");
        player.playClimb();
    }
    public override void UpdateState(PlayerStateManager player)
    {
        // when shift is released, a new state is entered
        if (!Input.GetKey(KeyCode.Mouse0) || !(player.getCurrentColor() == "orange"))
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
                player.setVelocity(new Vector2(0f, 0.4f) * player.getJumpForce());
                player.SwitchState(player.FallingState);
            }
        }

        // initiates a dash from wall climb
        if (player.getCanDash() && (Input.GetKey(KeyCode.Mouse0)) && (player.getCurrentColor() == "yellow"))
        {
            player.SwitchState(player.DashState);
        }

        // switches back to wall grab state if no vertical velocity is detected
        if (Mathf.Abs(player.getInputY()) < Mathf.Epsilon)
        {
            player.SwitchState(player.WallGrabState);
        }

        // detects if a wall jump request is active, or if top of wall is reached
        if (Input.GetKeyDown(KeyCode.Space) || (previousClimbDirection == 1 && !player.getTouchingLeft() && !player.getTouchingRight()))
        {
            // switches to jump state
            player.setJumpRequest(false);
            player.SwitchState(player.WallJumpState);
        }
    }
    public override void UpdatePhysics(PlayerStateManager player)
    {
        // if the player is touching the wall, update the value
        if (player.getTouchingLeft() || player.getTouchingRight())
        {
            // if player is climbing upward
            if (player.getInputY() > 0)
            {
                previousClimbDirection = 1;
            }
            // if player is climbing downward
            else if (player.getInputY() < 0)
            {
                previousClimbDirection = -1;
            }
        }

        // if touching wall, the player can move as usual
        // if last climbed direction was down and the player is inputting up, allow movement
        if ((player.getTouchingLeft() || player.getTouchingRight()) || (previousClimbDirection == -1 && player.getInputY() > 0))
        {
            player.setVelocity(new Vector2(0, player.getInputY() * player.getClimbSpeed()));
        }
        else
        {
            player.setVelocity(new Vector2(0, 0));
        }

        // flips the sprite depending on direction
        if (player.getTouchingLeft())
        {
            player.setSpriteDirection(true);
        }
        else if(player.getTouchingRight())
        {
            player.setSpriteDirection(false);
        }
    }
}
