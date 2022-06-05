using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles state transitions of final boss
public class FinalBossController : MonoBehaviour
{
    //variable initialisation
    public int hp = 50;
    public GameObject lordSword;
    public Transform shotTransform;
    public Transform batTransform;
    public GameObject bat;
    public GameObject shot;
    public GameObject door;
    float knockbackTime = 0;
    float textTimer = 0;

    //animation
    Animator animator;
    int isBat = Animator.StringToHash("isBat");
    int isShoot = Animator.StringToHash("isShoot");
    public bool batmode = false;
    public bool shootmode = false;

    //idle check
    bool idle;
    int progress;
    float idleTime = 0;
    float idleDelay = 1.5f;

    //other references
    bool active = false;
    PlayerController playerController;
    public GameObject player;
    public Text text;
    public GameObject crown;

    public StateMachine stateMachine = new StateMachine();
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        enterIdle();
        progress = 0;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //animation handling
    public void exitBatAnim()
    {
        batmode = false;
        animator.SetBool(isBat, batmode);
    }

    public void exitLaserAnim()
    {
        shootmode = false;
        animator.SetBool(isShoot, shootmode);
    }

    //idle state
    public void enterIdle()
    {
        idle = true;
        idleTime = Time.fixedTime;
        stateMachine.ChangeState(new BossIdleState(this));
    }

    //shown when player enters the room
    public void activate()
    {
        active = true;
        idleTime = Time.fixedTime;
        text.text = "you shouldn't have come here!";
        textTimer = Time.fixedTime;
    }

    //checks how to change between each state
    void handleProgress()
    {
        switch (progress)
        {
            case 1:
                lordSword.SetActive(true);
                stateMachine.ChangeState(new BossSwordState(this));
                break;
            case 2:
                //laser state;
                shootmode = true;
                animator.SetBool(isShoot, shootmode);
                stateMachine.ChangeState(new BossLaserState(this));
                break;
            case 3:
                //bat state;
                batmode = true;
                animator.SetBool(isBat, batmode);
                stateMachine.ChangeState(new BossBatState(this, 1));
                break;
            case 4:
                enterIdle();
                progress = 0;
                break;
        }
    }

    //instantiating new prefabs
    public void fireShot()
    {
        Instantiate(shot, shotTransform.position, Quaternion.identity);
    }

    public void summonBat()
    {
        Instantiate(bat, batTransform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        if (active)
        {
            stateMachine.Update();

            //Get out of idle code
            if (Time.fixedTime >= idleTime + idleDelay && idle)
            {
                idle = false;
                progress++;
                handleProgress();
            }

            if(Time.fixedTime >= textTimer + 5f)
            {
                text.text = "";
            }
        }
    }

    public void loseHP(int hploss)
    {
        knockbackTime = Time.fixedTime;
        hp -= hploss;

        Debug.Log("Decrementing hp" + hp);
        if (hp < 1)
        {
            door.SetActive(false);
            Instantiate(crown, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    //so sword can pass through
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "hurtsPlayer")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "weapon" && GetComponent<CapsuleCollider2D>().IsTouching(collision) && Time.fixedTime > knockbackTime + 0.5)
        {
            //knockback code
            knockbackTime = Time.fixedTime;
            Vector2 physicsVelocity = Vector2.zero;
            physicsVelocity.y += 2;

            Rigidbody2D r = GetComponent<Rigidbody2D>();
            r.velocity = new Vector2(0, physicsVelocity.y);

            loseHP(playerController.getLevel());
        }
    }
}
