using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // 싱글톤 인스턴스

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스는 파괴
        }
    }
    public void GameStart()
    {
        Time.timeScale = 1f; // 게임 시작
        SceneManager.LoadScene("MainScene"); // 게임 씬 로드
    }
    public void GameEnd()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; // 게임 일시 정지
    }

    public void GameQuit()
    {
        Application.Quit(); // 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중지
#endif

        Debug.Log("Game Quit");
    }
}
