using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
    [System.Serializable]
    public class TrackableObject
    {
        public GameObject targetInScene;      // 씬에 미리 배치된 오브젝트
        public GameObject prefab;             // 재생성에 사용할 프리팹

        [HideInInspector] public Vector3 spawnPosition;
        [HideInInspector] public GameObject currentInstance;
        [HideInInspector] public bool isRespawning = false;
    }

    public float respawnDelay = 3f;
    public List<TrackableObject> trackedObjects = new();

    void Start()
    {
        // 시작 시 초기 위치 저장 및 currentInstance 설정
        foreach (var obj in trackedObjects)
        {
            if (obj.targetInScene != null)
            {
                obj.spawnPosition = obj.targetInScene.transform.position;
                obj.currentInstance = obj.targetInScene;
            }
        }
    }

    void Update()
    {
        foreach (var obj in trackedObjects)
        {
            if (obj.currentInstance == null && !obj.isRespawning)
            {
                StartCoroutine(RespawnCoroutine(obj));
            }
        }
    }

    private IEnumerator RespawnCoroutine(TrackableObject obj)
    {
        obj.isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);

        obj.currentInstance = Instantiate(obj.prefab, obj.spawnPosition, Quaternion.identity);
        obj.isRespawning = false;
    }
}
