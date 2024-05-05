using UnityEngine;

public class Mailbox : MonoBehaviour
{
    private bool playerInRange = false; // Flag to track if the player is in range to interact with the mailbox
    private bool activated = false; // Flag to track if the mailbox interaction has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is in range to interact with the mailbox
            playerInRange = true;
            Debug.Log("Player in range to interact with the mailbox.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is no longer in range to interact with the mailbox
            playerInRange = false;
        }
    }

    private void Update()
    {
        // Check if the player is in range and presses the E key to interact with the mailbox
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !activated)
        {
            ObjectiveManager om = FindObjectOfType<ObjectiveManager>();
            if (om != null)
            {
                om.CompleteObjective("Mail");
                activated = true; // Set the activated flag to true to prevent duplicate interactions
            }
            else
            {
                Debug.LogError("ObjectiveManager not found in the scene!");
            }
        }
    }
}
