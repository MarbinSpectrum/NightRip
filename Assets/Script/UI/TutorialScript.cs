using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialScript : MonoBehaviour
{
    public enum GameLevel { 쉬움 , 보통 , 어려움 , 악몽};

    float TutorialTime = 0;

    bool axisInUse1 = false;
    bool axisInUse2 = false;
    bool startgame = false;
    int select = 1;

    public Animator Fade;

    public GameObject XBox;

    public GameObject SelectLevel;

    public GameObject[] Level;

    void Start()
    {
        SoundManager.HowToPlayBGM(true);
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal Trigger") == 0)
        {
            axisInUse1 = false;
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            axisInUse2 = false;
        }

        #region[조작방법]
        TutorialTime += Time.timeScale;
        if (XBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button2) && TutorialTime > 15f)
            {
                TutorialTime = 0;
                XBox.SetActive(false);
                SelectLevel.SetActive(true);
                ShowLevel(select);
                Fade.Rebind();
            }
        }
        else if (SelectLevel.activeSelf && !startgame)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button2) && TutorialTime > 15f)
            {
                TutorialTime = 0;
                Fade.SetFloat("Speed", -1);
                startgame = true;
                SoundManager.SystemOnSE(true);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetAxisRaw("Horizontal Trigger") == -1 && !axisInUse1)  || (Input.GetAxisRaw("Horizontal") <= -0.75f && !axisInUse2))
            {
                axisInUse1 = true;
                axisInUse2 = true;
                select = select - 1 < 0 ? Level.Length - 1 : select - 1;
                ShowLevel(select);
                SoundManager.SystemOnSE(true);
                Fade.Rebind();

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetAxisRaw("Horizontal Trigger") == 1 && !axisInUse1) ||  (Input.GetAxisRaw("Horizontal") >= 0.75f && !axisInUse2))
            {
                axisInUse1 = true;
                axisInUse2 = true;
                select = select + 1 >= Level.Length ? 0 : select + 1;
                ShowLevel(select);
                SoundManager.SystemOnSE(true);
                Fade.Rebind();
            }
        }
        else if (startgame && TutorialTime > 145f && TutorialTime <= 150f)
        {
            SoundManager.OffBGM();
        }
        else if (startgame && TutorialTime > 150f)
        {
            SoundManager.OffBGM();
            PlayerPrefs.SetInt("GameLevel" + GameManager.player_data, select);
            SceneManager.LoadScene("StartGame");
        }
        #endregion
    }

    void ShowLevel(int choice)
    {
        for(int i = 0; i < Level.Length; i++)
        {
            Level[i].SetActive(i == choice);
        }
        GameManager.GameLevel = (GameLevel)choice;
    }
}
