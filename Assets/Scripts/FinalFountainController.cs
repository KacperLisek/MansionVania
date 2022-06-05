using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//slightly different to other fountain due to different checkpoint
public class FinalFountainController : MonoBehaviour, Interactable
{
    public Text text;
    float textWait = 0;
    public void OnInteraction()
    {
        text.text = "You feel refreshed for a tense battle";
        textWait = Time.fixedTime;
    }

    public int reward()
    {
        return 4;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime >= textWait + 3)
        {
            text.text = "";
        }
    }
}
