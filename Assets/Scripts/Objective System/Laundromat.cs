using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laundromat : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private VideoManager vm;

    private enum status { OFF, ON, READY, DONE}
    status state;

    [SerializeField]
    private TMPro.TextMeshProUGUI Instructions;

    [SerializeField]
    private Material on_mat;

    [SerializeField]
    private Material off_mat;

    [SerializeField]
    private Material done_mat;

    [SerializeField]
    private AudioSource washerSounds;
    [SerializeField]
    private AudioSource alarm;

    [SerializeField]
    private float waitDuration;

    private bool playerInRange;
    public bool cycleComplete;

    public ObjectiveManager om;
    private Objective obj;

    private void Start()
    {
        cycleComplete = false;
        obj = om.GetObjective("Laundromat");
        washerSounds.loop = true;
        alarm.time = 9;
        state = status.OFF;
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && obj.status == Objective.Status.Inactive)
            StartCoroutine(FirstWashCycle());
        
        
        switch (state) // Indicator light handler
        {
            case status.OFF:
                indicator.GetComponent<Renderer>().material = off_mat;
                break;
            case status.ON:
                indicator.GetComponent<Renderer>().material = on_mat;
                break;
            case status.READY:
                indicator.GetComponent<Renderer>().material = done_mat;
                break;
            case status.DONE:
                indicator.GetComponent<Renderer>().material = off_mat;
                break;
        }

        if (playerInRange)
        {
            switch (state) // Display instructions
            {
                case status.OFF:
                case status.DONE:
                    Instructions.text = "PRESS E TO START CYCLE";
                    break;
                case status.ON:
                    Instructions.text = "WAIT FOR CYCLE TO FINISH";
                    break;
                case status.READY:
                    Instructions.text = "PRESS E TO GRAB LAUNDRY";
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Laundromat detected Player");

            
            
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left Laundromat boundary");
            playerInRange = false;
            Instructions.text = "";
        }
    }

    IEnumerator FirstWashCycle()
    {
        
        Instructions.text = "";
        vm.PlayCutscene(3); 
        yield return new WaitForSeconds(8); // wait for cutscene to finish
        state = status.ON;
        om.StartObjective("Laundromat");
        washerSounds.Play();
        yield return new WaitForSeconds(waitDuration);
        state = status.READY;
        Debug.Log("First Cycle Finished");
        washerSounds.Stop();
        alarm.Play();
        yield return new WaitUntil(() => playerInRange && Input.GetKeyDown(KeyCode.E));
        state = status.DONE;
        Instructions.text = "";
        alarm.Stop();
        cycleComplete = true;
        Debug.Log("Wash Completed!");
    }
}
