using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State class, handles walking between waypoints with flying support
public class PatrolState : IState
{
    EnemyController owner;
    Waypoint waypoint;
    bool ycheck = false;
    public PatrolState(EnemyController owner) { this.owner = owner; }
    public void Enter()
    {
        //Debug.Log("entering patrol state");
        waypoint = owner.waypoint;
    }
    public void Execute()
    {
        //Debug.Log("updating patrol state");
        //if flying then consider y coordinate
        if (owner.flying)
        {
            float ydistance = waypoint.transform.position.y - owner.transform.position.y;

            if (ydistance > -0.5 && ydistance < 0.5)
            {
                ycheck = true;
            }
            else if (ydistance < 0)
            {
                owner.moveDown();
            }
            else if (ydistance > 0)
            {
                owner.moveUp();
            }
        }
        else
        {
            ycheck = true;
        }

        float xdistance = waypoint.transform.position.x - owner.transform.position.x;

        if(xdistance > -0.5 && xdistance < 0.5 && ycheck)
        {
            Waypoint nextWaypoint = waypoint.nextWaypoint;
            waypoint = nextWaypoint;
        }
        else if (xdistance < 0)
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
        //Debug.Log("exiting patrol state");
        // stop moving
    }
}
