using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Video;

public class ATM : MonoBehaviour
{
    public ObjectiveManager obj;
    public Light emmission;
    VideoPlayer vid;
    public bool playerInRange = false;
    private bool activated = false;
    public GameObject cash;
    Rigidbody rb;

    private void Start()
    {
        vid = GetComponent<VideoPlayer>();
        emmission.enabled = false;
        rb = cash.GetComponent<Rigidbody>();
        rb.detectCollisions = false;

        //StartCoroutine(Withdrawal()); // for testing
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !activated)
        {
            activated = true;
            if (obj.StartObjective("ATM"))
                StartCoroutine(BeginWithdrawal());
            else { Debug.Log("ATM objective couldn't activate.."); activated = false;}

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("ATM: Player in range");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerInRange = false;
    }

    IEnumerator BeginWithdrawal()
    {
        emmission.enabled = true;
        vid.Play();
        yield return new WaitForSeconds((float)vid.length);
        obj.CompleteObjective("ATM");
        StartCoroutine(Withdrawal());
    }

    IEnumerator Withdrawal()
    {
        float elapsedTime = 0;
        Vector3 startPos = rb.position;
        Vector3 endPos = startPos + cash.transform.right * 1f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / 2f;
            rb.position = Vector3.Lerp(startPos, endPos, lerpFactor);
            yield return null;
        }
        vid.Stop();
        rb.position = endPos;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
    }
}
