using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    [SerializeField] protected float m_walkSpeed = 5;
    [SerializeField] protected float m_runMultFactor = 2;
    [SerializeField] protected float m_turnSpeed;
    [SerializeField] protected float m_jumpForce;

    [SerializeField] protected float m_cameraSpeed = 2f;
    protected Animator m_animator;
    [SerializeField] protected Rigidbody m_rigidBody;
    [SerializeField] protected AudioSource m_footstep;
    [SerializeField] protected int m_camera_zoom_max = -1;
    [SerializeField] protected int m_camera_zoom_min = -3;

    // Actual State of the player
    [Header("Player States (read only)")]
    [SerializeField] private bool m_isDead;
    [SerializeField] private bool m_isSaving;
    [SerializeField] private bool m_dialogueOn;
    [SerializeField] protected bool m_isPlayerCrouch = false;
    [SerializeField] protected bool m_isMoving = false;
    [SerializeField] protected float m_currentSpeed;

    // Inputs gestion
    [SerializeField] protected bool m_isShiftHold = false;
    
    // Cameras gestion
    [SerializeField]
    protected Transform m_cameraPivot;
    [SerializeField]
    protected EnergyBar energyBar;
    [SerializeField]
    protected LifeBar lifeBar;

    protected int currentCamera = 0;
    protected Vector3 initial_orientation;
    protected Vector3 lastMousePosition = Vector3.zero;

    protected bool m_wasGrounded;
    protected Vector3 m_currentDirection = Vector3.zero;

    protected float m_jumpTimeStamp = 0;
    protected float m_minJumpInterval = 1.50f;

    protected bool m_isGrounded;
    protected List<Collider> m_collisions = new List<Collider>();

    protected PlayerInteraction m_playerInteraction;

    // Use this for initialization
    protected virtual void Awake() {
        m_isDead = false;
        m_dialogueOn = false;
        m_isSaving = false;
        initial_orientation = Vector3.forward;
        m_footstep.Play();
        m_footstep.loop = true;
        m_footstep.Pause();

        m_currentSpeed = 0.0f;

        // Components
        m_animator = GetComponent<Animator>();
        m_playerInteraction = GetComponent<PlayerInteraction>();
    }

    protected virtual void Start()
    {
        RegisterInputs();
    }

    public virtual void RegisterInputs()
    {
        // Register Camera movements.
        InputManager.SubscribeMouseMovementsEvent("HorizontalCamera", MoveFollowingCamera);
        InputManager.SubscribeMouseMovementsEvent("VerticalCamera", MoveFollowingCamera);

        // Register Player Movements
        InputManager.SubscribeMouseMovementsChangedEvents(InputManager.ActionsLabels.Movement, new string[] { "Horizontal", "Vertical" }, new System.Action[] { MoveCharacter, StopMovements });
        InputManager.SubscribeButtonEvents(InputManager.ActionsLabels.Jump, "Jump", new System.Action[] { JumpingAndLanding, null, null });
        InputManager.SubscribeMouseMovementsChangedEvents(InputManager.ActionsLabels.Sprint, "Boost", new System.Action[] { MakePlayerSprint, StopSprint });

        // Register transformation inputs
        InputManager.SubscribeButtonEvents(InputManager.ActionsLabels.Transformation, "Transformation", new System.Action[] { FormsController.Instance.CloseTransformationWheel, null, FormsController.Instance.OpenTransformationWheel });

        // Register Fighting inputs
        InputManager.SubscribeMouseMovementsChangedEvent(InputManager.ActionsLabels.Attack, "Fire1", InputManager.EventTypeChanged.Changed, Attack);

        // Register inventory inputs
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.OpenInventory, "OpenInventory", InputManager.EventTypeButton.Down, InventoryManager.Instance.OpenOrCloseInventory);
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.Interact, "Interact", InputManager.EventTypeButton.Down, Interact);
    }   

    protected virtual void Update()
    {
        /// Check if some inputs has been received.
        InputManager.GetVirtualButtonInputs();
        //inputsManager.GetMouseMoveInput();
        InputManager.GetMouseMovementsChangedInput();

        m_wasGrounded = m_isGrounded;

        // Update the animator with player data
        m_animator.SetFloat("Speed", GetCurrentSpeed());
        m_animator.SetFloat("Health", lifeBar.GetCurrentSizeLifeBar());
    }

    protected void LateUpdate() {
        // Check if some mouse movements inputs has been received.
        InputManager.GetMouseMoveInput();
    }

    #region FollowingCamera
    /**
     * Callback to move the following Camera
     */
    private void MoveFollowingCamera()
    {
        float mouseX = Input.GetAxis("HorizontalCamera");
        float mouseY = Input.GetAxis("VerticalCamera");
        Vector3 dir = new Vector3(mouseX, mouseY, 0f);
        lastMousePosition = Input.mousePosition;
        if (!FormsController.Instance.GetComponent<FormsController>().IsTransformationWheelOpened() && !InventoryManager.Instance.bag_open && !m_dialogueOn)
        {
            UpdateFollowingCamera(dir.x, -dir.y);
        }
        Transform cameraTrans = m_cameraPivot.GetChild(currentCamera);
        float z;
        z = Mathf.Clamp(Input.mouseScrollDelta.y * 0.3f + cameraTrans.localPosition.z, m_camera_zoom_min, m_camera_zoom_max);
        cameraTrans.localPosition = new Vector3(cameraTrans.localPosition.x, cameraTrans.localPosition.y, z);
    }

    protected virtual void UpdateFollowingCamera(float deltaX, float deltaY) {
        m_cameraPivot.localPosition = this.transform.Find("UpAnchor").position;

        if (deltaX == 0 && deltaY == 0) return;

        Vector3 rotate = m_cameraPivot.localEulerAngles + new Vector3(deltaY * m_cameraSpeed, deltaX * m_cameraSpeed, 0);
        if (rotate.x >= 180f) rotate.x -= 360f;
        if (rotate.x > -20f && rotate.x < 90f) {
            m_cameraPivot.localEulerAngles = rotate;
        }
    }
    #endregion

    #region Others callbacks
    public void MakePlayerSprint()
    {
        if(m_isMoving)
        {
            m_currentSpeed = m_walkSpeed * (Input.GetAxis("Boost") * m_runMultFactor);
        }
    }

    public void StopSprint()
    {
        m_currentSpeed = m_isMoving ? m_walkSpeed : 0.0f;
    }

    public void SwicthIsPlayerCrouch()
    {
        m_isPlayerCrouch = !m_isPlayerCrouch;
    }

    public void Interact()
    {
        if(m_playerInteraction != null)
        {
            m_playerInteraction.GetClosestItem().Interact();
        }
    }

    protected virtual void Attack() { }
    #endregion

    protected void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    protected void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        } else {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0)
            {
                m_isGrounded = false;
            }
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0)
        {
            m_isGrounded = false;
        }
    }

    protected virtual void StopMovements()
    {
        if (Input.GetAxis("Vertical").Equals(0.0f) && Input.GetAxis("Horizontal").Equals(0.0f))
        {
            m_currentSpeed = 0.0f;
            m_isMoving = false;
            m_footstep.Pause();
        }
    }

    protected void MoveCharacter() {
       if (!m_isDead && !m_dialogueOn && !m_isSaving) {
            m_isMoving = true;
            if (m_currentSpeed.Equals(0.0f))
            {
                m_currentSpeed = m_walkSpeed;
            }

            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            // Get the camera
            Transform cameraTrans = m_cameraPivot.GetChild(currentCamera);
            Vector3 projected_forward_camera = Vector3.ProjectOnPlane(cameraTrans.forward, new Vector3(0, 1, 0));

            // Calculated the movement vector
            float angle = SignedAngleBetween(initial_orientation, projected_forward_camera, Vector3.up);
            Vector3 NextDir = new Vector3(h, 0, v);
            NextDir = Quaternion.Euler(0f, angle, 0f) * NextDir;

            // Control Judy's movement
            Move(NextDir.normalized, h, v);
        }
    }

    protected virtual void Move(Vector3 nextDir, float h, float v) {
        WalkOrRun();

        if (!nextDir.Equals(Vector3.zero))
        {
            transform.rotation = Quaternion.LookRotation(nextDir);
        }

        transform.position += nextDir * GetCurrentSpeed() * Time.deltaTime;
    }

    protected virtual void JumpingAndLanding() { }

    public void WalkOrRun()
    {
        m_footstep.UnPause();
        if (m_isShiftHold)
        {
            // Run
            if (energyBar.canRun && energyBar.GetCurrentEnergy() > 0f)
            {
                m_footstep.pitch = 1.7f;
            }
        }
        else
        {
            // Walk
            m_footstep.pitch = 1f;
        }
    }

    protected virtual void GetInputs(Vector3 NextDir, float h, float v) { }

    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n) {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] (not used but included here for completeness)
        float angle360 = (signed_angle + 180) % 360;

        return signed_angle;
    }

    public float GetCurrentSpeed() {
        return m_currentSpeed;
        //return (m_isRunning ? m_walkSpeed * m_runMultFactor : (m_isMoving ? m_walkSpeed : 0.0f));
    }
    
    public void setDeath(bool isDead) {
        m_isDead = isDead;
    }

    public bool isDeath()
    {
        return m_isDead;
    }

    public void setDialogue(bool isDialogueOn) {
        m_dialogueOn = isDialogueOn;
    }

    public void setIsSaving(bool isSaving) {
        m_isSaving = isSaving;
    }

    void OnDrawGizmosSelected() {
        //Camera camera = GameObject.Find("AimedCamera").GetComponent<Camera>();
        //Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(p, 0.1F);
    }
		

}
