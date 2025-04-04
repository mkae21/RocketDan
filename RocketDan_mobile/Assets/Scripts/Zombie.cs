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
    [SerializeField] private float rayOffsetX = 0.7f;
    [SerializeField] private float rayOffsetY = 0.6f;
    [SerializeField] private float rayHeadOffSetX = 0.3f;
    [SerializeField] private float rayHeadOffSetY = 1f;

    private LayerMask layerMask;
    private Vector2 jumpDirection = new Vector2(0,1f).normalized;
    private Vector2 rayPos;
    private Vector2 rayHeadPos;
    private bool isJump;
    private bool isStepped; 
    private bool isWall;

    private Rigidbody2D rb;
    private Animator anim;

#region 이벤트
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = 3f;
        isJump = false;
        isStepped = false;
        isFloor = true;
        isWall = false;

        CheckFirst();
        CheckHead();
    }

    void FixedUpdate()
    {
        ZombieMove();

        // //앞 검사
        // rayPos = new Vector2(transform.position.x - rayOffsetX, transform.position.y + rayOffsetY);
        // Debug.DrawRay(rayPos, Vector2.left * 0.2f, Color.red);

        // //위 검사
        // rayHeadPos = new Vector2(transform.position.x - rayHeadOffSetX , transform.position.y + rayHeadOffSetY);
        // Debug.DrawRay(rayHeadPos, Vector2.up * 0.2f, Color.red);
    }


    void OnCollisionEnter2D(Collision2D collision)
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
                    Jump();
                }
            }
        }
        // 바닥에 닿았는지 확인
        if(collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("바닥에 닿음");
            isFloor = true;
        }
    }

#endregion

#region public 메서드
    public void OnAttack()
    {
        Debug.Log("공격중");
    }

    public void InitLayer()
    {
        layerMask = 1  << gameObject.layer;
        Debug.Log("InitLayer: " + LayerMask.LayerToName(gameObject.layer));
    }

#endregion

#region private 메서드
    private void ZombieMove()
    {
        if (!isStepped && rb.velocity.x >= -speed)
            rb.velocity = new Vector2(-speed, rb.velocity.y);

    }


    private void ChangeMass()
    {
        if(isWall)
        {
            rb.mass = 8f;
            anim.SetBool("IsAttacking",true);
        }
        else
        {
            rb.mass = 1f;
            anim.SetBool("IsAttacking",false);
        }
    }

    private void CheckFirst()
    {
        StartCoroutine(CheckFirstCoroutine());
    }
    private void CheckHead()
    {
        StartCoroutine(CheckHeadCoroutine());
    }

    private void MoveBack()
    {
        StartCoroutine(MoveBackCoroutine());
    }

    private void Jump(){
        StartCoroutine(JumpCoroutine());
    }
#endregion

#region 코루틴
    IEnumerator CheckFirstCoroutine()
    {
        
        while(true)
        {
            rayPos = new Vector2(transform.position.x - rayOffsetX, transform.position.y + rayOffsetY);
            Debug.DrawRay(rayPos, Vector2.left * 0.2f, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.left, 0.2f);

            if(hit.collider != null && hit.collider.CompareTag("Hero"))
            {
                Debug.Log("내가 1등이다");
                isWall = true;
                ChangeMass();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator CheckHeadCoroutine()
    {
        while(true)
        {
            rayHeadPos = new Vector2(transform.position.x - rayHeadOffSetX , transform.position.y + rayHeadOffSetY);
            Debug.DrawRay(rayHeadPos, Vector2.up * 0.2f, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayHeadPos, Vector2.up, 0.2f,layerMask);

            if(hit.collider != null && hit.collider.CompareTag("Zombie") && !isJump && !isStepped && isFloor && isWall)
            {
                Debug.Log("머리 밟힘 Raycast");
                MoveBack();
                
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator JumpCoroutine()
    {
        while (!isWall)
        {
            isJump = true;
            isFloor = false;
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.5f);
            isJump = false;
        }
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
            rb.velocity = new Vector2(backSpeed,rb.velocity.y);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;

        //초기화
        isStepped = false;
        isWall = false; 
        
        ChangeMass();
    }
#endregion

}
