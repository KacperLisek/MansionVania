using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//controls all enemies
public class EnemyController : MonoBehaviour
{
    //public variables
    public int hp = 1;
    public float movementSpeed = 1;
    public bool flying = false;
    public bool canMove = true;
    public PlayerController player;
    public GameObject xpOrb;

    //other variables used
    Vector2 velocity;
    float knockbackTime = 0;
    public Waypoint waypoint;
    protected bool seenTarget;
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    bool facingLeft = false;
    bool directionChanged = false;

    //animation
    public Sprite sprite;
    public Sprite madSprite;
    public RuntimeAnimatorController controller;
    public RuntimeAnimatorController madController;

    public StateMachine stateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        velocity.y = rigidbody.velocity.y;
        stateMachine.ChangeState(new PatrolState(this));
        spriteRenderer = GetComponent<SpriteRenderer>();
        //stateMachine.ChangeState(new AttackState(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //checks state machine
        stateMachine.Update();
        if (Time.fixedTime > knockbackTime + 1)
        {
            canMove = true;
        }
        if (canMove)
        {
            if (flying)
            {
                rigidbody.velocity = velocity;
                velocity = Vector2.zero;
            }
            else
            {
                rigidbody.velocity = new Vector2(velocity.x, rigidbody.velocity.y);
                velocity.x = 0;
                velocity.y = rigidbody.velocity.y;
            }
        }
        //flips sprite
        if (directionChanged)
        {
            directionChanged = false;
            if (facingLeft)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    //movement functions called by states
    public void moveLeft()
    {
        if (canMove)
        {
            velocity.x -= movementSpeed;
            if (facingLeft != true)
            {
                directionChanged = true;
            }
            facingLeft = true;
        }
    }

    public void moveRight()
    {
        if (canMove)
        {
            velocity.x += movementSpeed;
            if (facingLeft == true)
            {
                directionChanged = true;
            }
            facingLeft = false;
        }
    }
    public void moveUp()
    {
        if (canMove && flying)
            velocity.y += movementSpeed;
    }

    public void moveDown()
    {
        if (canMove && flying)
            velocity.y -= movementSpeed;
    }

    //handles hp loss and drops xp on death
    public void loseHP(int hploss)
    {
        //knockbackTime = Time.fixedTime;
        hp -= hploss;
        canMove = false;

        Debug.Log("Decrementing hp" + hp);
        if (hp < 1)
        {
            Instantiate(xpOrb, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            //xp drop
        }
    }

    //change things on target found
    public virtual void foundTarget()
    {
        if (seenTarget == false)
        {
            stateMachine.ChangeState(new AttackState(this));
            spriteRenderer.sprite = madSprite;
            if (GetComponent<Animator>())
            {
                GetComponent<Animator>().runtimeAnimatorController = madController;
            }
        }
        seenTarget = true;
        //Debug.Log("I see you");
    }

    //change back to normal
    public void lostTarget()
    {
        seenTarget = false;
        stateMachine.ChangeState(new PatrolState(this));
        //change sprite to normal
        spriteRenderer.sprite = sprite;
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().runtimeAnimatorController = controller;
        }
        //Debug.Log("I lost you");
    }

    public virtual void fireShot(bool playerToLeft)
    {
        Debug.Log("Enemy class cannot fire shots");
    }

    //handles getting hit by sword
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "weapon" && GetComponent<CapsuleCollider2D>().IsTouching(collision) && Time.fixedTime > knockbackTime + 0.5)
        {
            //knockback code
            knockbackTime = Time.fixedTime;
            Vector2 physicsVelocity = Vector2.zero;
            if (collision.transform.position.x <= this.transform.position.x)
            {
                physicsVelocity.x += 2;
            }
            else
            {
                physicsVelocity.x -= 2;
            }
            physicsVelocity.y += 2;

            Rigidbody2D r = GetComponent<Rigidbody2D>();
            r.velocity = new Vector2(physicsVelocity.x, physicsVelocity.y);
            
            loseHP(player.getLevel());
        }
    }
}
