using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Subclass of enemy controller, handles totem specific functions
public class TotemController : EnemyController
{
    public Transform shotTransform;
    public GameObject shot;
    public bool upgrade = false;

    //overrides found target to change to shoot state instead
    public override void foundTarget()
    {
        if (seenTarget == false)
        {
            stateMachine.ChangeState(new ShootState(this, upgrade));
            if (GetComponent<Animator>())
            {
                GetComponent<Animator>().runtimeAnimatorController = madController;
            }
        }
        seenTarget = true;
        //Debug.Log("I see you");
    }

    //fires a shot left or right
    public override void fireShot(bool playerToLeft)
    {
        //fix
        if (playerToLeft)
        {
            Instantiate(shot, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
        else
        {
            Instantiate(shot, shotTransform.position, Quaternion.identity);
        }
    }
}
