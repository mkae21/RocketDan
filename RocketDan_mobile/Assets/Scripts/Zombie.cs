using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Zombie Move Settings")]

    public bool isFloor;
    private float speed;

    [Header("Zombie Speed Settings")]
    [SerializeField] private float jumpForce = 18f;
    [SerializeField] private float backSpeed = 2f;
    private Vector2 jumpDirection = new Vector2(0,1f).normalized;
    private Vector2 rayPos;
    private bool isJump;
    private bool isStepped; 
    private bool isWall;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 3f;
        isJump = false;
        isStepped = false;
        isFloor = true;
        isWall = false;
    }

    void FixedUpdate()
    {
        ZombieMove();
        rayPos = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.6f);
        Debug.DrawRay(rayPos, Vector2.left * 0.2f, Color.red);
    }

    private void ZombieMove()
    {
        if (!isStepped && rb.velocity.x >= -speed)
            rb.velocity = new Vector2(-speed, rb.velocity.y);

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 좀비끼리 충돌했을 때
        if (collision.gameObject.CompareTag("Zombie"))
        {
            if(!isJump && !isStepped)
            {
                float other = collision.transform.position.x;
                float me = transform.position.x;

                if (me > other)
                {
                    Debug.Log("같은 레이어 좀비와 충돌 → 점프!");
                    JumpOver();
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        // 바닥에 닿았는지 확인
        if(other.gameObject.CompareTag("Floor"))
        {
            Debug.Log("바닥에 닿음");
            isFloor = true;
        }

        if(other.gameObject.CompareTag("Hero"))
        {
            isWall = true;
            ChangeMass();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie") && !isJump && !isStepped)
        {
            float other = collision.transform.position.y;
            float me = transform.position.y;


            if(me < other && isFloor && isWall){
                Debug.Log("✅ 머리 밟힘 조건 통과 → 뒤로 이동!");
                MoveBack();
            }
            else
            {
                if (me >= other) Debug.Log("❌ 머리보다 위에 있지 않음");
                if (!isFloor) Debug.Log("❌ 바닥에 닿아있지 않음");
                if (!isWall) Debug.Log("❌ 벽(영웅)에 닿아있지 않음");
            }
        }
    }

    

    IEnumerator Jump()
    {
        if(!isWall){
            isJump = true;
            isFloor = false;
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.5f);
            isJump = false;
        }
    }
    private void ChangeMass()
    {
        if(isWall)
            rb.mass = 8f;
        else
            rb.mass = 1f;
    }

    private void CheckFirst()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.left, 0.2f);

        if(hit.collider != null && hit.collider.CompareTag("Hero"))
        {
            Debug.Log("내가 1등이다");
            isWall = true;
        }
    }
    private void MoveBack()
    {
        StartCoroutine(MoveBackCoroutine());
    }

    private void JumpOver(){
        StartCoroutine(Jump());
    }

    IEnumerator MoveBackCoroutine()
    {
        // 이동 중지
        rb.velocity = Vector2.zero;

        float timer = 0f;
        float duration = 0.3f;

        isStepped = true;

        while (timer < duration)
        {
            rb.velocity = new Vector2(backSpeed,0);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;

        //초기화
        isStepped = false;
        isWall = false; 
        
        // 다시 체크, 애매하게 끼인 경우를 위해
        CheckFirst();
        ChangeMass();
    }
}
