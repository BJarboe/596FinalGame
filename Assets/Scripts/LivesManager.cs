using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement pM;
    public int maxLives;

    [SerializeField]
    private ObjectiveManager oM;

    [SerializeField]
    private TMPro.TextMeshProUGUI instructions;

    [SerializeField]
    private EnemyBehavior jason;
    public bool godMode;

    private void Start()
    {
        godMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!godMode && pM.deathCount > maxLives)
            SceneManager.LoadScene("GameOver");

        if (!godMode && Input.GetKeyDown(KeyCode.G))
        {
            godMode = true;
            StartCoroutine(Prompt());
        }

        if (godMode)
        {
            DisableJason();
            oM.final_objective_active = godMode;
        }
    }

    void DisableJason()
    {
        jason.SetSightRange(0);
        jason.Patrolling();
        jason.transform.position = Vector3.zero;
        Debug.Log("BANISHED");
    }

    IEnumerator Prompt()
    {
        if (godMode)
            instructions.text = "ENTERING GOD MODE";
        yield return new WaitForSeconds(1);
        instructions.text = "";
    }

}
