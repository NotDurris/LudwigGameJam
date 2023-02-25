using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    [SerializeField]
    private Dialogue dialogue;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("Max");
            int itemIndex = FindObjectOfType<ItemSpawner>().FindIndexOfItem(transform.position);
            ItemSpawner.itemPickedUp[itemIndex] = true;
            FindObjectOfType<DialogueSystem>().StartDialogue(dialogue);
        }
    }
}
