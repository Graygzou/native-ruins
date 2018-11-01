using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] protected float walkSpeed = 5;
    [SerializeField] protected float runMultFactor = 2;
    [SerializeField] protected float turnSpeed;
    // Should be readonly
    [SerializeField] protected float currentSpeed;

    [Header("Jump")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float minJumpInterval = 1.50f;

    [Header("Camera")]
    [SerializeField] protected float cameraSpeed = 2f;
    [SerializeField] protected Rigidbody rigidBody;
    
    [SerializeField] protected int camera_zoom_max = -1;
    [SerializeField] protected int camera_zoom_min = -3;

    [SerializeField] protected AudioSource footstep;

    // Inputs gestion
    [SerializeField] protected bool _isShiftHold = false;
    public bool IsShiftHold { get { return _isShiftHold; } }
    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected bool isPlayerCrouch = false;

    protected Animator animator;
    protected Camera cameraPivot;

    protected int currentCamera = 0;
    protected Vector3 initial_orientation;
    protected Vector3 lastMousePosition = Vector3.zero;

    protected bool wasGrounded;
    protected Vector3 currentDirection = Vector3.zero;

    protected float jumpTimeStamp = 0;
    

    protected bool isGrounded;
    protected List<Collider> collisions = new List<Collider>();

    protected PlayerInteraction playerInteraction;

    protected virtual void Awake()
    {
        cameraPivot = Camera.main;

        initial_orientation = Vector3.forward;
        /*
        footstep.Play();
        footstep.loop = true;
        footstep.Pause();*/

        currentSpeed = 0.0f;

        // Components
        animator = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    protected virtual void Start()
    {
        RegisterInputs();
    }

    #region Inputs register
    public virtual void RegisterInputs()
    {
        // Register all the events.
        RegisterCameraMovementsInputs();
        RegisterPlayerMovementsInputs();
        RegisterTransformationInputs();
        RegisterFightingInputs();
        RegisterInventoryInputs();
    }

    public virtual void RegisterCameraMovementsInputs()
    {
        InputManager.SubscribeMouseMovementsEvent("HorizontalCamera", MoveFollowingCamera);
        InputManager.SubscribeMouseMovementsEvent("VerticalCamera", MoveFollowingCamera);
    }

    public virtual void RegisterPlayerMovementsInputs()
    {
        InputManager.SubscribeMouseMovementsChangedEvents(InputManager.ActionsLabels.Movement, new string[] { "Horizontal", "Vertical" }, new System.Action[] { MoveCharacter, StopMovements });
        InputManager.SubscribeButtonEvents(InputManager.ActionsLabels.Jump, "Jump", new System.Action[] { null, JumpingAndLanding, null });
        InputManager.SubscribeMouseMovementsChangedEvents(InputManager.ActionsLabels.Sprint, "Boost", new System.Action[] { MakePlayerSprint, StopSprint });
    }

    public virtual void RegisterTransformationInputs()
    {
        // Register transformation inputs
        InputManager.SubscribeButtonEvents(InputManager.ActionsLabels.Transformation, "Transformation", new System.Action[] { FormsController.Instance.CloseTransformationWheel, null, FormsController.Instance.OpenTransformationWheel });
    }

    public virtual void RegisterFightingInputs()
    {
        InputManager.SubscribeMouseMovementsChangedEvent(InputManager.ActionsLabels.Attack, "Fire1", InputManager.EventTypeChanged.Changed, Attack);
    }

    public virtual void RegisterInventoryInputs()
    {
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.OpenInventory, "OpenInventory", InputManager.EventTypeButton.Down, InventoryManager.Instance.OpenOrCloseInventory);
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.Interact, "Interact", InputManager.EventTypeButton.Down, Interact);
    }
    #endregion

    #region Inputs deregister
    public virtual void DeRegisterInputs()
    {
        // Unregister all the events.
        DeregisterCameraMovementsInputs();
        DeregisterPlayerMovementsInputs();
        DeregisterTransformationInputs();
        DeregisterFightingInputs();
        DeregisterInventoryInputs();
    }

    public virtual void DeregisterCameraMovementsInputs()
    {
        InputManager.UnsubscribeMouseMovementsEvent("HorizontalCamera");
        InputManager.UnsubscribeMouseMovementsEvent("VerticalCamera");
    }

    public virtual void DeregisterPlayerMovementsInputs()
    {
        InputManager.UnsubscribeMouseMovementsChangedEvent(InputManager.ActionsLabels.Movement);
        InputManager.UnsubscribeButtonEvent(InputManager.ActionsLabels.Jump);
        InputManager.UnsubscribeMouseMovementsChangedEvent(InputManager.ActionsLabels.Sprint);
    }

    public virtual void DeregisterTransformationInputs()
    {
        InputManager.UnsubscribeButtonEvent(InputManager.ActionsLabels.Transformation);
    }

    public virtual void DeregisterFightingInputs()
    {
        InputManager.UnsubscribeMouseMovementsChangedEvent(InputManager.ActionsLabels.Attack);
    }

    public virtual void DeregisterInventoryInputs()
    {
        InputManager.UnsubscribeButtonEvent(InputManager.ActionsLabels.OpenInventory);
        InputManager.UnsubscribeButtonEvent(InputManager.ActionsLabels.Interact);
    }
    #endregion

    protected virtual void Update()
    {
        /// Check if some inputs has been received.
        InputManager.GetVirtualButtonInputs();
        //inputsManager.GetMouseMoveInput();
        InputManager.GetMouseMovementsChangedInput();

        wasGrounded = isGrounded;

        // Update the animator with player data
        animator.SetFloat("Speed", GetCurrentSpeed());
    }

    protected void LateUpdate() {
        // Check if some mouse movements inputs has been received.
        InputManager.GetMouseMoveInput();
    }

    #region FollowingCamera
    /**
     * Callback to move the following Camera
     */
    public void MoveFollowingCamera()
    {
        float mouseX = Input.GetAxis("HorizontalCamera");
        float mouseY = Input.GetAxis("VerticalCamera");
        Vector3 dir = new Vector3(mouseX, mouseY, 0f);
        lastMousePosition = Input.mousePosition;
        if (!FormsController.Instance.GetComponent<FormsController>().IsTransformationWheelOpened() && !InventoryManager.Instance.bag_open)
        {
            UpdateFollowingCamera(dir.x, -dir.y);
        }
        Transform cameraTrans = cameraPivot.transform;
        float z;
        z = Mathf.Clamp(Input.mouseScrollDelta.y * 0.3f + cameraTrans.localPosition.z, camera_zoom_min, camera_zoom_max);
        cameraTrans.localPosition = new Vector3(cameraTrans.localPosition.x, cameraTrans.localPosition.y, z);
    }

    protected virtual void UpdateFollowingCamera(float deltaX, float deltaY) {
        cameraPivot.transform.parent.localPosition = transform.Find("UpAnchor").position;

        if (deltaX == 0 && deltaY == 0) return;

        Vector3 rotate = cameraPivot.transform.parent.localEulerAngles + new Vector3(deltaY * cameraSpeed, deltaX * cameraSpeed, 0);
        if (rotate.x >= 180f) rotate.x -= 360f;
        if (rotate.x > -20f && rotate.x < 90f) {
            cameraPivot.transform.parent.localEulerAngles = rotate;
        }
    }
    #endregion

    #region Others callbacks
    public void MakePlayerSprint()
    {
        if(isMoving)
        {
            currentSpeed = walkSpeed * (Input.GetAxis("Boost") * runMultFactor);
            footstep.pitch = 1.7f;
        }
    }

    public void StopSprint()
    {
        currentSpeed = isMoving ? walkSpeed : 0.0f;
        footstep.pitch = isMoving ? 1.0f : 0.0f;
    }

    public void SwicthIsPlayerCrouch()
    {
        isPlayerCrouch = !isPlayerCrouch;
    }

    public void Interact()
    {
        if(playerInteraction != null && playerInteraction.GetClosestItem() != null)
        {
            playerInteraction.GetClosestItem().Interact();
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
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGrounded = true;
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
            isGrounded = true;
            if (!collisions.Contains(collision.collider))
            {
                collisions.Add(collision.collider);
            }
        } else {
            if (collisions.Contains(collision.collider))
            {
                collisions.Remove(collision.collider);
            }
            if (collisions.Count == 0)
            {
                isGrounded = false;
            }
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }
        if (collisions.Count == 0)
        {
            isGrounded = false;
        }
    }

    protected virtual void StopMovements()
    {
        if (Input.GetAxis("Vertical").Equals(0.0f) && Input.GetAxis("Horizontal").Equals(0.0f))
        {
            currentSpeed = 0.0f;
            isMoving = false;
            footstep.Pause();
        }
    }

    protected void MoveCharacter() {
        isMoving = true;
        if (currentSpeed.Equals(0.0f))
        {
            currentSpeed = walkSpeed;
        }

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // Get the camera
        Vector3 projected_forward_camera = Vector3.ProjectOnPlane(cameraPivot.transform.forward, new Vector3(0, 1, 0));

        // Calculated the movement vector
        float angle = SignedAngleBetween(initial_orientation, projected_forward_camera, Vector3.up);
        Vector3 NextDir = new Vector3(h, 0, v);
        NextDir = Quaternion.Euler(0f, angle, 0f) * NextDir;

        // Control Judy's movement
        Move(NextDir.normalized, h, v);
    }

    protected virtual void Move(Vector3 nextDir, float h, float v) {
        if (!nextDir.Equals(Vector3.zero))
        {
            transform.rotation = Quaternion.LookRotation(nextDir);
        }
        transform.position += nextDir * GetCurrentSpeed() * Time.deltaTime;
    }

    protected virtual void JumpingAndLanding() { }

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
        return currentSpeed;
        //return (m_isRunning ? m_walkSpeed * m_runMultFactor : (m_isMoving ? m_walkSpeed : 0.0f));
    }

    void OnDrawGizmosSelected() {
        //Camera camera = GameObject.Find("AimedCamera").GetComponent<Camera>();
        //Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(p, 0.1F);
    }
		

}
