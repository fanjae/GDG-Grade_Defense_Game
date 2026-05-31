using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    [Header("Cursor")]
    [SerializeField] private RectTransform cursorImage;
    [SerializeField] private Vector2 cursorOffset = new Vector2(-40f, 0f);

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Main";

    private int selectedIndex = 0;

    private void Start() // Button Listener 연결
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);

        UpdateCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 윗키 이동
        {
            selectedIndex = 0;
            UpdateCursor();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // 아랫키 이동
        {
            selectedIndex = 1;
            UpdateCursor();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // 엔터 누르면 메뉴 작동
        {
            if (selectedIndex == 0) StartGame();
            else ExitGame();
        }
    }

    private void UpdateCursor()
    {
        RectTransform target = selectedIndex == 0
            ? startButton.GetComponent<RectTransform>()
            : exitButton.GetComponent<RectTransform>();

        cursorImage.anchoredPosition = target.anchoredPosition + cursorOffset;
    }

    private void StartGame() // 게임 시작 씬으로 넘어감
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void ExitGame() // 종료 처리(에디터와 빌드 파일 각각 독립적으로 작동하도록 구현)
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
