using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    public Transform player;
    public Transform playerCam;
    public float throwForce = 10;
    bool hasPlayer = false;
    bool beingCarried = false;
    public AudioClip[] soundToPLay;
    private AudioSource aud;
    private float soundRange = 25f;
    private Sound.SoundType soundType = Sound.SoundType.Default;
    private bool touched = false;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if(dist <= 5f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer= false;
        }

        if(hasPlayer && Input.GetButtonDown("Use")) 
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = playerCam;
            beingCarried=true;
        }

        if(beingCarried)
        {
            if(touched)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                touched = false;
            }

            if(Input.GetMouseButtonDown(0))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
                RandomAudio();
            }
            else if(Input.GetMouseButtonDown(1))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
            }
        }
    }

    void RandomAudio()
    {
        if (aud.isPlaying)
        {
            return;
        }

        aud.clip = soundToPLay[Random.Range(0, soundToPLay.Length)];
        aud.Play();

        var sound = new Sound(transform.position, soundRange, soundType);

        Sounds.MakeSound(sound);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(beingCarried)
        {
            touched = true;
        }
    }
}
