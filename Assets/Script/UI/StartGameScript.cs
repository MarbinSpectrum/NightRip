using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartGameScript : MonoBehaviour
{
    [TextArea]
    public string[] prologue_txt;
    public Text prologue_text;
    string buf = "";
    int txt = 0;
    int length = 0;
    float time = 0;
    float prologue_time = 0;
    bool startgame = false;
    public Animator Fade;
    public void Update()
    {
        prologue_time += Time.deltaTime;
        time += Time.deltaTime;
        prologue_text.text = buf;
        try
        {
            if (length >= prologue_txt[txt].Length && time > 2)
            {
                txt++;
                buf = "";
                length = 0;
                time = 0;
            }
            else if (time > 0.1f)
            {
                buf = prologue_txt[txt].Substring(0, length);
                length++;
                time = 0;
                if(!startgame)
                    SoundManager.SystemOnSE(true);
            }
        }
        catch { }
        if(txt > 2 && !startgame)
        {
            prologue_time = 0;
            Fade.SetFloat("Speed", -1);
            startgame = true;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button2) && !startgame)
        {
            SoundManager.SystemOnSE(true);
            prologue_time = 0;
            Fade.SetFloat("Speed", -1);
            startgame = true;
        }
        if (startgame && prologue_time > 3.5f)
        {
            SceneManager.LoadScene("Stage1-0");
        }

    }
}
