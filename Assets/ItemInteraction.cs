using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] private GameObject item; // Reference to the item GameObject
    [SerializeField] private GameObject mailbox; // Reference to the mailbox GameObject
    private bool playerInRange;
    private bool itemPickedUp;
    private bool itemInteracted;

    private void Start()
    {
        playerInRange = false;
        itemPickedUp = false;
        itemInteracted = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!itemPickedUp)
            {
                Debug.Log("Item picked up");
                itemPickedUp = true;
                // Add logic here for what to do after picking up the item
            }
            else if (itemPickedUp && !itemInteracted && Vector3.Distance(transform.position, mailbox.transform.position) <= 2f)
            {
                Debug.Log("Item dropped off at mailbox");
                itemInteracted = true;
                Destroy(item); // Destroy the item GameObject
                // Add logic here for what to do after dropping off the item
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered item interaction range");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited item interaction range");
        playerInRange = false;
    }
}
