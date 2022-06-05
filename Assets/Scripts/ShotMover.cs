using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the movement of shots
public class ShotMover : MonoBehaviour
{
    public float speed = 3.0f;
    bool flagForDeletion = false;
    float lastPos = 0f;
    Rigidbody2D r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        r.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //if object hasn't moved then delete - prevents lasers getting stuck
        if(transform.position.x == lastPos)
        {
            flagForDeletion = true;
        }
        else
        {
            lastPos = transform.position.x;
        }
        if (flagForDeletion)
        {
            Destroy(this.gameObject);
        }
    }

    //delete object if colliding with certain objects, ignore if not
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "solid" || collision.gameObject.tag == "ladder")
        {
            flagForDeletion = true;
        }
        else
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "solid" || collision.gameObject.tag == "ladder")
        {
            flagForDeletion = true;
        }
        else
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
    }
}
