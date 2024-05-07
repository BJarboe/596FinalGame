using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryObj : MonoBehaviour
{
    //[SerializeField]
    //private ObjectiveManager om;
    public int itemCount;
    public int numItems;
    public bool playerInRange;
    public string prompt;

    private enum Progress { INACTIVE, ACTIVE, CHECKOUT, PAYMENT, DONE}
    private Progress progress;

    public TMPro.TextMeshProUGUI instructions;


    public static GroceryObj Instance { get; private set; } // Singleton instance 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        itemCount = 0;
        progress = Progress.INACTIVE;
        playerInRange = false;
    }

    private void Update()
    {
        if (progress == Progress.INACTIVE && itemCount > 0)
        {
            progress = Progress.ACTIVE;
            //om.StartObjective("Groceries");
            Debug.Log("START");
        }
        
        if (itemCount == numItems && progress == Progress.ACTIVE && playerInRange)
        {
            progress = Progress.CHECKOUT;
            StartCoroutine(CheckOut());
        }
        if (progress == Progress.PAYMENT && playerInRange)
            instructions.text = prompt;
    }

    IEnumerator CheckOut()
    {
        // Dialogue or something
        yield return new WaitForSeconds(3);
        progress = Progress.PAYMENT;
        yield return new WaitUntil(() => playerInRange && Input.GetKeyDown(KeyCode.E)) ;
        instructions.text = "";
        progress = Progress.DONE;
        //om.CompleteObjective("Groceries");
        Debug.Log("COMPLETE");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            instructions.text = "";
        }
    }

}
