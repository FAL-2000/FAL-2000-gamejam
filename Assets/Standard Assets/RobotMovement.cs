using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ATTENTION GAMEJAM PARTICIPATORS!!!
// Implement this abstract class to implement robot's AI logic.
// Try not to put logic to Update() method, use Move() instead,
// it's called every frame and it stores robot's state.
// If you have custom class members that have to be stored in history,
// serialize them to string (.toString()) and call
// GetComponent<TimeManipulated>().SetRobotState(state), where
// state is a List<string>.
// Implement deserialization logic in SetRobotState().
// Check my example CustomMovement and ask me questions.
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
