using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject leftarm; //-6, 0
    [SerializeField] GameObject rightarm; //6, 0

    void Start()
    {
        StartCoroutine(NormalPattern1());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator NormalPattern1()
    {
        yield return new WaitForSeconds(1.0f);
        leftarm.transform.DOMove(new Vector2(-7.07f, 0.82f),0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.5f);
        leftarm.transform.DOMove(new Vector2(1.5f, -2.5f),0.5f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(0.5f);
        CameraShake.instance.smoothShakeCamera(5, 0.5f);
        leftarm.transform.DOMove(new Vector2(-6f, 0f),1f);

        rightarm.transform.DOMove(new Vector2(7.07f, 0.82f),0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.5f);
        rightarm.transform.DOMove(new Vector2(-1.5f, -2.5f),0.5f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(0.5f);
        CameraShake.instance.smoothShakeCamera(5, 0.5f);
        rightarm.transform.DOMove(new Vector2(6f, 0f),1f);

        yield return new WaitForSeconds(1.5f);
        leftarm.transform.DOMove(new Vector2(-6.0f, 1.0f), 0.5f).SetEase(Ease.OutCubic);
        rightarm.transform.DOMove(new Vector2(6.0f, 1.0f), 0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.5f);
        leftarm.transform.DOMove(new Vector2(-6.0f, -2.5f), 0.5f).SetEase(Ease.OutBounce);
        rightarm.transform.DOMove(new Vector2(6.0f, -2.5f), 0.5f).SetEase(Ease.OutBounce);
        CameraShake.instance.smoothShakeCamera(10, 1f);
        yield return new WaitForSeconds(0.6f);
        leftarm.transform.DOMove(new Vector2(-6.0f, -2f), 0.3f).SetEase(Ease.Unset);
        rightarm.transform.DOMove(new Vector2(6.0f, -2f), 0.3f).SetEase(Ease.Unset);
        yield return new WaitForSeconds(0.3f);
        leftarm.transform.DOMove(new Vector2(-1f, -2f), 0.3f).SetEase(Ease.InCubic);
        rightarm.transform.DOMove(new Vector2(1f, -2f), 0.3f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(0.3f);
        leftarm.transform.DOMove(new Vector2(-3f, -2f), 0.5f).SetEase(Ease.Unset);
        rightarm.transform.DOMove(new Vector2(3f, -2f), 0.5f).SetEase(Ease.Unset);
        CameraShake.instance.smoothShakeCamera(5, 0.5f);


        yield return null;
    }
}
