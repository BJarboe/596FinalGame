using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement pM;
    public int maxLives;

    // Update is called once per frame
    void Update()
    {
        if (pM.deathCount > maxLives)
            SceneManager.LoadScene("GameOver");
    }
}
