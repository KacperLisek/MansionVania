using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//boss laser state controller
public class BossLaserState : IState
{
    FinalBossController owner;
    float startTime = 0;
    float shotTime = 0;
    float fireRate = 1f;
    public BossLaserState(FinalBossController owner) { this.owner = owner;}
    public void Enter()
    {
        startTime = Time.fixedTime;
    }

    public void Execute()
    {
        //shoots lasers at a delay
        if(Time.fixedTime >= startTime + 1f)
        {
            if(Time.fixedTime >= shotTime + fireRate)
            {
                owner.fireShot();
                shotTime = Time.fixedTime;
            }
        }
        if(Time.fixedTime >= startTime + 8f)
        {
            owner.enterIdle();
        }
    }

    public void Exit()
    {
        owner.exitLaserAnim();
    }
}
