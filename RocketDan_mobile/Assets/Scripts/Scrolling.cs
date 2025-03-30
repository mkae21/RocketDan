using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public float speed = 0.1f;
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed);
    }
}
