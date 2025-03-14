using System.Collections;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // declare variables and declare initial values
    private float speed = 15f;
    private float jumpForce = 20f;
    private float climbSpeed = 12f;
    private bool jumpRequest = false;
    private bool canDash = true;
    private bool canEnableDash = true;
    private bool canDie = true;
    private string currentColor = "white";

    private float respawnX = 0f;
    private float respawnY = 0f;

    // define the components which are to be used
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private BoxCollider2D rbRight;
    private BoxCollider2D rbLeft;
    private BoxCollider2D rbDown;
    private TrailRenderer trail;
    private Transform runner;
    [SerializeField] private GameObject RightCollision;
    [SerializeField] private GameObject LeftCollision;
    [SerializeField] private GameObject DownCollision;
    [SerializeField] private ParticleSystem feetParticles;
    [SerializeField] private ParticleSystem runParticles;
    [SerializeField] private GameObject trailObject;
    [SerializeField] private GameObject runningObject;

    // declare the sound effects
    [SerializeField] private AudioSource runAS;
    [SerializeField] private AudioSource jumpAS;
    [SerializeField] private AudioSource dashAS;
    [SerializeField] private AudioSource climbAS;
    [SerializeField] private AudioSource deathAS;

    // creating an instance of each state
    private PlayerBaseState currentState;
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerCrouchState CrouchState = new PlayerCrouchState();
    public PlayerRunState RunState = new PlayerRunState();
    public PlayerJumpState JumpState = new PlayerJumpState();
    public PlayerFallingState FallingState = new PlayerFallingState();
    public PlayerDashState DashState = new PlayerDashState();
    public PlayerWallGrabState WallGrabState = new PlayerWallGrabState();
    public PlayerWallClimbState WallClimbState = new PlayerWallClimbState();
    public PlayerWallJumpState WallJumpState = new PlayerWallJumpState();
    public PlayerDeathState DeathState = new PlayerDeathState();

    void Start()
    {
        // using getComponent to retrieve the components
        trail = trailObject.GetComponent<TrailRenderer>();
        runner = runningObject.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rbRight = RightCollision.GetComponent<BoxCollider2D>();
        rbLeft = LeftCollision.GetComponent<BoxCollider2D>();
        rbDown = DownCollision.GetComponent<BoxCollider2D>();

        // setting the state which the player starts in
        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        // updating the UpdateState method for the current state's script
        currentState.UpdateState(this);

        // detecting a space bar input
        if(Input.GetKey(KeyCode.Space))
        {
            jumpRequest = true;
            StartCoroutine(ResetJump());
        }
    }

    // Fixed Update is more reliable for movement related updates
    private void FixedUpdate()
    {
        // updating the UpdateState method for the current state's script
        currentState.UpdatePhysics(this);
    }

    // coroutine sets the jump request boolean to false after 0.3s
    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.3f);
        jumpRequest = false;
    }

    public IEnumerator ResetDash()
    {
        canEnableDash = false;
        yield return new WaitForSeconds(0.4f);
        canEnableDash = true;
    }
    

    // subroutine which changes the current state
    public void SwitchState(PlayerBaseState state)
    {
        // stop sound effects
        climbAS.Stop();
        runAS.Stop();
        currentState = state;
        state.EnterState(this);
    }

    // called when the player needs to reset position
    public void ResetToCheckpoint()
    {
        // sets to the checkpoint position from PlayerLocationTracker
        transform.position = new Vector2(respawnX, respawnY);
    }


    // accessor methods for the private attributes
    public float getInputX()
    {
        return Input.GetAxis("HorizontalWASD");
    }
    public float getInputY()
    {
        return Input.GetAxis("VerticalWASD");
    }
    public float getSpeed()
    {
        return speed;
    }
    public float getJumpForce()
    {
        return jumpForce;
    }
    public float getClimbSpeed()
    {
        return climbSpeed;
    }
    public float getX()
    {
        return rb.linearVelocity.x;
    }
    public float getY()
    {
        return rb.linearVelocity.y;
    }
    public bool getJumpRequest()
    {
        return jumpRequest;
    }
    public bool getCanDash()
    {
        return canDash;
    }
    public bool getTouchingRight()
    {
        return rbRight.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
    public bool getTouchingLeft()
    {
        return rbLeft.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
    public bool getTouchingDown()
    {
        return rbDown.IsTouchingLayers(LayerMask.GetMask("Ground")) || rbDown.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
    public string getCurrentColor()
    {
        return currentColor;
    }

    // mutator methods for the private attributes
    public void setVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }
    public void setJumpRequest(bool value)
    {
        jumpRequest = value;
    }
    public void setCanDash(bool value)
    {
        canDash = value;
    }
    public void setCanDie(bool value)
    {
        canDie = value;
    }
    public void triggerAnimator(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    public void setSpriteDirection(bool direction)
    {
        sprite.flipX = direction;

        if(direction){
            runner.eulerAngles = new Vector3(-40f, 90f, 0f);
        }
        else{
            runner.eulerAngles = new Vector3(-140f, 90f, 0f);
        }
    }
    public void setGravity(float multiplier)
    {
        rb.gravityScale = multiplier;
    }
    public void setRespawnX(float pos)
    {
        respawnX = pos;
    }
    public void setRespawnY(float pos)
    {
        respawnY = pos;
    }
    public void feet()
    {
        feetParticles.Play();
    }
    public void runStart()
    {
        runParticles.Play();
    }
    public void runStop()
    {
        runParticles.Stop();
    }
    public void setTrailColor(Color color)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        trail.colorGradient = gradient;
    }


    // player touches red, so reset stats, update colour and switch to death state
    public void setRed()
    {
        sprite.color = new Color(1f, 0f, 0f);
        var particleMain = feetParticles.main;
        particleMain.startColor = new Color(1f, 0f, 0f);
        var particle2Main = runParticles.main;
        particle2Main.startColor = new Color(1f, 0f, 0f);
        setTrailColor( new Color(1f, 0f, 0f) );
        currentColor = "red";
        canDash = false;
        jumpForce = 20f;
        speed = 15f;

        if(canDie)
        {
            SwitchState(DeathState);
        } 
    }

    // player touches orange, so reset stats and update current colour
    public void setOrange()
    {
        sprite.color = new Color(0.8868f, 0.4394f, 0f);
        var particleMain = feetParticles.main;
        particleMain.startColor = new Color(0.8868f, 0.4394f, 0f);
        var particle2Main = runParticles.main;
        particle2Main.startColor = new Color(0.8868f, 0.4394f, 0f);
        setTrailColor( new Color(0.8868f, 0.4394f, 0f) );
        currentColor = "orange";
        canDash = false;
        jumpForce = 20f;
        speed = 15f;
    }

    // player touches yellow, so reset stats, enable dash and update current colour
    public void setYellow()
    {
        sprite.color = new Color(1f, 0.9058f, 0f);
        var particleMain = feetParticles.main;
        particleMain.startColor = new Color(1f, 0.9058f, 0f);
        var particle2Main = runParticles.main;
        particle2Main.startColor = new Color(1f, 0.9058f, 0f);
        setTrailColor( new Color(1f, 0.9058f, 0f) );
        currentColor = "yellow";

        if (canEnableDash)
        {
            canDash = true;
        }
        jumpForce = 20f;
        speed = 15f;
    }

    // player touches green, so reset speed, increase jumpforce and update current colour
    public void setGreen()
    {
        sprite.color = new Color(0.0862f, 1f, 0f);
        var particleMain = feetParticles.main;
        particleMain.startColor = new Color(0.0862f, 1f, 0f);
        var particle2Main = runParticles.main;
        particle2Main.startColor = new Color(0.0862f, 1f, 0f);
        setTrailColor( new Color(0.0862f, 1f, 0f) );
        currentColor = "green";
        canDash = false;
        jumpForce = 35f;
        speed = 15f;
    }

    // player touches blue, so reset jumpforce, increase speed and update current colour
    public void setBlue()
    {
        sprite.color = new Color(0f, 1f, 0.9714f);
        var particleMain = feetParticles.main;
        particleMain.startColor = new Color(0f, 1f, 0.9714f);
        var particle2Main = runParticles.main;
        particle2Main.startColor = new Color(0f, 1f, 0.9714f);
        setTrailColor( new Color(0f, 1f, 0.9714f) );
        currentColor = "blue";
        canDash = false;
        jumpForce = 20f;
        speed = 25f;
    }

    // player touches yellow, so reset stats and update current colour
    public void setPink()
    {
        sprite.color = new Color(1f, 0f, 0.6955f);
        var particleMain = feetParticles.main;
        particleMain.startColor = new Color(1f, 0f, 0.6955f);
        var particle2Main = runParticles.main;
        particle2Main.startColor = new Color(1f, 0f, 0.6955f);
        setTrailColor( new Color(1f, 0f, 0.6955f) );
        currentColor = "pink";
        canDash = false;
        jumpForce = 20f;
        speed = 15f;
    }

    // player touches yellow, so reset stats and update current colour
    public void setWhite()
    {
        sprite.color = new Color(1f, 1f, 1f);
        var particleMain = feetParticles.main;
        particleMain.startColor = new Color(1f, 1f, 1f);
        var particle2Main = runParticles.main;
        particle2Main.startColor = new Color(1f, 1f, 1f);
        setTrailColor( new Color(1f, 1f, 1f) );
        currentColor = "white";
        canDash = false;
        jumpForce = 20f;
        speed = 15f;
    }
    
    // public methods to play sound effects
    public void playRun()
    {
        runAS.Play();
    }
    public void playJump()
    {
        jumpAS.Play();
    }
    public void playDash()
    {
        dashAS.Play();
    }
    public void playClimb()
    {
        climbAS.Play();
    }
    public void playDeath()
    {
        deathAS.Play();
    }
}

