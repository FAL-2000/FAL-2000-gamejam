using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TimeManipulatingPlayer : MonoBehaviour {
    private Camera m_Camera;
	public AudioReverbZone TimeStopSoundReverbZone;	//needs catcher, now manually put!
	public AudioReverbZone TimeRevindSoundReverbZone;	//needs catcher, now manually put!
	public ParticleSystem TimeBoom;

	public int TimeResource = 100;		//the Time Resource!
	public bool TimeManipulationHappening = false;	// is something FUN going on?
	public bool TimeResourceRecoverHappening = true;

	public Text ResourceViewText;
	public Image ResourceImage;

	public float WhenCanRecoverTimeResource = 0.0f;
	public float WaitBeforeResourceRegen = 3.0f;

	public AudioSource TimeTravelSound;
	public AudioSource EndOfTimeTravelSound;
	public AudioSource TimeResourceRecoverSound;

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

		if (TimeManipulationHappening == true) {
			
			TimeResourceRecoverHappening = false;

			TimeResource = TimeResource - 1;
			if (TimeResource < 1)
				TimeResource = 1;	//ettei tuu nollajakoja!

			WhenCanRecoverTimeResource = Time.time + WaitBeforeResourceRegen;
		} 
		else if ((TimeResource < 100) && (Time.time > WhenCanRecoverTimeResource)) {

			if (TimeResourceRecoverHappening == false)
			{
				TimeResourceRecoverSound.Play();
				TimeResourceRecoverHappening = true;
			}

			TimeResource++;
		}

		// UI VISUALS
	
		//float TEMP = -1f*(Screen.width * TimeResource/100);	//THISWOKRS!

		float TEMP = 1f*(Screen.width * (100-TimeResource)/100);	//FROM LEFT

		ResourceImage.rectTransform.offsetMin = new Vector2 (TEMP,-20f);



		//ResourceViewText.text = "RESOURCE = " + TimeResource;

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

			if (TimeResource < 10){		//No timeresource left = cant do any time manipulation and back to real world!

				StopAllTimeTravelExtras();
				timeManipulated.SetTimeState(TimeState.Normal);

			}

            else if (CrossPlatformInputManager.GetButtonDown("TimeStop"))
            {
				if ((currentTimeState == TimeState.Normal) && (TimeResource > 0))
				{
                    timeManipulated.SetTimeState(TimeState.Stop);
					TimeStopSoundReverbZone.gameObject.SetActive(true);
					TimeManipulationHappening = true;
					this.PlayStartofTimeWeirdness();
				}
                else if (currentTimeState == TimeState.Stop)
				{
                    timeManipulated.SetTimeState(TimeState.Normal);
					StopAllTimeTravelExtras();
				}
            }
			else if ((currentTimeState == TimeState.Backward) && !(CrossPlatformInputManager.GetButton("TimeBack")))
			{
				timeManipulated.SetTimeState(TimeState.Normal);
				StopAllTimeTravelExtras();
			}
			else if ((currentTimeState != TimeState.Stop) && (CrossPlatformInputManager.GetButton("TimeBack")))
            {
				if (TimeResource > 0)
                {
                    timeManipulated.SetTimeState(TimeState.Backward);
					TimeRevindSoundReverbZone.gameObject.SetActive(true);
					TimeBoom.Play();
					TimeManipulationHappening = true;
					this.PlayStartofTimeWeirdness();
                }
			}

            

        }


    }

	private void StopAllTimeTravelExtras()	//all extras go down!
	{
		
		TimeStopSoundReverbZone.gameObject.SetActive(false);				
		TimeRevindSoundReverbZone.gameObject.SetActive(false);
		TimeBoom.Stop();
		
		TimeManipulationHappening = false;
		
		TimeTravelSound.Stop();
	
		if (EndOfTimeTravelSound.isPlaying == false)
			EndOfTimeTravelSound.Play ();
	
	
	}

	private void PlayStartofTimeWeirdness()
	{
	//	EndOfTimeTravelSound.Stop();
		if (TimeTravelSound.isPlaying == false)	//it should not be playing mutta varmuude varalta
		{
			TimeTravelSound.Play();
		}

	}

}
