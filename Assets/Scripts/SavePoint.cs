using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    Player player;
    public Vector3 savePoint;
    private float marginY = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCondition playerCondition = collision.gameObject.GetComponent<PlayerCondition>();
            if (playerCondition != null)
            {
                Debug.Log($"Save Point : {savePoint}");
                playerCondition.point = this;
                UpdatedSavePoint();
            }
        }
    }
    public void UpdatedSavePoint()
    {
        savePoint = CharacterManager.Instance.Player.transform.localPosition;
    }
}
