using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float launchPower; // Ctrl 누른 시간에 곱할 힘 계수

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
                Debug.Log($"기 모으는 중... {holdTime}");
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                float force = holdTime * launchPower;
                

                // 큐브 밖으로 살짝 밀어내기
                Vector3 exitPos = transform.position + Vector3.up * (GetColliderRadius() + 0.3f);
                transform.position = exitPos;

                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = Vector3.zero;

                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                 
                isInsideCube = false;
                playerController.movementLocked = false;
                holdTime = 0f;

                // 탈출한 시간 기록
                lastExitTime = Time.time;

                Debug.Log($"발사! 힘: {force}");
            }
        }
    }

    private float exitCooldown = 1f; // 1초 동안 충돌 무시
    private float lastExitTime = -10f;  // 초기값, 언제 마지막으로 탈출했는지 기록

    private void OnTriggerEnter(Collider other)
    {
        // 탈출 후 일정 시간 내에는 큐브 안으로 다시 들어가지 않도록 무시
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

            Debug.Log("큐브 안으로 들어감");
        }
    }

    // 내부 위치 계산 (큐브 안 딱 맞게)
    private Vector3 GetInsidePosition(Collider cubeCollider)
    {
        Vector3 center = cubeCollider.bounds.center;
        // 큐브 중심에서 조금 더 안쪽으로 들어가게 방향 설정
        Vector3 directionToCenter = (center - transform.position).normalized;

        float playerRadius = GetColliderRadius();
        float cubeInnerMargin = Mathf.Min(cubeCollider.bounds.extents.x, cubeCollider.bounds.extents.y, cubeCollider.bounds.extents.z);
        float offset = playerRadius + 0.3f; // 더 깊게 들어가도록 offset 늘림

        Vector3 targetPos = center - directionToCenter * (cubeInnerMargin - offset);
        return targetPos;
    }

    // 콜라이더 반경 구하기
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
