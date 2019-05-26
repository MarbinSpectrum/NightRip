using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Platform_Maker : MonoBehaviour
{
    #region[잡다변수]
    [HideInInspector] public bool awake = true;

    [HideInInspector] public int now_option;

    //이동관련 변수
    [HideInInspector] public bool route_rotation = true;
    [HideInInspector] public List<Vector3> waypoint = new List<Vector3>() { new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0) };
    [HideInInspector] public bool showpoints = false;
    [HideInInspector] public bool repeat_rotation = false;
    [HideInInspector] public Vector3 start_pos = Vector3.zero;
    [HideInInspector] public float move_state = 0;
    [HideInInspector] public float max_move_state = 0;
    [HideInInspector] public float move_speed = 0.01f;

    //은시니관련 변수
    [HideInInspector] public bool collider_enabled_check = true;
    [HideInInspector] public bool repeat_hide = true;
    [HideInInspector] public bool collider_state = true;
    [HideInInspector] public bool showanimation_option = false;
    [HideInInspector] public float tranform_show_time = 1f;
    [HideInInspector] public float tranform_hide_time = 1f;
    [HideInInspector] public float animation_start_time = 0f;
    [HideInInspector] public float animation_show_time = 1f;
    [HideInInspector] public float animation_hide_time = 4f;
    [HideInInspector] public float animation_end_time = 5f;
    [HideInInspector] public float now_animation_time = 0f;
    #endregion

    #region[Update]
    void Update()
    {
        #region[이동하는 액션]
        if (now_option == 1)
        {
            if (Time.timeScale == 0) return;
            if (GetComponent<PolygonCollider2D>() && !GetComponent<PolygonCollider2D>().enabled)
            {
                GetComponent<PolygonCollider2D>().enabled = true;
            }
            if (GetComponent<BoxCollider2D>() && !GetComponent<BoxCollider2D>().enabled)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
            if (move_state >= max_move_state)
            {
                if (repeat_rotation && !route_rotation)
                {
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        gameObject.transform.GetChild(0).transform.parent = transform.parent;
                    }
                    move_state = 0;
                }
                else if (repeat_rotation || route_rotation)
                {
                    move_state = 0;
                }
                else
                {
                    move_state = max_move_state;
                }
            }
            else
            {
                float level_change = (GameManager.GameLevel == TutorialScript.GameLevel.쉬움 || GameManager.GameLevel == TutorialScript.GameLevel.보통) ? 1f : GameManager.GameLevel == TutorialScript.GameLevel.어려움 ? 1.2f : GameManager.GameLevel == TutorialScript.GameLevel.악몽 ? 1.2f : 1;
                move_state += move_speed * level_change;
            }

            #region[애니메이션 작동]
            float length = 0;
            int nowpos = route_rotation ? waypoint.Count : waypoint.Count - 1;
            int check_length = route_rotation ? waypoint.Count + 1 : waypoint.Count;
            for (int i = 1; i < check_length; i++)
            {
                length += Vector3.Distance(waypoint[(i - 1) % waypoint.Count], waypoint[i % waypoint.Count]);
                if (length > move_state)
                {
                    length -= Vector3.Distance(waypoint[(i - 1) % waypoint.Count], waypoint[i % waypoint.Count]);
                    nowpos = i - 1;
                    break;
                }
            }
            float dis = length - move_state;
            Vector3 dir = Vector3.Normalize(waypoint[nowpos % waypoint.Count] - waypoint[(nowpos + 1) % waypoint.Count]);
            start_pos = waypoint[nowpos % waypoint.Count] + dir * dis;
            transform.position = start_pos;
            #endregion

        }
        #endregion

        #region[은신하는 액션]
        else if (now_option == 2)
        {
            if (Time.timeScale == 0) return;
            if (collider_enabled_check)
            {
                if (GetComponent<PolygonCollider2D>())
                {
                    GetComponent<PolygonCollider2D>().enabled = collider_state;
                }
                if (GetComponent<BoxCollider2D>())
                {
                    GetComponent<BoxCollider2D>().enabled = collider_state;
                }
            }

            #region[애니메이션 작동]
            if (GetComponent<SpriteRenderer>())
            {
                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                now_animation_time += Time.deltaTime;
                now_animation_time = now_animation_time > animation_end_time ? (repeat_hide ? 0 : animation_end_time) : now_animation_time;
                if (GameManager.GameLevel != TutorialScript.GameLevel.쉬움)
                {
                    if (now_animation_time < animation_show_time - tranform_show_time || now_animation_time > animation_hide_time + tranform_hide_time)
                    {
                        Set_Color(gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0));
                        if (collider_enabled_check)
                        {
                            collider_state = false;
                        }
                    }
                    else if (now_animation_time >= animation_show_time - tranform_show_time && now_animation_time <= animation_show_time)
                    {
                        if (collider_enabled_check)
                        {
                            collider_state = true;
                        }
                        float alpha = (now_animation_time - (animation_show_time - tranform_show_time)) / tranform_show_time;
                        Set_Color(gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha));
                    }
                    else if (now_animation_time >= animation_hide_time && now_animation_time <= animation_hide_time + tranform_hide_time)
                    {
                        if (collider_enabled_check)
                        {
                            collider_state = true;
                        }
                        float alpha = (now_animation_time - animation_hide_time) / tranform_hide_time;
                        Set_Color(gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1 - alpha));
                    }
                    else
                    {
                        if (collider_enabled_check)
                        {
                            collider_state = true;
                        }
                        Set_Color(gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1));
                    }
                }
                else
                {
                    if (collider_enabled_check)
                    {
                        collider_state = true;
                    }
                    Set_Color(gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1));
                }
            }
            #endregion
        }
        #endregion
    }
    #endregion

    #region[스프라이트컬러세팅]
    void Set_Color(GameObject obj, Color color)
    {
        if (obj.GetComponent<SpriteRenderer>() && obj.layer == LayerMask.NameToLayer("Background"))
            obj.GetComponent<SpriteRenderer>().color = color;
        for (int i = 0; i < obj.transform.childCount; i++)
            Set_Color(obj.transform.GetChild(i).gameObject, color);
    }
    #endregion

    #region[OnDrawGizmos]
    private void OnDrawGizmos()
    {
        if (!GetComponent<Collider2D>())
        {
            gameObject.AddComponent(typeof(PolygonCollider2D));
            gameObject.AddComponent(typeof(BoxCollider2D));
            GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x * 0.9f, GetComponent<BoxCollider2D>().size.y * 0.25f);
            GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<BoxCollider2D>().offset.x, GetComponent<BoxCollider2D>().size.y * 2f);
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (!GetComponent<SpriteRenderer>())
        {
            gameObject.AddComponent(typeof(SpriteRenderer));
        }

        #region[이동경로그리기]
        if (now_option == 1)
        {
            float move_length = 0;
            for (int i = 0; i < waypoint.Count; i++)
            {
                Gizmos.color = Color.green;
                if (i != waypoint.Count - 1)
                {
                    Gizmos.DrawLine(waypoint[i], waypoint[(i + 1) % waypoint.Count]);
                    move_length += Vector3.Distance(waypoint[i], waypoint[(i + 1) % waypoint.Count]);
                }
                else if (route_rotation)
                {
                    Gizmos.DrawLine(waypoint[i], waypoint[(i + 1) % waypoint.Count]);
                    move_length += Vector3.Distance(waypoint[i], waypoint[(i + 1) % waypoint.Count]);
                }
                max_move_state = move_length;
            }
        }
        #endregion

    }
    #endregion

    #region[OnTriggerEnter2D]
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag.Equals("Player"))
        {
            col.transform.parent = transform;
        }
        else if (col.transform.tag.Equals("Monster") && col.transform.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            if (col.transform.gameObject.GetComponent<BoxCollider2D>() && !col.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger)
                col.transform.parent = transform;
        }
    }
    #endregion

    #region[OnTriggerExit2D]
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag.Equals("Player"))
        {
            col.transform.parent = transform.parent;
        }
        else if (col.transform.tag.Equals("Monster") && col.transform.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            if (col.transform.gameObject.GetComponent<BoxCollider2D>() && !col.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger)
                col.transform.parent = transform.parent;
        }
    }
    #endregion
}

