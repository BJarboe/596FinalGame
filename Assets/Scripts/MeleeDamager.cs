using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeDamager : MonoBehaviour
{
    [SerializeField] private BoxCollider col;
    public EnemyBehavior enemy;

    private bool isEnemyAttacking;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        enemy = GetComponent<EnemyBehavior>();
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
        //Damage or respawn player
    }
}
