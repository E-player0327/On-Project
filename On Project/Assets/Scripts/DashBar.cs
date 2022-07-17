using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    [SerializeField] private GameObject Dashbar;
    [SerializeField] private GameObject DashChargeBar;
    Image dashbarImage;
    Image dashChargeBarImage;

    private void Start()
    {
        dashbarImage = Dashbar.GetComponent<Image>();
        dashChargeBarImage = DashChargeBar.GetComponent<Image>();
    }

    private void Update()
    {
        dashbarImage.fillAmount = (float)Player.Instance.currentDashAmount / (float)Player.Instance.DashAmount;

        if (Player.Instance.currentDashAmount == (float)Player.Instance.DashAmount)
            dashChargeBarImage.fillAmount = 1;
        else
            dashChargeBarImage.fillAmount = (float)Player.Instance.currentDashChargeTime / (float)Player.Instance.DashChargeTime;
    }
}
