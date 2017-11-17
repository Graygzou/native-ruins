﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementControllerHuman : MovementController {

	[SerializeField] private Actions actions = null;

    new void Start() {
        base.Start();
        // Set the attribute to the desire amount
        m_moveSpeed = 10;
        m_turnSpeed = 5;
        m_jumpForce = 5;
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
			if (NextDir.Equals (Vector3.zero)) {
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

			m_moveSpeed = m_animator.GetFloat ("Speed");
		}
	}
}

	