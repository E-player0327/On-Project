using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject hpbar;
    Image hpbarImage;

    private void Start()
    {
        hpbarImage = hpbar.GetComponent<Image>();
    }

    private void Update()
    {
        hpbarImage.fillAmount = (float)Player.Instance.currentHp / (float)Player.Instance.Hp;
    }
}
