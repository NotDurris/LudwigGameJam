using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{

	[SerializeField] private Text nameText;
	[SerializeField] private Text levelText;
	[SerializeField] private Slider hpSlider;
	[SerializeField] private Image Fill;
	[SerializeField] private Gradient colours;
	[SerializeField] private Transform unitStation;
	[SerializeField] private TMP_Text damageText;

	private int health;

	private void Update() {
		hpSlider.value = Mathf.Lerp(hpSlider.value, health, 0.02f);
		float percent = hpSlider.value / hpSlider.maxValue;
		Fill.color = colours.Evaluate(percent);
	}

	public void SetHUD(Unit unit, int currentHP)
	{
		nameText.text = unit.unitName;
		levelText.text = "Lvl " + unit.unitLevel;
		hpSlider.maxValue = unit.unitHealth + unit.unitLevel;
		health = currentHP;
	}

	public Transform GetStation(){
		return unitStation;
	}
	public void SetHP(int hp)
	{
		int damageTaken = hp - health;
		damageText.text = (damageTaken).ToString();
		if(damageTaken < 0){
			damageText.color = Color.red;
		}else{
			damageText.color = Color.green;
		}
		health = hp;	
		damageText.gameObject.SetActive(true);
	}

}
