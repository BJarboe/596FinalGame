using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public float rotationSpeed;
    private enum Axis {  x, y, z };
    [SerializeField]
    private Axis axis;

    // Update is called once per frame
    void Update()
    {
        switch (axis)
        {
            case Axis.x:
                transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0); break;
            case Axis.y:
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); break;
            case Axis.z:
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); break;
        }
    }
}
