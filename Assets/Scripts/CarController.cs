using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float suspensionTravel = 0.2f;
    [SerializeField] private float suspensionForce = 10.0f;
    [SerializeField] private float wheelRadius = 0.32f;

    private new Rigidbody rigidbody;

    private Transform frontLeftWheelTransform;
    private Transform frontRightWheelTransform;
    private Transform rearLeftWheelTransform;
    private Transform rearRightWheelTransform;

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

        Transform wheels = transform.Find("Wheels");

        frontLeftWheelTransform = wheels.Find("FrontLeftWheel");
        frontRightWheelTransform = wheels.Find("FrontRightWheel");
        rearLeftWheelTransform = wheels.Find("RearLeftWheel");
        rearRightWheelTransform = wheels.Find("RearRightWheel");
    }

    private void FixedUpdate()
    {
        RaycastWheel(frontLeftWheelTransform);
        RaycastWheel(frontRightWheelTransform);
        RaycastWheel(rearLeftWheelTransform);
        RaycastWheel(rearRightWheelTransform);
    }

    private void RaycastWheel(Transform wheelTransform)
    {
        float distance = wheelRadius;
        Vector3 wheelPos = wheelTransform.position;
        Vector3 down = transform.TransformDirection(Vector3.down);
        LayerMask layerMask = ~LayerMask.NameToLayer("Ground");
        RaycastHit hit;
        if (Physics.Raycast(wheelPos, down, out hit, distance, layerMask))
        {
            float compressionPercent = (distance / hit.distance);
            rigidbody.AddForceAtPosition(-down * suspensionForce * compressionPercent, wheelPos);

            Debug.DrawRay(wheelPos, down * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(wheelPos, down * distance, Color.white);
        }
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
