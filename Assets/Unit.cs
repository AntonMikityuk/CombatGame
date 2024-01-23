using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHp;
    public int curHp;

    public bool TakeDamage(int dmg)
    {
        curHp -= dmg;

        if (curHp <= 0)
            return true;
        else
            return false;
    }
    public void Heal(int hphealed)
    {
        curHp += hphealed;
        if (curHp > maxHp)
            curHp = maxHp;
    }
}
