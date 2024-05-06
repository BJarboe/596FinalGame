using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public bool playerNear;
    public bool buffer = false;
    public bool open = false;

    public float open_distance;

    public float open_time;

    [SerializeField]
    private GameObject door1;
    [SerializeField]
    private GameObject door2;

    private Vector3 startPos1;
    private Vector3 startPos2;

    private Vector3 endPos1;
    private Vector3 endPos2;

    private Rigidbody rb1;
    private Rigidbody rb2;

    private void Start()
    {
        rb1 = door1.GetComponent<Rigidbody>();
        rb2 = door2.GetComponent<Rigidbody>();

        startPos1 = rb1.position;
        startPos2 = rb2.position;

        endPos1 = startPos1 + door1.transform.right * open_distance;
        endPos2 = startPos2 - door2.transform.right * open_distance;
    }

    private void Update()
    {
        if (playerNear)
        {
            if (!open)
                StartCoroutine( OpenDoor() );
        }
        else if (!buffer && open)
        {
            StartCoroutine(CloseDoor());
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !buffer)
        {
            playerNear = true;
            StartCoroutine( SetBuffer() );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }

    IEnumerator SetBuffer()
    {
        buffer = true;
        yield return new WaitForSeconds(7);
        buffer = false;
        yield return null;
    }

    IEnumerator OpenDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / 2f;
            rb1.position = Vector3.Lerp(startPos1, endPos1, lerpFactor);
            rb2.position = Vector3.Lerp(startPos2, endPos2, lerpFactor);
            yield return null;
        }
        open = true;
    }

    IEnumerator CloseDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / 2f;
            rb1.position = Vector3.Lerp(endPos1, startPos1, lerpFactor);
            rb2.position = Vector3.Lerp(endPos2, startPos2, lerpFactor);
            yield return null;
        }
        open = false;
    }

}
