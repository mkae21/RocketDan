using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    private float width;
    void Awake()
    {
        BoxCollider2D backGorundCollider = GetComponent<BoxCollider2D>();
        width = backGorundCollider.size.x;
    }

    void FixedUpdate()
    {
        if(transform.position.x <= -width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2f,0);
        transform.position = (Vector2)transform.position + offset;
    }
}
