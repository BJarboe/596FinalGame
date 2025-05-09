using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Camera myCamera;
    public AnimationCurve curve;

    public bool skipScene = false;

    public GameObject player;
    public string movementScript;
    
    private bool GUIActive;
    private bool checkMove;
    private bool checkF;
    private int sceneNum;

    public GameObject BoxName;
    public GameObject BoxText;
    public TMPro.TextMeshProUGUI Name;
    public TMPro.TextMeshProUGUI Text;
    public TMPro.TextMeshProUGUI Click;
    public TMPro.TextMeshProUGUI Instructions;
    public TMPro.TextMeshProUGUI TODO;
    public TMPro.TextMeshProUGUI Atm;
    public TMPro.TextMeshProUGUI Laundry;
    public TMPro.TextMeshProUGUI Mail;
    public TMPro.TextMeshProUGUI Grocery;
    public GameObject notif1;
    public GameObject notif2;
    public GameObject notif3;

    public AudioSource phoneRing;
    public AudioSource aud1a;
    public AudioSource aud1b;
    public AudioSource aud1c;
    public AudioSource aud1d;
    public AudioSource aud1e;
    public AudioSource aud1f;
    public AudioSource phoneNotif;
    public AudioSource aud1g;
    public AudioSource aud1h;
    public AudioSource phoneDisconn;

    public AudioSource aud2a;
    public AudioSource aud2b;
    public AudioSource aud2c;

    public ObjectiveManager om;
    //public int NumObjectives; // set in inspector

    /* amount completed boolean:
         om.completed == NumObjectives;
    */


    public VideoManager vm;

    /* Method to play cutscene
        vm.PlayCutscene( CUTSCENE-NUMBER );
        yield return new WaitForSeconds( CUTSCENE-DURATION );
    */


    public static DialogueManager Instance { get; private set; } // Singleton instance 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    } // allows other scripts to call functions without messing with memory

    // Start is called before the first frame update
    void Start()
    {
        checkF = false;
        checkMove = false;
        if (!skipScene)
            (player.GetComponent(movementScript) as MonoBehaviour).enabled = false; ////

        sceneNum = 0;
        
        BoxName.SetActive(false);
        BoxText.SetActive(false);
        Name.text = "";
        Text.text = "";
        Click.text = "";
        Instructions.text = "";

        TODO.text = "";
        Atm.text = "";
        Laundry.text = "";
        Mail.text = "";
        Grocery.text = "";

        notif1.SetActive(false);
        notif2.SetActive(false);
        notif3.SetActive(false);

        if (!skipScene)
            StartCoroutine("phoneDial"); ////
    }

    // check if coroutine finishes to avoid issues with controls
    private bool isCoroutineRunning = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) & GUIActive)
        {
            sceneNum++;
            switch (sceneNum)
            {
                case 1:
                    StartCoroutine("Scene1b");
                    break;
                case 2:
                    StartCoroutine("Scene1c");
                    break;
                case 3:
                    StartCoroutine("Scene1d");
                    break;
                case 4:
                    StartCoroutine("Scene1e");
                    break;
                case 5:
                    StartCoroutine("Scene1f");
                    break;
                case 6:
                    StartCoroutine("Scene1g");
                    break;
                case 7:
                    StartCoroutine("Scene1h");
                    break;
                case 8:
                    GUIActive = false;
                    StartCoroutine("Scene2");
                    break;
            }
        }

        if (sceneNum >= 8 && isCoroutineRunning == false)
        {
            (player.GetComponent(movementScript) as MonoBehaviour).enabled = true;
        }

        if (checkMove == false & Input.GetKeyDown(KeyCode.W) & sceneNum == 8)
        {
            Instructions.text = "";
            checkMove = true;
        }

        if (checkF == false & Input.GetKeyDown(KeyCode.F) & sceneNum == 8)
        {
            Instructions.text = "";
            checkF = true;
        }
    } 

    IEnumerator phoneDial()
    {
        phoneRing.Play();
        yield return new WaitForSeconds(4f);
        GUIActive = true;

        StartCoroutine("Scene1a");
    }

    IEnumerator Scene1a()
    {
        BoxName.SetActive(true);
        BoxText.SetActive(true);

        StartCoroutine("SmoothLerp");

        Name.text = "JUNIA";
        Text.text = "Hey Meg, where are you? I can't see your car anywhere.";
        Click.text = "CLICK TO CONTINUE";
        aud1a.Play();
        yield break;
    }

    private IEnumerator SmoothLerp()
    {
        isCoroutineRunning = true;
        float rotationAmount = 45f;
        Quaternion startRotation = myCamera.transform.rotation;
        Quaternion leftRotation = startRotation * Quaternion.Euler(0, -rotationAmount, 0);
        Quaternion rightRotation = startRotation * Quaternion.Euler(0, rotationAmount, 0);

        float elapsedTime = 0;
        float duration = 0.8f; // Duration for each rotation phase

        // Turn left with easing
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            myCamera.transform.rotation = Quaternion.Slerp(startRotation, leftRotation, curve.Evaluate(t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1); // Pause at the left
        elapsedTime = 0;

        // Turn right with easing
        while (elapsedTime < duration * 2) // Longer duration for a larger movement
        {
            float t = elapsedTime / (duration * 2);
            myCamera.transform.rotation = Quaternion.Slerp(leftRotation, rightRotation, curve.Evaluate(t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1); // Pause at the right
        elapsedTime = 0;

        // Return to starting position with easing
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            myCamera.transform.rotation = Quaternion.Slerp(rightRotation, startRotation, curve.Evaluate(t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isCoroutineRunning = false;
    }  

    IEnumerator Scene1b()
    {
        phoneRing.Stop();
        aud1a.Stop(); 
        Name.text = "MEG";
        Text.text = "Wait, I thought I told you. I can't tonight. I'm at my sister's right now to look \nafter her kids.";
        aud1b.Play();
        yield break;
    }

    IEnumerator Scene1c()
    {
        aud1b.Stop(); 
        Name.text = "JUNIA";
        Text.text = "Oh, but you said we we're gonna run errands together and it's kind of late.";
        aud1c.Play();
        yield break;
    }

    IEnumerator Scene1d()
    {
        aud1c.Stop();
        Name.text = "MEG";
        Text.text = "I know, I know. I'm sorry and I promise I'll make it up to you, but I can't \nleave the kids.";
        aud1d.Play();
        yield break;
    }

    IEnumerator Scene1e()
    {
        aud1d.Stop();
        Name.text = "JUNIA";
        Text.text = "It's totally fine. I get it. I guess I'll see you tommorow morning then?";
        aud1e.Play();
        yield break;
    }

    IEnumerator Scene1f()
    {
        aud1e.Stop();
        Name.text = "MEG";
        Text.text = "Yes, for sure. Tomorrow morning.";
        aud1f.Play();
        yield break;
    }

    IEnumerator Scene1g()
    {
        aud1f.Stop();
        Name.text = "MEG";
        Text.text = "Okay, good night then.";
        aud1g.Play();
        yield break;
    }

    IEnumerator Scene1h()
    {
        aud1g.Stop();
        Name.text = "MEG";
        Text.text = "Love you. See you later.";
        aud1h.Play();
        yield break;
    }

    IEnumerator Scene2()
    {
        aud1h.Stop();
        phoneDisconn.Play();
        Instructions.text = "PRESS WASD TO MOVE AND SHIFT TO SPRINT";
        Name.text = "JUNIA";
        Text.text = "I'm off work now, but I still have things to do. Let's look at the list.";
        Click.text = "";
        aud2a.Play();
        yield return new WaitForSeconds(5.5f);
        phoneNotif.Play();
        TODO.text = "TO DO:";
        Atm.text = "collect money from the ATM";
        Laundry.text = "pick up clothes from laundromat";
        Mail.text = "drop off mail";
        Grocery.text = "buy groceries";
        yield return new WaitForSeconds(0.5f);
        Text.text = "Alright. So I need to go to the laundromat, get groceries, drop off mail, and \nget some cash from the ATM.";
        aud2b.Play();
        yield return new WaitForSeconds(9.6f);
        Text.text = "Damn, it's so dark out. Where's my phone?";
        aud2c.Play(); 
        yield return new WaitForSeconds(5.7f);
        BoxName.SetActive(false);
        BoxText.SetActive(false);
        Name.text = "";
        Text.text = "";
        if (checkF != true)
        {
            Instructions.text = "PRESS F TO TURN ON FLASHLIGHT";
        }
    }
    
    public IEnumerator Scene4()
    {
        notif1.SetActive(true);
        phoneNotif.Play();
        yield return new WaitForSeconds(2f);
        notif2.SetActive(true);
        phoneNotif.Play();
        yield return new WaitForSeconds(2f);
        notif3.SetActive(true);
        phoneNotif.Play();
        yield return new WaitForSeconds(4f);
        notif1.SetActive(false);
        notif2.SetActive(false);
        notif3.SetActive(false);
    }
}
