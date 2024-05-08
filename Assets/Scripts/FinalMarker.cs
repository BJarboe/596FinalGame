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

    public bool markerActive = false;

    public void Start()
    {
        startPos = transform.position;

        // Initially set the marker to be inactive
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (objectiveManager.final_objective_active && !markerActive)
        {
            // Activate the marker immediately
            ActivateMarker();
        }
        // Apply floating animation
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, rotation * Time.deltaTime);
    }

    public void ActivateMarker()
    {
        // Activate the marker
        gameObject.SetActive(true);
        markerActive = true;
    }

    public void DeactivateMarker()
    {
        // Deactivate the marker
        markerActive = false;
        Destroy(gameObject);
    }
}
