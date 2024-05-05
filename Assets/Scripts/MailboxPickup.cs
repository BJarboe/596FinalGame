using System.Collections;
using UnityEngine;

public class MailboxPickup : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private Material onMat;
    [SerializeField] private Material offMat;
    [SerializeField] private Material doneMat;
    [SerializeField] private float waitDuration;

    private bool playerInRange;
    private bool mailPickedUp;
    private bool activateMarker;

    private void Start()
    {
        indicator.GetComponent<Renderer>().material = offMat;
        mailPickedUp = false;
        activateMarker = false;
    }

    private void Update()
    {
        if (!mailPickedUp)
        {
            activateMarker = true;
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
                StartCoroutine(PickupMail());
        }
    }

    IEnumerator PickupMail()
    {
        Debug.Log("Picking up mail");
        indicator.GetComponent<Renderer>().material = onMat;
        yield return new WaitForSeconds(waitDuration);
        Debug.Log("Mail picked up");
        indicator.GetComponent<Renderer>().material = doneMat;
        mailPickedUp = true;
        yield return new WaitUntil(() => playerInRange && Input.GetKeyDown(KeyCode.E));
        // Add any additional logic for completing mail pickup
        indicator.GetComponent<Renderer>().material = offMat;
        activateMarker = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered mailbox pickup range");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited mailbox pickup range");
        playerInRange = false;
    }
}
