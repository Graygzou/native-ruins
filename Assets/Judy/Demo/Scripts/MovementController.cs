using UnityEngine;
using System.Collections.Generic;

public class MovementController : MonoBehaviour {





    [SerializeField] private float m_moveSpeed = 10;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 5;
    [SerializeField] private Animator m_animator;
	[SerializeField] private Actions actions;
    [SerializeField] private Rigidbody m_rigidBody;


    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for(int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider)) {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
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

        if(validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        } else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

	void Update () {
        DirectUpdate();

        m_wasGrounded = m_isGrounded;
    }

   
    private void DirectUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
	
		Vector3 NextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Animate (NextDir);
			
	
		//if(NextDir != Vector3.zero)
			//transform.rotation = Quaternion.LookRotation(NextDir);

		transform.rotation = Quaternion.LookRotation(NextDir);
		transform.position += Vector3.Normalize(NextDir) * m_moveSpeed * Time.deltaTime;


		JumpingAndLanding();

    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
			actions.Jump ();
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);

        }


    }

	private void Animate(Vector3 NextDir){
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
	}
}

	