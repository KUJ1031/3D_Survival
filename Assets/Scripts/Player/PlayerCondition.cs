using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}


public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;
    private PlayerController playerController;
    private Renderer renderer;

    void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        renderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0f)
        {
            Die();
        }

    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void PermanentSpeedUp(float amount)
    {
        float originalSpeed = playerController.moveSpeed;
        float boostedSpeed = originalSpeed + amount;

        // �ӵ� ����
        playerController.moveSpeed = boostedSpeed;
    }
    public void PermanentJumpPowerUp(float amount)
    {
        float originalJumpPower = playerController.jumpPower;
        float boostedJumpPower = originalJumpPower + amount;

        // �ӵ� ����
        playerController.jumpPower = boostedJumpPower;
    }
    public void ResetSpeed()
    {
        playerController.moveSpeed = playerController.defaultSpeed;
    }

    public void ResetJumpPower()
    {
        playerController.jumpPower = playerController.beforeJumpPower;
    }

    public void TempororySpeedUp(float amount)
    {
        StartCoroutine(SpeedUpRoutine(amount, 5f));
    }
    private IEnumerator SpeedUpRoutine(float amount, float duration)
    {
        float originalSpeed = playerController.moveSpeed;
        float boostedSpeed = originalSpeed + amount;

        // �ӵ� ����
        playerController.moveSpeed = boostedSpeed;

        // 5�� ���
        yield return new WaitForSeconds(duration);

        // ���������� ���� �ӵ��� ���� (0.5�� ����)
        float elapsed = 0f;
        float transitionTime = 0.5f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            playerController.moveSpeed = Mathf.Lerp(boostedSpeed, originalSpeed, elapsed / transitionTime);
            yield return null;
        }

        // ����
        playerController.moveSpeed = originalSpeed;
    }

    public bool CheckDoubleJumpEnable(float time)
    {
        Debug.Log($"���� ������ + {time}�ʸ�ŭ Ȱ��ȭ�Ǿ����ϴ�.");
        StartCoroutine(ResetDoubleJumpAfterTime(time));
        playerController.doubleJumpTime = true;
        return playerController.doubleJumpTime;
    }

    private IEnumerator ResetDoubleJumpAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Double Jump Time: ����");
        playerController.doubleJumpTime = false;
    }

    public void TempororyInvincibility(float time)
    {
        if (!playerController.isInvincible)
        {
            Debug.Log($"������ + {time}�ʸ�ŭ Ȱ��ȭ�Ǿ����ϴ�.");
            StartCoroutine(InvincibilityCoroutine(time));
        }
           
    }

    private IEnumerator InvincibilityCoroutine(float time)
    {
        playerController.isInvincible = true;

        yield return new WaitForSeconds(time);
        Debug.Log($"{time}�ʰ� ���� ���� �ð��� ��Ȱ��ȭ�Ǿ����ϴ�.");
        playerController.isInvincible = false;
    }
    public void Die()
    {
        Debug.Log("Player is dead");
    }

    public void TakePhysicalDamage(int damage)
    {
        if (playerController.isInvincible) return;

        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;

    }
}
