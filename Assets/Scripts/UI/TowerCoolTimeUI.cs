using UnityEngine;
using UnityEngine.UI;

public class TowerCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownFill;

    public void SetCooldown(float value)
    {
        if (cooldownFill == null) return ;
        cooldownFill.fillAmount = Mathf.Clamp01(value); // 0~1값 제한
    }
}