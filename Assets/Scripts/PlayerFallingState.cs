using UnityEngine;
using System.Collections;

public class PlayerFallingState : PlayerBaseState
{
    private float fallMultiplier = 4f;
    private float weightlessGravity = 2f;

    // what happens when this state is switched to
    public override void EnterState(PlayerStateManager player)
    {
        player.triggerAnimator("FallTrigger");
        player.setGravity(8f);
    }

    // what happens every frame whilst this state is active
    public override void UpdateState(PlayerStateManager player)
    {
        // detects if horizontal input isn't zero
        if (Mathf.Abs(player.getX()) > Mathf.Epsilon && player.getTouchingDown())
        {
            // switches player to run state
            player.feet();
            player.SwitchState(player.RunState);
        }
        // detects if player is touching the ground
        else if (player.getTouchingDown())
        {
            // switches to idle state
            player.feet();
            player.SwitchState(player.IdleState);

        }

        // initiates a dash if arrow keys are pressed whilst a dash is possible
        if (player.getCanDash() && (Input.GetKey(KeyCode.Mouse0)) && (player.getCurrentColor() == "yellow"))
        {
            player.SwitchState(player.DashState);
        }

        // detects if wall grab button is pressed whilst next to a wall
        if ((player.getTouchingLeft() || player.getTouchingRight()) && (Input.GetKey(KeyCode.Mouse0)) && (player.getCurrentColor() == "orange"))
        {
            player.SwitchState(player.WallGrabState);
        }
    }

    // what happens every frame whilst this state is active
    public override void UpdatePhysics(PlayerStateManager player)
    {
        // at the peak of the player's jump, they experience reduced gravity
        if (player.getY() > -1)
        {
            player.setGravity(weightlessGravity);
        }
        // gravity is then increased to a maximum as the jump ends
        else
        {
            player.setGravity(fallMultiplier);
        }


        // detects horizontal input and uses it to change player velocity
        if (Mathf.Abs(player.getInputX()) > 0)
        {
            player.setVelocity(new Vector2(player.getInputX() * player.getSpeed(), player.getY()));
        }
        else
        {
            player.setVelocity(new Vector2(0, player.getY()));
        }

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

    // Wait seconds coroutine which prevents dashing from being enabled immediately
    public IEnumerator waitSeconds(float time, PlayerStateManager player)
    {
        yield return new WaitForSeconds(0.25f);
    }
}
