using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class Bullet : RobotMovement {
    public float speed;
    private Vector3 m_origin;
    public override void Move()
    {
        transform.position += transform.forward * speed;
        if ((m_origin - transform.position).sqrMagnitude > 100000)
            Destroy(gameObject);
    }

    public override void SetRobotState(List<string> robotState)
    {
    }

    public override void TimeStateChange(TimeState old, TimeState nu)
    {
    }

    // Use this for initialization
    void Start () {
        //float y = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        //transform.position = new Vector3(transform.position.x, y, transform.position.z);
        m_origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<TimeManipulated>().GetTimeState() == TimeState.Stop && 
            CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            GameObject plr = GameObject.FindGameObjectWithTag("Player");
            Vector3 bulletpos = new Vector3(transform.position.x, plr.transform.position.y, transform.position.z);
            Vector3 targetDir = (plr.transform.position - bulletpos).normalized;
            float tol = Mathf.Deg2Rad * 10;
            Debug.Log(-Mathf.Cos(tol));
            if (Vector3.Dot(targetDir, plr.transform.forward) < -Mathf.Cos(tol) && (bulletpos - plr.transform.position).sqrMagnitude < 9)
            {
                transform.forward = GameObject.FindGameObjectWithTag("Player").transform.forward;
            }
        }
        if (GetComponent<TimeManipulated>().GetTimeState() == TimeState.Backward && GetComponent<TimeManipulated>().NoMovementHistory())
            Destroy(gameObject);
	}

    void OnTriggerEnter(Collider coll)
    {
        if (GetComponent<TimeManipulated>().GetTimeState() == TimeState.Normal)
        {
            if (coll.tag == "Player")
                Debug.Log("DEAD");
            if (coll.tag == "Enemy")
                Debug.Log("Enemy Dead");
            if (coll.tag != "Bullet")
                Destroy(gameObject);
        }
    }
}
