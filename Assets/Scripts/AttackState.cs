using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemies apart from totem enter this state to chase the players position
public class AttackState : IState
{
    EnemyController owner;
    GameObject player;
    public AttackState(EnemyController owner) { this.owner = owner; }
    public void Enter()
    {
        player = GameObject.Find("Player");
    }
    public void Execute()
    {
        //Debug.Log("updating patrol state");
        if (owner.flying)
        {
            float ydistance = player.transform.position.y - owner.transform.position.y;

            if (ydistance < -0.5)
            {
                owner.moveDown();
            }
            else if (ydistance > 0.5)
            {
                owner.moveUp();
            }
        }

        float xdistance = player.transform.position.x - owner.transform.position.x;

        if (xdistance < 0)
        {
            owner.moveLeft();
        }
        else if (xdistance > 0)
        {
            owner.moveRight();
        }
    }
    public void Exit()
    {

    }
}
