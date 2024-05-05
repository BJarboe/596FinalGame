using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeDamager : MonoBehaviour
{
    public VideoManager vm;
    [SerializeField] private BoxCollider col;
    public EnemyBehavior enemy;
    public PlayerMovement player;

    private bool isEnemyAttacking;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehavior>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        isEnemyAttacking = enemy.GetIsAttacking();
        if (isEnemyAttacking)
        {
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Play death cutscene
        /* Method to play cutscene
        vm.PlayCutscene( CUTSCENE-NUMBER );
        yield return new WaitForSeconds( CUTSCENE-DURATION );
        */

        //reset player and enemy
    }
}
