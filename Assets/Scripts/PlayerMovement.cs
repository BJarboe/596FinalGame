using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    //Assingables
    public Transform playerCam;
    public Transform orientation;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float walkSpeed = 1000;
    public float walkMaxSpeed = 10;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    public float crouchSpeed = 500;
    public float crouchMaxSpeed = 5;
    public Transform top;   // prevents the player from standing up if crouched underneath an object.
    private bool attemptingToStand = false;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Input
    float x, y;
    bool jumping, crouching, sliding;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Prevent Looking
    public bool canLook = true;

    //Sprinting
    public float sprintSpeed = 4500;  // Sprinting speed
    private bool isSprinting = false;
    public float sprintMaxSpeed = 20;
    public float stamina = 100f;     // Max stamina
    public float currentStamina;    // Current stamina
    public float staminaDrain = 20f; // Stamina drain per second while sprinting
    public float staminaRecovery = 10f; // Stamina recovery per second
    public float recoveryDelay = 2f; // Delay before stamina recovery starts after sprinting
    private float recoveryTimer = 0;
    public GameObject staminaObject;
    public Image StaminaBar;

    //Respawn point
    [SerializeField] private Vector3 respawnPoint;


    // Audio
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;  //Array of footstep sounds
    public float footstepSprintingDelay = 0.35f;
    public float footstepWalkingDelay = 0.50f; // Delay between footsteps
    private float lastFootstepTime = 0;  // Last time a footstep sound was played

    public AudioSource breathingAudioSource;
    public AudioClip staminaRecoverySound;

    public int deathCount;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina = stamina;
        deathCount = 0;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        MyInput();
        if (canLook)
        {
            Look();
        }

        // player can continue to sprint until stamina is out, then must recharge
        if (isSprinting)
        {
            if (currentStamina > 0)
            {
                currentStamina -= staminaDrain * Time.deltaTime;
                recoveryTimer = 0;
            }
            else
            {
                isSprinting = false;
                currentStamina = 0;
            }
        }
        else
        {
            if (recoveryTimer > recoveryDelay)
            {
                currentStamina = Mathf.Min(stamina, currentStamina + staminaRecovery * Time.deltaTime);
            }
            else
            {
                recoveryTimer += Time.deltaTime;
            }
        }
        
        // Used to display the stamina bar when the player is sprinting
        if(currentStamina < stamina)
        {
            staminaObject.SetActive(true);
            StaminaBar.fillAmount = currentStamina / stamina;
            if (!breathingAudioSource.isPlaying)
            {
                breathingAudioSource.loop = true;
                breathingAudioSource.Play();
            }
        }
        else
        {
            staminaObject.SetActive(false);
            breathingAudioSource.Stop();
        }
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isSprinting = !isSprinting;

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouch();
        }
            
        if (Input.GetKeyUp(KeyCode.LeftControl) || attemptingToStand)
            StopCrouch();
    }

    private void StartCrouch()
    {
        crouching = true;
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                sliding = true;
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        // check if there is space to stand up before un-crouching
        if (CanStandUp())
        {
            attemptingToStand = false;
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            crouching = false;
        }
        else
        {
            attemptingToStand = true;
        }
    }

    private void Movement()
    {
        // Disable sprinting if the player is not moving or is crouched
        if((rb.velocity.magnitude < 0.5 && isSprinting) || crouching)
        {
            isSprinting = false;
        }
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = (isSprinting) ? sprintMaxSpeed : walkMaxSpeed;

        // Adjust the maximum speed further if crouching
        if (crouching)
        {
            maxSpeed = crouchMaxSpeed;  // Apply the crouched max speed
        }

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (sliding && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        if (grounded && sliding) multiplierV = 0f;

        if (isSprinting)
        {
            //Apply forces to move player
            rb.AddForce(orientation.transform.forward * y * sprintSpeed * Time.deltaTime * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * x * sprintSpeed * Time.deltaTime * multiplier);
            PlayFootsteps(footstepSprintingDelay);
        }
        else if (crouching)
        {
            //Apply forces to move player
            rb.AddForce(orientation.transform.forward * y * crouchSpeed * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * x * crouchSpeed * multiplier);
        }
        else
        {
            //Apply forces to move player
            rb.AddForce(orientation.transform.forward * y * walkSpeed * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * x * walkSpeed * multiplier);
            PlayFootsteps(footstepWalkingDelay);
        }
    }

    private void PlayFootsteps(float footstepDelay)
    {
        // Check if the player is grounded, moving, and enough time has passed since the last footstep
        if (grounded && rb.velocity.magnitude > 0.5f && Time.time - lastFootstepTime > footstepDelay)
        {
            // Play a random footstep sound from the array
            AudioClip clip = footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)];
            footstepAudioSource.PlayOneShot(clip);
            lastFootstepTime = Time.time;  // Update the last footstep time
        }
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Set max speed
        float maxSpeed = (isSprinting) ? sprintMaxSpeed : walkMaxSpeed;

        //Slow down sliding
        if (sliding)
        {
            rb.AddForce(sprintSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            if (rb.velocity.magnitude < 0.5f) sliding = false;
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(sprintSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(sprintSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    // Checks if player can stand up without interference
    private bool CanStandUp()
    {
        float distanceToCheck = 2f;  // The distance to check for clearance
        RaycastHit hit;

        // Below line is used for debugging
        // Debug.DrawRay(top.position, Vector3.up * distanceToCheck, Color.red);

        // Cast a ray upwards from the "Top" GameObject
        if (Physics.Raycast(top.position, Vector3.up, out hit, distanceToCheck))
        {
            return false;  // There is something directly above within the distance
        }

        return true;  // Nothing is above, player can stand up
    }

    public void SetRespawnPoint(Vector3 respawnPoint)
    {
        Debug.Log("Set Respawn Point called");
        this.respawnPoint = respawnPoint;
    }

    public void Respawn()
    {
        Debug.Log("Respawn called");
        deathCount++;
        transform.position = respawnPoint;
    }
}