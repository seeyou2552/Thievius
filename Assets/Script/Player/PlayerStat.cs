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

    Stat health { get { return uiStat.health; } }

    Stat stamina { get { return uiStat.stamina; } }

    public event Action OnTakeDamage;

    void Update()
    {
        // stamina.Add(stamina.passiveValue * Time.deltaTime);
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }


    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }
}
