using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toll : MonoBehaviour
{
    public GameObject invisibleWall;
    private ObjectiveManager objectiveManager;
    [SerializeField]
    private TMPro.TextMeshProUGUI instructions;
    private bool playerInRange = false;
    public AudioSource p;

    // Start is called before the first frame update
    void Start()
    {
        // Find the ObjectiveManager script in the scene
        objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the ObjectiveManager is found and the final objective is active
        if (objectiveManager.final_objective_active)
        {

            // Check if the "E" key is pressed
            if (Input.GetKeyDown(KeyCode.E) && playerInRange)
            {
                if (playerInRange)
                    instructions.text = "PRESS E TO ACTIVATE";

                instructions.text = "";
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
