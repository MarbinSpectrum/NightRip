using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Maker : MonoBehaviour
{
    #region[잡다변수]
    [HideInInspector] public float movePower = 2;
    [HideInInspector] public float jumpPower = 50;
    [HideInInspector] public bool double_jump = false;
    bool first_jump = true;
    bool second_jump = false;

    [HideInInspector] public int player_max_hp = 1;
    int player_now_hp = 1;

    Animator animator;
    bool attack0 = false;
    bool attack1 = false;
    bool attack2 = false;

    bool arrow = false;

    bool death = false;

    bool roll = false;
    int rolldic = 0;

    bool axisInUse1 = false;

    bool useitem = false;
    bool healcheck = false;

    public bool stop = false;
    float damage = 0;
    float damagetime = 2.5f;
    float crouch_value = 0.5f;

    BoxCollider2D boxCollider2D;
    float box_size_y = 0;

    Rigidbody2D rigid;

    RaycastHit2D isground;
    RaycastHit2D upground;
    SpriteRenderer spriterenderer;

    Vector3 movement;

    public GameObject Arrow_prefab;
    GameObject Death_Back;
    GameObject emp;
    #endregion

    #region[Awake]
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        box_size_y = boxCollider2D.size.y;
        rigid = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponentInChildren<SpriteRenderer>();
        player_now_hp = player_max_hp;
        Death_Back = Camera.main.transform.GetChild(0).gameObject;
    }
    #endregion

    #region[Update]
    void Update() 
    {  
        Ani();
        if(damage < damagetime - 0.25f && !death)
        {
            Attack();
            Jump();
            Move();
            Crouch();
            Roll();
            Use_Item();
        }
        damage -= Time.deltaTime;

    }
    #endregion

    #region[플레이어 애니메이션]
    void Ani()
    {
        if (Time.timeScale == 0) return;
        animator.speed = GameManager.SpeedUp ? 1.25f : 1;
        animator.SetBool("Damage", damage < damagetime - 0.25f ? false : true);
        if (damage <= 0)
            spriterenderer.color = new Color(spriterenderer.color.r, spriterenderer.color.g, spriterenderer.color.b, 1);
        else
            spriterenderer.color = new Color(spriterenderer.color.r, spriterenderer.color.g, spriterenderer.color.b, (int)(damage * 10) % 2 == 0 ? 1 : 0);

        if (Input.GetAxisRaw("Horizontal") == 0 && !stop)
        {
            RaycastHit2D wallcheck = Physics2D.BoxCast(transform.position + new Vector3(0.2f, 0, 0) * (Input.GetAxisRaw("Horizontal Trigger") < 0 ? -1 : Input.GetAxisRaw("Horizontal Trigger") > 0 ? 1 : 0), new Vector2(boxCollider2D.size.x * 0.8f, boxCollider2D.size.y * 0.5f), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
            animator.SetBool("Run", Input.GetAxisRaw("Horizontal Trigger") == 0 || Mathf.Abs(rigid.velocity.x) < 1f || wallcheck ? false : true);
        }
        else if (Input.GetAxisRaw("Horizontal Trigger") == 0 && !stop)
        {
            RaycastHit2D wallcheck = Physics2D.BoxCast(transform.position + new Vector3(0.2f, 0, 0) * (Input.GetAxisRaw("Horizontal") < 0 ? -1 : Input.GetAxisRaw("Horizontal") > 0 ? 1 : 0), new Vector2(boxCollider2D.size.x * 0.8f, boxCollider2D.size.y * 0.5f), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
            animator.SetBool("Run", Input.GetAxisRaw("Horizontal") == 0 || Mathf.Abs(rigid.velocity.x) < 1f || wallcheck ? false : true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if(Input.GetAxisRaw("Horizontal") == 0 || Input.GetAxisRaw("Horizontal Trigger") == 0 && !stop)
        {
            animator.SetBool("Turn", (Input.GetAxisRaw("Horizontal Trigger") < 0 && !spriterenderer.flipX) || (Input.GetAxisRaw("Horizontal Trigger") > 0 && spriterenderer.flipX) ? true : false);
            animator.SetBool("Turn", (Input.GetAxisRaw("Horizontal") < 0 && !spriterenderer.flipX) || (Input.GetAxisRaw("Horizontal") > 0 && spriterenderer.flipX) ? true : false);
        }

        if (!animator.GetBool("Damage") && !animator.GetBool("UseItem") && !stop && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            spriterenderer.flipX = Input.GetAxisRaw("Horizontal") < 0 ? true : (Input.GetAxisRaw("Horizontal") > 0 ? false : spriterenderer.flipX);
            spriterenderer.flipX = Input.GetAxisRaw("Horizontal Trigger") < 0 ? true : (Input.GetAxisRaw("Horizontal Trigger") > 0 ? false : spriterenderer.flipX);
        }

        //animator.SetBool("Jump", rigid.velocity.y <= 0 ? !isground : !isground);
        animator.SetBool("Jump", !isground);
        animator.SetFloat("VelocityY", rigid.velocity.y);

        if(animator.GetBool("Crouch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
            upground = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + 0.1f), new Vector2(boxCollider2D.size.x * 0.5f, boxCollider2D.size.y), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));

        if(Input.GetAxisRaw("Vertical") >= -0.75f && !stop)
        {
            animator.SetBool("Crouch", Input.GetAxisRaw("Vertical Trigger") < 0 ? true : upground && rigid.velocity.y == 0);
        }
        else if (!stop)
        {
            animator.SetBool("Crouch", Input.GetAxisRaw("Vertical") < -0.75f ? true : upground && rigid.velocity.y == 0);
        }

        animator.SetBool("Attack0", attack0);
        animator.SetBool("Attack1", attack1);
        animator.SetBool("Attack2", attack2);
        animator.SetBool("Arrow", arrow);
        animator.SetBool("Roll", roll);
        animator.SetBool("UseItem", useitem);
        animator.SetBool("IsGround", isground);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_UseItem"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 0)
            {
                healcheck = false;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f && !healcheck)
            {
                EffectManager.Play_Energy(transform.position, spriterenderer.flipX);
                SoundManager.HealSE(true);
                healcheck = true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                if(useitem)
                {
                    GameManager.Use_Item();
                    useitem = false;
                }
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Run_Do"))
        {
            arrow = false;
            if ((animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < Time.deltaTime) || (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.5f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < 0.5f + Time.deltaTime) || (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 1 - Time.deltaTime))
            {
                SoundManager.StepSE(true);
            }
        }


        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            arrow = false;
            attack0 = false;
            attack1 = false;
            attack2 = false;
            roll = false;
            useitem = false;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                rigid.velocity = Vector2.zero;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Land_Attack"))
        {
            arrow = false;
            if (attack0 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.9f)
            {
                attack0 = false;
                attack1 = false;
                attack2 = false;
                SoundManager.AttackSE(true);
                SoundManager.YapSE(true, 2);
            }
        }

        if (attack0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack0"))
        {
            arrow = false;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.9f)
            {
                attack0 = false;
                SoundManager.AttackSE(true);
                SoundManager.YapSE(true, 3);
            }
        }

        if (attack1 && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack1"))
        {
            arrow = false;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.9f)
            {
                attack0 = false;
                attack1 = false;
                SoundManager.AttackSE(true);
                SoundManager.YapSE(true, 3);
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack2"))
        {
            arrow = false;
            if (attack2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.9f)
            {
                attack0 = false;
                attack1 = false;
                attack2 = false;
                SoundManager.AttackSE(true);
                SoundManager.YapSE(true, 2);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Arrow_Land") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Arrow_Air") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Arrow_Crouch"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.75f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < 0.9f)
            {
                if (emp == null)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Arrow_Crouch"))
                        emp = Instantiate(Arrow_prefab, transform.position - new Vector3(0, 0.1f, 0), Quaternion.identity);
                    else
                        emp = Instantiate(Arrow_prefab, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                    emp.transform.localScale = new Vector3(spriterenderer.flipX ? -1 : +1, 1, 1);
                    emp.GetComponent<Rigidbody2D>().AddForce(new Vector2(spriterenderer.flipX ? -10 : +10, 0), ForceMode2D.Impulse);
                    SoundManager.ArrowSE(true);
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.9f)
            {
                arrow = false;
                emp = null;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Death"))
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                GameManager.GameOver_UI_static.SetActive(true);
                gameObject.SetActive(false);
            }
        }


    }
    #endregion

    #region[플레이어 좌우 이동]
    void Move()
    {
        if (stop) return;
        if ((Input.GetAxisRaw("Vertical") >= -0.75f && Input.GetAxisRaw("Vertical Trigger") >= 0) && ((!arrow && !upground && !useitem && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Arrow_Land")) || !isground))
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                int dic = Input.GetAxisRaw("Horizontal Trigger") < 0 ? -1 : Input.GetAxisRaw("Horizontal Trigger") > 0 ? 1 : 0;
                rigid.velocity = new Vector2(dic * movePower * (GameManager.SpeedUp ? 1.25f : 1) * ((attack0 || attack1 || attack2) ? 0.5f : 1), rigid.velocity.y);
            }
            else if(Input.GetAxisRaw("Horizontal Trigger") == 0)
            {
                int dic = Input.GetAxisRaw("Horizontal") < 0 ? -1 : Input.GetAxisRaw("Horizontal") > 0 ? 1 : 0;
                rigid.velocity = new Vector2(dic * movePower * (GameManager.SpeedUp ? 1.25f : 1) * ((attack0 || attack1 || attack2) ? 0.5f : 1), rigid.velocity.y);
            }
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            rigid.velocity = new Vector2(rolldic * movePower*1.5f * (GameManager.SpeedUp ? 1.25f : 1), rigid.velocity.y);
        }
    }
    #endregion

    #region[플레이어 점프]
    void Jump()
    {
        isground = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.05f) + new Vector2(boxCollider2D.offset.x, boxCollider2D.offset.y), new Vector2(boxCollider2D.size.x * 0.8f, boxCollider2D.size.y), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
        if (stop) return;
        first_jump = isground && rigid.velocity.y <= 0 ? true : first_jump;
        //second_jump = isground ? true : second_jump;
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0)) && !useitem && !upground && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            if ((Input.GetAxisRaw("Vertical") >= -0.75f && Input.GetAxisRaw("Vertical Trigger") >= 0))
            {
                if (first_jump)
                {
                    attack0 = false;
                    attack1 = false;
                    attack2 = false;
                    first_jump = false;
                    second_jump = true;
                    roll = false;
                    arrow = false;
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                    SoundManager.JumpSE(true);

                }
                else if (second_jump && double_jump)
                {
                    first_jump = false;
                    second_jump = false;
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                    SoundManager.JumpSE(true);
                    SoundManager.YapSE(true, 1);
                }
            }
        }

    }
    #endregion

    #region[플레이어 숙이기]
    void Crouch()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Crouch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Arrow_Crouch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Crouch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Arrow_Crouch"))
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, -box_size_y * (1 - crouch_value) * 0.5f);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, box_size_y * crouch_value);
        }
        else
        {
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, 0);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, box_size_y);
        }
    }
    #endregion

    #region[플레이어 구르기]
    void Roll()
    {
        if (Input.GetAxisRaw("Right Trigger") == 0)
        {
            axisInUse1 = false;
        }
        if (stop) return;
        if ((Input.GetKeyDown(KeyCode.D) || (Mathf.Abs(Input.GetAxisRaw("Right Trigger")) == 1 && !axisInUse1)) && !useitem && isground && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            axisInUse1 = true;
            roll = true;
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                rolldic = Input.GetAxisRaw("Horizontal Trigger") < 0 ? -1 : Input.GetAxisRaw("Horizontal Trigger") > 0 ? +1 : spriterenderer.flipX ? -1 : +1;
            }
            else if (Input.GetAxisRaw("Horizontal Trigger") == 0)
            {
                rolldic = Input.GetAxisRaw("Horizontal") < 0 ? -1 : Input.GetAxisRaw("Horizontal") > 0 ? +1 : spriterenderer.flipX ? -1 : +1;
            }
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Horizontal Trigger") == 0)
            {
                rolldic = Input.GetAxisRaw("Right Trigger") > 0 ? 1 : Input.GetAxisRaw("Right Trigger") < 0 ? -1 : rolldic;
            }
            spriterenderer.flipX = (rolldic == -1) ? true : (rolldic == +1) ? false : spriterenderer.flipX;
        }
    }
    #endregion

    #region[플레이어 공격]
    void Attack()
    {
        if (stop) return;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if (!attack0)
                attack0 = true;
            else if (!attack1)
                attack1 = true;
            else if (!attack2)
                attack2 = true;

        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (!arrow)
                arrow = true;
        }
    }
    #endregion

    #region[아이템 사용]
    void Use_Item()
    {
        if (stop) return;
        if ((Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.F)) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_UseItem") && !animator.GetBool("Crouch") && !useitem && isground && !attack0 && !attack1 && !attack2 && !arrow && !upground && GameManager.Can_Use_Item())
        {
            useitem = true;
        }
    }
    #endregion

    #region[OnTriggerEnter2D]
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name.Equals("Move Scene") && !animator.GetBool("Death"))
        {
            stop = true;
            if (GameManager.fadeout)
                col.GetComponent<MoveSceneObject>().MoveScene();
        }
        else if (col.transform.name.Equals("Enter Trigger"))
        {
            if (col.transform.GetComponent<TriggerScript>().this_type == TriggerScript.Type.enter)
                StartCoroutine(col.GetComponent<TriggerScript>().Set_Trigger());
        }
        else if (col.transform.name.Equals("Stop Trigger"))
        {
            stop = !stop;
            col.gameObject.SetActive(false);
        }
    }
    #endregion

    #region[OnTriggerStay2D]
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.name.Equals("Sign") && !animator.GetBool("Death"))
        {
            if(GameManager.Read_UI && !GameManager.Read_UI.activeSelf && !GameManager.View_UI.activeSelf)
            {
                if (!GameManager.Sign_UI) return;
                GameManager.Read_UI.transform.GetChild(1).GetComponent<Text>().text = "읽어보기";
                GameManager.Read_UI.SetActive(true);  // 표지판 UI가 켜지고
                SignScript.select_sign = col.GetComponent<SignScript>(); // 표지판선택
                GameManager.ViewSign(col.transform.position + new Vector3(0, 0.5f, 0)); // 표지판 UI 이동
            }
            else if (GameManager.Read_UI && GameManager.Read_UI.activeSelf)
            {
                if (!GameManager.Sign_UI) return;
                if ((Input.GetAxisRaw("Vertical") > 0.75f || Input.GetAxisRaw("Vertical Trigger") > 0 || Input.GetKey(KeyCode.UpArrow)) && GameManager.Read_UI.activeSelf) // 표지판 UI가 뜬 상태에서 화살표 위버튼을 누름
                {
                    GameManager.Read_UI.SetActive(false); // 표지판 UI가 꺼지고
                    GameManager.View_UI.SetActive(true); // 설명 UI가 켜짐
                }
            }
        }
        else if (col.transform.name.Equals("Item") && !animator.GetBool("Death"))
        {
            if (GameManager.Read_UI && !GameManager.Read_UI.activeSelf && !GameManager.View_UI.activeSelf)
            {
                if (!GameManager.Sign_UI) return;
                if (col.GetComponent<SignScript>())
                {
                    if (col.GetComponent<SignScript>().this_item != GameManager.Item.없음)
                    {
                        GameManager.Read_UI.transform.GetChild(1).GetComponent<Text>().text = "확인하기";
                        GameManager.Read_UI.SetActive(true);  // 표지판 UI가 켜지고
                        SignScript.select_sign = col.GetComponent<SignScript>(); // 표지판선택
                        GameManager.ViewSign(col.transform.position + new Vector3(0, 0.5f, 0)); // 표지판 UI 이동
                    }
                }
            }
            else if (GameManager.Read_UI && GameManager.Read_UI.activeSelf)
            {
                if (!GameManager.Sign_UI) return;
                if ((Input.GetAxisRaw("Vertical") > 0.75f || Input.GetAxisRaw("Vertical Trigger") > 0 || Input.GetKey(KeyCode.UpArrow)) && GameManager.Read_UI.activeSelf) // 표지판 UI가 뜬 상태에서 화살표 위버튼을 누름
                {
                    if (col.GetComponent<SignScript>())
                    {
                        if (col.GetComponent<SignScript>().this_item == GameManager.Item.회복포션)
                        {
                            int amount = GameManager.GameLevel == TutorialScript.GameLevel.쉬움 ? 3 : GameManager.GameLevel == TutorialScript.GameLevel.보통 ? 2 : GameManager.GameLevel == TutorialScript.GameLevel.어려움 ? 1 : 1;
                            GameManager.Has_Item.Item0_Has = true;
                            if (GameManager.Has_Item.Item0_maxnum < 30)
                            {
                                GameManager.Has_Item.Item0_maxnum += amount;
                            }
                            GameManager.Has_Item.Item0_num += amount;
                            if (GameManager.Has_Item.Item0_num > 30)
                            {
                                GameManager.Has_Item.Item0_num = 30;
                            }
                            col.GetComponent<SignScript>().this_item = GameManager.Item.없음;
                            col.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                            GameManager.Item_Pos.Add(col.transform.position);
                        }
                        else if (col.GetComponent<SignScript>().this_item == GameManager.Item.응급처치키트)
                        {
                            GameManager.Has_Item.Item1_Has = true;
                            GameManager.Has_Item.Item1_num = 1;
                            col.GetComponent<SignScript>().this_item = GameManager.Item.없음;
                            col.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                            GameManager.Item_Pos.Add(col.transform.position);
                        }
                    }
                    GameManager.Read_UI.SetActive(false); // 표지판 UI가 꺼지고
                    GameManager.View_UI.SetActive(true); // 설명 UI가 켜짐
                }
            }
        }
        else if (col.transform.GetComponent<HurtObjectScript>() && (col.transform.GetComponent<HurtObjectScript>().Immediately_damage || damage <= 0) && !animator.GetBool("Death") && !stop)
        {
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(col.transform.position.x > transform.position.x ? -150f : +150f, 100));
            float level_damage = GameManager.GameLevel == TutorialScript.GameLevel.쉬움 ? 0.5f : GameManager.GameLevel == TutorialScript.GameLevel.보통 ? 1f : GameManager.GameLevel == TutorialScript.GameLevel.어려움 ? 2 : GameManager.GameLevel == TutorialScript.GameLevel.악몽 ? 100 : 1;
            GameManager.player_now_hp -= (int)(col.transform.GetComponent<HurtObjectScript>().damage * level_damage);
            useitem = false;
            if (GameManager.player_now_hp <= 0 && !animator.GetBool("Death"))
            {
                stop = true;
                damage = -1;
                animator.SetBool("Death", true);
                if (GameManager.Read_UI) GameManager.Read_UI.SetActive(false);
                if (GameManager.View_UI) GameManager.View_UI.SetActive(false);
                if (GameManager.Player_UI) GameManager.Player_UI.SetActive(false);
                if (GameManager.Boss_UI) GameManager.Boss_UI.SetActive(false);
                Death_Back.SetActive(true);
                SoundManager.OffSE();
                SoundManager.OffBGM();
                SoundManager.DeathSE(true);
            }
            else if (GameManager.player_now_hp > 0)
            {
                damage = damagetime;
                GameManager.UpdateHpBar();
                GameManager.VibrationCamera(0.05f);
                SoundManager.HurtSE(true);
            }
        }
    }
    #endregion

    #region[OnTriggerExit2D]
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.name.Equals("Sign"))
        {
            if (!GameManager.Sign_UI) return;
            col.GetComponent<SignScript>().text = ""; // 표지판 글씨 초기화
            GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().text = ""; // 설명 UI 글씨 초기화
            GameManager.View_UI.SetActive(false);  // 표지판 UI가 꺼지고
            GameManager.Read_UI.SetActive(false); // 설명 UI가 꺼짐
        }
        else if (col.transform.name.Equals("Item"))
        {
            if (!GameManager.Sign_UI) return;
            col.GetComponent<SignScript>().text = ""; // 표지판 글씨 초기화
            GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().text = ""; // 설명 UI 글씨 초기화
            GameManager.View_UI.SetActive(false);  // 표지판 UI가 꺼지고
            GameManager.Read_UI.SetActive(false); // 설명 UI가 꺼짐
        }
    }
    #endregion
}

