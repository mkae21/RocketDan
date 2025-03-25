using Unity.Mathematics;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float maxSpeed = 3.5f;
    public float minSpeed = 1.3f;
    private float speed;

    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speed = UnityEngine.Random.Range(minSpeed,maxSpeed);
    }
    void FixedUpdate()
    {
        
        rb.velocity = new Vector2(-1,0) * speed;
    }
}
