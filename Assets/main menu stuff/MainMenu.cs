using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource click;
    [SerializeField] private float delay = 2.0f; // Delay in seconds, adjust as needed

    public void Play()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        click.Play(); // Play click sound
        yield return new WaitForSeconds(delay); // Wait while sound plays
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next scene
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit game");
    }
}
