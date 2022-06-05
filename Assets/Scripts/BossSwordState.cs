using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Boss sword phase
public class BossSwordState : IState
{
    FinalBossController owner;
    GameObject sword;
    float attackStartTime = 0;
    Vector2 originalPos;
    float dist;
    public BossSwordState(FinalBossController owner) { this.owner = owner; sword = owner.lordSword; }
    //holds sword up
    public void Enter()
    {
        originalPos = sword.transform.position;
        attackStartTime = Time.fixedTime;
        sword.transform.Rotate(new Vector3(0, 0, -45));
        sword.transform.Translate(new Vector2(0, 2));
        dist = 0;
    }

    public void Execute()
    {
        //moves sword down
        if (Time.fixedTime >= attackStartTime + 1.5f) {
            dist += 0.2f;
            if (dist < 3.1f)
                sword.transform.Translate(new Vector2(0, -0.2f));
        }
        if (Time.fixedTime >= attackStartTime + 3.5f)
        {
            owner.enterIdle();
        }
    }

    //resets sword pos
    public void Exit()
    {
        sword.transform.Rotate(new Vector3(0, 0, 45));
        sword.transform.position = originalPos;
        sword.SetActive(false);
    }
}
