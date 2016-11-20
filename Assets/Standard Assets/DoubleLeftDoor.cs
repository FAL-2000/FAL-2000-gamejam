using UnityEngine;
using System.Collections;

public class DoubleLeftDoor : MonoBehaviour
{
    //private AudioSource doorOpenSound; 
    /*public AudioSource[] sounds;
    public AudioSource noise1;
    public AudioSource noise2;*/


    private static short CLOSED = 0;
    private static short CLOSING = 1;
    private static short OPEN = 2;
    private static short OPENING = 3;

    private short state = 0;

    public float movementSpeed = 1.0F;
    public float maxMove = 1.35F;

    private float targetZ;
    private float originalZ;

    // Use this for initialization
    void Start()
    {
        //doorOpenSound = GetComponent<AudioSource>();
        originalZ = transform.localPosition.z;
        targetZ = originalZ - maxMove;
        /*sounds = GetComponents<AudioSource>();
        noise1 = sounds[0];
        noise2 = sounds[1];*/
    }

    // Update is called once per frame
    void Update()
    {
        if (state == OPENING)
        {
            if (transform.localPosition.z > targetZ)
            {
                transform.Translate(new Vector3(0.0F, 1.0F, 0.0F) * Time.deltaTime * movementSpeed);
            }
            else
            {
                state = OPEN;
            }
        }
        else if (state == CLOSING)
        {
            if (transform.localPosition.z < originalZ)
            {
                transform.Translate(new Vector3(0F, -1.0F, 0F) * Time.deltaTime * movementSpeed);
            }
            else
            {
                state = CLOSED;
            }
        }

    }

    public void buttonPress()
    {
        if (state == CLOSED || state == CLOSING)
        {
            state = OPENING;
            //noise1.Play();

        }
        else if (state == OPEN || state == OPENING)
        {
            state = CLOSING;
            //noise2.Play();
        }
    }

    public bool isCLosedOrClosing()
    {
        if (state == CLOSED || state == CLOSING)
            return true;
        else
            return false;
    }
}
