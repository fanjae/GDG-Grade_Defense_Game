using TMPro;
using UnityEngine;
public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    public void UpdateCoin(int coin) // 코인 정보 업데이트
    {
        if (coinText == null) return;
        coinText.text = $"x {coin}";
    }
}