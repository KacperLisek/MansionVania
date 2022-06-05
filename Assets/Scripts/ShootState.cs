using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State for the totems to enter when firing
public class ShootState : IState
{
    EnemyController owner;
    GameObject player;
    bool upgrade;
    bool playerToLeft = false;
    float shotTimer;
    public float fireRate = 3f;
    float shotDelay = 3f;
    public ShootState(EnemyController owner, bool upgrade) { this.owner = owner; this.upgrade = upgrade;}
    //finds player and starts the timer
    public void Enter()
    {
        shotTimer = Time.fixedTime;
        player = GameObject.Find("Player");
    }
    public void Execute()
    {
        //checks direction of player
        if(player.transform.position.x < owner.transform.position.x)
        {
            playerToLeft = true;
        }
        else
        {
            playerToLeft = false;
        }
        //default firing pattern, shoot if player is within the same y coordinate
        if (upgrade == false)
        {
            float ydistance = player.transform.position.y - owner.transform.position.y;

            if (ydistance > -0.5 && ydistance < 0.5)
            {
                if (Time.fixedTime > shotTimer + fireRate)
                {
                    shotTimer = Time.fixedTime;
                    owner.fireShot(playerToLeft);
                }
            }
        }//handles upgraded version with 1 second delay between shots
        else
        {
            float ydistance = player.transform.position.y - owner.transform.position.y;

            if (ydistance > -0.5 && ydistance < 0.5)
            {
                if (Time.fixedTime > shotTimer + shotDelay)
                {
                    owner.fireShot(playerToLeft);
                    shotTimer = Time.fixedTime;
                    if (shotDelay == 3f)
                    {
                        shotDelay = 1f;
                    }
                    else
                    {
                        shotDelay = 3f;
                    }
                }
            }
        }
    }
    public void Exit()
    {

    }

}
