using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour {

    private Rigidbody body;
    
    [Header("Gravity")]
    public float gravity = -20;

    [Header("Acceleration")]
    public float groundAcceleration = 60;
    public float airAcceleration = 10;

    [Header("Speed settings")]
    public float moveSpeed = 6;

    [Header("Friction settings")]
    public float airFriction = 2;
    public float groundFriction = 10;

    [Header("Jump settings")]
    public float jumpSpeed = 10;
    [Range(0, 1)]
    public float gravityFraction = 0.5f;

    [Header("Collision settings")]
    public float groundDistance = 0.801f;
    public LayerMask mask;

    private bool jumping = false;
    private Vector3 velocity = Vector3.zero;

    public bool isGrounded { get; set; }

    public new CapsuleCollider collider;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        velocity = body.velocity;

        CheckGrounded();

        float accel = isGrounded ? groundAcceleration : airAcceleration;
        float friction = isGrounded ? groundFriction : airFriction;

        // Motion
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (moveDir.sqrMagnitude > 1)
                moveDir.Normalize();

            if (moveDir.sqrMagnitude > 0)
                Accelerate(transform.TransformDirection(moveDir), moveSpeed, accel);
            else
                Friction(accel);

            Friction(friction);
        }

        //Gravity and jumping
        {
            /*if (isGrounded)
            {
                velocity -= transform.up * Vector3.Dot(velocity, transform.up);
            }
            else
            {
                velocity += transform.up * gravity * Time.deltaTime;
            }*/
            velocity += transform.up * gravity * Time.deltaTime;

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                velocity = transform.up * (Vector3.Dot(velocity, transform.up) + jumpSpeed);
            }

        }
        
        body.velocity = velocity;
	}

    void Accelerate(Vector3 dir, float speed, float acceleration)
    {
        float currentSpeed = Vector3.Dot(velocity, dir);

        float diff = speed - currentSpeed;

        if (diff < 0)
            return;

        float addSpeed = Time.deltaTime * acceleration;

        if (addSpeed > diff)
            addSpeed = diff;

        velocity += addSpeed * dir;
    }

    void Friction(float amount)
    {
        //This whole function can be optimized.
        // it can also not be optimized.
        // cpu's are great, aren't they

        // TODO friction on gravity axis for some reason

        Vector3 up = Vector3.Dot(transform.up, velocity) * transform.up;
        Vector3 v = velocity - up;

        float frictionDecrease = amount * Time.deltaTime;
        float currentSpeed = v.magnitude;

        float newSpeed = currentSpeed - frictionDecrease;

        if (newSpeed < 0)
            newSpeed = 0;

        velocity = v.normalized * newSpeed + up;
    }

    void CheckGrounded()
    {
        isGrounded = true;

        /*if (Vector3.Dot(velocity, transform.up) > 0.1)
            return;*/

        Vector3 c = collider.center + transform.position;
        Vector3 up = transform.up;
        float h = collider.height / 2;

        RaycastHit info;

        if(Physics.CapsuleCast(c + up * h, c - up * h, collider.radius, -up, out info))
        {
            if(info.distance < 0.05)
            {
                isGrounded = true;
            }
        }
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
        body.velocity = newVelocity;
    }
}
