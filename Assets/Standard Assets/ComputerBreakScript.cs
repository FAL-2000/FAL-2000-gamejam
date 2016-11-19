using UnityEngine;
using System.Collections;

/// <summary>
/// Scrip that watches that computer is undirtubed. if toucned, disables holostuffen.
/// </summary>
public class ComputerBreakScript : MonoBehaviour {

	public float LocationY;
	public float LocationX;
	public float LocationZ;
	public GameObject Screen;
	public GameObject KeyBoard;

	public AudioSource Off;

	public bool InRightPlace = true;

	// Use this for initialization
	void Start () {
	
		this.LocationY = Mathf.RoundToInt(this.transform.localPosition.y);
		this.LocationX = Mathf.RoundToInt(this.transform.localPosition.x);
		this.LocationZ = Mathf.RoundToInt(this.transform.localPosition.z);

	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if ((this.LocationY != Mathf.RoundToInt(this.transform.localPosition.y)) && (this.LocationX != Mathf.RoundToInt(this.transform.localPosition.x)) && (this.LocationZ != Mathf.RoundToInt(this.transform.localPosition.z))) {
			Screen.SetActive (false);
			KeyBoard.SetActive (false);
			if (InRightPlace == true)
			{
				Off.Play ();
				InRightPlace = false;
			}
		} else {
			Screen.SetActive (true);
			KeyBoard.SetActive (true);
			InRightPlace = true;
		}

	}
}
