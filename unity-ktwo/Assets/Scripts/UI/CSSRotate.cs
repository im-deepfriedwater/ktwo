using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSRotate : MonoBehaviour
{
    public float rotationSpeed = -100f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
