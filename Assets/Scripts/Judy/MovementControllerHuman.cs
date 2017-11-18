using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class MovementControllerHuman : MovementController {

	[SerializeField] private Actions actions = null;
    private Camera playerCamera;
    private Camera   aimCamera;
    private bool isAiming;

    //public Transform aimCamHolder;

    new void Start() {
        base.Start();
        // Set the attribute to the desire amount
        m_moveSpeed = 10;
        m_turnSpeed = 5;
        m_jumpForce = 5;

        // Get the regular camera
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        isAiming = false;

        // Disable the aim camera
        aimCamera = GameObject.Find("AimedCamera").GetComponent<Camera>();
        aimCamera.enabled = false;
    }

    // In this case, the vector3 NextDir is not used.
    override protected void JumpingAndLanding(Vector3 NextDir)
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space)) {
            m_jumpTimeStamp = Time.time;
			actions.Jump ();
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);

        }
    }

	override protected void GetInputs(Vector3 NextDir){

		if (m_isGrounded) {
            if (isAiming) {
                GameObject.FindWithTag("Player").transform.rotation = Quaternion.LookRotation(NextDir);
            }

            // If the player try to aim
            if (Input.GetMouseButton(1) && InventoryManager.isBowEquiped) {
                Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);
                Vector3 lookPos = ray.GetPoint(20);
                if (!isAiming) {
                    // Play the animation to take the bow and an arrow
                    isAiming = true;
                    m_animator.SetBool("Aiming", true);
                    m_animator.Play("BowDrawArrow");

                    // SetActive the arrow
                    GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(true);

                    // To stay in the idle bow state
                    m_animator.SetBool("Reloading", false);
                    m_animator.Play("BowDrawIdle");

                    // change the camera for the aim
                    //SwitchToAim();
                } else {
                    // Play the animation
                    isAiming = false;
                    m_animator.SetBool("Aiming", false);
                    m_animator.Play("Idle");

                    // change the camera for the aim
                    //SwitchToRegular();
                }
            } else if(Input.GetMouseButton(0)) {
                if (InventoryManager.isBowEquiped && isAiming) {
                    Debug.Log("Fire !");
                    StartCoroutine("FireArrow");
                } else if (InventoryManager.isTorchEquiped) {
                    m_moveSpeed = 0f;
                    m_animator.SetFloat("Speed_f", 0f);
                    m_animator.SetBool("Fight", true);
                    m_animator.Play("SwordSlash"); //joue animation attaque
                    // Attack(length ray);
                }
            } else if (NextDir.Equals (Vector3.zero)) {
				m_footstep.Pause ();
				if (Input.GetKey (KeyCode.LeftControl)) {
					actions.Wary ();
				} else {
					actions.Stay ();
				}

			} else {
                if (!isAiming) {
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

        yield return new WaitForSeconds(.5f);
        // Launch the animation to reload
        m_animator.SetBool("Reloading", true);
        m_animator.SetBool("Aiming", true);
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(true);
        m_animator.Play("BowDrawArrow");

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

	