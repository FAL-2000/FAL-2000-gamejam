using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class TimeManipulatingPlayer : MonoBehaviour {
    private Camera m_Camera;

    // Use this for initialization
    void Start () {
        m_Camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        TimeManipulation();
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Physics.Raycast(m_Camera.transform.position, m_Camera.transform.rotation * Vector3.forward, out hit);
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForceAtPosition((hit.point - m_Camera.transform.position).normalized * 10, hit.point, ForceMode.Impulse);
        }
    }

    private void TimeManipulation()
    {
        TimeManipulated[] list = UnityEngine.Object.FindObjectsOfType<TimeManipulated>();
        if (list.Length <= 0)
            return;
        TimeState currentTimeState = list[0].GetTimeState();
        foreach (TimeManipulated timeManipulated in list)
        {
            if (CrossPlatformInputManager.GetButtonDown("TimeStop"))
            {
                if (currentTimeState == TimeState.Normal)
                    timeManipulated.SetTimeState(TimeState.Stop);
                else if (currentTimeState == TimeState.Stop)
                    timeManipulated.SetTimeState(TimeState.Normal);
            }
            else if (currentTimeState != TimeState.Stop)
            {
                if (CrossPlatformInputManager.GetButton("TimeBack"))
                {
                    timeManipulated.SetTimeState(TimeState.Backward);
                }
                else
                {
                    timeManipulated.SetTimeState(TimeState.Normal);
                }
            }
        }
    }

}
