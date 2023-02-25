using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BattleInitiator : MonoBehaviour
{
    [SerializeField]
    private Unit[] EnemyUnits;
    private List<Unit> encounterableUnits = new List<Unit>();
    [SerializeField, Range(0f,1f)]
    private float encounterChance;
    [SerializeField]
    private float Cooldown;
    private bool InsideGrass;

    private float currentCooldown;
    private int distInt = 0;

    [SerializeField]
    private Transform grassPartsPrefab;
    private Transform grassParts;

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")){
            if(other.GetComponent<PlayerMovement>().isMoving()){
                if(grassParts == null){
                    grassParts = Instantiate(grassPartsPrefab, other.transform.position + Vector3.down*0.2f, Quaternion.identity);
                }else{
                    if(!grassParts.GetComponent<ParticleSystem>().isPlaying){
                        grassParts.gameObject.SetActive(false);
                        grassParts.position = other.transform.position + Vector3.down*0.2f;
                        grassParts.gameObject.SetActive(true);
                    }
                }
                distInt = (Mathf.FloorToInt(Vector3.Distance(other.transform.position, Vector3.zero)) / 30) + 1;
                encounterableUnits.AddRange( EnemyUnits.Where( ( s, i ) => i < distInt));             
                InsideGrass = true;
            }else{
                InsideGrass = false;
            }
            
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        InsideGrass = false;
    }
    private void Start() {
        currentCooldown = Cooldown;
    }

    private void Update() {
        if(InsideGrass){
            if(currentCooldown > 0){
                currentCooldown -= Time.deltaTime;
            }else{
                float randNum = Random.Range(0f,1f);
                if(randNum < encounterChance){
                    StartBattle(encounterableUnits[Random.Range(0, encounterableUnits.Count)], Random.Range(distInt + 1, distInt + 5));                  
                }
            }
        }else{
            if(currentCooldown != Cooldown) currentCooldown = Cooldown;
        }

    }

    public void StartBattle(Unit enemyUnit, int enemyLevel, TrainerDetails trainerDeets = null)
    {
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        BattleSystem.enemyUnit = enemyUnit;
        BattleSystem.enemyLevel = enemyLevel;
        BattleSystem.trainerDeets = trainerDeets;
        string transitionEffect;
        if(trainerDeets == null){
            transitionEffect = "transitionWild";
        }else{
            transitionEffect = "transitionTrainer";
        }
        StartCoroutine(GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().LoadNewScene(2, transitionEffect));
    }
}
