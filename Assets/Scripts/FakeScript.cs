using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles fake player movement in endgame cutscene
public class FakeScript : MonoBehaviour
{
    public RuntimeAnimatorController idleAnim;
    float time;
    bool stopped = true;
    bool changedAnim = true;
    bool movementDone = false;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Time.fixedTime <= time + 2f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);
        }
        else if(changedAnim)
        {
            GetComponent<Animator>().runtimeAnimatorController = idleAnim;
            changedAnim = false;
        }
    }
}
