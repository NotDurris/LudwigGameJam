using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : ScriptableObject
{
	public string unitName;
	public int unitLevel;
	public int unitHealth;
	public int unitDamage;
	public int unitExperience;
	public Transform unitVisual;

	public abstract void Setup();
	public abstract bool TakeDamage(int dmg);
	public abstract void Reset();
	public abstract int GetHp();
}
