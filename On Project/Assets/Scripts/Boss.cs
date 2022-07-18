using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] float lasertime;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
    [SerializeField] GameObject leftarm; //-6, 0
    [SerializeField] GameObject rightarm; //6, 0
    [SerializeField] GameObject laser;

    public int Hp { get { return hp; } }

    public int currentHp { get; private set; }
    bool isAttack = false;

    Animator headAnimator;
    Animator bodyAnimator;
    LineRenderer laserLineRenderer;

    private void Awake()
    {
        bodyAnimator = body.GetComponent<Animator>();
        headAnimator = head.GetComponent<Animator>();
        laserLineRenderer = laser.GetComponent<LineRenderer>();
    }

    void Start()
    {
        currentHp = hp;
        bodyAnimator.Play("body-idle");
    }


    private void Update()
    {
        if (currentHp > 0 && isAttack != true)
            StartCoroutine(NormalPattern1());
    }


    IEnumerator NormalPattern1()
    {
        isAttack = true;
        bodyAnimator.enabled = true;
        head.GetComponent<HeadAnimation>().isHeadIdle = true;
        yield return new WaitForSeconds(1.0f);
        bodyAnimator.enabled = false;
        head.GetComponent<HeadAnimation>().isHeadIdle = false;
        leftarm.transform.DOMove(new Vector2(-7.07f, 0.82f), 0.5f).SetEase(Ease.OutCubic);
        body.transform.DOMove(new Vector2(-0.2f, 0.3f), 0.3f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        leftarm.transform.DOMove(new Vector2(1.5f, -2.5f), 0.5f).SetEase(Ease.InCubic);
        body.transform.DOMove(new Vector2(1f, -0.7f), 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        CameraShake.instance.smoothShakeCamera(5, 0.5f);
        leftarm.transform.DOMove(new Vector2(-6f, 0f), 1f);

        rightarm.transform.DOMove(new Vector2(7.07f, 0.82f), 0.5f).SetEase(Ease.OutCubic);
        body.transform.DOMove(new Vector2(0.2f, 0.3f), 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        rightarm.transform.DOMove(new Vector2(-1.5f, -2.5f), 0.5f).SetEase(Ease.InCubic);
        body.transform.DOMove(new Vector2(-1f, -0.7f), 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        CameraShake.instance.smoothShakeCamera(5, 0.5f);
        rightarm.transform.DOMove(new Vector2(6f, 0f), 1f);
        body.transform.DOMove(Vector2.zero, 0.5f).SetEase(Ease.Unset);
        bodyAnimator.enabled = true;
        head.GetComponent<HeadAnimation>().isHeadIdle = true;

        yield return new WaitForSeconds(2);
        bodyAnimator.enabled = false;
        head.GetComponent<HeadAnimation>().isHeadIdle = false;
        leftarm.transform.DOMove(new Vector2(-6.0f, 2.0f), 0.5f).SetEase(Ease.OutCubic);
        rightarm.transform.DOMove(new Vector2(6.0f, 2.0f), 0.5f).SetEase(Ease.OutCubic);
        body.transform.DOMove(Vector2.up * 0.5f, 0.5f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.5f);
        leftarm.transform.DOMove(new Vector2(-6.0f, -2.5f), 0.5f).SetEase(Ease.OutBounce);
        rightarm.transform.DOMove(new Vector2(6.0f, -2.5f), 0.5f).SetEase(Ease.OutBounce);
        body.transform.DOMove(Vector2.down * 2, 0.5f).SetEase(Ease.Unset);
        CameraShake.instance.smoothShakeCamera(10, 1f);
        yield return new WaitForSeconds(0.6f);
        leftarm.transform.DOMove(new Vector2(-6.0f, -2f), 0.3f).SetEase(Ease.Unset);
        rightarm.transform.DOMove(new Vector2(6.0f, -2f), 0.3f).SetEase(Ease.Unset);
        body.transform.DOMove(Vector2.down * 0.5f, 0.3f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.3f);
        leftarm.transform.DOMove(new Vector2(-1f, -2f), 0.3f).SetEase(Ease.InCubic);
        rightarm.transform.DOMove(new Vector2(1f, -2f), 0.3f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(0.3f);
        leftarm.transform.DOMove(new Vector2(-3f, -2f), 0.5f).SetEase(Ease.Unset);
        rightarm.transform.DOMove(new Vector2(3f, -2f), 0.5f).SetEase(Ease.Unset);
        bodyAnimator.enabled = true;
        head.GetComponent<HeadAnimation>().isHeadIdle = true;
        CameraShake.instance.smoothShakeCamera(5, 0.5f);
        StartCoroutine(specialPattern1());
        yield return null;
    }

    IEnumerator specialPattern1()
    {
        float temp = -90;
        yield return new WaitForSeconds(2);
        bodyAnimator.enabled = false;
        head.GetComponent<HeadAnimation>().isHeadIdle = false;
        body.transform.DOMove(Vector2.up * 1, 1f);
        leftarm.transform.DOMove(new Vector2(-6f, 0f), 1f);
        rightarm.transform.DOMove(new Vector2(6f, 0f), 1f);
        head.transform.DOMove(Vector2.up * 2, 1f);
        headAnimator.Play("head-attack_open");
        CameraShake.instance.shakeCamera(3, 1);
        yield return new WaitForSeconds(1);
        CameraShake.instance.smoothShakeCamera(5, 1);
        yield return new WaitForSeconds(1);
        CameraShake.instance.shakeCamera(7, 0.3f);
        for (float i = 0; i < lasertime; i += Time.fixedDeltaTime)
        {
            float radian = temp * Mathf.PI / 180;
            RaycastHit2D hit = Physics2D.Raycast(Vector2.up * 0.2f, new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized, Mathf.Infinity);
            Debug.DrawLine(Vector2.up * 0.2f, hit.point, Color.yellow);
            laserLineRenderer.enabled = true;
            laserLineRenderer.SetPosition(0, Vector2.up * 1.2f);
            laserLineRenderer.SetPosition(1, hit.point);
            temp += 7.2f / lasertime;
            yield return new WaitForFixedUpdate();
        }
        laserLineRenderer.enabled = false;
        headAnimator.Play("head-attack_close");
        body.transform.DOMove(Vector2.zero, 1f);
        head.transform.DOMove(Vector2.up, 1f);
        yield return new WaitForSeconds(1f);
        isAttack = false;
        yield return null;
    }
}
