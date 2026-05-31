using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    [Header("Cursor")]
    [SerializeField] private RectTransform cursorImage;

    [Header("Cursor Position")]
    [SerializeField] private Vector2 backToMenuCursorPosition = new Vector2(-720f, -240f);
    [SerializeField] private Vector2 exitCursorPosition = new Vector2(90f, -240f);

    [Header("Scene")]
    [SerializeField] private string mainMenuScene = "TitleMenu";

    private int selectedIndex = 0;

    private void Start() // 버튼 리스너 등록
    {
        mainMenuButton.onClick.AddListener(BackMainMenu);
        exitButton.onClick.AddListener(ExitGame);

        UpdateCursor();
    }

    private void Update() // 키 입력에 따른 커서 이동
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            selectedIndex = 0;
            UpdateCursor();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = 1;
            UpdateCursor();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectedIndex == 0) BackMainMenu();
            else ExitGame();
        }
    }

    private void UpdateCursor() // 커서 업데이트
    {
        cursorImage.anchoredPosition = selectedIndex == 0 ? backToMenuCursorPosition : exitCursorPosition;
    }

    private void BackMainMenu() // 메뉴 복귀
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    private void ExitGame() // 게임 종료
    {
        Time.timeScale = 1f;
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}