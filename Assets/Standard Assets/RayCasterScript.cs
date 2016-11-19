using UnityEngine;
using System.Collections;

public class RayCasterScript : MonoBehaviour {

    public float interactDistance = 3F;
    public bool ownsKeyRed = false;

    Camera camera;
    DoorScript2 ds2;
    DoubleLeftDoor dld;
    DoubleRightDoor drd;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            //print("I'm looking at " + hit.transform.name);   
            if (Input.GetKeyDown("e"))
            {
                //Debug.Log("E pressed");

                switch (hit.transform.tag)
                {
                    case "KeyNone":
                        {
                            ds2 = hit.transform.GetChild(0).GetComponent<DoorScript2>();
                            //Debug.Log("Calling buttonPress()");
                            ds2.buttonPress();
                            break;
                        }
                    case "KeyRed":
                            if (ownsKeyRed)
                            {
                                ds2 = hit.transform.GetChild(0).GetComponent<DoorScript2>();
                                //Debug.Log("Calling buttonPress()");
                                ds2.buttonPress();
                            }
                            else
                            {
                                //Debug.Log("Player missing red key");
                            }
                        break;
                    case "DoubleKeyNone":
                        dld = hit.transform.Find("DoorLeft").GetComponent<DoubleLeftDoor>();
                        drd = hit.transform.Find("DoorRight").GetComponent<DoubleRightDoor>();
                        dld.buttonPress();
                        drd.buttonPress();

                        break;
                    default:
                        {
                            break;
                        }
                        
                }
               
            }
        }
        /*else
            print("I'm looking at nothing!");*/
    }
}
