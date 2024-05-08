using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMarker : MonoBehaviour
{
    [SerializeField] private GameObject item;
    public ObjectiveManager objectiveManager;

    // Floating animation
    private Vector3 startPos;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float rotation = 50f;
    [SerializeField] public AudioSource p;

    private bool markerActive = false;

    private void Start()
    {
        startPos = transform.position;

        // Initially set the marker to be inactive
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (objectiveManager.final_objective_active && !markerActive)
        {
            // Activate the marker with a delay
            StartCoroutine(ActivateMarkerWithDelay(14f)); // Delay for 5 seconds
        }

        if (markerActive)
        {
            // Apply floating animation
            Vector3 tempPos = startPos;
            tempPos.y += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
            transform.position = tempPos;
            transform.Rotate(Vector3.up, rotation * Time.deltaTime);
        }
    }

    public IEnumerator ActivateMarkerWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Activate the marker
        gameObject.SetActive(true);
        markerActive = true;

        // Play the sound
        p.Play();
    }

    public void DeactivateMarker()
    {
        // Deactivate the marker

        markerActive = false;
        Destroy(gameObject);
    }
}
