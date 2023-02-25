using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    public Vector2[] itemSpawnLocations;
    public static bool[] itemPickedUp;

    [SerializeField]
    private TMPro.TMP_Text itemText;
    // Start is called before the first frame update
    void Start()
    {
        if(itemPickedUp == null){
            itemPickedUp = new bool[itemSpawnLocations.Length];
            for(int i = 0; i < itemPickedUp.Length; i++){
                itemPickedUp[i] = false;
            }
        }

        int itemTextTracker = 0;
        for(int i = 0; i < itemSpawnLocations.Length; i ++){
            Vector2 itemPos = itemSpawnLocations[i];
            bool pickedUp = itemPickedUp[i];
            GameObject cItem = Instantiate(itemPrefab, itemPos, Quaternion.identity, transform);
            if(pickedUp == true){
                cItem.SetActive(false);
                itemTextTracker++;
            }
        }
        itemText.text = itemTextTracker.ToString();
    }

    private void Update() {
        int itemTextTracker= 0;
        foreach(bool picked in itemPickedUp){
            if(picked){
                itemTextTracker++;
            }
        }
        itemText.text = itemTextTracker.ToString();
    }

    public int FindIndexOfItem(Vector2 pos){
        for(int i = 0; i < itemSpawnLocations.Length; i++){
            if(pos == itemSpawnLocations[i]){
                return i;
            }
        }
        return -1;
    }
}
