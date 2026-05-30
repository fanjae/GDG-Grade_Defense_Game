using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴.
    // 게임 전체에서 하나만 존재하는 관리자 객체
    public static GameManager Instance { get; private set; }

    [Header("Coin UI")]
    [SerializeField] private CoinUI coinUI;

    [Header("Life UI")]
    [SerializeField] private LifeUI lifeUI;

    [Header("Life Setting")]
    [SerializeField] private int maxLife = 30;

    [Header("Coin Setting")]
    [SerializeField] private int startCoin = 0;

    private int currentLife;
    private int coin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에 따른 삭제 방지.
        }
        else
        { 
            Destroy(gameObject); 
        }
    }

    private void Start() // 게임 초기 설정
    {
        currentLife = maxLife;
        coin = startCoin;

        UpdateLifeUI();
        UpdateCoinUI();
    }

    public void AddCoin(int amount) // 코인 증가
    {
        coin += amount;
        UpdateCoinUI();
    }

    public bool TryUseCoin(int amount) // 코인 사용 시도
    {
        if (coin < amount) // 돈 부족
            return false;

        coin -= amount; 
        UpdateCoinUI();
        return true;
    }

    public void DecreaseLife(int amount) // 라이프 감소
    {
        currentLife -= amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife); // 현재 Life가 0이하로 떨어지지 않도록 조정.

        UpdateLifeUI();

        if (currentLife <= 0) // 현재 라이프가 0이면 게임오버
        {
            // GameOver();  
        }
    }

    private void UpdateLifeUI() // 라이프 갱신
    {
        if (lifeUI != null) lifeUI.UpdateLife(currentLife);
    }

    private void UpdateCoinUI() // 코인 정보 갱신
    {
        if (coinUI != null) coinUI.UpdateCoin(coin);
    }
}
