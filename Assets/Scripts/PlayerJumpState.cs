using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float lowJumpMultiplier = 11f;
    private float regularGravity = 5f;
    private float weightlessGravity = 2f;

    // what happens when this state is switched to
    public override void EnterState(PlayerStateManager player)
    {
        player.triggerAnimator("JumpTrigger");
        player.playJump();

        player.setJumpRequest(false);

        // initiates a jump as soon as this state is switched to
        player.setVelocity(new Vector2(player.getX(), player.getJumpForce()));
    }

    // what happens every frame whilst this state is active
    public override void UpdateState(PlayerStateManager player)
    {
        // detects if vertical velocity is negative (downwards)
        if (player.getY() < -0.1)
        {
            // switches player to falling state
            player.SwitchState(player.FallingState);
        }

        // initiates a dash if arrow keys are pressed whilst a dash is possible
        if (player.getCanDash() && (Input.GetKey(KeyCode.Mouse0)) && (player.getCurrentColor() == "yellow"))
        {
            player.SwitchState(player.DashState);
        }

        // detects if wall grab button is pressed whilst next to a wall
        if ((player.getTouchingLeft() || player.getTouchingRight()) && Input.GetKey(KeyCode.Mouse0) && (player.getCurrentColor() == "orange"))
        {
            player.SwitchState(player.WallGrabState);
        }
    }

    // what happens every frame whilst this state is active
    public override void UpdatePhysics(PlayerStateManager player)
    {
        // at the peak of the player's jump, they experience reduced gravity
        if (player.getY() < 1)
        {
            player.setGravity(weightlessGravity);
        }
        // if space is held, gravity remains constant for a normal jump
        else if (Input.GetKey(KeyCode.Space))
        {
            player.setGravity(regularGravity);
        }
        // if space is released, gravity is increased for a shorter jump
        else
        {
            player.setGravity(lowJumpMultiplier);
        }

        // detects horizontal input and uses it to change player velocity
        player.setVelocity(new Vector2(player.getInputX() * player.getSpeed(), player.getY()));

        // flips sprite depending on direction
        if (player.getX() < 0)
        {
            player.setSpriteDirection(true);
        }
        else if (player.getX() > 0)
        {
            player.setSpriteDirection(false);
        }
    }
}
