using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lifeText;

    public void UpdateLife(int currentLife) // Life 정보 업데이트
    {
        if (lifeText == null) return;

        lifeText.text = $"x {currentLife}";
    }
}