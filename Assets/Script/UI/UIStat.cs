using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStat : MonoBehaviour
{
    public Stat health;
    public Stat stamina;
    void Start()
    {
        CharacterManager.Instance.Player.stat.uiStat = this;
    }
}
