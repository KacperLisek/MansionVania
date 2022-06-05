using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls Enemies Vision
public class VisionController : MonoBehaviour
{
    EnemyController owner;
    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Draws vision colliders
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawLine(Vector3.up, Vector3.down);

        if (GetComponent<CircleCollider2D>() != null)
        {
            Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // is it the players flag
        if (collision.gameObject.name == "PlayerFlag")
        {
            owner.foundTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // is it the players flag
        if (collision.gameObject.name == "PlayerFlag")
        {
            owner.lostTarget();
        }
    }
}
