using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogBox : MonoBehaviour
{
    [SerializeField] private int lettersPerSecond;
    [SerializeField] private Color highlightedText;
    [SerializeField] private TMP_Text txt;
    [SerializeField] private Transform actionSelector;
    [SerializeField] private Transform attackGUIBox;
    [SerializeField] private Transform healGUIBox;

    [SerializeField] private List<TMP_Text> actionTexts;
    public List<Slider> AttackValues;
    public Slider healTimer;
    public List<Transform> healTargets;
    public Transform healCursor;

    public void SetDialog(string dialog){
        txt.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog){
        txt.text = null;
        foreach(var letter in dialog.ToCharArray()){
            txt.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }

    public void EnableActionSelector(bool enable){
        actionSelector.gameObject.SetActive(enable);
    }

    public void EnableAttackMenu(bool enable){
        attackGUIBox.gameObject.SetActive(enable);
    }

    public void EnableHealMenu(bool enable){
        healGUIBox.gameObject.SetActive(enable);
    }

    public void UpdateActionSelection(int selectedAction){
        for(int i = 0; i < actionTexts.Count; i++){
            if (i == selectedAction){
                actionTexts[i].color = highlightedText;
            }else{
                actionTexts[i].color = Color.black;
            }
        }
    }
}
