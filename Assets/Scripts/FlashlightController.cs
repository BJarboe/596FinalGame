using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    // Public variables
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;

    // Private variables
    private Light flashlight;
    private AudioSource audioSource;

    //private Vector3 v3Offset;
    //private GameObject goFollow;
    //private float speed = 0.5f;

    private void Start()
    {
        // Get Light component in the same GameObject
        flashlight = GetComponent<Light>();

        //goFollow = Camera.main.gameObject;
        //v3Offset = transform.position - goFollow.transform.position;

        if (flashlight == null)
        {
            Debug.LogWarning("Light component is not attached. Attach a Light component manually.");
        }
        else
        {
            flashlight.enabled = false;
        }

        // Get or add AudioSource component to the same GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled;

                // Play audio effect based on flashlight state
                if (flashlight.enabled)
                {
                    PlayAudioEffect(turnOnSound);
                }
                else
                {
                    PlayAudioEffect(turnOffSound);
                }

                //transform.position = goFollow.transform.position + v3Offset;
                //transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
            }
            else
            {
                Debug.LogWarning("Cannot control flashlight as Light component is not attached.");
            }
        }
    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}