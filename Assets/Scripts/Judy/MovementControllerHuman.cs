using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        aimCamera = GameObject.Find("CameraPivot").transform.GetChild(1).GetComponent<Camera>();
        isAiming = false;
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
            if(Input.GetMouseButton(0) && InventoryManager.isTorchEquiped && !isAiming) {
                m_moveSpeed = 0f;
                m_animator.SetFloat("Speed_f", 0f);
                Attack();
            } else if(Input.GetMouseButton(1)) {

                Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);
                Vector3 lookPos = ray.GetPoint(20);

                if (!isAiming) {
                    Debug.Log("Click Aim !");
                    isAiming = true;
                    // change the camera for the aim
                    SwitchToAim();
                } else {
                    Debug.Log("Click UnAim !");
                    isAiming = false;
                    // change the camera for the aim
                    SwitchToRegular();
                }
            }
			else if (NextDir.Equals (Vector3.zero)) {
				m_footstep.Pause ();
				if (Input.GetKey (KeyCode.LeftControl)) {
					actions.Wary ();
				} else {
					actions.Stay ();
				}

			} else {
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

            if(isAiming) {
                GameObject.FindWithTag("Player").transform.rotation = Quaternion.LookRotation(NextDir);
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

    private void Attack()
    {
        Animator judyAnim = this.gameObject.GetComponent<Animator>();
        judyAnim.SetBool("Fight", true);
        judyAnim.Play("SwordSlash"); //joue animation attaque
        RaycastHit hit;
        float distance = 25f; //distance de l'animal pour pouvoir lui infliger des degats
        Ray Judy = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * distance);
        if (Physics.SphereCast(Judy, 1.5f, out hit, distance))
        {
            if (hit.collider.tag == "Animal")
            {
                hit.transform.gameObject.GetComponent<AgentProperties>().takeDamages(15f);
                //Inflige degat a l'animal
            }
        }
        float currTime = judyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= 1 - 0.06)
        {
            judyAnim.SetBool("Fight", false);
            judyAnim.Play("Idle");
        }
    }
}

	