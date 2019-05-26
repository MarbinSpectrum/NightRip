using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Camera_Maker))]
public class Camera_Maker_Custom : Editor
{
    Camera_Maker _editor;

    GameObject follow_obj;

    int[] language_value = new int[] { 0, 1 };

    #region[OnEnable]
    void OnEnable()
    {
        _editor = target as Camera_Maker;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        #region[언어선택]
        Language_Data.select_language = EditorGUILayout.IntPopup(Language_Data.language_title_name[Language_Data.select_language] + " : ", Language_Data.select_language, Language_Data.language_name, language_value);
        #endregion

        #region[메뉴 생성]
        GUIStyle Option_Title_Style = new GUIStyle();
        Option_Title_Style.fontStyle = FontStyle.Bold;
        Option_Title_Style.alignment = TextAnchor.MiddleLeft;
        #endregion

        #region[제목 생성]
        GUIStyle Title_Style = new GUIStyle();
        Title_Style.fontSize = 20;
        Title_Style.normal.textColor = new Color(34 / 255f, 177 / 255f, 76 / 255f);
        EditorGUILayout.LabelField(Language_Data.camera_title_name[Language_Data.select_language], Title_Style, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        EditorGUILayout.HelpBox(Language_Data.camera_title_help_data[Language_Data.select_language], MessageType.None);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField(Language_Data.option_setting_title[Language_Data.select_language], Option_Title_Style);

            #region[범위 점 들]
            GUIStyle point_style;
            point_style = EditorStyles.foldout;
            point_style.margin = new RectOffset(15, 0, 0, 0);
            _editor.showpoints = EditorGUILayout.Foldout(_editor.showpoints, Language_Data.point_data_title[Language_Data.select_language], point_style);
            if (_editor.showpoints)
            {
                for (int i = 0; i < _editor.camera_pos.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    _editor.camera_pos[i] = EditorGUILayout.Vector3Field("", _editor.camera_pos[i]);
                    if (_editor.camera_pos.Count > 4)
                    {
                        if (GUILayout.Button(Language_Data.delete_name[Language_Data.select_language]))
                        {
                            _editor.camera_pos.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button(Language_Data.create_name[Language_Data.select_language]))
                {
                    Vector3 Create_Point = (_editor.camera_pos[0] + _editor.camera_pos[1]) * 0.5f;
                    _editor.camera_pos.Insert(1, Create_Point);
                }
            }
            else
            {
                EditorGUILayout.HelpBox(Language_Data.camera_point_data_title_help_data[Language_Data.select_language], MessageType.None);
            }
            #endregion

            #region[카메라 축]
            EditorGUILayout.LabelField(Language_Data.camera_pivot_name[Language_Data.select_language]);
            _editor.camera_pivot = EditorGUILayout.Vector3Field("", _editor.camera_pivot);
            EditorGUILayout.HelpBox(Language_Data.camera_pivot_help_data[Language_Data.select_language], MessageType.None);
            #endregion

            #region[따라가는 오브젝트]
            _editor.goal_obj = EditorGUILayout.ObjectField(Language_Data.camera_goal_name[Language_Data.select_language], _editor.goal_obj, typeof(Transform), true) as Transform;
            EditorGUILayout.HelpBox(Language_Data.camera_goal_help_data[Language_Data.select_language], MessageType.None);
            #endregion

            #region[카메라 스피드]
            _editor.camera_speed = EditorGUILayout.FloatField(Language_Data.camera_speed_name[Language_Data.select_language], _editor.camera_speed);
            EditorGUILayout.HelpBox(Language_Data.camera_speed_help_data[Language_Data.select_language], MessageType.None);
            _editor.camera_speed = _editor.camera_speed < 0 ? 0 : _editor.camera_speed;
            #endregion

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
    #endregion

    #region[OnSceneGUI]
    void OnSceneGUI()
    {
        // Tools.current = Tool.None;
        Event m_Event = Event.current;

        #region[시작시 점 위치]
        if (_editor.awake)
        {
            _editor.awake = false;
            _editor.camera_pos[0] = _editor.transform.position + new Vector3(+1, +1, 0);
            _editor.camera_pos[1] = _editor.transform.position + new Vector3(1, -1, 0);
            _editor.camera_pos[2] = _editor.transform.position + new Vector3(-1, -1, 0);
            _editor.camera_pos[3] = _editor.transform.position + new Vector3(-1, +1, 0);
        }
        #endregion

        #region[점에 핸들달기]
        for (int i = 0; i < _editor.camera_pos.Count; i++)
        {
            _editor.camera_pos[i] = Handles.FreeMoveHandle(_editor.camera_pos[i], Quaternion.identity, 0.05f, new Vector3(10, 10, 10), Handles.RectangleHandleCap);
        }
        #endregion

        #region[폴리곤 테두리 표시]
        Handles.color = Color.cyan;
        Vector3[] MouseLineVector;
        MouseLineVector = new Vector3[_editor.camera_pos.Count + 1];
        for (int j = 0; j < _editor.camera_pos.Count; j++)
        {
            MouseLineVector[j] = _editor.camera_pos[j];
        }
        MouseLineVector[_editor.camera_pos.Count] = _editor.camera_pos[0];
        Vector3 closestPoint = HandleUtility.ClosestPointToPolyLine(MouseLineVector);
        #endregion

        #region[점 추가]
        if (m_Event.type == EventType.MouseDown && m_Event.button == 0)
        {
            Vector3 start = _editor.camera_pos[0];
            Vector3 end = _editor.camera_pos[1];
            float dis = HandleUtility.DistancePointLine(closestPoint, start, end);
            int num = 1;
            float distance = 0;
            for (int i = 1; i < _editor.camera_pos.Count; i++)
            {
                distance = HandleUtility.DistancePointLine(closestPoint, _editor.camera_pos[i - 1], _editor.camera_pos[i]);
                if (distance < dis)
                {
                    num = i;
                    dis = distance;
                }

            }
            distance = HandleUtility.DistancePointLine(closestPoint, _editor.camera_pos[_editor.camera_pos.Count - 1], _editor.camera_pos[0]);
            if (distance < dis)
                num = _editor.camera_pos.Count;
            _editor.camera_pos.Insert(num, closestPoint);
        }
        #endregion

    }
    #endregion
}