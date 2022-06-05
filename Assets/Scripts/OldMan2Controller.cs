using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//interactable that cycles through text
public class OldMan2Controller : MonoBehaviour, Interactable
{
    public Text text;
    int currentLine = 0;
    string[] lines = { "you shouldn't be here", "go away", "final warning" };
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
        if (currentLine > 2)
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
