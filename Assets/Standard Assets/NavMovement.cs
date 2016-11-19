using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NavMovement : RobotMovement {
    private Vector3 lastVelocity = Vector3.zero;
    public override void Move()
    {
        GameObject plr = GameObject.FindGameObjectWithTag("Player");
        GetComponent<NavMeshAgent>().SetDestination(plr.transform.position);
        lastVelocity = GetComponent<NavMeshAgent>().velocity;
    }

    public override void SetRobotState(List<string> robotState)
    {
    }

    public override void TimeStateChange(TimeState old, TimeState nu)
    {
        if (nu == TimeState.Normal)
        {
            GetComponent<NavMeshAgent>().Resume();
        }
        if (old != TimeState.Normal && nu == TimeState.Normal)
        {
            GetComponent<NavMeshAgent>().velocity = lastVelocity;
        }
        if (nu != TimeState.Normal)
        {
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            GetComponent<NavMeshAgent>().Stop();
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }

}
