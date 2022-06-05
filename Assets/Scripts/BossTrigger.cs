using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//checks if player has entered the boss room, closes door behind them
public class BossTrigger : MonoBehaviour
{
    public GameObject door;
    public FinalBossController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            controller.activate();
            door.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
