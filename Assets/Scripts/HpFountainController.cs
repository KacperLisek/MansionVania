using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//refills player hp when interacted with
public class HpFountainController : MonoBehaviour, Interactable
{
    public Text text;
    float textWait = 0;
    public void OnInteraction()
    {
        text.text = "The fountain heals your wounds";
        textWait = Time.fixedTime;
    }

    public int reward()
    {
        return 3;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.fixedTime >= textWait + 3)
        {
            text.text = "";
        }
    }
}
