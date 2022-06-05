using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles interaction, displays text
public class SignController : MonoBehaviour, Interactable
{
    public Text text;
    float time = 0;
    public void OnInteraction()
    {
        text.text = "legend has it my friend alec made the sprites";
        time = Time.fixedTime;
    }

    //no reward
    public int reward()
    {
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.fixedTime >= time + 3f)
        {
            text.text = "";
        }
    }
}
