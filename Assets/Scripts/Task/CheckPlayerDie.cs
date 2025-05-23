using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerDie : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCondition playerCondition = collision.gameObject.GetComponent<PlayerCondition>();
            if (playerCondition != null)
            {
                playerCondition.Die();
            }
        }
    }
}
