using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { BUSY, PLAYERSTARTTURN, PLAYERATTACK, PLAYERHEAL, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	//Trainer info
	public static TrainerDetails trainerDeets;
	//Variables to be defined entering the battle
	public static Unit enemyUnit;
	public static int enemyLevel;

	//Determine Varialbe for Player Unit
	public Coots playerUnit;

	//Dialog Box For UI of player interactions and text
	[SerializeField] private DialogBox dialogBox;
	
	//Each units hud
	[SerializeField] private BattleHUD playerHUD;
	[SerializeField] private BattleHUD enemyHUD;

	private int currentAction;

	//State
	private BattleState state;

	private Animator playerAnimator;
	private Animator enemyAnimator;

	//Attacking Variables
	private float AttackTime = 10;
	private float cAttackTime;
	private float attackMultiplier;
	private bool previousTurnHealed;

	//Healing Variables
	private float HealTime = 10;
	private float cHealTime;
	private float healMultiplier;
	
	private AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
		am = FindObjectOfType<AudioManager>();
		FindObjectOfType<LevelManager>().PauseGame(false);
		playerUnit.Setup();
		enemyUnit.unitLevel = enemyLevel;
		enemyUnit.Setup();
		StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
		//START OF BATTLE

		//Create visuals of player and enemy
		Transform playerVis = Instantiate(playerUnit.unitVisual, playerHUD.GetStation());
		Transform enemyVis = Instantiate(enemyUnit.unitVisual, enemyHUD.GetStation());

		//Store animators
		playerAnimator = playerVis.GetComponent<Animator>();
		enemyAnimator = enemyVis.GetComponent<Animator>();

		//Set Health bar to be correct
		playerHUD.SetHUD(playerUnit, playerUnit.GetHp());
		enemyHUD.SetHUD(enemyUnit, enemyUnit.GetHp());

		//Switch to text and action selection menu
		EnableMenu(0);

		//Wait until text has been written
		if(trainerDeets == null){
			yield return StartCoroutine(dialogBox.TypeDialog("A wild " + enemyUnit.unitName + " approaches..."));
		}else{
			yield return StartCoroutine(dialogBox.TypeDialog(trainerDeets.Name + " has challenged you to a battle."));
		}
		
		//Enter Player Turn
		StartCoroutine(PlayerTurn());
	}

	IEnumerator PlayerTurn()
	{
		//Switch to text and action selection menu
		EnableMenu(0);
		//Write text to choose an action
		yield return StartCoroutine(dialogBox.TypeDialog("Choose an action."));
		//Set state to start of player's turn, continue in update
		state = BattleState.PLAYERSTARTTURN;
	}

	private void Update(){
		float healthPercent = (float)playerUnit.GetHp() / (float)(playerUnit.unitHealth + playerUnit.unitLevel);
		if(healthPercent < 0.2f && !am.IsPlaying("LowHealth") && !(state == BattleState.WON || state == BattleState.LOST)){
			am.Play("LowHealth");
		}else if(healthPercent >= 0.2f || (state == BattleState.WON || state == BattleState.LOST)){
			am.Pause("LowHealth");
		}
		//If state of battle is players turn
		if(state == BattleState.PLAYERSTARTTURN){
			//Allow player to choose action
			HandleActionSelection();
		}
		if(state == BattleState.PLAYERATTACK){
			dialogBox.AttackValues[0].value = attackMultiplier;
			dialogBox.AttackValues[1].value = cAttackTime / AttackTime;
			am.SetPitch("Correct", attackMultiplier + 1);
			if(attackMultiplier == 1){
				am.Play("Max");
				cAttackTime = 0;
			}
        	if(cAttackTime > 0){
				if(Input.GetKeyDown(KeyCode.Z)){
					attackMultiplier = Mathf.Clamp01(attackMultiplier + (0.01f * playerUnit.unitLevel / (((attackMultiplier + 1) * (attackMultiplier + 1)))));
					am.Play("Correct");		
				}
				cAttackTime -= Time.deltaTime;
        	}else{
				StartCoroutine(PlayerAttackResults());
			}
		}
		if(state == BattleState.PLAYERHEAL){
			dialogBox.healTimer.value = cHealTime / HealTime;
			am.SetPitch("Correct", 1);
        	if(cHealTime > 0){
				if(Input.GetKeyDown(KeyCode.Z)){
					bool hitTarget = false;
					foreach(Transform target in dialogBox.healTargets){
						if(!hitTarget && Mathf.Abs(dialogBox.healCursor.position.x - target.position.x) < 0.5f){
							am.Play("Correct");
							healMultiplier++;
							target.GetComponent<NewPosition>().FindNewPosition();
							hitTarget = true;
						}
					}
					if(!hitTarget){
						am.Play("Wrong");
						if((healMultiplier - 1) >= 0){
							healMultiplier--;
						}						
					}							
				}

				cHealTime -= Time.deltaTime;
        	}else{
				StartCoroutine(PlayerHealResults());
			}
		}
	}

	private void HandleActionSelection(){
		//Determine which selection currently hovered
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			if(currentAction < 1){
				++currentAction;
			}
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow)){
			if(currentAction > 0){
				--currentAction;
			}
		}

		//Update selected action
		dialogBox.UpdateActionSelection(currentAction);

		//Select action with z, and attack or heal accordingly
		if(Input.GetKeyDown(KeyCode.Z)){
			am.Play("Interact");
			if(currentAction == 0){
				//Attack
				StartAttack();
			}
			else if(currentAction == 1){
				//Heal
				StartHeal();
			}
		}
	}

	private void StartAttack(){
		//Player Chose To Attack
		//Stop any state based code from running by changing state to busy
		state = BattleState.BUSY;
		previousTurnHealed = false;
		//Switch to attack menu
		EnableMenu(1);
		//Set to default values for attacking
		//Time given to attack
		AttackTime = 3 + 20 / enemyUnit.unitLevel;
		//time keeper
		cAttackTime = AttackTime;
		//Attack Multiplier for how well attack is done
		attackMultiplier = 0;
		//Change State to PlayerAttack, continues in update
		state = BattleState.PLAYERATTACK;
	}

	private void StartHeal(){
		//Player Chose To Heal
		//Stop any state based code from running by changing state to busy
		state = BattleState.BUSY;
		previousTurnHealed = true;
		//Switch to heal menu
		EnableMenu(2);
		//Set to default values for healing
		HealTime = 5 * (playerUnit.unitLevel / 10 + 1);
		cHealTime = HealTime;
		dialogBox.healCursor.GetComponent<PingPong>().Speed = 5 + playerUnit.unitLevel/15;
		healMultiplier = 0;
		//Change State to PlayerHeal, continues in update
		state = BattleState.PLAYERHEAL;
	}

	IEnumerator PlayerAttackResults()
	{
		state = BattleState.BUSY;
		EnableMenu(0);
		yield return StartCoroutine(dialogBox.TypeDialog("The attack was successful!"));
		
		playerAnimator.Play("Attack");
		am.Play("Hit");
		bool isDead = enemyUnit.TakeDamage(Mathf.FloorToInt((playerUnit.unitDamage + playerUnit.unitLevel / 2) * attackMultiplier) + 1);

		enemyHUD.SetHP(enemyUnit.GetHp());

		
		yield return new WaitForSeconds(1f);
		
		if(isDead)
		{
			state = BattleState.WON;
			StartCoroutine(EndBattle());
		} else
		{
			
			StartCoroutine(EnemyTurn());
		}
	}
	IEnumerator PlayerHealResults()
	{
		state = BattleState.BUSY;
		EnableMenu(0);
		playerAnimator.Play("Heal");
		int AmountHealed = Mathf.FloorToInt(1.4f * healMultiplier);

		playerUnit.Heal(AmountHealed);

		playerHUD.SetHP(playerUnit.GetHp());
		StartCoroutine(dialogBox.TypeDialog("You successfully healed " + AmountHealed + " health!"));

		yield return new WaitForSeconds(2f);
		StartCoroutine(EnemyTurn());
	}
	IEnumerator EnemyTurn()
	{
		state = BattleState.ENEMYTURN;
		yield return StartCoroutine(dialogBox.TypeDialog(enemyUnit.unitName + " attacks!"));
		int enemyDamage = (enemyUnit.unitDamage + Mathf.FloorToInt(enemyUnit.unitLevel / 2 + 1)) / (Convert.ToInt32(previousTurnHealed) + 1);

		enemyAnimator.Play("Attack");
		am.Play("Hit");
		bool isDead = playerUnit.TakeDamage(enemyDamage);

		playerHUD.SetHP(playerUnit.GetHp());

		yield return new WaitForSeconds(1f);
		state = BattleState.BUSY;

		if(isDead)
		{
			state = BattleState.LOST;
			StartCoroutine(EndBattle());
		} else
		{
			StartCoroutine(PlayerTurn());
		}

	}

	IEnumerator EndBattle()
	{
		if(state == BattleState.WON)
		{
			playerUnit.GainXP(enemyUnit.unitLevel);
			if(trainerDeets == null){
				yield return StartCoroutine(dialogBox.TypeDialog("You defeated the wild " + enemyUnit.unitName));
			}else{
				yield return StartCoroutine(dialogBox.TypeDialog("You defeated trainer " + trainerDeets.Name));
			}
			yield return new WaitForSeconds(1f);
			yield return StartCoroutine(dialogBox.TypeDialog("Coots gained, " + enemyUnit.unitLevel + " Exp!"));
			yield return new WaitForSeconds(1f);
			if(trainerDeets != null){
				BattleManager.trainerIndices.Add(trainerDeets.trainerID);
			}
			if(trainerDeets != null && trainerDeets.trainerID == 7){
				StartCoroutine(FindObjectOfType<LevelManager>().LoadNewScene(3));
			}else{
				StartCoroutine(FindObjectOfType<LevelManager>().LoadNewScene(1));
			}	
		} else if (state == BattleState.LOST)
		{
			
			yield return StartCoroutine(dialogBox.TypeDialog("Coots has fainted"));
			yield return new WaitForSeconds(1f);
			yield return StartCoroutine(dialogBox.TypeDialog("Ludwig whited out!"));
			yield return new WaitForSeconds(1f);
			PlayerMovement.Position = Vector2.zero;
			playerUnit.Heal(playerUnit.unitHealth + playerUnit.unitLevel);
			StartCoroutine(FindObjectOfType<LevelManager>().LoadNewScene(1));
		}
	}

	public void EnableMenu(int index){	
		if(index == 0){
			dialogBox.EnableActionSelector(true);
		}else{
			dialogBox.EnableActionSelector(false);
		}
		if(index == 1){
			dialogBox.EnableAttackMenu(true);
		}else{
			dialogBox.EnableAttackMenu(false);
		}
		if(index == 2){
			dialogBox.EnableHealMenu(true);
		}else{
			dialogBox.EnableHealMenu(false);
		}
	}
}
