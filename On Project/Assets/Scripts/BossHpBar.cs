using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private GameObject bosshpbar;
    Image bosshpbarImage;

    private void Start()
    {
        bosshpbarImage = bosshpbar.GetComponent<Image>();
    }

    private void Update()
    {
        bosshpbarImage.fillAmount = (float)Boss.instance.currentHp / (float)Boss.instance.Hp;
    }
}
