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
    [SerializeField] private GameObject dashEffectPrefeb;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D;

    int jumpCount;
    int lookDir;
    int dashCount;
    float dashCooltime = 0;
    bool isDash = false;
    bool canMove = true;
    

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        jumpCount = jumpAmount;
        dashCount = dashAmount;
    }

    void Update()
    {
        lookDir = (spriteRenderer.flipX ? -1 : 1);
        bool hitGround = Physics2D.BoxCast(transform.position, groundCastSize, 0f, Vector2.down, groundCastDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D[] EnemyHit = Physics2D.BoxCastAll(transform.position, enemyCastSize, 0f, Vector2.right * lookDir, enemyCastDistance);

        if (canMove)
        {
            if (Input.GetButton("Horizontal"))
            {
                rigidbody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rigidbody2D.velocity.y);
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            }
            if (Input.GetButtonUp("Horizontal"))
            {
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                jumpCount -= 1;
            }
            else if (hitGround)
            {
                jumpCount = jumpAmount;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0 && isDash != true)//좌쉬프트 누르면 대쉬하게
            {
                dashCount--;
                StartCoroutine(Dash());
            }

            if(dashCount < dashAmount)
            {
                dashCooltime += Time.deltaTime;
                if(dashCooltime > 4)
                {
                    dashCount += 1;
                    dashCooltime = 0;
                }
            }
        }


        foreach (RaycastHit2D hit in EnemyHit)
        {
            if (hit.collider.CompareTag("Enemy"))
            {

            }

            if (hit.collider.CompareTag("Boss"))
            {

            }
        }

    }

    IEnumerator Dash()
    {
        canMove = false;
        isDash = true;
        for (int i = 0; i < 5; i++)
        {
            transform.Translate(new Vector3(lookDir, 0, 0));
            var dashEffect = Instantiate(dashEffectPrefeb, transform.position, Quaternion.identity);
            dashEffect.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            yield return new WaitForSeconds(0.005f);
        }
        canMove = true;
        isDash = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - groundCastDistance), groundCastSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + lookDir * enemyCastDistance, transform.position.y), enemyCastSize);
    }
}