using UnityEngine;

public class Mailbox : MonoBehaviour
{
    private bool playerInRange = false; // Flag to track if the player is in range to interact with the mailbox

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
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ObjectiveManager om = FindObjectOfType<ObjectiveManager>();
            if (om != null)
            {
                om.CompleteObjective("Mail");
            }
            else
            {
                Debug.LogError("ObjectiveManager not found in the scene!");
            }
        }
    }
}
