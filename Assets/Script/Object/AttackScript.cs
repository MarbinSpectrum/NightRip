using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    #region[잡다변수]
    float time = 0;

    public enum Type { ArrowAttack, SwordAttack }

    [SerializeField]
    [Header("공격종류")]
    public Type this_type;
    #endregion

    #region[Awake]
    private void Awake()
    {
        if(this_type == Type.ArrowAttack)
        {
            transform.name = "Arrow";
        }
        else if (this_type == Type.SwordAttack)
        {
            transform.name = "Attack Check";
        }
    }
    #endregion

    #region[Update]
    private void Update()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (this_type == Type.ArrowAttack)
        {
            time += Time.deltaTime;
            if (time > 5f)
            {
                Destroy(gameObject);
            }
        }
        else if (this_type == Type.SwordAttack)
        {
            transform.position = transform.parent.position + (transform.parent.GetComponent<SpriteRenderer>().flipX ? new Vector3(-0.32f, 0,0) : new Vector3(+0.32f, 0,0));
        }
    }
    #endregion

    #region[OnTriggerEnter2D]
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (this_type == Type.ArrowAttack)
        {
            if (col.transform.gameObject.layer == LayerMask.NameToLayer("Background") || col.transform.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)
                {
                    transform.parent = col.transform;
                    if (col.transform.gameObject.layer == LayerMask.NameToLayer("Background"))
                    {
                        if(!col.transform.tag.Equals("Wall"))
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        }
                    }

                    if (col.transform.gameObject.tag.Equals("Monster"))
                    {
                        col.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        if (GameManager.Stun_Arrow)
                        {
                            col.transform.parent.GetComponent<Rigidbody2D>().AddForce(StageManager.player_static.transform.position.x < col.transform.parent.transform.position.x ? new Vector2(+30, 15) : new Vector2(-30, 15));
                            col.transform.parent.GetComponent<MonsterScript>().damage = true;
                            col.transform.parent.GetComponent<MonsterScript>().player = StageManager.player_static;
                            col.transform.parent.GetComponent<MonsterScript>().time = 0;
                            //col.transform.parent.GetComponent<Rigidbody2D>().AddForce(col.transform.position.x > transform.parent.position.x ? new Vector2(+5, 70) : new Vector2(-5, 70));
                        }
                        else
                        {
                            col.transform.parent.GetComponent<Rigidbody2D>().AddForce(col.transform.position.x > transform.position.x ? new Vector2(+10, 0) : new Vector2(-10, 0));
                        }

                        col.transform.parent.GetComponent<MonsterScript>().hp -= GameManager.Arrow_Damage;
                        if (col.transform.parent.GetComponent<MonsterScript>().hp <= 0)
                            col.transform.parent.GetComponent<MonsterScript>().Dead();

                        GameManager.VibrationCamera(0.05f);
                        SoundManager.ArrowDamaageSE(true);

                        EffectManager.Play_Hit_Normal(transform.position, !(col.transform.position.x > transform.position.x));
                        EffectManager.Play_Hit_Small(transform.position, !(col.transform.position.x > transform.position.x));

                        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Target") && !GameManager.Penetrate_Arrow)
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                            Destroy(gameObject);
                        }
                    }
                    else if (col.transform.gameObject.tag.Equals("Boss"))
                    {
                        if (GameManager.Boss_UI)
                        {
                            GameManager.Boss_now_hp -= GameManager.Arrow_Damage;
                            GameManager.VibrationCamera(0.05f);
                            SoundManager.ArrowDamaageSE(true);

                            GameManager.UpdateHpBar();

                            EffectManager.Play_Hit_Normal(transform.position, !(col.transform.position.x > transform.position.x));
                            EffectManager.Play_Hit_Small(transform.position, !(col.transform.position.x > transform.position.x));
                        }
                    }
                }
            }
        }
        else if (this_type == Type.SwordAttack)
        {
            if (col.transform.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                if (col.transform.gameObject.tag.Equals("Monster") && col.transform.parent.GetComponent<MonsterScript>())
                {
                    col.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    if (GameManager.Stun_Sword)
                    {
                        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) != 0)
                        {
                            col.transform.parent.GetComponent<Rigidbody2D>().AddForce(Input.GetAxisRaw("Horizontal") > 0 ? new Vector2(+25, 70) : Input.GetAxisRaw("Horizontal") < 0 ? new Vector2(-25, 70) : col.transform.position.x > transform.parent.position.x ? new Vector2(+5, 70) : new Vector2(-5, 70));
                        }
                        else
                        {
                            col.transform.parent.GetComponent<Rigidbody2D>().AddForce(Input.GetAxisRaw("Horizontal Trigger") > 0 ? new Vector2(+25, 70) : Input.GetAxisRaw("Horizontal Trigger") < 0 ? new Vector2(-25, 70) : col.transform.position.x > transform.parent.position.x ? new Vector2(+5, 70) : new Vector2(-5, 70));
                        }
                        col.transform.parent.GetComponent<MonsterScript>().damage = true;
                        col.transform.parent.GetComponent<MonsterScript>().player = StageManager.player_static;
                        col.transform.parent.GetComponent<MonsterScript>().time = 0;
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) != 0)
                        {
                            col.transform.parent.GetComponent<Rigidbody2D>().AddForce(Input.GetAxisRaw("Horizontal") > 0 ? new Vector2(+25, 0) : Input.GetAxisRaw("Horizontal") < 0 ? new Vector2(-25, 0) : col.transform.position.x > transform.parent.position.x ? new Vector2(+5, 0) : new Vector2(-5, 0));
                        }
                        else
                        {
                            col.transform.parent.GetComponent<Rigidbody2D>().AddForce(Input.GetAxisRaw("Horizontal Trigger") > 0 ? new Vector2(+25, 0) : Input.GetAxisRaw("Horizontal Trigger") < 0 ? new Vector2(-25, 0) : col.transform.position.x > transform.parent.position.x ? new Vector2(+5, 0) : new Vector2(-5, 0));
                        }
                    }

                    col.transform.parent.GetComponent<MonsterScript>().hp -= GameManager.Sword_Damage;
                    if (col.transform.parent.GetComponent<MonsterScript>().hp <= 0)
                        col.transform.parent.GetComponent<MonsterScript>().Dead();

                    GameManager.VibrationCamera(0.1f);
                    SoundManager.ArrowDamaageSE(true);

                    EffectManager.Play_Hit_Medium(col.transform.position + (col.transform.position.x > transform.parent.position.x ? new Vector3(+col.GetComponent<BoxCollider2D>().size.x * 0.5f, -col.GetComponent<BoxCollider2D>().size.y * 0.5f, 0) : new Vector3(-col.GetComponent<BoxCollider2D>().size.x * 0.5f, -col.GetComponent<BoxCollider2D>().size.y * 0.5f, 0)), !(col.transform.position.x > transform.parent.position.x));

                }
                else if (col.transform.gameObject.tag.Equals("Boss"))
                {
                    if(GameManager.Boss_UI)
                    {
                        GameManager.Boss_now_hp -= GameManager.Sword_Damage;
                        GameManager.VibrationCamera(0.1f);
                        SoundManager.ArrowDamaageSE(true);

                        GameManager.UpdateHpBar();

                        EffectManager.Play_Hit_Medium(col.transform.position + (col.transform.position.x > transform.parent.position.x ? new Vector3(+col.GetComponent<BoxCollider2D>().size.x * 0.5f, -col.GetComponent<BoxCollider2D>().size.y * 0.5f, 0) : new Vector3(-col.GetComponent<BoxCollider2D>().size.x * 0.5f, -col.GetComponent<BoxCollider2D>().size.y * 0.5f, 0)), !(col.transform.position.x > transform.parent.position.x));
                    }
                }
                else if (col.transform.gameObject.name.Equals("Bell"))
                {
                    col.transform.gameObject.layer = LayerMask.NameToLayer("Default");
                    col.GetComponent<BoxCollider2D>().enabled = false;
                    col.GetComponent<Rigidbody2D>().AddForce(col.transform.position.x > transform.parent.position.x ? new Vector2(+1500, 500) : new Vector2(-1500, 500));
                    GameManager.VibrationCamera(0.05f);
                    SoundManager.ArrowDamaageSE(true);
                    SoundManager.BellSE(true);

                    EffectManager.Play_Hit_Normal(transform.position, !(col.transform.position.x > transform.position.x));
                    EffectManager.Play_Hit_Small(transform.position, !(col.transform.position.x > transform.position.x));

                    GameManager.SaveGame(col.transform.parent.gameObject);
                }
                else if (col.transform.gameObject.name.Equals("Lever"))
                {
                    StartCoroutine(col.GetComponent<TriggerScript>().Set_Trigger());
                    GameManager.VibrationCamera(0.1f);
                    SoundManager.ArrowDamaageSE(true);
                }
                else if (col.transform.gameObject.name.Equals("Treasure_Box"))
                {
                    col.transform.parent.GetComponent<Animator>().SetBool("Open", true);
                    col.transform.gameObject.SetActive(false);
                    GameManager.VibrationCamera(0.1f);
                    SoundManager.ArrowDamaageSE(true);
                }
            }
        }
    }

    #endregion
}