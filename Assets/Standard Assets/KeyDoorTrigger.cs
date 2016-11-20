using UnityEngine;
using System.Collections;

public class KeyDoorTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (other.tag == "Enemy")
        {
            if (transform.name == "GlassDoorTrigger")
            {
                Debug.Log("Robot detected at door");
                DoorScript2 ds2 = transform.Find("Glass_Door").GetComponent<DoorScript2>();
                if (ds2.isCLosedOrClosing())
                    ds2.buttonPress();
            }
            else if (transform.name == "DoubleDoor")
            {
                Debug.Log("Robot detected at DoubleDoor");
                DoubleRightDoor drd = transform.Find("DoorRight").GetComponent<DoubleRightDoor>();
                DoubleLeftDoor dld = transform.Find("DoorLeft").GetComponent<DoubleLeftDoor>();
                if (drd.isCLosedOrClosing())
                {
                    drd.buttonPress();
                    dld.buttonPress();
                }
            }

        }
    }
}
