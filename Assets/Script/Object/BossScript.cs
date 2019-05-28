using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public enum Boss_Type { Death , Summoner , Slime}

    [SerializeField]
    [Header("보스 종류")]
    public Boss_Type this_type;

    [SerializeField]
    [Header("보스 애니메이션")]
    Animator animator;

    [SerializeField]
    [Header("파이어볼")]
    GameObject fire_ball_prefab;

    [SerializeField]
    [Header("슬라임몬스터")]
    GameObject[] slime_prefab;

    float time = -2;
    float boss_time = 0;
    float fireball_time = 0;
    float deathtime = 0;
    int pattern = -1;
    int backpattern = 0;
    int fireballcount = 0;
    bool patterncheck = false;
    float starty = 0;
    float speed = 5;
    float turn_time = 0;
    float move_speed = 1;
    #region[Update]
    private void Update()
    {
        time += Time.deltaTime;
        boss_time += Time.deltaTime;
        #region[죽음의 사자]
        if (this_type == Boss_Type.Death)
        {
            if (boss_time <= 5)
                starty = transform.position.y;
            #region[보스죽음]
            if (!animator.GetBool("Death") && animator.GetInteger("Pattern") != 44)
                animator.SetBool("Death", GameManager.Boss_now_hp <= 0 ? true : false);
            if (animator.GetInteger("Pattern") == 44)
            {
                animator.SetBool("Death", false);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Demon_Death"))
            {
                StageManager.player_static.GetComponent<Player_Maker>().stop = true;
                deathtime += Time.deltaTime;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (deathtime >= 3f)
                {
                    Camera_Maker[] cam = StageManager.camera_static.GetComponents<Camera_Maker>();
                    try
                    {
                        cam[1].enabled = false;
                        cam[2].enabled = true;
                    }
                    catch { Debug.Log("1번이나 , 2번카메라 없음"); }
                    if (GameManager.Boss_UI) GameManager.Boss_UI.SetActive(false);

                    StageManager.player_static.GetComponent<Player_Maker>().stop = false;
                    animator.SetInteger("Pattern", 44);
                }
                if (time > 0.25f)
                {
                    EffectManager.Play_Boom(transform.position + new Vector3(Random.Range(-0.5f, +0.5f), Random.Range(-0.5f, +0.5f), 0), 0);
                    time = 0;
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Demon_Death_End"))
            {
                Destroy(gameObject);
            }
            if (GameManager.Boss_UI && GameManager.Boss_now_hp <= 0) return;
            #endregion

            #region[보스패턴]
            if (time > 3f / animator.speed)
            {
                animator.speed = (GameManager.GameLevel == TutorialScript.GameLevel.쉬움) ? 0.75f : (GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) ? 0.75f : 1.25f;
                if (pattern == -1)
                {
                    int r = 0;
                    if (boss_time > 10f)
                    {
                        if ((GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) || GameManager.GameLevel == TutorialScript.GameLevel.쉬움)
                        {
                            do { r = Random.Range(0, 4); } while (backpattern == r);
                        }
                        else
                        {
                            do { r = Random.Range(0, 5); } while (backpattern == r);
                        }
                    }
                    pattern = r;
                }
                if (pattern == 0 || pattern == 1 || pattern == 2)
                {
                    #region[패턴1]
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Demon_Pattern0"))
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                        {
                            time = 0;
                            pattern = -1;
                            animator.SetInteger("Pattern", -1);
                            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * 2 * animator.speed;
                        }
                        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.55f)
                        {
                            GetComponent<Rigidbody2D>().velocity = transform.lossyScale.x == -1 ? new Vector2(2f * animator.speed, GetComponent<Rigidbody2D>().velocity.y) : new Vector2(-2f, GetComponent<Rigidbody2D>().velocity.y);
                        }
                        else
                        {
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            GameManager.VibrationCamera(0.1f);
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, StageManager.player_static.transform.position) <= 2f)
                        {
                            animator.SetInteger("Pattern", 1);
                            SoundManager.ExplosionSE(true);
                        }
                        else
                        {
                            backpattern = pattern;
                            transform.localScale = transform.position.x > StageManager.player_static.transform.position.x ? new Vector3(+1, 1, 1) : transform.position.x < StageManager.player_static.transform.position.x ? new Vector3(-1, 1, 1) : transform.localScale;
                            GetComponent<Rigidbody2D>().velocity = transform.lossyScale.x == -1 ? new Vector2(4f * animator.speed, GetComponent<Rigidbody2D>().velocity.y) : new Vector2(-4f * animator.speed, GetComponent<Rigidbody2D>().velocity.y);
                        }
                    }
                }
                #endregion
                else if (pattern == 3 || pattern == 4)
                {
                    #region[패턴2]
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Demon_Pattern1"))
                    {
                        if (transform.position.y > StageManager.camera_static.transform.position.y + StageManager.camera_static.GetComponent<Camera>().orthographicSize * 2)
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                            fireball_time += Time.deltaTime;
                            if (fireball_time > 1 / animator.speed)
                            {
                                GameObject emp = Instantiate(fire_ball_prefab, transform.position, Quaternion.identity);
                                Vector3 dic = StageManager.player_static.transform.position - emp.transform.position;
                                dic = dic.normalized;
                                emp.GetComponent<Rigidbody2D>().AddForce(dic * 0.03f * animator.speed);
                                fireball_time = 0;
                                fireballcount++;
                            }
                            if (fireballcount > 3 * animator.speed)
                            {
                                time = 0;
                                pattern = -1;
                                fireballcount = 0;
                                fireball_time = 0;
                                animator.SetInteger("Pattern", -1);

                                transform.position = new Vector3(StageManager.camera_static.transform.position.x, starty, transform.position.z) + (Random.Range(0, 100) > 50 ? +3 : -3) * new Vector3(StageManager.camera_static.GetComponent<Camera>().orthographicSize, 0, 0);
                                SoundManager.ExplosionSE(true);
                            }
                        }
                        else
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2 * animator.speed);
                        }
                    }
                    else
                    {
                        backpattern = pattern;
                        animator.SetInteger("Pattern", 2);
                        transform.position = new Vector3(StageManager.camera_static.transform.position.x, StageManager.camera_static.transform.position.y - StageManager.camera_static.GetComponent<Camera>().orthographicSize, transform.position.z);
                        transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        #region[소환사]
        else if (this_type == Boss_Type.Summoner)
        {
            #region[보스죽음]
            if (!animator.GetBool("Death") && animator.GetInteger("Pattern") != 44)
                animator.SetBool("Death", GameManager.Boss_now_hp <= 0 ? true : false);
            if (animator.GetInteger("Pattern") == 44)
            {
                animator.SetBool("Death", false);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Summoner_Death"))
            {
                animator.SetBool("Death", false);
                StageManager.player_static.GetComponent<Player_Maker>().stop = true;
                deathtime += Time.deltaTime;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (deathtime >= 3f)
                {
                    Camera_Maker[] cam = StageManager.camera_static.GetComponents<Camera_Maker>();
                    try
                    {
                        cam[1].enabled = false;
                        cam[0].enabled = true;
                    }
                    catch { Debug.Log("0번이나 , 1번카메라 없음"); }
                    if (GameManager.Boss_UI) GameManager.Boss_UI.SetActive(false);

                    StageManager.player_static.GetComponent<Player_Maker>().stop = false;
                    animator.SetInteger("Pattern", 44);
                }
                if (time > 0.25f)
                {
                    EffectManager.Play_Boom(transform.position + new Vector3(Random.Range(-0.2f, +0.2f), Random.Range(-0.3f, +0.3f), 0), 0);
                    time = 0;
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Summoner_Death_End"))
            {
                Destroy(gameObject);
            }
            if (GameManager.Boss_UI && GameManager.Boss_now_hp <= 0) return;
            #endregion

            #region[보스패턴]
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Summoner") && boss_time > 6f)
            {
                if(StageManager.camera_static.transform.position.x + StageManager.camera_static.GetComponent<Camera>().orthographicSize < transform.position.x)
                {
                    speed = -speed;
                    GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                }
                else if (StageManager.camera_static.transform.position.x - StageManager.camera_static.GetComponent<Camera>().orthographicSize > transform.position.x)
                {
                    speed = -speed;
                    GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                }
            }
            if (boss_time > 6f)
            {
                turn_time += Time.deltaTime;
                if (turn_time > 0.5f)
                {
                    move_speed = -move_speed;
                    turn_time = 0;
                }
                if (pattern == -1)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(speed, move_speed);
                else
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            if (time > 3f / animator.speed)
            {
                animator.speed = (GameManager.GameLevel == TutorialScript.GameLevel.쉬움) ? 0.75f : (GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) ? 0.75f : 1.25f;
                if (pattern == -1)
                {
                    int r = 0;
                    if (boss_time > 6f)
                    {
                        if ((GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) || GameManager.GameLevel == TutorialScript.GameLevel.쉬움)
                        {
                            do { r = Random.Range(0, 4); } while (backpattern == r);
                        }
                        else
                        {
                            do { r = Random.Range(0, 5); } while (backpattern == r);
                        }
                    }
                    pattern = r;
                }
                if (pattern == 0 || pattern == 1 || pattern == 2)
                {
                    #region[패턴1]
                    if (animator.GetInteger("Pattern") != 1)
                        animator.SetInteger("Pattern", 1);
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Summoner_Pattern0"))
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.8f && !patterncheck)
                        {
                            fireballcount++;
                            patterncheck = true;
                        }
                        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.99f && patterncheck)
                        {
                            patterncheck = false;
                        }
                        if (fireballcount > 3)
                        {
                            patterncheck = false;
                            fireballcount = 0;
                            time = 0;
                            pattern = -1;
                            animator.SetInteger("Pattern", -1);
                            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * 2 * animator.speed;
                        }
                    }
                    #endregion
                }
                else if (pattern == 3 || pattern == 4)
                {
                    #region[패턴2]
                    if (animator.GetInteger("Pattern") != 2)
                        animator.SetInteger("Pattern", 2);
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Summoner_Pattern1"))
                    {
                        if (transform.position.y > StageManager.camera_static.transform.position.y + StageManager.camera_static.GetComponent<Camera>().orthographicSize * 2)
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                            fireball_time += Time.deltaTime;
                            if (fireball_time > 1 / animator.speed)
                            {
                                /*
                                GameObject emp = Instantiate(fire_ball_prefab, transform.position, Quaternion.identity);
                                Vector3 dic = StageManager.player_static.transform.position - emp.transform.position;
                                dic = dic.normalized;
                                emp.GetComponent<Rigidbody2D>().AddForce(dic * 0.03f * animator.speed);
                                fireball_time = 0;
                                fireballcount++;
                                */
                            }
                            if (fireballcount > 3 * animator.speed)
                            {
                                /*
                                time = 0;
                                pattern = -1;
                                fireballcount = 0;
                                fireball_time = 0;
                                animator.SetInteger("Pattern", -1);

                                transform.position = new Vector3(StageManager.camera_static.transform.position.x, starty, transform.position.z) + (Random.Range(0, 100) > 50 ? +3 : -3) * new Vector3(StageManager.camera_static.GetComponent<Camera>().orthographicSize, 0, 0);
                                SoundManager.ExplosionSE(true);
                                */
                            }
                        }
                        else
                        {
                           // GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2 * animator.speed);
                        }
                    }
                    else
                    {
                        backpattern = pattern;
                        animator.SetInteger("Pattern", 2);
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        #region[슬라임]
        else if (this_type == Boss_Type.Slime)
        {
            #region[보스죽음]
            if (!animator.GetBool("Death") && animator.GetInteger("Pattern") != 44)
                animator.SetBool("Death", GameManager.Boss_now_hp <= 0 ? true : false);
            if (animator.GetInteger("Pattern") == 44)
            {
                animator.SetBool("Death", false);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Death"))
            {
                animator.SetBool("Death", false);
                StageManager.player_static.GetComponent<Player_Maker>().stop = true;
                deathtime += Time.deltaTime;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (deathtime >= 3f)
                {
                    Camera_Maker[] cam = StageManager.camera_static.GetComponents<Camera_Maker>();
                    try
                    {
                        cam[1].enabled = false;
                        cam[0].enabled = true;
                    }
                    catch { Debug.Log("0번이나 , 1번카메라 없음"); }
                    if (GameManager.Boss_UI) GameManager.Boss_UI.SetActive(false);

                    StageManager.player_static.GetComponent<Player_Maker>().stop = false;
                    animator.SetInteger("Pattern", 44);
                }
                if (time > 0.25f)
                {
                    EffectManager.Play_Boom(transform.position + new Vector3(Random.Range(-0.2f, +0.2f), Random.Range(-0.3f, +0.3f), 0), 0);
                    time = 0;
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Death_End"))
            {
                Destroy(gameObject);
            }
            if (GameManager.Boss_UI && GameManager.Boss_now_hp <= 0) return;
            #endregion
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.05f) + new Vector2(boxCollider2D.offset.x, boxCollider2D.offset.y), new Vector2(boxCollider2D.size.x * 0.8f, boxCollider2D.size.y), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
            animator.SetBool("Fly", !hit);
            #region[보스패턴]
            if (time > 3f / animator.speed)
            {
              
                animator.speed = (GameManager.GameLevel == TutorialScript.GameLevel.쉬움) ? 1f : (GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) ? 1f : 1.5f;
                if (pattern == -1)
                {
                    int r = 0;
                    if (boss_time > 6f)
                    {
                        if ((GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) || GameManager.GameLevel == TutorialScript.GameLevel.쉬움)
                        {
                            do { r = Random.Range(0, 4); } while (backpattern == r);
                        }
                        else
                        {
                            do { r = Random.Range(0, 5); } while (backpattern == r);
                        }
                    }
                    pattern = r;
                }
                if (pattern == 0 || pattern == 1 || pattern == 2)
                {
                    #region[패턴1]
                    if (animator.GetInteger("Pattern") != 1)
                        animator.SetInteger("Pattern", 1);
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Pattern0"))
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.3f && !patterncheck)
                        {
                            fireballcount++;
                            patterncheck = true;
                            Debug.Log("");
                            float levelchange = (GameManager.GameLevel == TutorialScript.GameLevel.쉬움) ? 1f : (GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) ? 1f : 2f;
                            float levelchange2 = (GameManager.GameLevel == TutorialScript.GameLevel.쉬움) ? 1f : (GameManager.Boss_now_hp > GameManager.Boss_max_hp * 0.5f && GameManager.GameLevel == TutorialScript.GameLevel.보통) ? 1f : 1.5f;
                            GetComponent<Rigidbody2D>().mass = levelchange * 2;
                            GetComponent<Rigidbody2D>().AddForce(new Vector2(StageManager.player_static.transform.position.x < transform.position.x ? -100 * levelchange : 100* levelchange, 750* levelchange2));
                        }
                        else if (!hit && patterncheck)
                        {
                            patterncheck = false;
                        }
                        if (fireballcount > 10 / animator.speed)
                        {
                            backpattern = pattern;
                            patterncheck = false;
                            fireballcount = 0;
                            time = 0;
                            pattern = -1;
                            animator.SetInteger("Pattern", -1);
                        }
                    }
                    #endregion
                }
                else if (pattern == 3 || pattern == 4)
                {
                    #region[패턴2]
                    if (animator.GetInteger("Pattern") != 2)
                        animator.SetInteger("Pattern", 2);
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Pattern1"))
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.3f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 <= 0.8f  && !patterncheck)
                        {

                            fireballcount++;
                            patterncheck = true;
                            GameObject emp = Instantiate(slime_prefab[Random.Range(0, slime_prefab.Length)], transform.position, Quaternion.identity);
                            emp.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100));
                            emp.transform.parent = transform.parent;
                        }
                        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.8f && patterncheck)
                        {
                            patterncheck = false;
                        }
                        if (fireballcount > 10 / animator.speed)
                        {
                            backpattern = pattern;
                            patterncheck = false;
                            fireballcount = 0;
                            time = 0;
                            pattern = -1;
                            animator.SetInteger("Pattern", -1);
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion
    }
    #endregion
}
