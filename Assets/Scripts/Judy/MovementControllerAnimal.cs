using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementControllerAnimal : MovementController {

    [SerializeField] protected float m_minSpeed;
    [SerializeField] protected float m_maxSpeed;

    new void Start() {
        base.Start();
        // Set the attribute to the desire amount
        //m_moveSpeed = 1;
        //m_minSpeed = 1;
        //m_maxSpeed = 3;
        //m_turnSpeed = 1;
        //m_jumpForce = 3;
    }


    override protected void JumpingAndLanding(Vector3 NextDir)
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
			//actions.Jump ();
			if(!NextDir.Equals(Vector3.zero))
				m_rigidBody.AddForce((Vector3.up+transform.forward) * m_jumpForce, ForceMode.Impulse);
			else
				m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }
    }

	override protected void GetInputs(Vector3 NextDir, float h, float v){
		if (m_isGrounded) {
            if (Input.GetMouseButton(0)) //attaque
            {
                m_moveSpeed = 0f;
                m_animator.SetFloat("Speed_f", 0f);
                Attack();
            } else { // Movements Directionnal
                if (!NextDir.Equals (Vector3.zero)) {
                    if (Input.GetKey (KeyCode.LeftShift) && !EnergyBar.GetComponent<EnergyBar>().energyIsAt0) {
                        if (EnergyBar.GetComponent<Scrollbar>().size > 0f)
                        {
                            m_moveSpeed = m_maxSpeed;
                            m_animator.SetFloat("Speed_f", m_maxSpeed);
                            m_footstep.UnPause();
                            m_footstep.pitch = 1.7f;
                        } else {
                            EnergyBar.GetComponent<EnergyBar>().energyIsAt0 = true;
                        }
				    } else {
					    m_moveSpeed = m_minSpeed;
					    m_animator.SetFloat ("Speed_f", m_minSpeed);
					    m_footstep.UnPause ();
					    m_footstep.pitch = 1f;
				    }
			    } else {
				    m_moveSpeed = 0f;
				    m_animator.SetFloat ("Speed_f", 0f);
				    m_footstep.Pause ();
			    }
            }
        } else {
            Animator judyAnim = this.gameObject.GetComponent<Animator>();
            float currTime = judyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currTime >= 1 - 0.06)       //si l'animation attack n'a pas fini on ne passe pas en locomotion
            {
                m_animator.SetBool("Attack_state", false);
                m_animator.Play("Locomotion");
            }
        }
	}

    private void Attack()
    {
        Animator judyAnim = this.gameObject.GetComponent<Animator>();
        judyAnim.SetBool("Attack_state", true);
        judyAnim.Play("Attack"); //joue animation attaque

        RaycastHit hit;
        float distance = 25f; //distance de l'animal pour pouvoir lui infliger des degats
        Ray Judy = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * distance);
        if (Physics.SphereCast(Judy, 1.5f, out hit, distance))
        {
            if (hit.collider.tag == "Animal")
            {
                hit.transform.gameObject.GetComponent<AgentProperties>().takeDamages(30f);
                //Inflige degat a l'animal
            }
        }
    }

    /*private void Animate(Vector3 NextDir){
		if (m_isGrounded) {
			if (NextDir.Equals (Vector3.zero)) {
				if (Input.GetKey (KeyCode.LeftControl)) {
					actions.Wary ();
				} else {
					actions.Stay ();
				}

			} else {
				if (Input.GetKey(KeyCode.LeftShift)) {
					if (Input.GetKey (KeyCode.LeftControl)) {
						actions.CrouchingRun ();
					} else {
						actions.Run ();
					}
				} else {
					if (Input.GetKey (KeyCode.LeftControl)) {
						actions.Sitting ();
					} else {
						actions.Walk ();
					}
				}

			}

			m_moveSpeed = m_animator.GetFloat ("Speed");
		}
	}*/


}

	