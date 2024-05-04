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
    public static VideoManager Instance { get; private set; } // Singleton Instance



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
