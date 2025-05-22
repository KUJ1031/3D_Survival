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

        // 속도 증가
        playerController.moveSpeed = boostedSpeed;
    }
    public void PermanentJumpPowerUp(float amount)
    {
        float originalJumpPower = playerController.jumpPower;
        float boostedJumpPower = originalJumpPower + amount;

        // 속도 증가
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

        // 속도 증가
        playerController.moveSpeed = boostedSpeed;

        // 5초 대기
        yield return new WaitForSeconds(duration);

        // 점진적으로 원래 속도로 복귀 (0.5초 동안)
        float elapsed = 0f;
        float transitionTime = 0.5f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            playerController.moveSpeed = Mathf.Lerp(boostedSpeed, originalSpeed, elapsed / transitionTime);
            yield return null;
        }

        // 보정
        playerController.moveSpeed = originalSpeed;
    }

    public bool CheckDoubleJumpEnable(float time)
    {
        Debug.Log($"더블 점프가 + {time}초만큼 활성화되었습니다.");
        StartCoroutine(ResetDoubleJumpAfterTime(time));
        playerController.doubleJumpTime = true;
        return playerController.doubleJumpTime;
    }

    private IEnumerator ResetDoubleJumpAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Double Jump Time: 종료");
        playerController.doubleJumpTime = false;
    }

    public void TempororyInvincibility(float time)
    {
        if (!playerController.isInvincible)
        {
            Debug.Log($"무적이 + {time}초만큼 활성화되었습니다.");
            StartCoroutine(InvincibilityCoroutine(time));
        }
           
    }

    private IEnumerator InvincibilityCoroutine(float time)
    {
        playerController.isInvincible = true;

        yield return new WaitForSeconds(time);
        Debug.Log($"{time}초가 지나 무적 시간이 비활성화되었습니다.");
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
