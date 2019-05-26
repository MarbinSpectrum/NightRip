using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Player_Maker))]
public class Player_Maker_Custom : Editor
{
    Player_Maker _editor;

    #region[언어 변수]
    int[] language_value = new int[] { 0, 1 };
    #endregion

    #region[OnEnable]
    void OnEnable()
    {
        _editor = target as Player_Maker;
        if (!_editor.GetComponent<BoxCollider2D>())
        {
            _editor.gameObject.AddComponent<BoxCollider2D>();
        }
        if (!_editor.GetComponent<Rigidbody2D>())
        {
            _editor.gameObject.AddComponent<Rigidbody2D>();
        }
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        #region[GUI 스타일 지정]
        GUIStyle stat_style = new GUIStyle();
        stat_style.fontStyle = FontStyle.Bold;
        #endregion

        #region[언어선택]
        Language_Data.select_language = EditorGUILayout.IntPopup(Language_Data.language_title_name[Language_Data.select_language] + " : ", Language_Data.select_language, Language_Data.language_name, language_value);
        #endregion

        #region[제목 생성]
        GUIStyle Title_Style = new GUIStyle();
        Title_Style.fontSize = 20;
        Title_Style.normal.textColor = new Color(34 / 255f, 177 / 255f, 76 / 255f);
        EditorGUILayout.LabelField(Language_Data.player_title_name[Language_Data.select_language], Title_Style, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        EditorGUILayout.HelpBox(Language_Data.player_title_help_data[Language_Data.select_language], MessageType.None);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion

        #region[플레이어 설정]
        EditorGUILayout.LabelField(Language_Data.player_stat_name[Language_Data.select_language], stat_style);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            #region[플레이어 이동속도]
            _editor.movePower = EditorGUILayout.FloatField(Language_Data.player_speed_name[Language_Data.select_language], _editor.movePower);
            _editor.movePower = _editor.movePower < 0 ? 0 : _editor.movePower;
            EditorGUILayout.HelpBox(Language_Data.player_speed_help_data[Language_Data.select_language], MessageType.None);
            #endregion

            #region[플레이어 점프력]
            _editor.jumpPower = EditorGUILayout.FloatField(Language_Data.player_jump_name[Language_Data.select_language], _editor.jumpPower);
            _editor.jumpPower = _editor.jumpPower < 0 ? 0 : _editor.jumpPower;
            EditorGUILayout.HelpBox(Language_Data.player_jump_help_data[Language_Data.select_language], MessageType.None);
            #endregion

            #region[이단점프 가능여부]
            _editor.double_jump = EditorGUILayout.Toggle(Language_Data.double_jump_name[Language_Data.select_language], _editor.double_jump);
            EditorGUILayout.HelpBox(Language_Data.double_jump_help_data[Language_Data.select_language], MessageType.None);
            #endregion

            #region[플레이어 체력]
            _editor.player_max_hp = EditorGUILayout.IntField(Language_Data.player_hp_name[Language_Data.select_language], _editor.player_max_hp);
            _editor.player_max_hp = _editor.player_max_hp <= 0 ? 1 : _editor.player_max_hp;
            EditorGUILayout.HelpBox(Language_Data.player_hp_help_data[Language_Data.select_language], MessageType.None);
            #endregion
        }
        EditorGUILayout.EndVertical();

        #endregion
    }
    #endregion
}