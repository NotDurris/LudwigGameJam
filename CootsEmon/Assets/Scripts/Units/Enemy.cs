using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Enemy")]
public class Enemy : Unit
{
	private int currentHP;
	public override int GetHp(){
		return currentHP;
	}
	public override void Setup(){
        currentHP = unitHealth + unitLevel;
    }
    public override bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0)
			return true;
		else
			return false;
	}
    public override void Reset()
    {
    }
}
