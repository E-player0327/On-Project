using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    public static Boss instance { get; private set; }

    [SerializeField] int hp;
    [SerializeField] float lasertime;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
    [SerializeField] GameObject leftarm; //-6, 0
    [SerializeField] GameObject rightarm; //6, 0
    [SerializeField] GameObject laser;
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject core;

    public int Hp { get { return hp; } }

    public int currentHp { get; set; }
    public bool isAttackCycle { get; private set; }
    public bool isAttack { get; private set; }

    Animator headAnimator;
    Animator bodyAnimator;
    LineRenderer laserLineRenderer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        bodyAnimator = body.GetComponent<Animator>();
        headAnimator = head.GetComponent<Animator>();
        laserLineRenderer = laser.GetComponent<LineRenderer>();
    }

    void Start()
    {
        currentHp = hp;
    }


    private void Update()
    {
        if (currentHp > 0 && isAttackCycle != true)
            StartCoroutine(NormalAttack());
    }


    IEnumerator CoreOpen()
    {
        yield return new WaitForSeconds(0.7f);
        isAttack = true;
        leftarm.transform.DOMove(new Vector2(-6.0f, -2.5f), 0.6f).SetEase(Ease.OutBounce);
        rightarm.transform.DOMove(new Vector2(6.0f, -2.5f), 0.6f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.3f);
        isAttack = false;
        CameraShake.instance.smoothShakeCamera(5, 1);
        yield return new WaitForSeconds(0.7f);
        bodyAnimator.enabled = true;
        bodyAnimator.Play("body-core_open");
        yield return new WaitForSeconds(1.5f);
        core.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        core.SetActive(false);
        bodyAnimator.Play("body-core_close");
        yield return new WaitForSeconds(1f);
        leftarm.transform.DOMove(new Vector2(-6.0f, 0f), 1f).SetEase(Ease.OutCubic);
        rightarm.transform.DOMove(new Vector2(6.0f, 0f), 1f).SetEase(Ease.OutCubic);
        isAttackCycle = false;
    }

    IEnumerator NormalAttack()
    {
        isAttackCycle = true;
        bodyAnimator.enabled = true;
        bodyAnimator.Play("body-idle");
        head.GetComponent<HeadAnimation>().time = 0;
        head.GetComponent<HeadAnimation>().isHeadIdle = true;
        yield return new WaitForSeconds(1.0f);
        bodyAnimator.enabled = false;
        head.GetComponent<HeadAnimation>().isHeadIdle = false;

        leftarm.transform.DOMove(new Vector2(-7.07f, 0.82f), 0.5f).SetEase(Ease.OutCubic);
        body.transform.DOMove(new Vector2(-0.2f, 0.3f), 0.3f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        isAttack = true;
        head.transform.DOMove(new Vector2(1.2f, 0.3f), 0.5f).SetEase(Ease.Unset);
        leftarm.transform.DOMove(new Vector2(1.5f, -2.5f), 0.5f).SetEase(Ease.InCubic);
        body.transform.DOMove(new Vector2(1f, -0.7f), 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        CameraShake.instance.smoothShakeCamera(5, 0.5f);
        leftarm.transform.DOMove(new Vector2(-6f, 0f), 1f);

        rightarm.transform.DOMove(new Vector2(7.07f, 0.82f), 0.5f).SetEase(Ease.OutCubic);
        body.transform.DOMove(new Vector2(0.2f, 0.3f), 0.5f).SetEase(Ease.Unset);
        head.transform.DOMove(Vector2.up * 1.5f, 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        rightarm.transform.DOMove(new Vector2(-1.5f, -2.5f), 0.5f).SetEase(Ease.InCubic);
        body.transform.DOMove(new Vector2(-1f, -0.7f), 0.5f).SetEase(Ease.Unset);
        head.transform.DOMove(new Vector2(-1.2f, 0.3f), 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        CameraShake.instance.smoothShakeCamera(5, 0.5f);
        rightarm.transform.DOMove(new Vector2(6f, 0f), 1f);
        body.transform.DOMove(Vector2.zero, 0.5f).SetEase(Ease.Unset);
        head.transform.DOMove(Vector2.up * 1.5f, 0.5f).SetEase(Ease.Unset);
        isAttack = false;
        bodyAnimator.enabled = true;
        head.GetComponent<HeadAnimation>().isHeadIdle = true;

        yield return new WaitForSeconds(0.5f);
        bodyAnimator.enabled = false;
        head.GetComponent<HeadAnimation>().isHeadIdle = false;
        isAttack = true;
        leftarm.transform.DOMove(new Vector2(-6.0f, 2.0f), 0.5f).SetEase(Ease.OutCubic);
        rightarm.transform.DOMove(new Vector2(6.0f, 2.0f), 0.5f).SetEase(Ease.OutCubic);
        body.transform.DOMove(Vector2.up * 0.5f, 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        leftarm.transform.DOMove(new Vector2(-6.0f, -2.5f), 0.5f).SetEase(Ease.OutBounce);
        rightarm.transform.DOMove(new Vector2(6.0f, -2.5f), 0.5f).SetEase(Ease.OutBounce);
        body.transform.DOMove(Vector2.down * 2, 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.22f);
        CameraShake.instance.smoothShakeCamera(10, 1f);
        if (Player.Instance.hitGround)
            Player.Instance.Stun(1.5f);
        yield return new WaitForSeconds(0.28f);
        leftarm.transform.DOMove(new Vector2(-6.0f, -2f), 0.3f).SetEase(Ease.Unset);
        rightarm.transform.DOMove(new Vector2(6.0f, -2f), 0.3f).SetEase(Ease.Unset);
        body.transform.DOMove(Vector2.down * 0.5f, 0.3f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.3f);
        leftarm.transform.DOMove(new Vector2(-1f, -2f), 0.3f).SetEase(Ease.InCubic);
        rightarm.transform.DOMove(new Vector2(1f, -2f), 0.3f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(0.3f);
        leftarm.transform.DOMove(new Vector2(-3f, -2f), 0.5f).SetEase(Ease.Unset);
        rightarm.transform.DOMove(new Vector2(3f, -2f), 0.5f).SetEase(Ease.Unset);
        isAttack = false;
        bodyAnimator.enabled = true;
        head.GetComponent<HeadAnimation>().isHeadIdle = true;
        CameraShake.instance.smoothShakeCamera(5, 0.5f);

        int randAttack = Mathf.FloorToInt(Random.Range(0, 1.99f));

        if (randAttack == 0)
            StartCoroutine(SpecialAttack1());
        else if (randAttack == 1)
            StartCoroutine(SpecialAttack2());
    }

    IEnumerator SpecialAttack1() //???????? ?????? ????(?????? ???? ????)
    {
        float temp = -90;
        yield return new WaitForSeconds(1);
        bodyAnimator.enabled = false;
        head.GetComponent<HeadAnimation>().isHeadIdle = false;
        body.transform.DOMove(Vector2.up * 1, 1f);
        leftarm.transform.DOMove(new Vector2(-6f, 0f), 1f);
        rightarm.transform.DOMove(new Vector2(6f, 0f), 1f);
        head.transform.DOMove(Vector2.up * 2, 1f);
        headAnimator.Play("head-attack_open");
        CameraShake.instance.shakeCamera(3, 1);
        yield return new WaitForSeconds(1);
        CameraShake.instance.smoothShakeCamera(5, 1.5f);
        yield return new WaitForSeconds(1);
        head.transform.DOMove(Vector2.up * 0.3f, 0.5f);
        body.transform.DOMove(Vector2.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        CameraShake.instance.shakeCamera(7, 0.1f);
        for (float i = 0; i < lasertime; i += Time.fixedDeltaTime)
        {
            float radian = temp * Mathf.PI / 180;
            RaycastHit2D hit = Physics2D.Raycast(Vector2.up * 0.2f, new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized, Mathf.Infinity);

            if (hit.collider.CompareTag("Player"))
                Player.Instance.Hurt(20);
            
            Debug.DrawLine(Vector2.up * 0.2f, hit.point, Color.yellow);
            laserLineRenderer.enabled = true;
            laserLineRenderer.SetPosition(0, Vector2.down * 0.5f);
            laserLineRenderer.SetPosition(1, hit.point);
            temp += 7.2f / lasertime;
            yield return new WaitForFixedUpdate();
        }
        laserLineRenderer.enabled = false;
        headAnimator.Play("head-attack_close");
        body.transform.DOMove(Vector2.zero, 1f);
        head.transform.DOMove(Vector2.up, 1f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(CoreOpen());
        yield return null;
    }

    IEnumerator SpecialAttack2() //?? ??????(?????? ????)?? ?????????????? 5?? ????
    {
        yield return new WaitForSeconds(1);
        bodyAnimator.enabled = false;
        head.GetComponent<HeadAnimation>().isHeadIdle = false;
        body.transform.DOMove(Vector2.up * 1, 1f);
        leftarm.transform.DOMove(new Vector2(-6f, 0f), 1f);
        rightarm.transform.DOMove(new Vector2(6f, 0f), 1f);
        head.transform.DOMove(Vector2.up * 2, 1f);
        headAnimator.Play("head-attack_open");


        yield return new WaitForSeconds(2f);
        Vector2 dir;

        for (int i = 0; i < 5; i++)
        {
            dir = Player.Instance.transform.position - transform.position;
            Instantiate(fireball, Vector2.up, Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        laserLineRenderer.enabled = false;
        headAnimator.Play("head-attack_close");
        body.transform.DOMove(Vector2.zero, 1f);
        head.transform.DOMove(Vector2.up, 1f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(CoreOpen());
    }

    
}
