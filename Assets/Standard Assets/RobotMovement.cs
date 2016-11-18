using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class RobotMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void Move();
    public abstract void SetRobotState(List<string> robotState);
}
