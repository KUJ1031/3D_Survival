using UnityEngine;

public class PlayerCameraSetup : MonoBehaviour
{
    public Transform player;           // �÷��̾� Transform (�巡�׷� �Ҵ�)
    public Vector3 offset = new Vector3(0, 3, -5); // 3��Ī ������

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.LookAt(player.position + Vector3.up * 1.5f); // ī�޶� �÷��̾� ����� �ٶ󺸰�
        }
    }
}
