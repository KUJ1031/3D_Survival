using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJump : MonoBehaviour
{

    private PlayerController playerController;
    private float originalJumpPower;

    private void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        originalJumpPower = playerController.jumpPower;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.OnHighJump();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.jumpPower = originalJumpPower;
        }
    }

}
