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
    private bool soundPlayed = false; // Flag to track if the sound has been played

    [SerializeField] public AudioSource p;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player enters the range, set playerInRange to true
            playerInRange = true;

            // Play the sound if it hasn't been played yet
            if (!soundPlayed)
            {
                p.Play();
                soundPlayed = true; // Set the flag to true to indicate that the sound has been played
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player exits the range, set playerInRange to false
            playerInRange = false;
            instructions.text = "";
        }
    }

    void Update()
    {
        // Check if the ObjectiveManager is found and the final objective is active
        if (objectiveManager.final_objective_active)
        {
            // Check if the player is in range and the finalMarker is not null
            if (playerInRange && finalMarker != null)
            {
                // Activate the marker when the final objective is active and the player is in range
                finalMarker.ActivateMarker();
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

                // Update UI instructions
                instructions.text = playerInRange ? "PRESS E TO ACTIVATE" : "";

                // Check if the invisible wall reference is not null
                if (invisibleWall != null)
                {
                    // Deactivate the invisible wall
                    invisibleWall.SetActive(false);
                }
            }
        }
    }
}
