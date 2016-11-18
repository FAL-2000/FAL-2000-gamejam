using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PhysicsMovement : RobotMovement {
    private Vector3 m_lastAngularVelocity;
    private Vector3 m_lastVelocity;
    private string SerializeVector(Vector3 vec)
    {
        return vec.x + "," + vec.y + "," + vec.z;
    }
    private Vector3 DeserializeVector(string s)
    {
        string[] components = s.Split(new string[] { "," }, StringSplitOptions.None);
        return new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[2]));
    }

    public override void Move()
    {
        // Serialilze custom properties to string (Pay attention how to serialize Vector3)
        Rigidbody rb = GetComponent<Rigidbody>();
        List<string> state = new List<string>();
        state.Add(SerializeVector(rb.angularVelocity));
        state.Add(SerializeVector(rb.velocity));
        GetComponent<TimeManipulated>().SetRobotState(state);
    }

    public override void SetRobotState(List<string> robotState)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = DeserializeVector(robotState[0]);
        rb.velocity = DeserializeVector(robotState[1]);
    }

    public override void TimeStateChange(TimeState old, TimeState nu)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (old != TimeState.Normal && nu == TimeState.Normal)
        {
            rb.isKinematic = false;
            rb.angularVelocity = m_lastAngularVelocity;
            rb.velocity = m_lastVelocity;
        }
        if (old == TimeState.Normal && nu != TimeState.Normal)
        {
            m_lastAngularVelocity = rb.angularVelocity;
            m_lastVelocity = rb.velocity;
            rb.isKinematic = true;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
