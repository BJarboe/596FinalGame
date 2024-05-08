using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroceryObj : MonoBehaviour
{
    [SerializeField]
    private ObjectiveManager om;
    public int itemCount;
    public int numItems;
    public bool playerInRange;
    public string prompt;

    public GameObject screen;

    public GameObject NameObject;
    public GameObject TextBoxObject;
    public TMPro.TextMeshProUGUI Name;
    public TMPro.TextMeshProUGUI TextBox;
    public TMPro.TextMeshProUGUI Click;

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
        screen.SetActive(false);
    }

    private void Update()
    {
        if (progress == Progress.INACTIVE && itemCount > 0)
        {
            progress = Progress.ACTIVE;
            om.StartObjective("Groceries");
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
        NameObject.SetActive(true);
        TextBoxObject.SetActive(true);
        float tmp = Name.fontSize;
        Name.fontSize = 21f;
        Name.text = "SDSU Graduate";
        TextBox.text = "Getting your weekly groceries at a 7 eleven..";
        Click.text = "CLICK TO CONTINUE";

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        TextBox.text = "Classy.. $35.99 for ya..";

        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        Click.text = "";
        TextBox.text = "the iPad's gonna ask you a question..";
        screen.SetActive(true);

        yield return new WaitForSeconds(2);

        progress = Progress.PAYMENT;
        

        yield return new WaitUntil(() => playerInRange && Input.GetKeyDown(KeyCode.E)) ;

        instructions.text = "";
        TextBox.text = "I wouldn't be out for too much longer...";
        progress = Progress.DONE;
        om.CompleteObjective("Groceries");
        Debug.Log("COMPLETE");

        yield return new WaitForSeconds(5);

        screen.SetActive(false);
        TextBox.text = "";
        Name.text = "";
        TextBox.fontSize = tmp;
        TextBoxObject.SetActive(false);
        NameObject.SetActive(false);
        
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
