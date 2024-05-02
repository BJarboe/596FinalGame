using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // Make sure you have this namespace to access TextMeshPro elements

public class HoverTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textMesh; // Assign this in the inspector
    public Color normalColor = Color.white; // Default color
    public Color hoverColor = Color.red; // Color when hovered
    public AudioSource hoverSound;

    private void Start()
    {
        if (textMesh == null)
            textMesh = GetComponentInChildren<TextMeshProUGUI>(); // Automatically find TextMeshPro in children if not assigned
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.color = hoverColor; // Change to hover color
        hoverSound.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = normalColor; // Change back to normal color
    }
}
