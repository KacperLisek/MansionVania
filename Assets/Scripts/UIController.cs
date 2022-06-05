using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls and updates the UI, playercontroller activates this when variables change
public class UIController : MonoBehaviour
{
    public Text currentHp;
    public Text currentLevel;
    public Text deathText;
    public Slider slider;

    public GameObject dashPanel;
    public GameObject jumpPanel;

    bool deathActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHp(int newHp)
    {
        currentHp.text = newHp.ToString();
    }

    public void updateLevel(int newLvl)
    {
        currentLevel.text = newLvl.ToString();
    }

    public void enableDash()
    {
        dashPanel.SetActive(true);
    }

    public void enableJump()
    {
        jumpPanel.SetActive(true);
    }

    public void toggleDeath()
    {
        if (!deathActive)
        {
            deathActive = true;
            deathText.enabled = true;
        }
        else
        {
            deathActive = false;
            deathText.enabled = false;
        }
    }

    public void updateSlider(float val)
    {
        slider.value = val;
    }
}
