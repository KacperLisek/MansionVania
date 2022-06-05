using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//interactable that cycles text on interact
public class OldManController : MonoBehaviour, Interactable
{
    public Text text;
    int currentLine = 0;
    string[] lines = {"brave adventurer!","the lord of the mansion has been terrorising our village!", "please stop him!"};
    public void OnInteraction()
    {
        //Debug.Log("old man touched");
        CycleText();
    }

    public int reward()
    {
        return 0;
    }

    void CycleText()
    {
        text.text = lines[currentLine];
        currentLine++;
        if(currentLine > 2)
        {
            currentLine = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
