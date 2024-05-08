using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;



public class VideoManager : MonoBehaviour
{
    public List<VideoPlayer> cutscenes = new List<VideoPlayer>();
    public GameObject player;
    public string movementScript;
    public Canvas ui;
    private enum Progress { START, HALFWAY, FINISH, TERMINATED}
    private Progress progress;

    [SerializeField]
    private ObjectiveManager om;

    public static VideoManager Instance { get; private set; } // Singleton Instance
    private void Awake()
    {
        progress = Progress.START;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        if (om.completed == om.halfway_mark && progress == Progress.START)
        {
            progress = Progress.HALFWAY;
            PlayCutscene(5);
        }
    }

    public void PlayCutscene(int num) { StartCoroutine(PlayVid(cutscenes[num]));}

    IEnumerator PlayVid(VideoPlayer vidplayer)
    {
        yield return new WaitForSeconds(2);
        ui.enabled = false;
        (player.GetComponent(movementScript) as MonoBehaviour).enabled = false;
        vidplayer.Play();
        yield return new WaitForSeconds(1); // padding for condition check
        yield return new WaitUntil(() => !vidplayer.isPlaying);
        (player.GetComponent(movementScript) as MonoBehaviour).enabled = true;
        vidplayer.Stop();
        ui.enabled = true;
    }
}
