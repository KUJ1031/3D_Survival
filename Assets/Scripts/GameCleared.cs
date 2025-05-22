using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCleared : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 게임 클리어 UI를 활성화하는 코드
            Debug.Log("Game Cleared!");
            UIManager.instance.GameCleared();
            GameManager.Instance.GameEnd(); // 게임 종료
        }
    }
}
