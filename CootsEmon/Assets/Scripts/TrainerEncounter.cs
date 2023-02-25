using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerEncounter : MonoBehaviour
{
    [SerializeField]
    private int trainerIndex;
    [SerializeField]
    private Unit trainersUnit;
    [SerializeField]
    private Dialogue initiateFightDialogue;
    [SerializeField]
    private Dialogue lostFightDialogue;
    [SerializeField]
    private float viewDist;
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private LayerMask Player;
    [SerializeField]
    private GameObject ExclaimationMark;
    private DialogueSystem ds;
    private bool Spoken;
    public bool Passed = false;

    private bool InBattle = false;

    private void Awake() {
        ExclaimationMark.SetActive(false);
        ds = GameObject.FindGameObjectWithTag("DialogueSystem").GetComponent<DialogueSystem>();
        InBattle = false;
    }

    public void LostBattleDialogue(){
        if(Passed && Physics2D.Raycast(transform.position, direction.normalized, viewDist, Player)){
            ds.StartDialogue(lostFightDialogue);
        }
    }

    private void Update() {
        if(!Passed){
            if(Physics2D.Raycast(transform.position, direction.normalized, viewDist, Player) && !InBattle){
                InBattle = true;
                ExclaimationMark.SetActive(true);
                FindObjectOfType<AudioManager>().Play("Max");
                ds.StartDialogue(initiateFightDialogue, true, trainersUnit, trainerIndex);
            }
        }
        else{
            if(Physics2D.OverlapCircle(transform.position, 2, Player) && !Spoken){
                if(Input.GetKeyDown(KeyCode.Z)){
                    ds.StartDialogue(lostFightDialogue);
                    Spoken = true;
                }
            }
        }
    }
}
