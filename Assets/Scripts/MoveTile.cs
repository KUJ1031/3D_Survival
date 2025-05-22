using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public float speed = 2f;            // �̵� �ӵ�
    public float moveRange = 3f;        // ���� ��ġ ���� �ִ� �̵� �Ÿ�

    private int direction = 1;          // �̵� ���� (1: ������, -1: ����)
    private float startX;               // ���� ��ġ ����

    void Start()
    {
        startX = transform.position.x;  // ���� �� ��ġ ����
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // ��� ��ġ�� �̵�
        pos.x += speed * direction * Time.deltaTime;

        // �̵��� �Ÿ� ���
        float distanceFromStart = pos.x - startX;

        // ���� �ʰ� �� ���� ����
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
