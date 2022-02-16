using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WheelColliderCar : MonoBehaviour
{
    [SerializeField] private float engineForce = 100.0f;
    [SerializeField] private float brakeForce = 100.0f;
    [SerializeField] private float maxSteerAngle = 20.0f;

    [SerializeField] private Transform centreOfMass;

    [SerializeField][ReadOnly] private float motorTorque = 0;
    [SerializeField][ReadOnly] private float brakeTorque = 0;
    [SerializeField][ReadOnly] private float desiredDirection = 0;
    [SerializeField][ReadOnly] private float currentDirection = 0;
    [SerializeField][ReadOnly] private float combinedDriection = 0;
    [SerializeField][ReadOnly] private float velocity = 0;

    private new Rigidbody rigidbody;
    private WheelColliderCollection wheelColliders;

    private bool initialised = false;

    private void OnEnable()
    {
        if (!initialised)
        {
            Init();
        }
    }

    private void Init()
    {
        initialised = true;

        rigidbody = GetComponent<Rigidbody>();
        wheelColliders = GetComponentInChildren<WheelColliderCollection>();

        Assert.IsNotNull(centreOfMass, "Missing centreOfMass transform reference.");
        if (centreOfMass)
        {
            rigidbody.centerOfMass = centreOfMass.localPosition;
        }
    }

    private void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        velocity = localVelocity.z;

        desiredDirection = verticalInput != 0 ? verticalInput / verticalInput : 0;
        if (verticalInput < 0)
        {
            desiredDirection *= -1;
        }
        currentDirection = velocity != 0 ? velocity / velocity : 0;
        if (Mathf.Abs(velocity) <= 0.1f)
        {
            currentDirection = 0;
        }
        else if (velocity < 0)
        {
            currentDirection *= -1;
        }

        combinedDriection = desiredDirection + currentDirection;

        motorTorque = 0;
        brakeTorque = 0;

        if (desiredDirection != 0 && combinedDriection == 0)
        {
            brakeTorque = brakeForce;
        }
        else
        {
            motorTorque = verticalInput * engineForce;
        }

        wheelColliders.SetMotorTorque(motorTorque);
        wheelColliders.SetBrakeTorque(brakeTorque);

        if (Input.GetKey(KeyCode.Space))
        {
            wheelColliders.SetHandBrakeTorque(brakeForce);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        wheelColliders.SetSteerAngle(horizontalInput * maxSteerAngle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 pos = transform.position;
            pos += Vector3.up * 1.0f;
            transform.position = pos;

            transform.rotation = Quaternion.AngleAxis(0.0f, transform.forward);
        }
    }
}
