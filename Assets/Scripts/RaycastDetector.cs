using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VisibleRaycast : MonoBehaviour
{
    public float rayDistance = 20f;
    public Vector3 rayDirection = Vector3.forward;
    public LayerMask detectionLayer;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.red;  // 레이저 색상
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.TransformDirection(rayDirection);
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, rayDistance, detectionLayer))
        {
            endPoint = hit.point;

            try
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("플레이어 명중!");
                    PlayerCondition playerCondition = hit.collider.GetComponent<PlayerCondition>();
                    playerCondition.Die();
                    // 감지 후 행동 추가 가능
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error: " + e.Message);
            }
            
        }
        else
        {
            endPoint = origin + direction * rayDistance;
        }

        // 레이저를 눈에 보이게 그림
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPoint);
    }
}
