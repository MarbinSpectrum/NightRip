using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour
{
    #region[잡다변수]
    [TextArea]
    public string txt = "";
    public TextAnchor align;
    public Color text_color;
    public int font_size;
    [HideInInspector] public string text = "";
    [HideInInspector] public static SignScript select_sign;

    public enum Type { Sign , Item }
    public Type this_type;

    public GameManager.Item this_item;

    public float dictime = 0.1f;

    float time = 0;
    #endregion

    #region[Awake]
    private void Awake()
    {
        if(this_type == Type.Sign)
            transform.name = "Sign";
        else if (this_type == Type.Item)
            transform.name = "Item";
    }
    #endregion

    #region[Update]
    void Update()
    {
        time += Time.deltaTime;
        if (select_sign && select_sign.Equals(this) && GameManager.View_UI.activeSelf && !text.Equals(txt) && time > dictime)
        {
            time = 0;
            text = txt.Substring(0, text.Length + 1);

            if (CheckText(text[text.Length - 1]))
                SoundManager.SystemOnSE(true);

            Set_Font_Style();
        }
    }
    #endregion

    #region[폰트 설정]
    void Set_Font_Style()
    {
        GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().text = text;
        GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().alignment = align;
        GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().color = text_color;
        GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().fontSize = font_size;
        GameManager.View_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().preferredWidth*1.25f, GameManager.View_UI.transform.GetChild(0).GetComponent<Text>().preferredHeight * 1.5f);
    }
    #endregion

    #region[특수문자 검사]
    bool CheckText(char c)
    {
        char[] check_list = new char[] { '-',',','!',' ','\n' };

        for(int i = 0; i < check_list.Length; i++)
            if (c == check_list[i]) return false;

        return true;
    }
    #endregion
}
