using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float invincibleTime;
    [SerializeField] private int jumpAmount;
    [SerializeField] private int dashAmount;
    [SerializeField] private float dashChargeTime;
    [SerializeField] private int hp;
    [SerializeField] private float groundCastDistance;
    [SerializeField] private Vector2 groundCastSize;
    [SerializeField] private float enemyCastDistance;
    [SerializeField] private Vector2 enemyCastSize;
    [SerializeField] private GameObject dashEffectPrefeb;
    [SerializeField] private GameObject HpBar;

    public int Hp { get { return hp; } }
    public int DashAmount { get { return dashAmount; } }
    public float DashChargeTime { get { return dashChargeTime; } }

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D;

    int jumpCount;
    int lookDir;
    public int currentDashAmount { get; private set; }
    public int currentHp { get; private set; }
    public float currentDashChargeTime { get; private set; }
    public bool hitGround { get; private set; }
    float currentInvincibleTime = 0;
    float stunTime;
    bool isDash = false;
    bool canMove = true;
    bool isInvincible = false;


    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        dashAmount = 4;
        jumpCount = jumpAmount;
        currentDashAmount = dashAmount;
        currentHp = hp;
    }

    void Update()
    {
        lookDir = (spriteRenderer.flipX ? -1 : 1);
        hitGround = Physics2D.BoxCast(transform.position, groundCastSize, 0f, Vector2.down, groundCastDistance, LayerMask.GetMask("Ground"));
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && currentDashAmount > 0 && isDash != true)//좌쉬프트 누르면 대쉬하게
            {
                currentDashAmount--;
                StartCoroutine(Dash());
            }

            if(Input.GetKeyDown(KeyCode.Z))
            {
                foreach(RaycastHit2D hit in EnemyHit)
                {
                    if (hit.collider.CompareTag("Core"))
                    {
                        Boss.instance.currentHp -= 10;
                    }
                }
            }

            if (currentDashAmount < dashAmount)
            {
                currentDashChargeTime += Time.deltaTime;
                if (currentDashChargeTime > dashChargeTime)
                {
                    currentDashAmount += 1;
                    currentDashChargeTime = 0;
                }
            }
        }

        if (currentInvincibleTime > 0)
        {
            currentInvincibleTime -= Time.deltaTime;
            isInvincible = true;
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            isInvincible = false;
            spriteRenderer.color = Color.white;
        }

        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            canMove = false;
        }
        else
            canMove = true;
    }

    IEnumerator Dash()
    {
        canMove = false;
        isDash = true;
        rigidbody2D.gravityScale = 0;
        rigidbody2D.velocity = Vector2.zero;
        for (int i = 0; i < 5; i++)
        {
            transform.Translate(new Vector3(lookDir, 0, 0));
            var dashEffect = Instantiate(dashEffectPrefeb, transform.position, Quaternion.identity);
            dashEffect.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            yield return new WaitForSeconds(0.005f);
        }
        canMove = true;
        isDash = false;
        rigidbody2D.gravityScale = 3.5f;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - groundCastDistance), groundCastSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + lookDir * enemyCastDistance, transform.position.y), enemyCastSize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible)
        {
            if (collision.CompareTag("Fireball"))
            {
                Hurt(10);
            }

            if (collision.CompareTag("Hand") && Boss.instance.isAttack)
            {
                Hurt(20);
            }
        }
    }

    public void Hurt(int damage)
    {   
        currentHp -= damage;
        currentInvincibleTime = invincibleTime;
        Stun(0.3f);
    }

    public void Stun(float time)
    {
        stunTime = time;
    }
}