using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemImage;


    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.Log("player not found");
        }
        inventory = player.GetComponent<Inventory>();
    }

    public void AddToInventory()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFull[i] == false)
            {
                // ITEM CAN BE ADDED TO INVENTORY
                inventory.isFull[i] = true;
                Instantiate(itemImage, inventory.slots[i].transform, false);
                Destroy(gameObject);
                break;
            }
        }
    }
}
