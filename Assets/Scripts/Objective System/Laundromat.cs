using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laundromat : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator;

    [SerializeField]
    private Material on_mat;

    [SerializeField]
    private Material off_mat;

    [SerializeField]
    private Material done_mat;

    [SerializeField]
    private AudioSource washerSounds;

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
        indicator.GetComponent<Renderer>().material = off_mat;
        washerSounds.loop = true;
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && obj.status == Objective.Status.Inactive)
            StartCoroutine(FirstWashCycle());
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
        }
    }

    IEnumerator FirstWashCycle()
    {
        indicator.GetComponent<Renderer>().material = on_mat;
        om.StartObjective("Laundromat");
        washerSounds.Play(20);
        yield return new WaitForSeconds(waitDuration);
        Debug.Log("First Cycle Finished");
        washerSounds.Stop();
        indicator.GetComponent<Renderer>().material = done_mat;
        yield return new WaitUntil(() => playerInRange && Input.GetKeyDown(KeyCode.E));
        indicator.GetComponent<Renderer>().material = off_mat;
        cycleComplete = true;
        Debug.Log("Wash Completed!");
    }
}