using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // 싱글톤 인스턴스
    public TextMeshProUGUI gameSaveText;
    public Image gameClearImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스는 파괴
        }
    }

    public void SetActiveGameClearedImage()
    {
        gameClearImage.gameObject.SetActive(true); // 게임 클리어 이미지 활성화
    }

    public void SettrueGameSavedText()
    {
        gameSaveText.gameObject.SetActive(true); // 게임 저장 텍스트 활성화
        Invoke("SetfalseGameSavedText", 1.5f); // 1초 후에 비활성화
    }

    public void SetfalseGameSavedText()
    {
        gameSaveText.gameObject.SetActive(false); // 게임 저장 텍스트 활성화
    }
}
