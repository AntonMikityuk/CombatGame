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

    public int maxMana;
    public int curMana;

    public bool TakeDamage(int dmg)
    {
        curHp -= dmg;

        if (curHp <= 0)
            return true;
        else
            return false;
    }
    public void Rest(int hphealed, int manarestored)
    {
        curHp += hphealed;
        if (curHp > maxHp)
            curHp = maxHp;
        curMana += manarestored;
        if (manarestored > maxMana)
            curMana = maxMana;
    }
}
