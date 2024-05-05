using UnityEngine;

public class CustomMarkerBox : MonoBehaviour
{
    public ObjectiveManager objManager;
    public string mailboxObjectiveID; // Objective ID associated with delivering the box to the mailbox
    public GameObject mailboxMarker; // Reference to the marker at the mailbox

    private Objective mailboxObjective;
    private Renderer markerRenderer;

    // Floating animation
    private Vector3 startPos;
    private float a = 0.5f;
    private float f = 1f;
    private float r = 50f;

    private void Start()
    {
        startPos = transform.position;
        markerRenderer = mailboxMarker.GetComponent<Renderer>();
        markerRenderer.enabled = false; // Disable the mailbox marker initially
        mailboxObjective = objManager.GetObjective(mailboxObjectiveID);
    }

    private void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * f) * a;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, r * Time.deltaTime);

        // Check if the mailbox objective is active or completed
        if (mailboxObjective != null && (mailboxObjective.status == Objective.Status.Active))
        {
            // Enable the marker at the mailbox
            markerRenderer.enabled = true;
        }
        else
        {
            // Disable the marker at the mailbox
            markerRenderer.enabled = false;
        }
    }
}
