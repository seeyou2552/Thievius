using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
}
public class PlayerStat : MonoBehaviour, IDamagalbe
{

    public UIStat uiStat;
    public float plusSpeed = 0;

    Stat health { get { return uiStat.health; } }

    Stat stamina { get { return uiStat.stamina; } }
    public PlayerController controller;

    public event Action OnTakeDamage;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }

    public void UseStamina(float amout)
    {
        stamina.Subtract(amout);
    }

    public bool CheckStamina()
    {
        return stamina.curValue >= 5f;
    }

    public void SpeedBuff(float amout)
    {
        StartCoroutine(SpeedUp(amout));
    }

    public IEnumerator SpeedUp(float amout)
    {
        plusSpeed = amout;
        yield return new WaitForSeconds(5f);
        plusSpeed = 0;

    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }
}
