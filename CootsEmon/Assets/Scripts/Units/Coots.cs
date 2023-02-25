using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Coots")]
public class Coots : Unit
{
    [SerializeField]
    private int currentHP = 0;
    public override int GetHp(){
		return currentHP;
	}
    public override void Setup(){
        if(currentHP == 0){
            currentHP = unitHealth + unitLevel;
        }
    }
    public void GainXP(int amount){
        unitExperience += amount;
        while(unitExperience >= unitLevel){
            unitExperience -= unitLevel;
            unitLevel++;
        }
    }
    public override bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0){
            currentHP = 0;
			return true;
        }
		else{
            return false;
        }
	}

    public void Heal(int amount)
	{
		currentHP += amount;
		if (currentHP > unitHealth + unitLevel)
			currentHP = unitHealth + unitLevel;
	}

    public override void Reset()
    {
        unitLevel = 5;
        unitExperience = 0;
        currentHP = unitHealth + unitLevel; 
    }
}
