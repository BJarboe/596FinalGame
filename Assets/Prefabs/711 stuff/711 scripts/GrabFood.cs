using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabFood : MonoBehaviour
{
    [SerializeField]
    private GroceryObj Groceries;

    [SerializeField]
    private string prompt;

    private bool playerInRange;

    private void Start()
    {
        playerInRange = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            Groceries.itemCount++;
            gameObject.SetActive(false);
            Groceries.instructions.text = "";
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Groceries.instructions.text = prompt;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Groceries.instructions.text = "";
        }
    }
}
