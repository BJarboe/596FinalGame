using UnityEngine;

public class Mailbox : MonoBehaviour
{
    public ObjectiveManager objManager; // Reference to the ObjectiveManager script
    public string endObjectiveID; // ID of the objective associated with delivering the box to the mailbox

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player has the box
            Box box = other.GetComponentInChildren<Box>();
            if (box != null && box.HasBox())
            {
                // Player has the box, trigger the completion of the objective
                objManager.CompleteObjective(endObjectiveID);
                // Optionally, destroy the box
                box.DestroyBox();
            }
        }
    }
}
