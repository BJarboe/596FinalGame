using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    private Light sign;
    public float temperature;
    public int padding;
    private int timer;
    void Start()
    {
        sign = GetComponent<Light>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < padding)
        {
            timer++;
            return;
        }

        timer = 0;
        float rand = Random.Range(0.0f, 100f);
        if (rand < temperature)
            sign.enabled = false;
        else
            sign.enabled = true;
    }
}
