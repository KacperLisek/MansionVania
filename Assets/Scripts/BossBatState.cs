using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//boss summon bat state
public class BossBatState : IState
{
    FinalBossController owner;
    float startTime = 0;
    int numberBats = 1;
    public BossBatState(FinalBossController owner, int numberBats) { this.owner = owner; this.numberBats = numberBats; }
    public void Enter()
    {
        startTime = Time.fixedTime;
    }

    public void Execute()
    {
        if (Time.fixedTime >= startTime + 1f && numberBats > 0)
        {
            owner.summonBat();
            startTime = startTime + 1f;
            numberBats--;
        }
        if(Time.fixedTime >= startTime + 3f)
        {
            owner.enterIdle();
        }
    }

    public void Exit()
    {
        owner.exitBatAnim();
    }
}
