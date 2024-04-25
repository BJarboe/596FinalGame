using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Usage:
 *      Objectives defined as objects in 'Objective' class
 *      
 *      Simple functions provided
 *          - functions: StartObjective, CompleteObjective, FailObjective
 *          - allows each actual objective to vary in game logic
 *          - can be called by other game-objects / events
 *      
 *      When all objectives are completed, player enters end-game with a final objective
 */



[System.Serializable]
public class Objective
{
    public string id;
    public string description;
    public enum Status { Inactive, Active, Completed, Failed};
    public Status status = Status.Inactive;
}
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    private List<Objective> objectives = new List<Objective>();
    public bool final_objective_active = false;

    
    public static ObjectiveManager Instance { get; private set; } // Singleton instance 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    } // allows other scripts to call functions without messing with memory

    public Objective GetObjective(string name)
    {
        return objectives.Find(o => o.id == name);
    }


    public bool StartObjective(string id)
    {
        Objective obj = GetObjective(id); // search for object with corresponding id

        if (obj == null || obj.status != Objective.Status.Inactive) return false;

        // ctrl-shft-/ to uncomment:    enables objective lock system
        /*foreach (Objective o in objectives)
        {
            if (o.status == Objective.Status.Active)
            {
                Debug.Log($"Tried to start {id} but {obj} is active..");
                return false;
            }
        }*/

        obj.status = Objective.Status.Active;
        Debug.Log($"Activated {obj.id}: {obj.description}");
        return true;
    }

    public void CompleteObjective(string id)
    {
        Objective obj = objectives.Find(o => o.id == id);
        if (obj == null || obj.status != Objective.Status.Active) return;

        obj.status = Objective.Status.Completed;
        Debug.Log($"Objective {obj.id} completed.");
        CheckCompletion();
    }

    public void FailObjective(string id)
    {
        Objective obj = objectives.Find(o => o.id == id);
        if (obj == null || obj.status != Objective.Status.Active) return;

        obj.status= Objective.Status.Failed;
        Debug.Log($"Objective {obj.id} failed.");
        // failure handling..
    }

    public void CheckCompletion()
    {
        foreach (Objective obj in objectives)
        {
            if (obj.status != Objective.Status.Completed) 
                return;
        }
        final_objective_active = true;
        FinalObjective();
    }

    public void FinalObjective()
    {
        if (final_objective_active)
        {
            Debug.Log("All objectives complete, entering end-game..");
            // end-game stuff .....
        }
    }
}
