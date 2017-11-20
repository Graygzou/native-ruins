using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class MovementControllerHuman : MovementController {

	[SerializeField] private ActionsNew actions = null;
    private Camera playerCamera;
    private Camera   aimCamera;
    private bool isAiming;
    private Vector3 offset;

    //public Transform aimCamHolder;

    new void Start() {
        base.Start();
        // Set the attribute to the desire amount
        m_moveSpeed = 10;
        m_turnSpeed = 5;
        m_jumpForce = 5;

        // Get the regular camera
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        aimCamera = GameObject.Find("AimedCamera").GetComponent<Camera>();
        aimCamera.enabled = false;
        offset = aimCamera.transform.position - GameObject.Find("Player").transform.position;
        isAiming = false;

        // Disable the aim camera
        //aimCamera = GameObject.Find("AimedCamera").GetComponent<Camera>();
        //aimCamera.enabled = false;
    }

    // In this case, the vector3 NextDir is not used.
    override protected void JumpingAndLanding(Vector3 NextDir)
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space)) {
            m_jumpTimeStamp = Time.time;
			actions.Jump();
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);

        }
    }

    override protected void UpdateCamera(float deltaX, float deltaY) {   
        if (!isAiming) {
            // Regular mode
            base.UpdateCamera(deltaX, deltaY);
        } else {
            // Aiming mode
            GameObject upperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2");

            // Debug
            Vector3 mousePositionVector3 = Input.mousePosition;
            mousePositionVector3.z = 10;
            mousePositionVector3 = aimCamera.ScreenToWorldPoint(mousePositionVector3);
            Vector3 targetdir = mousePositionVector3 - upperBody.transform.position;

            Debug.Log(targetdir);

            Vector3 p = aimCamera.ViewportToWorldPoint(new Vector3(1, 1, 10));

            //Debug.Log(p.x - targetdir.x);
            //Debug.Log(p.y - targetdir.y);


            // method1
            //Vector3 mousePositionVector3 = Input.mousePosition;
            //mousePositionVector3.z = 10;
            //mousePositionVector3 = aimCamera.ScreenToWorldPoint(mousePositionVector3);
            //Vector3 targetdir = mousePositionVector3 - upperBody.transform.position;
            //upperBody.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetdir);

            //cam.ViewportToWorldPoint(Vector3(0.5, 0.5, cam.nearClipPlane));


            // Emthod 2
            Vector3 rotate = upperBody.transform.eulerAngles + new Vector3(-targetdir.y * 4.0f, targetdir.x * 4.0f, 0f);

            if (rotate.x >= 180f) rotate.x -= 360f;
            if (rotate.x > -20f && rotate.x < 90f) {
                upperBody.transform.eulerAngles = rotate;
            };

            //upperBody.transform.rotation.x = Quaternion.Lerp(upperBody.transform.rotation, Quaternion.LookRotation(targetdir), Time.deltaTime);

            // Aiming mode


            //GameObject upperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2");
            //upperBody.transform.localEulerAngles = new Vector3(deltaX, deltaY, 0); // = Quaternion.AngleAxis(deltaX, Vector3.right);
            //aimCamera.transform.Rotate(deltaY, 0, 0);

            //aimCamera.transform.localRotation = Quaternion.AngleAxis(deltaY, aimCamera.transform.up);
            //transform.localRotation = Quaternion.AngleAxis(-deltaY, Vector3.right);


            //Vector3 mousePositionVector3 = new Vector3(deltaX, deltaY, 0);
            //mousePositionVector3 = aimCamera.ScreenToWorldPoint(mousePositionVector3);
            //Vector3 targetdir = mousePositionVector3 - transform.position;

            //GameObject upperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2");
            //upperBody.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetdir);


            //aimCamera.transform.position = GameObject.FindWithTag("Player").transform.position + offset;
            ////GameObject upperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2");

            //Vector3 rotate = aimCamera.transform.localEulerAngles + new Vector3(deltaY * m_cameraSpeed, deltaX * m_cameraSpeed, 0);
            //if (rotate.x >= 180f) rotate.x -= 360f;
            //if (rotate.x > -20f && rotate.x < 90f)
            //{
            //    aimCamera.transform.localEulerAngles = rotate;
            //}

            //aimCamera.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetdir);

            //Vector3 mousePositionVector3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            //mousePositionVector3 = Camera.main.ScreenToWorldPoint(mousePositionVector3);

            //Vector3 targetdir = mousePositionVector3 - transform.position;
            //GameObject upperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2");
            //upperBody.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetdir);


            //m_cameraPivot.localPosition = this.transform.Find("UpAnchor").position - new Vector3(-5, 0, 5);

            //if (deltaX == 0 && deltaY == 0) return;

            //Vector3 rotate = m_cameraPivot.localEulerAngles + new Vector3(deltaY * m_cameraSpeed, deltaX * m_cameraSpeed, 0);
            //if (rotate.x >= 180f) rotate.x -= 360f;
            //if (rotate.x > -20f && rotate.x < 90f)
            //{
            //    m_cameraPivot.localEulerAngles = rotate;
            //    //Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            //    // Set the player's rotation to this new rotation.
            //    //GameObject upperBody = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3");
            //    //upperBody.transform.Rotate(-deltaX, deltaY, 0);

            //}
        }
    }

    override protected void Move(Vector3 NextDir, float h, float v) {
        if(!isAiming) {
            // Regular state
            base.Move(NextDir, h, v);
        } else {
            // Aiming State

            //if (!NextDir.Equals(Vector3.zero))
            //    transform.rotation = Quaternion.LookRotation(NextDir);
            transform.position += (Vector3.forward * m_moveSpeed) * v * Time.deltaTime;
            transform.position += (Vector3.right * m_moveSpeed) * h * Time.deltaTime;
        }
    }

    override protected void GetInputs(Vector3 NextDir, float h, float v) {
		if (m_isGrounded) {
            // 1) Check if the player want to fight with the bow
            if (Input.GetMouseButtonDown(1) && InventoryManager.isBowEquiped) {
                //Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);
                //Vector3 lookPos = ray.GetPoint(20);
                if (!isAiming) {
                    isAiming = true;

                    // To take the idle state
                    actions.Aiming();

                    // SetActive the arrow
                    GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(true);

                    // To stay in the idle bow state
                    aimCamera.enabled = true;
                    playerCamera.enabled = false;
                    //m_animator.SetBool("Reloading", false);
                    //m_animator.Play("BowDrawIdle");

                    // change the camera for the aim
                    //SwitchToAim();
                } else {
                    isAiming = false;

                    // Play the animation
                    actions.ReleaseAiming();

                    // change the camera for the aim
                    aimCamera.enabled = false;
                    playerCamera.enabled = true;
                    
                    //SwitchToRegular();
                }
            }

            // 2) Check if the player want to hit something
            if(Input.GetMouseButtonDown(0)) {
                if (InventoryManager.isBowEquiped && isAiming) {
                    Debug.Log("Fire !");
                    StartCoroutine("FireArrow");
                } else if (InventoryManager.isTorchEquiped) {
                    //m_moveSpeed = 0f;
                    actions.HitWithTorch();
                    // Attack(length ray);
                }
            }

            // 3) Check the movement of the player
            if (isAiming) {
                // Bow mode
                if (NextDir.Equals(Vector3.zero)) {
                    m_footstep.Pause();
                    if (Input.GetKey(KeyCode.LeftControl)) {
                        actions.Wary();
                    } else {
                        actions.Stay();
                    }
                } else {
                    actions.MoveWithBow(h, v);
                }
                //GameObject.FindWithTag("Player").transform.rotation = Quaternion.LookRotation(NextDir);
            } else {
                // Regular mode
                // If the player is not moving at all
                if (NextDir.Equals(Vector3.zero)) {
                    m_footstep.Pause();
                    if (Input.GetKey(KeyCode.LeftControl)) {
                        actions.Wary();
                    } else {
                        actions.Stay();
                    }
                } else {
                    // If the player is running
                    if (Input.GetKey(KeyCode.LeftShift) && !EnergyBar.GetComponent<EnergyBar>().energyIsAt0) {
                        if (EnergyBar.GetComponent<Scrollbar>().size > 0f) {
                            m_footstep.UnPause ();
					        m_footstep.pitch = 1.7f;
                    
                            if (Input.GetKey (KeyCode.LeftControl)) {
						        actions.CrouchingRun ();
					        } else {
						        actions.Run ();
					        }
                        } else {
                            EnergyBar.GetComponent<EnergyBar>().energyIsAt0 = true;
                        }
                    // If the player is walking
				    } else {
					    m_footstep.UnPause ();
					    m_footstep.pitch = 1f;
					    if (Input.GetKey (KeyCode.LeftControl)) {
						    actions.Sitting ();
					    } else {
                            actions.Walk();
                        }
                    }
                }

            }
			m_moveSpeed = m_animator.GetFloat ("Speed");
        }
	}

    public void  SwitchToAim() {
        // Enable the right camera
        playerCamera.enabled = false;
        aimCamera.enabled = true;
        currentCamera = 1;
    }

    public void SwitchToRegular()
    {
        playerCamera.enabled = true;
        aimCamera.enabled = false;
        currentCamera = 0;
    }

    IEnumerator FireArrow() {
        Debug.Log("Fire !");
        // Fire the arrow
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(false);
        Attack();
        //yield return new WaitForSeconds(.5f);

        // Launch the animation to reload
        actions.Reloading();
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(true);
        yield return new WaitForSeconds(1f);
        
    }

    private void Attack() {
        RaycastHit hit;
        float distance = 100f; //distance de l'animal pour pouvoir lui infliger des degats
        Ray Judy = new Ray(transform.position, transform.forward);
        GameObject arrow = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D");
        Debug.DrawRay(arrow.transform.position, arrow.transform.forward * distance);
        if (Physics.SphereCast(Judy, 1.5f, out hit, distance)) {
            if (hit.collider.tag == "Animal") {
                //Inflige degat a l'animal
                hit.transform.gameObject.GetComponent<AgentProperties>().takeDamages(15f);
            }
        }
        //float currTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //if (currTime >= 1 - 0.06)
        //{
        //    m_animator.SetBool("Fight", false);
        //    m_animator.Play("Idle");
        //}
    }
}

	