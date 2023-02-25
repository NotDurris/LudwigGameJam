using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private TrainerEncounter[] trainersNotStatic;
    public static List<int> trainerIndices; 

    private void Awake() {
        if(trainerIndices == null){
            trainerIndices = new List<int>();
        }
        foreach(int id in trainerIndices){
            trainersNotStatic[id].Passed = true;
        }
    }

    public string GetTrainerName(int id){
        return trainersNotStatic[id].name;
    }
}
