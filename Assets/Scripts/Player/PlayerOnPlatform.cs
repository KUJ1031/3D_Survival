using UnityEngine;

public class PlayerOnPlatform : MonoBehaviour
{
    private bool isClimbing = false;     // Ŭ���̹� ���� üũ

    private float verticalInput = 0f;    // W/S �Է� �� ���� (1, 0, -1)
    private PlayerController playerController; // �÷��̾� ��Ʈ�ѷ�

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
        // �浹�� ������Ʈ�� �̵� �÷������� Ȯ��
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // �÷��̾ �÷��� �ڽ����� ����
            transform.SetParent(collision.transform);
        }
        // �浹�� ������Ʈ�� �������� �� �ִ� ������ Ȯ��
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
            // �÷������� ����� �θ� ���� ����
            transform.SetParent(null);
        }
        if (collision.gameObject.CompareTag("ClimbingWall"))
        {
            Debug.Log("ClimbingWall Escape");
            isClimbing = false;

        }
    }
}
