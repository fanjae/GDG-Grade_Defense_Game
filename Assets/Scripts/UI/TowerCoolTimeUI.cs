using UnityEngine;
using UnityEngine.UI;

public class TowerCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownFill;

    [Header("Color")]
    [SerializeField] private Color chargingColor = Color.white;
    [SerializeField] private Color readyColor = Color.black;

    public void SetCooldown(float value)
    {
        if (cooldownFill == null) return;

        float amount = Mathf.Clamp01(value);

        cooldownFill.fillAmount = amount;

        // 쿨타임 중이면 하양, 완료되면 검정
        cooldownFill.color = amount >= 1.0f ? readyColor : chargingColor;
    }
}