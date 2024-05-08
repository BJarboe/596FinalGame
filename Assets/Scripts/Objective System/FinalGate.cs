using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGate : MonoBehaviour
{
    public static FinalGate Instance { get; private set; } // Singleton instance 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField]
    private ObjectiveManager oM;
    [SerializeField]
    private GameObject barrier;
    [SerializeField]
    private GameObject marker;
    [SerializeField]
    private TMPro.TextMeshProUGUI instructions;
    public string prompt;
    public enum s { INACTIVE, ACTIVE, DONE}
    public s state;
    private bool playerInRange;

    private void Start()
    {
        state = s.INACTIVE;
        playerInRange = false;
    }

    private void Update()
    {
        switch (state)
        {
            case s.INACTIVE:
                if (oM.final_objective_active)
                    state = s.ACTIVE;
                break;

            case s.ACTIVE:
                if (!marker.activeSelf)
                    marker.SetActive(true);
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    state = s.DONE;
                    instructions.text = "";
                    marker.SetActive(false);
                }
                break;

            case s.DONE:
                if (barrier.activeSelf)
                    barrier.SetActive(false);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (state == s.ACTIVE)
                instructions.text = prompt;
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
