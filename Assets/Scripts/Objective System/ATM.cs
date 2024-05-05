using Mono.Reflection;
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
    [SerializeField]
    private TMPro.TextMeshProUGUI Instructions;
    private enum status { OFF, ON, READY, DONE }
    status state;

    private void Start()
    {
        vid = GetComponent<VideoPlayer>();
        emmission.enabled = false;
        rb = cash.GetComponent<Rigidbody>();
        rb.detectCollisions = false;
        state = status.OFF;
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

        if (playerInRange)
        {
            switch (state) // Display instructions
            {
                case status.OFF:
                    Instructions.text = "PRESS E TO INSERT DEBIT CARD";
                    break;
                case status.READY:
                    Instructions.text = "PRESS E TO COMPLETE TRANSACTION";
                    break;
                case status.DONE:
                case status.ON:
                    Instructions.text = "";
                    break;
            }
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
        Instructions.text = "";
    }

    IEnumerator BeginWithdrawal()
    {
        state = status.ON;
        emmission.enabled = true;
        vid.Play();
        yield return new WaitForSeconds((float)vid.length - 4.2f);
        vid.Stop();
        state = status.READY;
        yield return new WaitUntil(() => playerInRange && Input.GetKeyDown(KeyCode.E));
        obj.CompleteObjective("ATM");
        state = status.DONE;
        StartCoroutine(Withdrawal());
    }

    IEnumerator Withdrawal()
    {
        float elapsedTime = 0;
        Vector3 startPos = rb.position;
        Vector3 endPos = startPos + cash.transform.right * 0.7f;

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
        yield return new WaitForSeconds(1);
        emmission.enabled = false;
        cash.SetActive(false);
    }
}
