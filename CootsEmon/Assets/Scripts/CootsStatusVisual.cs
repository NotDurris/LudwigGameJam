using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CootsStatusVisual : MonoBehaviour
{
    [SerializeField]
    private Unit coot;
    public Sprite[] statusSprites;
    public Image img;
    public Slider healthSlider;
    public Slider experienceSlider;
    public TMP_Text levelText;

    // Update is called once per frame
    void Update()
    {
        healthSlider.maxValue = coot.unitHealth + coot.unitLevel;
        coot.Setup();
        healthSlider.value = coot.GetHp();
        float index = ((healthSlider.value / healthSlider.maxValue)* 2.999f);
        img.sprite = statusSprites[Mathf.FloorToInt(index)];

        experienceSlider.maxValue = coot.unitLevel * 2;
        experienceSlider.value = coot.unitExperience;
        levelText.text = "Lvl. " + coot.unitLevel;
    }
}
