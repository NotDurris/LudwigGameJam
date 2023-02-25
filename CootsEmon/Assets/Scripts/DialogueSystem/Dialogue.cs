using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue")]
[SerializeField]
public class Dialogue : ScriptableObject
{
    [SerializeField]
    private DialogueScreen[] dialogueScreens;

    public DialogueScreen GetScreen(int id){
        return dialogueScreens[id];
    }

    public int GetLength(){
        return dialogueScreens.Length;
    }
}
