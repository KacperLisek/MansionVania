using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//boss do nothing state
public class BossIdleState : IState
{
    FinalBossController owner;
    public BossIdleState(FinalBossController owner) { this.owner = owner; }
    public void Enter()
    {
        Debug.Log("Boss Entering Idle State");
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Boss Exiting Idle State");
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
