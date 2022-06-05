using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Main class, handles all player controls and certain events
public class PlayerController : MonoBehaviour
{
    //movement parameters
    public int movementSpeed = 5;
    public int numJumps = 1;
    int jumpStore = 1;

    //player variables
    int maxHp = 3;
    int currentHp = 3;
    int currentLevel = 1;
    int xp = 0;
    bool canJump = true;
    int jumpHeight = 8;
    int currentCheckpoint = 0;

    //extra movement parameters
    public bool jumpUnlocked = false;
    public bool unlockedDash = false;
    public int dashVelocity = 30;
    public float dashDelay = 1;
    public float attackActive = 0.5f;

    //variables instatiated for certain conditions
    bool swordLocked = false;
    bool canMove = true;
    bool attacking = false;
    bool facingLeft = false;
    bool directionChanged = false;

    //masks
    int groundMask = 1 << 8;
    int interactable = 1 << 6;
    int ladderMask = 1 << 3;

    //timers and miscellaneous
    public float jumpThreshold = 1;
    public GameObject sword;
    SpriteRenderer spriteRenderer;
    float attackTime = 0;
    float jumpTime = 0;
    float dashTime = 0;
    float interactTime = 0;
    float knockbackTime = 0;
    float invulnTime = 0;
    float levelTime = 0;
    BoxCollider2D hitbox;
    Interactable thingToInteract;

    //animation
    Animator animator;
    int isIdle = Animator.StringToHash("isIdle");
    int jumpTrigger = Animator.StringToHash("jumpTrigger");
    int isWalking = Animator.StringToHash("isWalking");

    //end of game cutscene objects
    public GameObject fakePlayer;
    public GameObject fakeSword;
    public Sprite cursedSprite;

    //ui and scenes
    public Text playerText;

    UIController ui;
    SceneManager sceneManager;

    //instatiates all variables and reloads if player has died
    void Start()
    {
        //fetches objects
        jumpStore = numJumps;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        ui = GameObject.Find("UI").GetComponent<UIController>();
        animator = GetComponent<Animator>();
        animator.SetBool(isIdle, true);

        //ladderMask = LayerMask.GetMask("ladder");

        //load from playerstats
        currentCheckpoint = PlayerStats.checkPoint;
        currentLevel = PlayerStats.currentLevel;
        unlockedDash = PlayerStats.dashUnlocked;
        jumpUnlocked = PlayerStats.jumpUnlocked;

        //transform.Translate(0, 0, 0);
        //check for checkpoint
        switch (currentCheckpoint)
        {
            case 1:
                transform.Translate(new Vector2(60, 11));
                break;
            case 2:
                transform.Translate(new Vector2(93, 44));
                break;
        }

        //update ui
        maxHp = currentLevel + 2;
        currentHp = maxHp;

        ui.updateLevel(currentLevel);
        ui.updateHp(currentHp);

        if (unlockedDash)
        {
            ui.enableDash();
        }

        if (jumpUnlocked)
        {
            ui.enableJump();
            jumpHeight = 16;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getLevel()
    {
        return currentLevel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ignore collision
        if(collision.gameObject.tag == "ignoreCollision")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
        //if player not invincible and checks position of collision relative to player
        if(collision.gameObject.tag == "hurtsPlayer" && Time.fixedTime >= invulnTime + 2)
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
                damageStep(true);
            else
                damageStep(false);
        }
        //kills player
        if(collision.gameObject.tag == "water")
        {
            currentHp = 0;
            ui.updateHp(currentHp);
            handlePlayerDeath();
        }
        //increments xp
        if (collision.gameObject.tag == "xpOrb")
        {
            xp++;
            Destroy(collision.gameObject);
            checkLevelUp();
        }
        //play end of game
        if(collision.gameObject.tag == "finish")
        {
            Destroy(collision.gameObject);
            endGame();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //ladder movement
        if (collision.gameObject.tag == "ladder")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            if (Input.GetKey(KeyCode.W))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 4);
            }
        }

        //if (collision.gameObject.tag == "hurtsPlayer" && Time.fixedTime < invulnTime + 2)
        //{
        //    Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        //}
    }

    void FixedUpdate()
    {
        //MOVEMENT

        // the new velocity to apply to the character
        Vector2 physicsVelocity = Vector2.zero;
        Rigidbody2D r = GetComponent<Rigidbody2D>();

        // move to the left
        if (Input.GetKey(KeyCode.A) && canMove)
        {
            animator.SetBool(isWalking, true);
            physicsVelocity.x -= movementSpeed;
            if (facingLeft != true)
            {
                directionChanged = true;
            }
            facingLeft = true;
        }//move right
        else if (Input.GetKey(KeyCode.D) && canMove)
        {
            animator.SetBool(isWalking, true);
            physicsVelocity.x += movementSpeed;
            if (facingLeft != false)
            {
                directionChanged = true;
            }
            facingLeft = false;
        }
        else
        {
            animator.SetBool(isWalking, false);
        }

        //flip object
        if (directionChanged)
        {
            directionChanged = false;
            if (facingLeft)
            {
                spriteRenderer.flipX = true;
                sword.transform.Rotate(0, 0, -45);
                sword.transform.Translate(-1.5f, 0, 0);
                sword.transform.Rotate(0, 0, -45);
                sword.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
                sword.transform.Rotate(0, 0, 45);
                sword.transform.Translate(1.5f, 0, 0);
                sword.transform.Rotate(0, 0, 45);
                sword.GetComponent<SpriteRenderer>().flipX = false;
            }
        }


        //DASH
        RaycastHit2D rightCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, 3f, groundMask);
        RaycastHit2D leftCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, 3f, groundMask);
        //raycasts to check if going to hit a groundmask before dash - stops players dashing into a wall
        if (Input.GetKey(KeyCode.E) && unlockedDash && canMove)
        {
            if(Time.fixedTime >= dashTime + dashDelay)
            {
                if (!rightCheck)
                {
                    Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * 3f, Color.red, 1);
                    //physicsVelocity.x += dashVelocity;
                    transform.Translate(new Vector2(3, 0));
                    dashTime = Time.fixedTime;
                    Debug.Log("Dash Success");
                }
                else
                {
                    Debug.Log("Dash Failed");
                }
            }
        }
        if (Input.GetKey(KeyCode.Q) && unlockedDash && canMove)
        {
            if (Time.fixedTime >= dashTime + dashDelay)
            {
                if (!leftCheck)
                {
                    Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * 3f, Color.red, 1);
                    //physicsVelocity.x -= dashVelocity;
                    transform.Translate(new Vector2(-3, 0));
                    dashTime = Time.fixedTime;
                    Debug.Log("Dash Success");
                }
                else
                {
                    Debug.Log("Dash Failed");
                }
            }
        }
        
        //cheat code
        //if (Input.GetKey(KeyCode.H))
        //{
        //    numJumps++;
        //}

        //jump code is designed to work for multiple jumps but this was not put into the final game
        //JUMP code
        if (Input.GetKey(KeyCode.W) && canMove)
        {
            //Debug.Log("Checking"+jumpStore);
            //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.up * 0.5f, Color.green, 1);
            RaycastHit2D ladderCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, 0.1f, ladderMask);
            if (ladderCheck)
            {
                r.velocity = new Vector2(physicsVelocity.x, 4);
            }
            //if able to jump and past jump delay then set the velocity up
            if (jumpStore > 0)
            {
                if (Time.fixedTime >= jumpTime + 0.25)
                {
                    animator.SetTrigger(jumpTrigger);
                    r.velocity = new Vector2(physicsVelocity.x, jumpHeight);
                    jumpStore--;
                    jumpTime = Time.fixedTime;
                }
            }
        }
        //Vector2 h = transform.TransformDirection(Vector2.down);
        //if player is touching ground then we can restore number of jumps
        Debug.DrawRay(transform.position, -Vector2.up * jumpThreshold, Color.red, 1);
        if (Physics2D.Raycast(new Vector2
            (transform.position.x,
            transform.position.y),
            -Vector2.up, jumpThreshold, groundMask))
        {
            //Debug.Log("resetting jumpstore");
            if (Time.fixedTime >= jumpTime + 0.25)
            {
                jumpStore = numJumps;
            }
        }

        // apply the updated velocity to the rigid body
        r.velocity = new Vector2(physicsVelocity.x,
        r.velocity.y);


        //ATTACK
        hitbox = sword.GetComponent<BoxCollider2D>();
        if (Input.GetKey(KeyCode.Space) && !swordLocked)
        {
            if(attacking == false)
            {
                sword.GetComponent<SpriteRenderer>().enabled = true;
                Debug.Log("Activating sword");
                attacking = true;
                attackTime = Time.fixedTime;
                hitbox.enabled = true;
                
            }
        }
        //end attack after enough time
        if(attacking == true)
        {
            if (Time.fixedTime >= attackTime + attackActive)
            {
                sword.GetComponent<SpriteRenderer>().enabled = false;
                Debug.Log("Deactivating");
                attacking = false;
                hitbox.enabled = false;
            }
        }

        //Interact
        RaycastHit2D right = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, 1, interactable);
        RaycastHit2D left = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, 1, interactable);
        //constant raycast right and left to check for interactables
        if(right || left)
        {
            playerText.text = "!";
            playerText.color = Color.white;
        }
        else if(levelTime == 0)
        {
            playerText.text = "";
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (Time.fixedTime >= interactTime + 1)
            {
                int reward = 0;
                if (right)
                {
                    //find object and interact
                    Interactable thingToInteract = right.collider.GetComponent<Interactable>();
                    thingToInteract.OnInteraction();
                    reward = thingToInteract.reward();
                }
                else if (left)
                {
                    Interactable thingToInteract = left.collider.GetComponent<Interactable>();
                    thingToInteract.OnInteraction();
                    reward = thingToInteract.reward();
                }
                handleReward(reward);
                interactTime = Time.fixedTime;
            }
        }

        //Update timers
        if(Time.fixedTime >= knockbackTime + 0.5)
        {
            canMove = true;
        }
        if(Time.fixedTime >= levelTime + 2.5 && levelTime != 0)
        {
            playerText.text = "";
            playerText.color = Color.white;
            levelTime = 0;
        }
    }

    //handles player damage, bool is given to know which direction to knockback
    void damageStep(bool goLeft)
    {
        currentHp--;
        ui.updateHp(currentHp);

        Vector2 physicsVelocity = Vector2.zero;
        if (goLeft)
        {
            physicsVelocity.x -= 3;
        }
        else
        {
            physicsVelocity.x += 3;
        }
        physicsVelocity.y += 6;

        //timers
        knockbackTime = Time.fixedTime;
        invulnTime = Time.fixedTime;
        canMove = false;

        Rigidbody2D r = GetComponent<Rigidbody2D>();
        r.velocity = new Vector2(physicsVelocity.x, physicsVelocity.y);

        if (currentHp == 0)
        {
            //player is dead - reload last checkpoint
            handlePlayerDeath();
        }
    }
    //wait for a certain amount of time before respawning the player
    IEnumerator death()
    {
        ui.toggleDeath();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(1);
    }

    //makes it so player cannot move or be hit - update static class of recent achievements
    void handlePlayerDeath()
    {
        knockbackTime = Time.fixedTime + 10;
        invulnTime = Time.fixedTime + 10;
        canMove = false;
        swordLocked = true;
        PlayerStats.checkPoint = currentCheckpoint;
        PlayerStats.currentLevel = currentLevel;
        PlayerStats.dashUnlocked = unlockedDash;
        PlayerStats.jumpUnlocked = jumpUnlocked;
        StartCoroutine(death());
    }

    //handles rewards given by interactables
    void handleReward(int reward)
    {
        switch (reward)
        {
            case 1:
                unlockedDash = true;
                ui.enableDash();
                break;
            case 2:
                jumpHeight = 16;
                jumpUnlocked = true;
                ui.enableJump();
                break;
            case 3:
                currentHp = maxHp;
                ui.updateHp(currentHp);
                currentCheckpoint = 1;
                break;
            case 4:
                currentHp = maxHp;
                ui.updateHp(currentHp);
                currentCheckpoint = 2;
                break;
        }
    }

    //runs when xp orb obtained, updates ui
    void checkLevelUp()
    {
        float xpPercent = (float) xp / (currentLevel * 5f);
        ui.updateSlider(xpPercent);
        if(xp >= currentLevel * 5)
        {
            //level up
            xp = 0;
            ui.updateSlider(xp);
            currentLevel++;
            ui.updateLevel(currentLevel);
            maxHp++;
            currentHp = maxHp;
            ui.updateHp(currentHp);

            playerText.text = "Level Up!";
            playerText.color = Color.yellow;
            levelTime = Time.fixedTime;
        }
    }

    //final cutscene, theatrics and instantiates new objects
    IEnumerator endGameCutscene()
    {
        yield return new WaitForSeconds(3);
        spriteRenderer.sprite = cursedSprite;
        yield return new WaitForSeconds(3);
        GameObject fakeplayer = Instantiate(fakePlayer, new Vector2(75, 43.6f), Quaternion.identity);
        fakePlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 0);
        yield return new WaitForSeconds(3);
        if (facingLeft)
        {
            directionChanged = true;
            facingLeft = false;
        }
        yield return new WaitForSeconds(2);
        playerText.text = "you shouldn't be here!";
        yield return new WaitForSeconds(3);
        playerText.text = "the crown is cursed!";
        yield return new WaitForSeconds(3);
        playerText.text = "";
        yield return new WaitForSeconds(2);
        Instantiate(fakeSword, new Vector2(70, 43.6f), Quaternion.Euler(new Vector3(0, 0, -45)));
        yield return new WaitForSeconds(3);
        //fade to black
        SceneManager.LoadScene(0);
    }
    //handles player settings and timers to make player not die or move
    void endGame()
    {
        currentHp = 99;
        canMove = false;
        knockbackTime = Time.fixedTime + 100f;
        levelTime = Time.fixedTime + 100f;
        animator.enabled = false;
        playerText.color = Color.white;
        swordLocked = true;

        StartCoroutine(endGameCutscene());
    }
}
