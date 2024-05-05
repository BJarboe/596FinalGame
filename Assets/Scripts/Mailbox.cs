using UnityEngine;

public class Mailbox : MonoBehaviour
{
    public ObjectiveManager objManager; // Reference to the ObjectiveManager script
    public string objectiveID; // ID of the objective associated with this mailbox

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player has the box
            Box box = other.GetComponentInChildren<Box>();
            if (box != null && box.HasBox())
            {
                // Player has the box, trigger the completion of the objective
                objManager.CompleteObjective(objectiveID);
                // Optionally, destroy the box
                box.DestroyBox();
            }
        }
    }
}
