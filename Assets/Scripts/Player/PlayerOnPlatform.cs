using UnityEngine;

public class PlayerOnPlatform : MonoBehaviour
{
    private bool isClimbing = false;     // 클라이밍 상태 체크

    private float verticalInput = 0f;    // W/S 입력 값 저장 (1, 0, -1)
    private PlayerController playerController; // 플레이어 컨트롤러

    private void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
    }
    void Update()
    {
        if (isClimbing && playerController != null)
        {
            float verticalInput = playerController.curMovementInput.y;
            Vector3 climbMovement = new Vector3(0, verticalInput * playerController.moveSpeed * Time.deltaTime, 0);
            transform.Translate(climbMovement);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 이동 플랫폼인지 확인
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // 플레이어를 플랫폼 자식으로 설정
            transform.SetParent(collision.transform);
        }
        // 충돌한 오브젝트가 오르내릴 수 있는 벽인지 확인
        if (collision.gameObject.CompareTag("ClimbingWall"))
        {
            Debug.Log("ClimbingWall");
            isClimbing = true;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // 플랫폼에서 벗어나면 부모 관계 해제
            transform.SetParent(null);
        }
        if (collision.gameObject.CompareTag("ClimbingWall"))
        {
            Debug.Log("ClimbingWall Escape");
            isClimbing = false;

        }
    }
}
