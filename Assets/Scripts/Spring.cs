using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float restLength = 2.0f;
    [SerializeField] private float travelScale = 0.2f;
    [SerializeField] private float springConstant = 10.0f;
    [SerializeField] private float dampingConstant = 1.0f;

    [SerializeField] private Transform connectionPointA;
    [SerializeField] private Transform connectionPointB;

    [SerializeField] private bool searchForRigidbodyParent = true;

    private Rigidbody connectionRigidbodyA;
    private Rigidbody connectionRigidbodyB;

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

        if (searchForRigidbodyParent)
        {
            connectionRigidbodyA = connectionPointA.GetComponentInParent<Rigidbody>();
        }
        else
        {
            connectionRigidbodyA = connectionPointA.GetComponent<Rigidbody>();
        }

        if (searchForRigidbodyParent)
        {
            connectionRigidbodyB = connectionPointB.GetComponentInParent<Rigidbody>();
        }
        else
        {
            connectionRigidbodyB = connectionPointB.GetComponent<Rigidbody>();
        }
    }
    private void FixedUpdate()
    {
        connectionRigidbodyA.AddForceAtPosition(GetForceOnPointA(), connectionPointA.position);
        connectionRigidbodyB.AddForceAtPosition(GetForceOnPointB(), connectionPointB.position);
    }

    private Vector3 GetForceOnPointA()
    {
        return GetForce(connectionPointA.position, connectionPointB.position, connectionRigidbodyA.velocity, connectionRigidbodyB.velocity);
    }
    private Vector3 GetForceOnPointB()
    {
        return GetForce(connectionPointB.position, connectionPointA.position, connectionRigidbodyB.velocity, connectionRigidbodyA.velocity);
    }

    private Vector3 GetForce(Vector3 mass1Pos, Vector3 mass2Pos, Vector3 mass1Velocity, Vector3 mass2Velocity)
    {
        Vector3 vecBetween = (mass1Pos - mass2Pos);
        // spring force
        float dist = vecBetween.magnitude;
        float scalar = travelScale * springConstant * (dist - restLength);
        Vector3 dir = vecBetween.normalized;

        // find speed of contraction/expansion for damping force
        float s1 = Vector3.Dot(mass2Velocity, dir);
        float s2 = Vector3.Dot(mass1Velocity, dir);
        float dampingScalar = -dampingConstant * (s1 + s2);

        return (-scalar + dampingScalar) * dir;
    }

    private void Update()
    {
        Debug.DrawLine(connectionPointA.position, connectionPointB.position);
    }
}
