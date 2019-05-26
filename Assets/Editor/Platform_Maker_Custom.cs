using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Platform_Maker))]
public class Platform_Maker_Custom : Editor
{
    Platform_Maker _editor;

    #region[언어 변수]
    int[] language_value = new int[] { 0, 1 };

    void Set_Language()
    {
        for (int i = 0; i < Language_Data.option.Length; i++)
        {
            Language_Data.option[i] = Language_Data.option_name[i, Language_Data.select_language];
        }
        for (int i = 0; i < Language_Data.option_help.Length; i++)
        {
            Language_Data.option_help[i] = Language_Data.option_help_data[i, Language_Data.select_language];
        }
    }
    #endregion

    #region[메뉴 변수]

    enum Menu { None, Move, Hide };

    #region[은신옵션]
    float enablepos = 0;
    float disenablepos = 1;
    #endregion

    #endregion

    #region[OnEnable]
    void OnEnable()
    {
        _editor = target as Platform_Maker;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        #region[언어선택]
        Language_Data.select_language = EditorGUILayout.IntPopup(Language_Data.language_title_name[Language_Data.select_language] + " : ", Language_Data.select_language, Language_Data.language_name, language_value);
        Set_Language();
        #endregion

        #region[제목 생성]
        GUIStyle Title_Style = new GUIStyle();
        Title_Style.fontSize = 20;
        Title_Style.normal.textColor = new Color(34 / 255f, 177 / 255f, 76 / 255f);
        EditorGUILayout.LabelField(Language_Data.plaform_title_name[Language_Data.select_language], Title_Style, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        EditorGUILayout.HelpBox(Language_Data.plaform_title_help_data[Language_Data.select_language], MessageType.None);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion

        #region[옵션 생성]
        _editor.now_option = EditorGUILayout.IntPopup(Language_Data.option_title_name[Language_Data.select_language] + " : ", _editor.now_option, Language_Data.option, new int[] { 0, 1, 2 });
        EditorGUILayout.HelpBox(Language_Data.option_help[_editor.now_option], MessageType.None);
        #endregion

        #region[메뉴 생성]
        GUIStyle Option_Title_Style = new GUIStyle();
        Option_Title_Style.fontStyle = FontStyle.Bold;
        Option_Title_Style.alignment = TextAnchor.MiddleLeft;
        #endregion

        #region[이동 설정]
        if (_editor.now_option == (int)Menu.Move)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                if (_editor.GetComponent<SpriteRenderer>())
                {
                    SpriteRenderer sprite = _editor.GetComponent<SpriteRenderer>();
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
                }
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField(Language_Data.option_setting_title[Language_Data.select_language], Option_Title_Style);

                #region[순환 여부]
                _editor.route_rotation = EditorGUILayout.Toggle(Language_Data.route_rotation_title[Language_Data.select_language] + " : ", _editor.route_rotation);
                EditorGUILayout.HelpBox(Language_Data.route_rotation_title_help_data[Language_Data.select_language], MessageType.None);
                #endregion

                #region[경로 점 들]
                GUIStyle point_style;
                point_style = EditorStyles.foldout;
                point_style.margin = new RectOffset(15, 0, 0, 0);
                _editor.showpoints = EditorGUILayout.Foldout(_editor.showpoints, Language_Data.point_data_title[Language_Data.select_language], point_style);
                if (_editor.showpoints)
                {
                    for (int i = 0; i < _editor.waypoint.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        _editor.waypoint[i] = EditorGUILayout.Vector3Field("", _editor.waypoint[i]);
                        if (_editor.waypoint.Count > 3)
                        {
                            if (GUILayout.Button(Language_Data.delete_name[Language_Data.select_language]))
                            {
                                _editor.waypoint.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button(Language_Data.create_name[Language_Data.select_language]))
                    {
                        Vector3 Create_Point = (_editor.waypoint[0] + _editor.waypoint[1]) * 0.5f;
                        _editor.waypoint.Insert(1, Create_Point);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox(Language_Data.point_data_title_help_data[Language_Data.select_language], MessageType.None);
                }
                #endregion

                #region[반복여부]
                if (!_editor.route_rotation)
                {
                    _editor.repeat_rotation = EditorGUILayout.Toggle(Language_Data.repeat_name[Language_Data.select_language] + " : ", _editor.repeat_rotation);
                    EditorGUILayout.HelpBox(Language_Data.repeat_rotation_name_help_data[0, Language_Data.select_language], MessageType.None);
                    EditorGUILayout.HelpBox(Language_Data.repeat_rotation_name_help_data[1, Language_Data.select_language], MessageType.Info);
                }

                #endregion

                #region[플렛폼 이동상태]
                _editor.move_state = EditorGUILayout.Slider(Language_Data.move_state_name[Language_Data.select_language] + " : ", _editor.move_state, 0, _editor.max_move_state);
                float length = 0;
                int nowpos = _editor.route_rotation ? _editor.waypoint.Count : _editor.waypoint.Count - 1;
                int check_length = _editor.route_rotation ? _editor.waypoint.Count + 1 : _editor.waypoint.Count;

                for (int i = 1; i < check_length; i++)
                {
                    length += Vector3.Distance(_editor.waypoint[(i - 1) % _editor.waypoint.Count], _editor.waypoint[i % _editor.waypoint.Count]);
                    if (length > _editor.move_state)
                    {
                        length -= Vector3.Distance(_editor.waypoint[(i - 1) % _editor.waypoint.Count], _editor.waypoint[i % _editor.waypoint.Count]);
                        nowpos = i - 1;
                        break;
                    }
                }
                float dis = length - _editor.move_state;
                Vector3 dir = Vector3.Normalize(_editor.waypoint[nowpos % _editor.waypoint.Count] - _editor.waypoint[(nowpos + 1) % _editor.waypoint.Count]);
                _editor.start_pos = _editor.waypoint[nowpos % _editor.waypoint.Count] + dir * dis;
                EditorGUILayout.HelpBox(Language_Data.move_state_help_data[Language_Data.select_language], MessageType.None);
                #endregion

                #region[플랫폼 이동속도]
                _editor.move_speed = EditorGUILayout.FloatField(Language_Data.move_speed_name[Language_Data.select_language] + " : ", _editor.move_speed);
                if (_editor.move_speed < 0)
                {
                    _editor.move_speed = 0;
                }
                #endregion

                #region[플랫폼 주기]
                length = 0;
                for (int i = 1; i < check_length; i++)
                {
                    length += Vector3.Distance(_editor.waypoint[(i - 1) % _editor.waypoint.Count], _editor.waypoint[i % _editor.waypoint.Count]);
                }
                float cycle = length / _editor.move_speed * 1 / 60f;
                EditorGUILayout.LabelField(Language_Data.route_cycle_name[Language_Data.select_language] + " : " + cycle.ToString("N2"));
                EditorGUILayout.HelpBox(Language_Data.route_cycle_help_data[Language_Data.select_language], MessageType.None);
                #endregion

                #region[현재 애니메이션 시간]
                EditorGUILayout.LabelField(Language_Data.now_platform_time_name[Language_Data.select_language] + " : " + (cycle * (_editor.move_state / _editor.max_move_state)).ToString("N2"));
                EditorGUILayout.HelpBox(Language_Data.now_platform_time_help_data[Language_Data.select_language], MessageType.None);
                #endregion

                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region[은신 설정]
        else if (_editor.now_option == (int)Menu.Hide)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                #region[GUI 스타일 모음]
                GUIStyle animation_option_style;
                animation_option_style = EditorStyles.foldout;
                animation_option_style.margin = new RectOffset(15, 0, 0, 0);

                GUIStyle MaxStyle = new GUIStyle();
                MaxStyle.alignment = TextAnchor.UpperRight;

                GUIStyle MinStyle = new GUIStyle();
                MinStyle.alignment = TextAnchor.UpperLeft;

                GUIStyle animation_setting_style = new GUIStyle();
                animation_setting_style.fontSize = 10;
                animation_setting_style.fontStyle = FontStyle.Bold;
                animation_setting_style.alignment = TextAnchor.MiddleCenter;

                #endregion

                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField(Language_Data.option_setting_title[Language_Data.select_language], Option_Title_Style);

                #region[판정 여부]
                _editor.collider_enabled_check = EditorGUILayout.Toggle(Language_Data.collider_enabled_check_name[Language_Data.select_language] + " : ", _editor.collider_enabled_check);
                EditorGUILayout.HelpBox(Language_Data.collider_enabled_check_help_data[Language_Data.select_language], MessageType.None);
                #endregion

                #region[반복 여부]
                _editor.repeat_hide = EditorGUILayout.Toggle(Language_Data.repeat_name[Language_Data.select_language] + " : ", _editor.repeat_hide);
                EditorGUILayout.HelpBox(Language_Data.repeat_hide_name_help_data[Language_Data.select_language], MessageType.None);
                #endregion

                #region[애니메이션 옵션]

                _editor.showanimation_option = EditorGUILayout.Foldout(_editor.showanimation_option, Language_Data.animation_option_title[Language_Data.select_language], animation_option_style);
                if (_editor.showanimation_option)
                {
                    #region[애니메이션 전체시간]
                    _editor.animation_end_time = EditorGUILayout.FloatField(Language_Data.animate_time_name[Language_Data.select_language] + " : ", _editor.animation_end_time);
                    if (_editor.animation_end_time < 0)
                    {
                        _editor.animation_end_time = 0;
                    }
                    else if (_editor.animation_end_time < _editor.tranform_hide_time + _editor.tranform_show_time)
                    {
                        _editor.animation_end_time = _editor.tranform_hide_time + _editor.tranform_show_time;
                    }
                    EditorGUILayout.HelpBox(Language_Data.animate_time_help_data[Language_Data.select_language], MessageType.None);
                    #endregion

                    #region[나타난상태로 전환 시간]]
                    _editor.tranform_show_time = EditorGUILayout.FloatField(Language_Data.animate_enabled_time_name[Language_Data.select_language] + " : ", _editor.tranform_show_time);
                    if (_editor.tranform_show_time < 0)
                    {
                        _editor.tranform_show_time = 0;
                    }
                    else if (_editor.tranform_show_time > _editor.animation_end_time)
                    {
                        _editor.tranform_show_time = _editor.animation_end_time;
                    }
                    else if (_editor.tranform_show_time > _editor.animation_hide_time)
                    {
                        _editor.tranform_show_time = _editor.animation_hide_time;
                    }
                    EditorGUILayout.HelpBox(Language_Data.animate_enabled_time_help_data[Language_Data.select_language], MessageType.None);
                    #endregion

                    #region[사라진상태로 전환 시간]
                    _editor.tranform_hide_time = EditorGUILayout.FloatField(Language_Data.animate_disenabled_time_name[Language_Data.select_language] + " : ", _editor.tranform_hide_time);
                    if (_editor.tranform_hide_time < 0)
                    {
                        _editor.tranform_hide_time = 0;
                    }
                    else if (_editor.tranform_hide_time > _editor.animation_end_time)
                    {
                        _editor.tranform_hide_time = _editor.animation_end_time;
                    }
                    else if (_editor.animation_end_time - _editor.tranform_hide_time < _editor.animation_show_time)
                    {
                        _editor.tranform_hide_time = _editor.animation_show_time;
                    }
                    EditorGUILayout.HelpBox(Language_Data.animate_disenabled_time_help_data[Language_Data.select_language], MessageType.None);
                    #endregion

                    #region[은신단계설정]
                    EditorGUILayout.LabelField(Language_Data.hide_state_name[Language_Data.select_language], animation_setting_style, GUILayout.Width(155));

                    enablepos = _editor.animation_show_time;
                    disenablepos = _editor.animation_hide_time;

                    EditorGUILayout.MinMaxSlider(ref enablepos, ref disenablepos, _editor.animation_start_time, _editor.animation_end_time, GUILayout.Width(155));

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(0 + "(" + Language_Data.second_name[Language_Data.select_language] + ")", MinStyle, GUILayout.Width(5));
                    EditorGUILayout.LabelField(_editor.animation_end_time.ToString("N2") + "(" + Language_Data.second_name[Language_Data.select_language] + ")", MaxStyle, GUILayout.Width(175));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField(Language_Data.show_start_name[Language_Data.select_language] + " : " + _editor.animation_show_time.ToString("N2"));
                    EditorGUILayout.LabelField(Language_Data.hide_start_name[Language_Data.select_language] + " : " + _editor.animation_hide_time.ToString("N2"));
                    EditorGUILayout.HelpBox(Language_Data.hide_state_help_data[Language_Data.select_language], MessageType.None);

                    if (enablepos - _editor.tranform_show_time < 0)
                    {
                        enablepos = _editor.tranform_show_time;
                    }
                    if (disenablepos + _editor.tranform_hide_time > _editor.animation_end_time)
                    {
                        disenablepos = _editor.animation_end_time - _editor.tranform_hide_time;
                    }
                    if (enablepos > disenablepos)
                    {
                        float emp = enablepos;
                        enablepos = disenablepos;
                        disenablepos = emp;
                    }
                    _editor.animation_show_time = enablepos;
                    _editor.animation_hide_time = disenablepos;

                    EditorGUILayout.Space();
                    #endregion

                    #region[애니메이션 슬라이더]
                    EditorGUILayout.LabelField(Language_Data.now_platform_time_name[Language_Data.select_language]);
                    _editor.now_animation_time = EditorGUILayout.Slider(_editor.now_animation_time, 0, _editor.animation_end_time, GUILayout.Width(210));
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(0 + "(" + Language_Data.second_name[Language_Data.select_language] + ")", MinStyle, GUILayout.Width(5));
                    EditorGUILayout.LabelField(_editor.animation_end_time.ToString("N2") + "(" + Language_Data.second_name[Language_Data.select_language] + ")", MaxStyle, GUILayout.Width(175));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.HelpBox(Language_Data.now_platform_time_help_data[Language_Data.select_language], MessageType.None);
                    #endregion
                }
                else
                {
                    EditorGUILayout.HelpBox(Language_Data.animation_option_help_data[Language_Data.select_language], MessageType.None);
                }
                #endregion

                #region[애니메이션 작동]
                if (_editor.GetComponent<SpriteRenderer>())
                {
                    SpriteRenderer sprite = _editor.GetComponent<SpriteRenderer>();
                    if (_editor.now_animation_time < _editor.animation_show_time - _editor.tranform_show_time || _editor.now_animation_time > _editor.animation_hide_time + _editor.tranform_hide_time)
                    {
                        Set_Color(_editor.gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0));
                    }
                    else if (_editor.now_animation_time >= _editor.animation_show_time - _editor.tranform_show_time && _editor.now_animation_time < _editor.animation_show_time)
                    {
                        float alpha = (_editor.now_animation_time - (_editor.animation_show_time - _editor.tranform_show_time)) / _editor.tranform_show_time;
                        Set_Color(_editor.gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha));
                    }
                    else if (_editor.now_animation_time >= _editor.animation_hide_time && _editor.now_animation_time <= _editor.animation_hide_time + _editor.tranform_hide_time)
                    {
                        float alpha = (_editor.now_animation_time - _editor.animation_hide_time) / _editor.tranform_hide_time;
                        Set_Color(_editor.gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1 - alpha));
                    }
                    else
                    {
                        Set_Color(_editor.gameObject, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1));
                    }
                }
                #endregion

                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

    }
    #endregion

    #region[OnSceneGUI]
    void OnSceneGUI()
    {
        //Tools.current = Tool.None;
        Event m_Event = Event.current;

        if (_editor.now_option == 1)
        {
            #region[시작시 점 위치]
            if (_editor.awake)
            {
                _editor.awake = false;
                _editor.waypoint[0] = _editor.transform.position;
                _editor.waypoint[1] = _editor.transform.position + new Vector3(1, 0, 0);
                _editor.waypoint[2] = _editor.transform.position + new Vector3(0.5f, 1, 0);
                _editor.start_pos = _editor.waypoint[0];
            }
            else
            {
                _editor.transform.position = _editor.start_pos;
            }
            #endregion

            #region[점에 핸들달기]
            for (int i = 0; i < _editor.waypoint.Count; i++)
            {
                _editor.waypoint[i] = Handles.FreeMoveHandle(_editor.waypoint[i], Quaternion.identity, 0.05f, new Vector3(10, 10, 10), Handles.RectangleHandleCap);
            }
            #endregion

            #region[폴리곤 테두리 표시]
            Handles.color = Color.cyan;
            Vector3[] MouseLineVector;
            if (_editor.route_rotation)
            {
                MouseLineVector = new Vector3[_editor.waypoint.Count + 1];
                for (int j = 0; j < _editor.waypoint.Count; j++)
                {
                    MouseLineVector[j] = _editor.waypoint[j];
                }
                MouseLineVector[_editor.waypoint.Count] = _editor.waypoint[0];
            }
            else
            {
                MouseLineVector = new Vector3[_editor.waypoint.Count];
                for (int j = 0; j < _editor.waypoint.Count; j++)
                {
                    MouseLineVector[j] = _editor.waypoint[j];
                }
            }
            Vector3 closestPoint = HandleUtility.ClosestPointToPolyLine(MouseLineVector);
            //Handles.DotHandleCap(0, closestPoint, Quaternion.identity, 0.05f, EventType.Repaint);
            #endregion

            #region[점 추가]
            if (m_Event.type == EventType.MouseDown && m_Event.button == 0)
            {
                //if(Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(m_Event.mousePosition.x, m_Event.mousePosition.y, -Camera.main.transform.position.z)),closestPoint) < 2f)
                // {
                Vector3 start = _editor.waypoint[0];
                Vector3 end = _editor.waypoint[1];
                float dis = HandleUtility.DistancePointLine(closestPoint, start, end);
                int num = 1;
                for (int i = 1; i < _editor.waypoint.Count; i++)
                {
                    float distance = HandleUtility.DistancePointLine(closestPoint, _editor.waypoint[i - 1], _editor.waypoint[i]);
                    if (distance < dis)
                    {
                        num = i;
                        dis = distance;
                    }

                }
                if (_editor.route_rotation)
                {
                    float distance = HandleUtility.DistancePointLine(closestPoint, _editor.waypoint[_editor.waypoint.Count - 1], _editor.waypoint[0]);
                    if (distance < dis)
                        num = _editor.waypoint.Count;
                }
                _editor.waypoint.Insert(num, closestPoint);
                // }
            }
            #endregion
        }
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

}
