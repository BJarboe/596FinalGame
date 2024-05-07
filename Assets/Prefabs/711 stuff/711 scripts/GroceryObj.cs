using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryObj : MonoBehaviour
{
    public int itemCount;
    private long padding;

    public TMPro.TextMeshProUGUI instructions;


    public static GroceryObj Instance { get; private set; } // Singleton instance 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        itemCount = 0;
        padding = 0;
    }

    private void Update()
    {
        if (padding > 2000)
        {
            Debug.Log($"Item Count = {itemCount}");
            padding = 0;
        }
        else
            padding++;
    }

}
