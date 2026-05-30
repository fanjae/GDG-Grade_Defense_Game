using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //게임 전채에서 단 하나만 존재하는 관리자 객체
    // ㄴ 디자인 기법 : 싱글톤 패턴
    //싱글톤 패턴, 편하지만 남용하면 독?
    // -> 1. 의존성이 강해짐
    // -> 2. 객체가 바뀌면 전부 수정해야 함
    // -> 3. 텍스트가 어려워질 수 있음

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 전환되어도 삭제되지 않도록
        }
        else
        { Destroy(gameObject); }
    }

    private void Start()
    {
        score = 0;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    { 
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if(scoreText!=null)
        { scoreText.text = $"Score : {score}"; }
    }
}

// public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
// {
//     protected static T instance;
//     public static T Instance
//     {
//         get { return instance; }
//     }
//     protected virtual void Awake()
//     {
//         if(instance == null)
//         { instance = this as T; }
//         else { Destroy(instance); }
//     }
// }
// public class SoundManager : Singleton<SoundManager>
// {
// 
// }
