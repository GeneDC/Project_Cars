using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelColliderCollection : MonoBehaviour
{
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

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
    }

    public void SetMotorTorque(float torque)
    {
        // All wheel drive?
        frontLeftWheelCollider.motorTorque = torque;
        frontRightWheelCollider.motorTorque = torque;
        rearLeftWheelCollider.motorTorque = torque;
        rearRightWheelCollider.motorTorque = torque;
    }

    public void SetBrakeTorque(float torque)
    {
        frontLeftWheelCollider.brakeTorque = torque;
        frontRightWheelCollider.brakeTorque = torque;
        rearLeftWheelCollider.brakeTorque = torque;
        rearRightWheelCollider.brakeTorque = torque;
    }

    public void SetHandBrakeTorque(float torque)
    {
        rearLeftWheelCollider.brakeTorque = torque;
        rearRightWheelCollider.brakeTorque = torque;
    }

    public void SetSteerAngle(float angle)
    {
        frontLeftWheelCollider.steerAngle = angle;
        frontRightWheelCollider.steerAngle = angle;
    }

    private void Update()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }


}
