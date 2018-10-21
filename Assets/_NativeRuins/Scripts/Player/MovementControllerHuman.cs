using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class MovementControllerHuman : MovementController {

    //[SerializeField] private ActionsNew actions = null;
    public float mouseSmoothness = 5.0f;
    private Camera aimCamera;
    private bool isAiming;
    private bool isReloading;
    private Vector3 offset;
    public Rigidbody m_Arrow;                               // Prefab of the arrow.

    private Vector3 targetDirection;                        // Direction toward the target (mouse position in the World)

    private Quaternion initUpperBody;
    private Quaternion initEpaule;
    private Quaternion initMain;
    private Quaternion initMainD;
    private Quaternion initFleche;

    private GameObject bow;
    bool hasArrowLeft;

    protected override void Awake() {
        base.Awake();
        // Set the attribute to the desire amount
        //m_moveSpeed = 10;
        //m_turnSpeed = 5;
        //m_jumpForce = 5;
        isAiming = false;
        isReloading = false;
        hasArrowLeft = false;

        // Disable the aim camera
        aimCamera = GameObject.Find("SportyGirl/AimedCamera").GetComponent<Camera>();
        offset = aimCamera.transform.position - GameObject.FindWithTag("Player").transform.position;
        aimCamera.enabled = false;

        // Get all the audio clips of the bow
        bow = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D");

        // Keep initial rotation for the animation
        initUpperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2").transform.rotation;
        initEpaule = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone").transform.rotation;
        initMain = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3").transform.rotation;
        initMainD = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3").transform.rotation;
        initFleche = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").transform.rotation;
    }

    override public void RegisterInputs()
    {
        base.RegisterInputs();

        // Special movements inputs
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.Crouch, "Crouch", InputManager.EventTypeButton.Down, SwicthIsPlayerCrouch);
        
        // Register the fighting inputs
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.Aiming, "Aiming", InputManager.EventTypeButton.Down, ChangedPlayerAimedState);
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.PutAwayWeapon, "PutAwayWeapon", InputManager.EventTypeButton.Down, InventoryManager.Instance.PutWeaponInBag);
        
        // Register others possible interactions
        /* TODO Later
        if (Input.GetKeyDown(KeyCode.I))
        {
            actions.OpenChest();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            actions.Celebrate();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            actions.DanceHipHop();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            actions.DanceSamba();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            actions.LookAround();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            bow.strech() 
        }*/
    }

    // In this case, the vector3 NextDir is not used.
    override protected void JumpingAndLanding()
    {
        bool jumpCooldownOver;
        if (rigidBody.velocity.magnitude <= 1.0)
        {
            jumpCooldownOver = (Time.time - jumpTimeStamp) >= minJumpInterval;
        }
        else
        {
            jumpCooldownOver = (Time.time - jumpTimeStamp) >= 1.0;
        }

        if (jumpCooldownOver && isGrounded)
        {
            jumpTimeStamp = Time.time;
            animator.Play("JumpMecanics", animator.GetLayerIndex("Movement Layer"));
            rigidBody.AddForce(Vector3.up * 45, ForceMode.Impulse);
        }
    }

    override protected void UpdateFollowingCamera(float deltaX, float deltaY) {
        if (!isAiming) {
            // Regular mode
            base.UpdateFollowingCamera(deltaX, deltaY);
        } else {
            
        }
    }

    override protected void Move(Vector3 nextDir, float h, float v)
    {
        if (!isAiming) {
            // Regular state
            base.Move(nextDir, h, v);
        } else {
            /*
            // Aiming State
            transform.position += (GameObject.FindWithTag("Player").transform.forward * GetCurrentSpeed()) * v * Time.deltaTime;
            transform.position += (GameObject.FindWithTag("Player").transform.right * GetCurrentSpeed()) * h * Time.deltaTime;
            aimCamera.transform.localPosition = GameObject.Find("OriginCamera").transform.localPosition;
            
            //actions.MoveWithBow(h, v);
            m_animator.SetFloat("VelX", h);
            m_animator.SetFloat("VelY", v);*/
        }
    }

    #region Callbacks

    override protected void Attack()
    {
        if (isGrounded)
        {
            if (InventoryManager.Instance.isBowEquiped && isAiming && !isReloading && hasArrowLeft)
            {
                Debug.Log("Attack");
                FireArrow();
            }
            else if (InventoryManager.Instance.isTorchEquiped)
            {
                Debug.Log("HitStick");
                HitWithStick();
            }
        }
    }

    public void ChangedPlayerAimedState()
    {
        if (isGrounded && InventoryManager.Instance.isBowEquiped)
        {
            if (!isAiming)
            {
                // Should change the subscribe inputs for crosshair

                // Equip the bow
                isAiming = true;

                StartCoroutine("DrawArrow");

                // change the camera for the aim
                aimCamera.enabled = true;
                cameraPivot.enabled = false;

                // Activate the crosshair
                MenuManager.Instance.EnableCrossHair();
                /*
                GameObject crosshair = GameObject.Find("Arrow_aim").gameObject;
                Transform largeurCrossHair = crosshair.transform.GetChild(0);
                Transform hauteurCrossHair = crosshair.transform.GetChild(1);
                largeurCrossHair.gameObject.SetActive(true);
                hauteurCrossHair.gameObject.SetActive(true);

                // Set his first position
                largeurCrossHair.transform.position = Input.mousePosition + initLargeurCrossHair;
                hauteurCrossHair.transform.position = Input.mousePosition + initHauteurCrossHair;*/
            }
            else
            {
                // Should comeback on the subscribe inputs for following camera


                // unbend the string
                GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D/BowRig_tex/Root/String").GetComponent<BowString>().Release();

                // Desactivate the crosshair
                GameObject crosshair = GameObject.Find("Arrow_aim").gameObject;
                Transform largeurCrossHair = crosshair.transform.GetChild(0);
                Transform hauteurCrossHair = crosshair.transform.GetChild(1);
                largeurCrossHair.gameObject.SetActive(false);
                hauteurCrossHair.gameObject.SetActive(false);

                // change the camera for the aim
                //SwitchToRegular();
                aimCamera.enabled = false;
                cameraPivot.GetComponent<Camera>().enabled = true;

                GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(false);

                // Remove the bow
                isAiming = false;
            }
            animator.SetBool("Aiming", isAiming);
        }
    }

    public void UpdatePlayerAimed()
    {
        GameObject upperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2");
        GameObject epaule = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone");
        GameObject main = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3");
        GameObject mainD = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3");
        GameObject fleche = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D");
        GameObject head = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigNeck/RigHead");
        GameObject bow = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D");

        //// Method 2
        //Vector3 rotate = upperBody.transform.eulerAngles + new Vector3(-Input.mousePosition.y * mouseSmoothness, Input.mousePosition.x * mouseSmoothness, 0f);
        //upperBody.transform.eulerAngles = rotate;

        // Update the crosshair (ADD Subscribe)
        /*
        largeurCrossHair.transform.position = Input.mousePosition + initLargeurCrossHair;
        hauteurCrossHair.transform.position = Input.mousePosition + initHauteurCrossHair;*/ 

        // Point the body toward the crosshair. We use Raycast
        Ray ray = aimCamera.ScreenPointToRay(Input.mousePosition);
        //Ray ray = new Ray(upperBody.transform.position, Vector3.forward);
        RaycastHit hitInfo;

        // Judy is in the layer 8.We need to avoid this layer
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        Vector3 targetPoint = Vector3.zero;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);
            Debug.DrawLine(upperBody.transform.position, hitInfo.point);
            Debug.DrawLine(epaule.transform.position, hitInfo.point);
            Debug.DrawLine(main.transform.position, hitInfo.point);
            Debug.DrawLine(mainD.transform.position, hitInfo.point);
            Debug.DrawLine(bow.transform.position, hitInfo.point);

            targetDirection = hitInfo.point - fleche.transform.position;

            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = (ray.origin + ray.direction.normalized * 100);
        }

        // TODO garder la tete vers la cible ?

        if (animator.GetCurrentAnimatorClipInfo(1).Length > 0 && animator.GetCurrentAnimatorClipInfo(1)[0].clip.name != "New Standing Draw Arrow")
        {
            Vector3 targetdir = targetPoint - upperBody.transform.position;
            Vector3 targetdir2 = targetPoint - main.transform.position;
            Vector3 targetdir3 = targetPoint - epaule.transform.position;
            Vector3 targetdir4 = targetPoint - mainD.transform.position;
            Vector3 targetdir5 = targetPoint - fleche.transform.position;
            Vector3 targetdir7 = (targetPoint - GameObject.FindWithTag("Player").transform.right) - bow.transform.position;

            Quaternion rotate = Quaternion.LookRotation(targetdir, GameObject.FindWithTag("Player").transform.right);
            //Vector3 rotate = upperBody.transform.eulerAngles + new Vector3(-targetdir.y, targetdir.x, 0f);
            upperBody.transform.rotation = rotate;

            // Rotation Epaule
            Vector3 bis2 = Vector3.Cross(targetdir2, GameObject.FindWithTag("Player").transform.right);
            Quaternion rotate3 = Quaternion.LookRotation(-bis2, Vector3.Cross(bis2, targetdir3));
            //Vector3 rotate = upperBody.transform.eulerAngles + new Vector3(-targetdir.y, targetdir.x, 0f);
            epaule.transform.rotation = rotate3;

            // Rotation Main
            Vector3 bis = Vector3.Cross(targetdir2, GameObject.FindWithTag("Player").transform.right);
            Quaternion rotate2 = Quaternion.LookRotation(bis, -Vector3.Cross(bis, targetdir2));
            main.transform.rotation = rotate2;

            // Rotation mainD
            Vector3 bis3 = Vector3.Cross(targetdir4, GameObject.FindWithTag("Player").transform.right);
            Quaternion rotate4 = Quaternion.LookRotation(-bis3, Vector3.Cross(bis3, targetdir4));
            mainD.transform.rotation = rotate4;

            // Rotation fleche
            fleche.transform.rotation = Quaternion.LookRotation(targetdir5);

            // Bow head
            Vector3 bis5 = Vector3.Cross(targetdir7, GameObject.FindWithTag("Player").transform.right);
            bow.transform.rotation = Quaternion.LookRotation(-targetdir7, GameObject.FindWithTag("Player").transform.right);
        }
        Vector3 targetdir6 = targetPoint - head.transform.position;
        // Rotation head
        Vector3 bis4 = Vector3.Cross(targetdir6, GameObject.FindWithTag("Player").transform.right);
        head.transform.rotation = Quaternion.LookRotation(-Vector3.Cross(bis4, targetdir6), targetdir6);
    }
    #endregion

    /*
    override protected void GetInputs(Vector3 NextDir, float h, float v) {
        if (m_isGrounded) {
            /*
            if (isAiming) {
                // Bow mode
                if (NextDir.Equals(Vector3.zero))
                {
                    m_footstep.Pause();
                    if (m_isPlayerCrouch)
                    {
                        aimCamera.transform.localPosition = GameObject.Find("OriginCamera").transform.localPosition - GameObject.Find("OriginCamera").transform.up * 0.15f;
                    }
                    else
                    {
                        aimCamera.transform.localPosition = GameObject.Find("OriginCamera").transform.localPosition;
                    }
                }
                //GameObject.FindWithTag("Player").transform.rotation = Quaternion.LookRotation(NextDir);
            } else {
            // Regular mode
            // If the player is not moving at all
            
            //}
			//m_moveSpeed = m_animator.GetFloat ("Speed");
        }
	}*/

    public void HitWithStick() {

        // Launch the animation
        //m_animator.Play("SwordAttack", m_animator.GetLayerIndex("Movement Layer"));
        //m_animator.Play("SwordAttack", m_animator.GetLayerIndex("Fight Layer"));
        animator.SetTrigger("Hit");

        // Change the clip to the firing clip and play it.
        GameObject torch = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Torch3D");
        torch.GetComponent<AudioSource>().Play();

        RaycastHit hitInfo;
        Debug.DrawRay(GameObject.Find("UpAnchor").transform.position, GameObject.Find("UpAnchor").transform.forward * 100f, Color.red);
        Vector3 rayPosition = transform.position + transform.forward * 1.5f + transform.up * 3f;
        if (Physics.Raycast(new Ray(rayPosition, rayPosition + transform.forward), out hitInfo, 10.0f)) {
            if (hitInfo.transform.tag == "Animal") {
                hitInfo.transform.gameObject.GetComponent<AgentProperties>().takeDamages(10.0f);
            }
        }
    }

    /*
    private IEnumerator Zoom() {
        yield return new WaitForSeconds(0.80f);
        // Play the zoom sound
        bow.GetComponent<BowScript>().PlayZoomSound();
        // Make the camera go forward

    }*/

    private IEnumerator DrawArrow()
    {
        // Launch the animation to reload
        isReloading = true;
        hasArrowLeft = InventoryManager.HasArrowLeft();
        
        // Update animator
        animator.SetBool("HasArrowLeft", hasArrowLeft);

        if (hasArrowLeft) {
            // SetActive the arrow
            GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(true);
        }
        yield return new WaitForSeconds(0.80f);

        // Make the string bend
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D/BowRig_tex/Root/String").GetComponent<BowString>().Stretch();

        yield return new WaitForSeconds(0.05f);
        isReloading = false;
    }

    private void FireArrow() {
        Debug.Log("Fire !");
        // Fire the arrow
        bow.GetComponent<BowScript>().FireArrow(targetDirection);

        // Come back in the original settings for a smooth animation
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2").transform.rotation = initUpperBody;
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone").transform.rotation = initEpaule;
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3").transform.rotation = initMain;
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3").transform.rotation = initMainD;
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").transform.rotation = initFleche;

        StartCoroutine("DrawArrow");
    }

    public void SetIsReloading(bool r) { 
        isReloading = r;
    }

    public bool getIsAiming() {
        return isAiming;
    }

    void OnDrawGizmosSelected()
    {
        //Camera camera = GameObject.Find("AimedCamera").GetComponent<Camera>();
        //Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(p, 0.1F);
        Vector3 r = GameObject.Find("UpAnchor").transform.position;
        Vector3 r2 = GameObject.Find("UpAnchor").transform.forward;
        Debug.DrawRay(GameObject.Find("UpAnchor").transform.position, GameObject.Find("UpAnchor").transform.forward, Color.green);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        Debug.DrawRay(transform.position + transform.forward * 2 + transform.up * 4, transform.forward, Color.blue);
    }

}

	