using UnityEngine;

public class Box : MonoBehaviour
{
    public ObjectiveManager obj; // Reference to the ObjectiveManager script
    // ID for the box pickup objective
    public GameObject pickupEffect; // Effect to play when the box is picked up

    private bool playerInRange = false; // Flag to track if the player is in range to pick up the box
    private bool activated = false; // Flag to track if the box pickup has been activated
    private Rigidbody rb; // Rigidbody component of the box

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check for player input to pick up the box
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !activated)
        {
            activated = true;
            if (obj.StartObjective("Mail"))
            {
                PickUpBox();
            }
            else
            {
                Debug.Log($"{"Mail"} objective couldn't activate..");
                activated = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log($"{"Mail"}: Player in range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void PickUpBox()
    {
        // Hide the box visually
        gameObject.SetActive(false);
        // Disable physics simulation for the box
        rb.isKinematic = true;

        // Play pickup effect if available
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
    }
}
