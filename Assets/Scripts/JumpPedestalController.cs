using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//rewards player with jump when interacted
public class JumpPedestalController : MonoBehaviour, Interactable
{
    public Text text;
    public void OnInteraction()
    {
        text.text = "You can Jump even Higher!";
        //change sprite
    }

    public int reward()
    {
        return 2;
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
