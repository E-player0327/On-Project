using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int jumpAmount;
    [SerializeField] private int dashAmount;
    [SerializeField] private float groundCastDistance;
    [SerializeField] private Vector2 groundCastSize;
    [SerializeField] private float enemyCastDistance;
    [SerializeField] private Vector2 enemyCastSize;

    public float MoveSpeed { get { return moveSpeed; } }
    public float JumpPower { get { return jumpPower; } }
    public int JumpAmount { get { return jumpAmount; } }
    public int DashAmount { get { return dashAmount; } }

    [SerializeField] SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D;

    int jumpCount;

    void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);

        rigidbody2D = GetComponent<Rigidbody2D>();

        jumpCount = jumpAmount; 
    }

    void Update()
    {
        bool hitGround = Physics2D.BoxCast(transform.position, groundCastSize, 0f, Vector2.down, groundCastDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D[] EnemyHit = Physics2D.BoxCastAll(transform.position, enemyCastSize, 0f, Vector2.right *(spriteRenderer.flipX ? -1 : 1), enemyCastDistance);

        if(Input.GetButton("Horizontal"))
        {
            rigidbody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rigidbody2D.velocity.y);
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        else if(Input.GetButtonUp("Horizontal"))
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            jumpCount -= 1;
        }
        else if(hitGround)
        {
            jumpCount = jumpAmount;
        }

        foreach(RaycastHit2D hit in EnemyHit)
        {
            if(hit.collider.CompareTag("Enemy"))
            {

            }
            
            if(hit.collider.CompareTag("Boss"))
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashAmount > 0)//좌쉬프트 누르면 대쉬하게
        {
            if (canDash)
            {
                StartCoroutine(Dash());
                dashAmount--;
            }
        }
    }

    IEnumerator Dash() //대쉬
    {
        canMove = false;
        canDash = false;
        Vector3 mousePos = .ScreenToWorldPoint(Input.mousePosition); //마우스의 위치를 월드위치로 변환
        Vector2 target = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y); //방향을 정함
        rigidbody2D.velocity = Vector2.zero;
        isDash = true;
        for (int i = 0; i < 5; i++) //방향으로 움직임
        {
            RaycastHit2D isGroundHit = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0f, Vector2.up, 0f, LayerMask.GetMask("Ground"));
            if (isGroundHit.collider != null)
                break;
            ObjectPool.GetObject(ObjectPool.instance.prefebs[2], null);
            rigidbody2D.position = rigidbody2D.position + target.normalized * dashSpeed;
            yield return new WaitForFixedUpdate();
        }
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        isDash = false;
        canMove = true;
        canDash = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - groundCastDistance), groundCastSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (spriteRenderer.flipX ? -1 : 1) * enemyCastDistance, transform.position.y), enemyCastSize);
    }
}