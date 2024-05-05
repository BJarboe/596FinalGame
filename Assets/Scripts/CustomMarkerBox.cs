using UnityEngine;

public class CustomMarkerBox : MonoBehaviour
{
    public ObjectiveManager objManager;
    public string boxObjectiveID; // Objective ID associated with picking up the box
    public string mailboxObjectiveID; // Objective ID associated with delivering the box to the mailbox
    public GameObject mailboxMarker; // Reference to the marker at the mailbox

    private Objective boxObjective;
    private Objective mailboxObjective;

    private Renderer rend;

    // Floating animation
    private Vector3 startPos;
    private float a = 0.5f;
    private float f = 1f;
    private float r = 50f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        startPos = transform.position;
        boxObjective = objManager.GetObjective(boxObjectiveID);
        mailboxObjective = objManager.GetObjective(mailboxObjectiveID);
        mailboxMarker.SetActive(false); // Disable the mailbox marker initially
    }

    private void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * f) * a;
        transform.position = tempPos;
        transform.Rotate(Vector3.up, r * Time.deltaTime);

        // Check if the box objective is completed
        if (boxObjective != null && boxObjective.status == Objective.Status.Completed)
        {
            // Enable the marker at the mailbox
            mailboxMarker.SetActive(true);
        }

        // Check if the mailbox objective is completed
        if (mailboxObjective != null && mailboxObjective.status == Objective.Status.Completed)
        {
            // Disable the marker at the mailbox
            mailboxMarker.SetActive(false);
        }
    }
}
