using UnityEngine;
using System.Collections;

public class KEY : MonoBehaviour {

	public GameObject KeyToRemove;

	public AudioSource GetSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {


			RayCasterScript MinneAntaa = other.GetComponentInChildren<RayCasterScript>();

			MinneAntaa.ownsKeyRed = true;

			GetSound.Play();
			
			
				
			KeyToRemove.gameObject.SetActive(false);
		}

	}
		
}
