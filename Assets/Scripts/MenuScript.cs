using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles main menu scene load
public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onStart()
    {
        Debug.Log("Loading Game");
        SceneManager.LoadScene(1);
    }

    public void onOption()
    {
        Debug.Log("Option Clicked");
    }

    public void onExit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    
}
