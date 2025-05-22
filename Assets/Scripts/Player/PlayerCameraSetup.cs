using UnityEngine;

public class PlayerCameraSetup : MonoBehaviour
{
    public Transform player;           // 플레이어 Transform (드래그로 할당)
    public Vector3 offset = new Vector3(0, 3, -5); // 3인칭 오프셋

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.LookAt(player.position + Vector3.up * 1.5f); // 카메라가 플레이어 상단을 바라보게
        }
    }
}
