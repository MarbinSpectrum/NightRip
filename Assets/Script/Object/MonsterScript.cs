using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    #region[잡다변수]
    [HideInInspector]
    public float time = 0;

    [HideInInspector]
    public float attack_time = 0;

    [HideInInspector]
    public bool damage = false;

    [HideInInspector]
    public bool attack = false;

    public enum Type { Witch, Dog, Skeleton , Lantern , Bat}

    Rigidbody2D rigidbody;

    RaycastHit2D isground;

    [SerializeField]
    [Header("몬스터 체력")]
    public int hp;

    [SerializeField]
    [Header("몬스터 종류")]
    public Type this_type;

    [SerializeField]
    [Header("몬스터 스프라이트")]
    SpriteRenderer sprite;

    [SerializeField]
    [Header("몬스터 애니메이션")]
    Animator animator;

    [SerializeField]
    [Header("몬스터 마법")]
    GameObject magic;
    GameObject magic_play;

    [SerializeField]
    [Header("몬스터 공격주기")]
    float attack_cycle;

    [SerializeField]
    [Header("플레이어")]
    public GameObject player;

    [SerializeField]
    [Header("몬스터 탐색여부")]
    public bool seek = true;
    #endregion

    #region[Awake]
    void Awake()
    {
        if (this_type == Type.Witch)
            transform.name = "Witch";
        if (this_type == Type.Dog)
            transform.name = "Dog";
        if (this_type == Type.Skeleton)
            transform.name = "Skeleton";
        if (this_type == Type.Lantern)
            transform.name = "Lantern";
        if (this_type == Type.Bat)
            transform.name = "Bat";
        rigidbody = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region[Update]
    void Update()
    {
        Ani();
        Damage_Time();
        Seek_Player();
        Attack();
    }
    #endregion

    #region[애니메이션]
    void Ani()
    {
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
        isground = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.2f), new Vector2(GetComponent<BoxCollider2D>().size.x * 0.8f, GetComponent<BoxCollider2D>().size.y), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
        float level_change = GameManager.GameLevel == TutorialScript.GameLevel.쉬움 ? 0.75f : GameManager.GameLevel == TutorialScript.GameLevel.보통 ? 1f : GameManager.GameLevel == TutorialScript.GameLevel.어려움 ? 1.2f : GameManager.GameLevel == TutorialScript.GameLevel.악몽 ? 1.2f : 1;
        animator.speed = level_change;
        if (player)
        {
            if (isground)
            {
                transform.localScale = transform.position.x > player.transform.position.x ? new Vector3(+Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z) : transform.position.x < player.transform.position.x ? new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z) : transform.localScale;
            }

            if (this_type == Type.Witch)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("WitchAttack"))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 0)
                    {
                        SoundManager.WitchSE(true);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.8f)
                    {
                        if (!magic_play)
                        {
                            magic_play = Instantiate(magic, transform.position, Quaternion.identity);
                            magic_play.GetComponent<Rigidbody2D>().AddForce(0.03f * Vector3.Normalize(player.transform.position - transform.position));
                            SoundManager.WitchSE(true, 2);
                        }
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        magic_play = null;
                        attack = false;
                        SoundManager.WitchSE(true, 1);
                    }
                }
            }
            else if (this_type == Type.Dog)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("DogAttack"))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 0)
                    {
                        rigidbody.AddForce(transform.lossyScale.x == -1 ? new Vector2(80, 150) : new Vector2(-80, 150));
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && isground)
                    {
                        attack = false;
                    }
                }
            }
            else if (this_type == Type.Skeleton)
            {

                if(Vector2.Distance(transform.position,player.transform.position) > 0.5f && !animator.GetCurrentAnimatorStateInfo(0).IsName("SkeletonAttack"))
                {
                    Vector2 vector2 = transform.lossyScale.x == -1 ? new Vector2(0.3f, 0) : new Vector2(-0.3f, 0);
                    RaycastHit2D nextground = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.2f) + vector2, new Vector2(GetComponent<BoxCollider2D>().size.x * 0.8f, GetComponent<BoxCollider2D>().size.y), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
                    if (!damage && nextground)
                    {
                        rigidbody.velocity = transform.lossyScale.x == -1 ? new Vector2(0.5f, rigidbody.velocity.y) : new Vector2(-0.5f, rigidbody.velocity.y);
                        rigidbody.velocity = rigidbody.velocity * level_change;
                        animator.SetBool("Walk", true);
                    }
                    if(!nextground)
                    {
                        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                        animator.SetBool("Walk", false);
                    }
                }
                else
                {
                    animator.SetBool("Walk", false);
                    if (!damage)
                    {
                        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                    }
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("SkeletonAttack"))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        attack = false;
                    }
                }
            }
            else if (this_type == Type.Lantern)
            {

                if (Vector2.Distance(transform.position, player.transform.position) > 1f && !animator.GetCurrentAnimatorStateInfo(0).IsName("LanternWanderer_Attack"))
                {
                    Vector2 vector2 = transform.lossyScale.x == -1 ? new Vector2(0.3f, 0) : new Vector2(-0.3f, 0);
                    RaycastHit2D nextground = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.2f) + vector2, new Vector2(GetComponent<BoxCollider2D>().size.x * 0.8f, GetComponent<BoxCollider2D>().size.y), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
                    if (!damage && nextground)
                    {
                        rigidbody.velocity = transform.localScale.x == -1 ? new Vector2(0.6f, rigidbody.velocity.y) : new Vector2(-0.6f, rigidbody.velocity.y);
                        rigidbody.velocity = rigidbody.velocity * level_change;
                    }
                    if (!nextground)
                    {
                        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                    }
                }
                else
                {
                    if (!damage)
                    {
                        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                    }
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("LanternWanderer_Attack"))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        attack = false;
                    }
                }
            }
            else if (this_type == Type.Bat)
            {
                transform.localScale = transform.position.x > player.transform.position.x ? new Vector3(+1, 1, 1) : transform.position.x < player.transform.position.x ? new Vector3(-1, 1, 1) : transform.localScale;
                if (attack)
                {
                    Vector3 dic = StageManager.player_static.transform.position - transform.position;
                    dic = dic.normalized;
                    rigidbody.velocity = dic * 1;
                    attack = false;
                }
            }
        }
    }
    #endregion

    #region[데미지시간]
    void Damage_Time()
    {
        if(damage)
        {
            magic_play = null;
            attack = false;
            attack_time = 0;
            time += Time.deltaTime;
            if (time > 0.5f && isground)
                damage = false;
        }
        else if(time != 0)
            time = 0;
    }
    #endregion

    #region[플레이어 탐색]
    void Seek_Player()
    {
        if(seek)
        {
            if (!player)
            {
                float level_change = GameManager.GameLevel == TutorialScript.GameLevel.쉬움 ? 0.75f : GameManager.GameLevel == TutorialScript.GameLevel.보통 ? 1f : GameManager.GameLevel == TutorialScript.GameLevel.어려움 ? 1.5f : GameManager.GameLevel == TutorialScript.GameLevel.악몽 ? 1.5f : 1;
                RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 3* level_change, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Object"));
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i] && hit[i].transform.name.Equals("Player"))
                    {
                        if (transform.localScale.x < 0 && transform.position.x < hit[i].transform.position.x)
                        {
                            player = hit[i].transform.gameObject;
                        }
                        else if (transform.localScale.x > 0 && transform.position.x > hit[i].transform.position.x)
                        {
                            player = hit[i].transform.gameObject;
                        }
                        break;
                    }
                }
            }
            else
            {
                if (Vector2.Distance(player.transform.position, transform.position) > 5)
                {
                    player = null;
                    attack = false;
                    attack_time = 0;
                }
            }
        }
        else
        {
            player = StageManager.player_static;
        }
       
    }
    #endregion

    #region[플레이어 공격]
    void Attack()
    {
        if(player)
        {
            float level_change = GameManager.GameLevel == TutorialScript.GameLevel.쉬움 ? 0.5f : GameManager.GameLevel == TutorialScript.GameLevel.보통 ? 1f : GameManager.GameLevel == TutorialScript.GameLevel.어려움 ? 1f : GameManager.GameLevel == TutorialScript.GameLevel.악몽 ? 1f : 1;
            if (!attack)
            {
                if (isground)
                {
                   
                    attack_time += Time.deltaTime;
                    if (attack_time > attack_cycle/ level_change)
                    {
                        if (this_type == Type.Skeleton)
                        {
                            if (Vector2.Distance(transform.position, player.transform.position) <= 0.5f)
                            {
                                attack = true;
                                attack_time = 0;
                            }
                        }
                        else if (this_type == Type.Lantern)
                        {
                            if (Vector2.Distance(transform.position, player.transform.position) <= 1f)
                            {
                                attack = true;
                                attack_time = 0;
                            }
                        }
                        else
                        {
                            attack = true;
                            attack_time = 0;
                        }
                    }
                }
                else
                {
                    if (this_type == Type.Bat)
                    {
                        attack_time += Time.deltaTime;
                        if (attack_time > attack_cycle/ level_change)
                        {
                            attack = true;
                            attack_time = 0;
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region[몬스터 죽음]
    public void Dead()
    {
        EffectManager.Play_Boom(transform.position,0);
        Destroy(gameObject);
    }
    #endregion

    #region[OnTriggerStay2D]
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.GetComponent<HurtObjectScript>() && col.transform.GetComponent<HurtObjectScript>().monster_dead && !col.transform.tag.Equals("Monster"))
        {
            hp -= col.transform.GetComponent<HurtObjectScript>().damage;
            if (hp <= 0)
            {
                Dead();
            }
        }
    }
    #endregion
}
