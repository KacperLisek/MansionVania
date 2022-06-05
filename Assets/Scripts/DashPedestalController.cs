using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//rewards player with dash ability
public class DashPedestalController : MonoBehaviour, Interactable
{
    public Text text;
    public void OnInteraction()
    {
        text.text = "Use Q or E to Quickly Dash!";
        //change sprite
    }

    public int reward()
    {
        return 1;
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
