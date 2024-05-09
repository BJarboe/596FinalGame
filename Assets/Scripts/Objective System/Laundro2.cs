using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laundro2 : MonoBehaviour
{
    public Laundromat lm;
    private EnemyBehavior enemy;

    [SerializeField]
    private GameObject indicator;

    [SerializeField]
    private Material on_mat;

    [SerializeField]
    private Material off_mat;

    [SerializeField]
    private Material done_mat;

    [SerializeField]
    private AudioSource dryerSounds;

    [SerializeField]
    private AudioSource alarm;

    [SerializeField]
    private float waitDuration;

    private bool playerInRange;
    public bool startedCycle;
    public bool activateMarker;
    public bool primeAudio;

    public ObjectiveManager om;
    private Objective obj;

    [SerializeField]
    private TMPro.TextMeshProUGUI Instructions;

    private enum status { OFF, ON, READY, DONE }
    status state;

    private void Start()
    {
        obj = om.GetObjective("Laundromat");
        dryerSounds.loop = true;
        startedCycle = false;
        activateMarker = false;
        primeAudio = true;
        dryerSounds.time = 4;
        alarm.time = 9;
        state = status.OFF;
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehavior>();
    }

    private void Update()
    {
        if (lm.cycleComplete)
        {
            activateMarker = true;
            if (primeAudio && playerInRange && Input.GetKeyDown(KeyCode.E))
                StartCoroutine(DryerCycle());
        }
        switch (state) // Indicator light handler
        {
            case status.OFF:
            case status.DONE:
                indicator.GetComponent<Renderer>().material = off_mat;
                break;
            case status.ON:
                indicator.GetComponent<Renderer>().material = on_mat;
                break;
            case status.READY:
                indicator.GetComponent<Renderer>().material = done_mat;
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

    IEnumerator DryerCycle()
    {
        Debug.Log("Starting Second Cycle");
        state = status.ON;
        dryerSounds.Play();
        startedCycle = true;
        yield return new WaitForSeconds(waitDuration);
        dryerSounds.Stop();
        alarm.Play();
        Debug.Log("Second Cycle finished");
        state = status.READY;
        primeAudio = false;
        yield return new WaitUntil(() => playerInRange && Input.GetKeyDown(KeyCode.E));
        om.CompleteObjective("Laundromat");
        alarm.Stop();
        dryerSounds.Stop();
        state = status.DONE;
        enemy.SetSightRange(enemy.GetSightRange() + 12);
        activateMarker = false;
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
        Debug.Log("Player left Laundromat boundary");
        playerInRange = false;
        Instructions.text = "";
    }
}
