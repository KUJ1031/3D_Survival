using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public float speed = 2f;            // 이동 속도
    public float moveRange = 3f;        // 시작 위치 기준 최대 이동 거리

    private int direction = 1;          // 이동 방향 (1: 오른쪽, -1: 왼쪽)
    private float startX;               // 시작 위치 저장

    void Start()
    {
        startX = transform.position.x;  // 시작 시 위치 저장
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // 상대 위치로 이동
        pos.x += speed * direction * Time.deltaTime;

        // 이동한 거리 계산
        float distanceFromStart = pos.x - startX;

        // 범위 초과 시 방향 반전
        if (distanceFromStart >= moveRange)
        {
            pos.x = startX + moveRange;
            direction = -1;
        }
        else if (distanceFromStart <= -moveRange)
        {
            pos.x = startX - moveRange;
            direction = 1;
        }

        transform.position = pos;
    }
}
