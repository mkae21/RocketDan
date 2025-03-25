using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(-1,0);
    }
}
