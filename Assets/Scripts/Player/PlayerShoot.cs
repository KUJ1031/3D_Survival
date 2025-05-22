using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float launchPower; // Ctrl ���� �ð��� ���� �� ���

    private Vector3 lastOutsidePosition;
    private bool isInsideCube = false;
    private float holdTime = 0f;

    private Rigidbody rb;
    private PlayerController playerController;
    private Collider playerCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (isInsideCube)
        {
            playerController.movementLocked = true;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                holdTime += Time.deltaTime ;
                Debug.Log($"�� ������ ��... {holdTime}");
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                float force = holdTime * launchPower;
                

                // ť�� ������ ��¦ �о��
                Vector3 exitPos = transform.position + Vector3.up * (GetColliderRadius() + 0.3f);
                transform.position = exitPos;

                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = Vector3.zero;

                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                 
                isInsideCube = false;
                playerController.movementLocked = false;
                holdTime = 0f;

                // Ż���� �ð� ���
                lastExitTime = Time.time;

                Debug.Log($"�߻�! ��: {force}");
            }
        }
    }

    private float exitCooldown = 1f; // 1�� ���� �浹 ����
    private float lastExitTime = -10f;  // �ʱⰪ, ���� ���������� Ż���ߴ��� ���

    private void OnTriggerEnter(Collider other)
    {
        // Ż�� �� ���� �ð� ������ ť�� ������ �ٽ� ���� �ʵ��� ����
        if (Time.time - lastExitTime < exitCooldown)
            return;

        if (other.CompareTag("PlayerShoot") && !isInsideCube)
        {
            lastOutsidePosition = transform.position;

            Vector3 insidePos = GetInsidePosition(other);
            transform.position = insidePos;

            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            isInsideCube = true;

            Debug.Log("ť�� ������ ��");
        }
    }

    // ���� ��ġ ��� (ť�� �� �� �°�)
    private Vector3 GetInsidePosition(Collider cubeCollider)
    {
        Vector3 center = cubeCollider.bounds.center;
        // ť�� �߽ɿ��� ���� �� �������� ���� ���� ����
        Vector3 directionToCenter = (center - transform.position).normalized;

        float playerRadius = GetColliderRadius();
        float cubeInnerMargin = Mathf.Min(cubeCollider.bounds.extents.x, cubeCollider.bounds.extents.y, cubeCollider.bounds.extents.z);
        float offset = playerRadius + 0.3f; // �� ��� ������ offset �ø�

        Vector3 targetPos = center - directionToCenter * (cubeInnerMargin - offset);
        return targetPos;
    }

    // �ݶ��̴� �ݰ� ���ϱ�
    private float GetColliderRadius()
    {
        if (playerCollider is SphereCollider sphere)
        {
            return sphere.radius * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (playerCollider is CapsuleCollider capsule)
        {
            return Mathf.Max(capsule.radius, capsule.height / 2f) * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (playerCollider is BoxCollider box)
        {
            return box.size.magnitude / 2f * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            return 0.5f;
        }
    }
}
