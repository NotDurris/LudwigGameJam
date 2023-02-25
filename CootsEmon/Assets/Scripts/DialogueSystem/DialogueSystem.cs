using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private Transform dialogueBox;

    [SerializeField]
    private Image profilePic;
    [SerializeField]
    private TMP_Text profileName;
    [SerializeField]
    private TMP_Text profileText;
    [SerializeField]
    private int lettersPerSecond;


    private Dialogue currentDialogue;
    private int currentScene = 0;
    private LevelManager lm;
    private bool InDialogue;
    private TrainerEncounter te;
    private bool isTyped = false;
    private Unit unit;
    private bool endInBattle;
    private int trainerID;
    private AudioManager am;

    private void Awake() {
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        dialogueBox.gameObject.SetActive(false);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Z) && isTyped){
            isTyped = false;
            FindObjectOfType<AudioManager>().Play("Interact");
            ProgressScene();
        }
    }

    public void StartDialogue(Dialogue dialogue, bool EndInBattle = false, Unit trainerUnit = null, int trainerIndex = -1){
        unit = trainerUnit;
        endInBattle = EndInBattle;
        trainerID = trainerIndex;
        currentScene = 0;
        currentDialogue = dialogue;
        dialogueBox.gameObject.SetActive(true);
        lm.PauseGame(true);
        StartCoroutine(DisplayScene(currentScene));
    }
    public void ProgressScene(){
        currentScene++;
        if(currentScene < currentDialogue.GetLength()){
            StartCoroutine(DisplayScene(currentScene));
        }else{
            EndDialogue();
            if(endInBattle){
                GameObject.FindGameObjectWithTag("BattleInitiator").GetComponent<BattleInitiator>().StartBattle(unit, unit.unitLevel, CreateDeets(trainerID));
            }
        }
    }
    public TrainerDetails CreateDeets(int id){
        TrainerDetails deets = new TrainerDetails();
        deets.Name = FindObjectOfType<BattleManager>().GetTrainerName(id);
        deets.trainerID = id;
        return deets;
    }
    public IEnumerator DisplayScene(int index){
        DialogueScreen ds = currentDialogue.GetScreen(index);
        if(ds.profilePic != null){
            profilePic.gameObject.SetActive(true);
            profileName.gameObject.SetActive(true);
            profilePic.sprite = ds.profilePic;
            profileName.text = ds.profilePic.name;
        }else{
            profilePic.gameObject.SetActive(false);
            profileName.gameObject.SetActive(false);
        }
        yield return StartCoroutine(TypeDialog(ds.dialogueText));
        isTyped = true;
    }
    public void EndDialogue(){
        currentScene = 0;
        currentDialogue = null;
        dialogueBox.gameObject.SetActive(false);
        lm.PauseGame(false);
    }

    public IEnumerator TypeDialog(string text){
        profileText.text = null;
        foreach(var letter in text.ToCharArray()){
            profileText.text += letter;
            yield return new WaitForSecondsRealtime(1f/lettersPerSecond);
        }
    }
}
