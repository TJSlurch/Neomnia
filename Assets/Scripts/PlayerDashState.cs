using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float dashSpeed = 60f;
    private float dashTime = 0.2f;                            

    // what happens when this state is switched to
    public override void EnterState(PlayerStateManager player)
    {
        player.triggerAnimator("SlideTrigger");
        player.playDash();
        player.setCanDash(false);
        player.StartCoroutine(player.ResetDash());

        // sets gravity to zero and launches player in the direction inputted 
        player.setGravity(0f);
        if (player.getInputX() != 0)
        {
            player.setVelocity(new Vector2(RoundAwayFromZero(player.getInputX()), 0).normalized * dashSpeed);
        }
        else
        {
            // if no input, dont't move in a direction
            player.setVelocity(new Vector2(0, 0).normalized * dashSpeed);
        }

        // starts the process of ending the dash
        player.StartCoroutine(endDash(dashTime, player));
    }

    // coroutine which resets the player's movement
    public IEnumerator endDash(float time, PlayerStateManager player)
    {
        // waits for the length of the dash
        yield return new WaitForSeconds(time);

        // resets gravity
        player.setGravity(5f);

        // detects if touching ground with input
        if (Mathf.Abs(player.getInputX()) > Mathf.Epsilon && player.getTouchingDown() && !(player.getCurrentColor() == "orange"))
        {
            // switches player to run state
            player.SwitchState(player.RunState);
        }
        else if(player.getTouchingDown() && (player.getCurrentColor() == "red"))
        {
            // switches to idle state
            player.SwitchState(player.IdleState);
        }
        else if(!(player.getCurrentColor() == "red"))
        {
            // switches player to falling state
            player.SwitchState(player.FallingState);
        }
    }

    // what happens every frame whilst this state is active
    public override void UpdateState(PlayerStateManager player)
    {
        // detects if wall grab button is pressed whilst dashing into a wall
        if (player.getTouchingLeft() && player.getX() < 0 && (Input.GetKey(KeyCode.Mouse0)) && (player.getCurrentColor() == "orange"))
        {
            player.SwitchState(player.WallGrabState);
        }
        // detects if wall grab button is pressed whilst dashing into a wall
        if (player.getTouchingRight() && player.getX() > 0 && (Input.GetKey(KeyCode.Mouse0)) && (player.getCurrentColor() == "orange"))
        {
            player.SwitchState(player.WallGrabState);
        }
    }

    // no movement occurs during idle state, so physics don't need updating
    public override void UpdatePhysics(PlayerStateManager player)
    {
        // flips sprite depending on direction
        if (player.getX() < 0)
        {
            player.setSpriteDirection(true);
        }
        else
        {
            player.setSpriteDirection(false);
        }
    }

    // function to round values away from zero
    private float RoundAwayFromZero(float value)
    {
        // if negative, rounds down
        if (value >= 0)
        {
            return Mathf.Ceil(value);
        }
        // if positive, round up
        else if (value <= 0)
        {
            return Mathf.Floor(value);
        }
        // if zero, return zero
        else
        {
            return 0f;
        }
    }

}
