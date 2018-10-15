using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementControllerAnimal : MovementController {

    [SerializeField] protected float m_minSpeed;
    [SerializeField] protected float m_maxSpeed;

    private AudioSource[] sons;
    private AudioSource sonAttaque;

    protected float m_minattackInterval = 1.0f;
    protected float m_attackTimeStamp = 0.0f;

    new void Awake() {
        base.Awake();
        sons = GetComponents<AudioSource>();
        sonAttaque = sons[1];

        m_animator = this.gameObject.GetComponent<Animator>();
        // Set the attribute to the desire amount
        //m_moveSpeed = 1;
        //m_minSpeed = 1;
        //m_maxSpeed = 3;
        //m_turnSpeed = 1;
        //m_jumpForce = 3;
    }

    protected override void Start()
    {
        base.Start();

        // Special movements
        inputsManager.SubscribeButtonEvents(InputManager.ActionsLabels.Jump, "Jump", new System.Action[] { JumpingAndLanding, null, null });
        // Register the fighting related actions
        //inputsManager.SubscribeButtonEvent(InputManager.ActionsLabels.Attack, "Fire1", InputManager.EventTypeButton.Down, Attack);
        inputsManager.SubscribeMouseMovementsChangedEvent(InputManager.ActionsLabels.Attack, "Fire1", InputManager.EventTypeChanged.Changed, Attack);
    }

    override protected void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
			//actions.Jump ();
            /*
			if(!NextDir.Equals(Vector3.zero))
				m_rigidBody.AddForce((Vector3.up+transform.forward) * m_jumpForce, ForceMode.Impulse);
			else
				m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);*/
        }
    }

    override protected void GetInputs(Vector3 NextDir, float h, float v){
		if (!m_isGrounded) {
            /*
            Animator judyAnim = this.gameObject.GetComponent<Animator>();
            float currTime = judyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currTime >= 1 - 0.06)       //si l'animation attack n'a pas fini on ne passe pas en locomotion
            {
                m_animator.SetBool("Attack_state", false);
                m_animator.Play("Locomotion");
            }*/
        }
	}

    private void Attack()
    {
        Debug.Log(Input.GetAxis("Fire1"));
        bool attackCooldownOver = (Time.time - m_attackTimeStamp) >= m_minattackInterval;

        if (attackCooldownOver)
        {
            m_attackTimeStamp = Time.time;

            Debug.Log(m_animator.GetCurrentAnimatorClipInfo(m_animator.GetLayerIndex("Base Layer"))[0].clip.name);
            GameObject playerRoot = GameObject.Find("Player");
            m_animator.SetTrigger("Attack");
            sonAttaque.Play();

            RaycastHit hit;
            float distance = 2f; //distance de l'animal pour pouvoir lui infliger des degats
                                 // For the bear
                                 //Ray Judy = new Ray(transform.position + transform.forward * 6f + transform.up * 4, transform.forward);
                                 //Debug.DrawRay(transform.position + transform.forward * 6f + transform.up * 4, transform.forward * distance);
                                 // For the wolf
            Ray Judy = new Ray(transform.position + transform.forward * 7.5f + transform.up * 3, transform.forward);
            Debug.DrawRay(transform.position + transform.forward * 7.5f + transform.up * 3, transform.forward * distance);
            if (Physics.Raycast(Judy, out hit, distance))
            {
                if (hit.collider.tag == "Animal")
                {
                    hit.transform.gameObject.GetComponent<AgentProperties>().takeDamages(50f);
                    //Inflige degat a l'animal
                }
            }
        }
        
    }

    void OnDrawGizmosSelected()
    {
        //Camera camera = GameObject.Find("AimedCamera").GetComponent<Camera>();
        //Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(p, 0.1F);
        //Debug.DrawRay(transform.position + transform.forward * 6 + transform.up * 5, transform.forward, Color.blue);
    }

}

	