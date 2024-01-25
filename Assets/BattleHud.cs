using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;
    public Slider manaSlider;

    public void setHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHp;
        hpSlider.value = unit.curHp;
        manaSlider.maxValue = unit.maxMana;
        manaSlider.value = unit.curMana;
    }

    public void SetHP(int hp)
    { 
        hpSlider.value = hp;
    }
    public void setMana(int mana)
    { 
        manaSlider.value = mana;
    }
}
