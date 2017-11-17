﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    [SerializeField] protected float m_moveSpeed;
    [SerializeField] protected float m_turnSpeed;
    [SerializeField] protected float m_jumpForce;

    [SerializeField] protected float m_cameraSpeed = 2f;
    [SerializeField] protected Animator m_animator = null;
    [SerializeField] protected Rigidbody m_rigidBody;
    [SerializeField] protected AudioSource m_footstep;

    protected Transform m_cameraPivot = null;
    protected Vector3 initial_orientation;
    protected GameObject EnergyBar;

    protected Vector3 lastMousePosition = Vector3.zero;

    protected bool m_wasGrounded;
    protected Vector3 m_currentDirection = Vector3.zero;

    protected float m_jumpTimeStamp = 0;
    protected float m_minJumpInterval = 1.40f;

    protected bool m_isGrounded;
    protected List<Collider> m_collisions = new List<Collider>();

    // Use this for initialization
    public void Start() {
        m_cameraPivot = GameObject.Find("CameraPivot").transform;
        EnergyBar = GameObject.Find("Gauges/Energy");
        initial_orientation = transform.forward;
        m_footstep.Play();
        m_footstep.loop = true;
        m_footstep.Pause();
    }

    protected void LateUpdate() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 dir = new Vector3(mouseX, mouseY, 0f);
        lastMousePosition = Input.mousePosition;

        UpdateCamera(dir.x, -dir.y);
        Transform cameraTrans = m_cameraPivot.GetChild(0);
        float z = Mathf.Clamp(Input.mouseScrollDelta.y * 0.3f + cameraTrans.localPosition.z, -32, -12);
        cameraTrans.localPosition = new Vector3(cameraTrans.localPosition.x, cameraTrans.localPosition.y, z);
    }

    protected void UpdateCamera(float deltaX, float deltaY) {
        m_cameraPivot.localPosition = this.transform.Find("UpAnchor").position;
        if (deltaX == 0 && deltaY == 0) return;

        Vector3 rotate = m_cameraPivot.localEulerAngles + new Vector3(deltaY * m_cameraSpeed, deltaX * m_cameraSpeed, 0);
        if (rotate.x >= 180f) rotate.x -= 360f;
        if (rotate.x > -20f && rotate.x < 90f)
        {
            m_cameraPivot.localEulerAngles = rotate;
        }
    }

    protected void OnCollisionEnter(Collision collision) {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    protected void OnCollisionStay(Collision collision) {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++) {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f) {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal) {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider)) {
                m_collisions.Add(collision.collider);
            }
        } else {
            if (m_collisions.Contains(collision.collider)) {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    protected void OnCollisionExit(Collision collision) {
        if (m_collisions.Contains(collision.collider)) {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    protected void Update() {
        DirectUpdate();

        m_wasGrounded = m_isGrounded;
    }

    //Vector3 newOrientation = Vector3.Normalize(transform.right*h)*Time.deltaTime*m_turnSpeed+transform.forward;
    //transform.rotation = Quaternion.LookRotation (Vector3.Project(transform.forward+transform.right, newOrientation));
    //transform.rotation = Quaternion.LookRotation (NextDir);
    //transform.position += transform.forward * m_moveSpeed * Time.deltaTime;

    //transform.forward.Set(projected_forward_camera.x, projected_forward_camera.y, projected_forward_camera.z);
    //transform.Rotate(0f,cameraTrans.eulerAngles.y-transform.eulerAngles.y,0f);
    protected void DirectUpdate() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform cameraTrans = m_cameraPivot.GetChild(0);
        Vector3 projected_forward_camera = Vector3.ProjectOnPlane(cameraTrans.forward, new Vector3(0, 1, 0));

        float angle = SignedAngleBetween(initial_orientation, projected_forward_camera, Vector3.up);
        Vector3 NextDir = new Vector3(h, 0, v);

        NextDir = Quaternion.Euler(0f, angle, 0f) * NextDir;
        if (!NextDir.Equals(Vector3.zero))
            transform.rotation = Quaternion.LookRotation(NextDir);
        transform.position += NextDir * m_moveSpeed * Time.deltaTime;

        GetInputs(NextDir);
        JumpingAndLanding(NextDir);

    }

    protected virtual void JumpingAndLanding(Vector3 NextDir) { }

    protected virtual void GetInputs(Vector3 NextDir) { }

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

    public float getCurrentSpeed() {
        return m_moveSpeed;
    }

}