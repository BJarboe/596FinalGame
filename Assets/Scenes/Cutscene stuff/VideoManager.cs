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
    public bool freezeJason;
    private enum Progress { START, HALFWAY, FINISH, TERMINATED}
    [SerializeField]
    private Progress progress;

    [SerializeField]
    private EnemyBehavior jason;

    [SerializeField]
    private ObjectiveManager om;

    public static VideoManager Instance { get; private set; } // Singleton Instance
    private void Awake()
    {
        progress = Progress.START;
        freezeJason = false;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        switch (progress)
        {
            case Progress.START:
                if (om.completed == om.halfway_mark)
                {
                    progress = Progress.HALFWAY;
                    PlayCutscene(5);
                }
                break;
            case Progress.HALFWAY:
                if (om.final_objective_active)
                {
                    progress = Progress.FINISH;
                }
                break;
        }

        if (freezeJason)
        {
            jason.playerInAttackRange = false;
            jason.playerInSightRange = false;
            jason.Patrolling();
        }
    }

    public void PlayCutscene(int num) { StartCoroutine(PlayVid(cutscenes[num]));}

    IEnumerator PlayVid(VideoPlayer vidplayer)
    {
        freezeJason = true;
        yield return new WaitForSeconds(2);
        ui.enabled = false;
        (player.GetComponent(movementScript) as MonoBehaviour).enabled = false;
        vidplayer.Play();
        yield return new WaitForSeconds(1); // padding for condition check
        yield return new WaitUntil(() => !vidplayer.isPlaying);
        (player.GetComponent(movementScript) as MonoBehaviour).enabled = true;
        vidplayer.Stop();
        ui.enabled = true;
        freezeJason = false;
    }
}
