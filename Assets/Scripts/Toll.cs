using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toll : MonoBehaviour
{
    public GameObject invisibleWall;
    [SerializeField] private ObjectiveManager objectiveManager;
    public TMPro.TextMeshProUGUI instructions;
    public FinalMarker finalMarker;
    private bool playerInRange = false;

    void Start()
    {
        // Find the ObjectiveManager script in the scene
        objectiveManager = FindObjectOfType<ObjectiveManager>();

        // Ensure finalMarker is properly assigned
        if (finalMarker == null)
        {
            Debug.LogError("FinalMarker is not assigned in the inspector!");
        }
    }

    void Update()
    {
        // Check if the ObjectiveManager is found and the final objective is active
        if (objectiveManager.final_objective_active)
        {
            // Check if finalMarker is not null before accessing it
            if (finalMarker != null)
            {
                // Activate the marker with a delay when the final objective is active
                StartCoroutine(finalMarker.ActivateMarkerWithDelay(14f)); // Delay for 5 seconds
            }

            // Check if the "E" key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check if finalMarker is not null before accessing it
                if (finalMarker != null)
                {
                    // Deactivate the marker when "E" is pressed
                    finalMarker.DeactivateMarker();
                }

                if (playerInRange)
                {
                    instructions.text = "PRESS E TO ACTIVATE";
                }
                else
                {
                    instructions.text = "";
                }

                // Check if the invisible wall reference is not null
                if (invisibleWall != null)
                {
                    // Deactivate the invisible wall
                    invisibleWall.SetActive(false);
                }
            }
        }
    }

    // You may need to implement OnTriggerEnter and OnTriggerExit to handle playerInRange
}
