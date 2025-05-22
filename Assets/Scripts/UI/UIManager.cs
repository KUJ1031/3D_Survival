using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // 싱글톤 인스턴스
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

    public void GameCleared()
    {
        gameClearImage.gameObject.SetActive(true); // 게임 클리어 이미지 활성화
    }
}
