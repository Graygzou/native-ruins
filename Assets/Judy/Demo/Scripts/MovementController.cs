using UnityEngine;
using System.Collections.Generic;

public class MovementController : MonoBehaviour {

    [SerializeField] private float m_moveSpeed = 10;
    [SerializeField] private float m_turnSpeed =5;
	[SerializeField] private float m_cameraSpeed = 0.3f;
    [SerializeField] private float m_jumpForce = 5;
    [SerializeField] private Animator m_animator;
	[SerializeField] private Actions actions;
    [SerializeField] private Rigidbody m_rigidBody;
	[SerializeField] private Transform m_cameraPivot = null;


	private Vector3 lastMousePosition = Vector3.zero;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();


	private void LateUpdate()
	{
		Vector3 dir = Input.mousePosition - lastMousePosition;
		lastMousePosition = Input.mousePosition;

		UpdateCamera(dir.x, -dir.y);
		Transform cameraTrans = m_cameraPivot.GetChild(0);
		float z = Mathf.Clamp(Input.mouseScrollDelta.y*0.3f + cameraTrans.localPosition.z, -8, -2);
		cameraTrans.localPosition = new Vector3(cameraTrans.localPosition.x, cameraTrans.localPosition.y, z);
	}

	private void UpdateCamera(float deltaX, float deltaY)
	{
		m_cameraPivot.localPosition = this.transform.Find("UpAnchor").position;
		if (deltaX == 0 && deltaY == 0) return;

		Vector3 rotate = m_cameraPivot.localEulerAngles + new Vector3(deltaY*m_cameraSpeed, deltaX*m_cameraSpeed, 0);
		if (rotate.x >= 180f) rotate.x -= 360f;
		if (rotate.x > -20f && rotate.x < 90f)
		{
			m_cameraPivot.localEulerAngles = rotate;
		}
	}



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
	
		Transform cameraTrans = m_cameraPivot.GetChild(0);
		Vector3 projected_forward_camera= Vector3.ProjectOnPlane (cameraTrans.forward, new Vector3 (0, 1, 0));

		Vector3 NextDir = new Vector3(h, 0, v);



		//Vector3 newOrientation = Vector3.Normalize(transform.right*h)*Time.deltaTime*m_turnSpeed+transform.forward;
		//transform.rotation = Quaternion.LookRotation (Vector3.Project(transform.forward+transform.right, newOrientation));
		//transform.rotation = Quaternion.LookRotation (NextDir);
		//transform.position += transform.forward * m_moveSpeed * Time.deltaTime;

		//transform.forward.Set(projected_forward_camera.x, projected_forward_camera.y, projected_forward_camera.z);
		//transform.Rotate(0f,cameraTrans.eulerAngles.y-transform.eulerAngles.y,0f);
		//NextDir = Quaternion.Euler(0f,cameraTrans.eulerAngles.y-transform.eulerAngles.y,0f)*NextDir;
		transform.rotation = Quaternion.LookRotation (NextDir);
		transform.position += NextDir * m_moveSpeed * Time.deltaTime;


		Animate (NextDir);
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

	