using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    #region[잡다변수]
    public GameObject Title_UI;

    public GameObject Title_Text;

    public GameObject Start_UI;
    public GameObject End_UI;
    public GameObject LoadGame_UI;
    public GameObject Select_UI;
    public GameObject Quit_Select_UI;

    public GameObject Select_Data_UI;
    public GameObject[] Save_Data = new GameObject[3];

    public Sprite[] level_img;

    float time = 0;

    bool visible = false;
    bool axisInUse1 = false;
    bool axisInUse2 = false;
    bool axisInUse3 = false;
    bool axisInUse4 = false;

    bool start_ui = false;

    public static bool start_load = false;

    int select_load_data = 0;
    public static float play_time = 0;
    public static int itemdata = 0;
    public static List<Vector3> itempos;
    struct GameData
    {
        public string scene_name;
        public Vector3 player_pos;
        public float playtime;
        public int itemdata;
        public int level;
        public List<Vector3> itempos;
        public int hasdata;
    }
    GameData[] gamedata = new GameData[3];

    bool gameload = true;
    bool DL_Select = false;
    bool SelectDelete = false;
    bool hasloaddata = false;

    public Animator player_ani;

    public static Vector3 load_location = Vector3.zero;

    #endregion

    #region[Awake]
    private void Awake()
    {
        itemdata = 0;
        GameManager.GameLevel = TutorialScript.GameLevel.보통;
        GameManager.PlayTime = 0;
        GameManager.Equip_Item = GameManager.Item.없음;
        SoundManager.TitleBGM(true);
        Select_UI.transform.position = Start_UI.transform.position;
        for(int i = 0; i < 3; i++)
        {
            gamedata[i].scene_name = PlayerPrefs.GetString("SceneName" + i, "Stage1-0");
            gamedata[i].player_pos = new Vector3(PlayerPrefs.GetFloat("PlayerPosX" + i, 0), PlayerPrefs.GetFloat("PlayerPosY" + i, 0),PlayerPrefs.GetFloat("PlayerPosZ" + i, 0));
            gamedata[i].playtime = PlayerPrefs.GetFloat("PlayTime" + i, 0);
            gamedata[i].itemdata = PlayerPrefs.GetInt("ItemData" + i, 0);
            gamedata[i].level = PlayerPrefs.GetInt("GameLevel" + i, 1);
            gamedata[i].hasdata = PlayerPrefs.GetInt("HasData" + i, 0);
            int number = 0;
            gamedata[i].itempos = new List<Vector3>();
            itempos = new List<Vector3>();
            while (true)
            {
                Vector3 vector = new Vector3(PlayerPrefs.GetFloat(number + "ItemPosX" + i, -100), PlayerPrefs.GetFloat(number + "ItemPosY" + i, -100), PlayerPrefs.GetFloat(number + "ItemPosZ" + i, -100));
                if(vector == new Vector3(-100,-100,-100))
                {
                    break;
                }
                gamedata[i].itempos.Add(vector);
                number++;
            }
            if (gamedata[i].hasdata != 0)
            {
                Save_Data[i].transform.GetChild(0).gameObject.SetActive(true);
                Save_Data[i].transform.GetChild(1).gameObject.SetActive(false);
                Save_Data[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = PlayTime(gamedata[i].playtime) + " ";
                Save_Data[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = AreaName(gamedata[i].scene_name) + " ";
                Save_Data[i].transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().sprite = level_img[gamedata[i].level];
            }
            else
            {
                Save_Data[i].transform.GetChild(0).gameObject.SetActive(false);
                Save_Data[i].transform.GetChild(1).gameObject.SetActive(true);
            }
            Save_Data[i].transform.GetChild(0).Find("Select").gameObject.SetActive(false);
            Save_Data[i].transform.GetChild(0).Find("SelectLoad").gameObject.SetActive(false);
            Save_Data[i].transform.GetChild(0).Find("SelectDelete").gameObject.SetActive(false);
        }
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (!Title_Text.activeSelf)
        {
            if(Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
            {
                Title_Text.transform.parent.GetComponent<Animator>().speed = 10;
            }
            return;
        }
        if(!start_ui)
        {
            start_ui = true;
        }
        if (Input.GetAxisRaw("Vertical Trigger") == 0)
        {
            axisInUse1 = false;
        }
        if (Input.GetAxisRaw("Horizontal Trigger") == 0)
        {
            axisInUse2 = false;
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            axisInUse3 = false;
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            axisInUse4 = false;
        }

        if (!visible)
        {
            #region[게임 UI 끄기 애니메이션]
            float alpha = time * 2f;
            if (alpha < 1 && LoadGame_UI.activeSelf)
            {
                ImageAlpha(LoadGame_UI.transform.GetChild(0).gameObject, 1 - alpha);
                time += Time.deltaTime;
            }
            #endregion

            else
            {
                LoadGame_UI.SetActive(false);
                if (gameload)
                {
                    
                    #region[타이틀 UI 조작]
                    /*
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxisRaw("Vertical Trigger") == -1 && !axisInUse1) || (Input.GetAxisRaw("Vertical Trigger") == +1 && !axisInUse1) || (Input.GetAxisRaw("Vertical") >= 0.75f && !axisInUse3) || (Input.GetAxisRaw("Vertical") <= -0.75f && !axisInUse3))
                    {
                        axisInUse1 = true;
                        axisInUse3 = true;
                        if (Select_UI.transform.position == Start_UI.transform.position)
                            Select_UI.transform.position = End_UI.transform.position;
                        else
                            Select_UI.transform.position = Start_UI.transform.position;
                        Select_UI.GetComponent<Animator>().SetTrigger("reset");
                        SoundManager.SystemOnSE(true);
                    }
                    */
                    if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                    {
                        visible = true;
                        time = 0;
                        ImageAlpha(LoadGame_UI.transform.GetChild(0).gameObject, 0);
                        /*if (Select_UI.transform.position == Start_UI.transform.position) // 게임시작선택
                        {
                            LoadGame_UI.SetActive(true);
                            select_load_data = 0;
                        }
                        
                        if (Select_UI.transform.position == End_UI.transform.position) // 게임종료선택
                        {
                            Application.Quit();
                        }
                        */
                        SoundManager.SystemOnSE(true);
                        LoadGame_UI.SetActive(true);
                        select_load_data = 0;
                    }
                    #endregion
                }
            }
        }
        else
        {
            #region[게임 UI 키기 애니메이션]
            float alpha = time * 2f;
            if (alpha < 1)
            {
                ImageAlpha(LoadGame_UI.transform.GetChild(0).gameObject, alpha);
                time += Time.deltaTime;
            }
            #endregion

            else
            {
                #region[로드 UI 조작]
                if (!start_load && !DL_Select)
                {
                    start_load = true;
                    Select_Data_UI.SetActive(true);
                    start_load = true;
                    Select_Data_UI.transform.position = Save_Data[select_load_data].transform.position;
                    for (int i = 0; i < 3; i++)
                    {
                        if (gamedata[i].hasdata != 0)
                        {
                            Save_Data[i].transform.GetChild(0).gameObject.SetActive(true);
                            Save_Data[i].transform.GetChild(1).gameObject.SetActive(false);
                            Save_Data[i].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                            Save_Data[i].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                            Save_Data[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = PlayTime(gamedata[i].playtime) + " ";
                            Save_Data[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = AreaName(gamedata[i].scene_name) + " ";
                            Save_Data[i].transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().sprite = level_img[gamedata[i].level];
                        }
                        else
                        {
                            Save_Data[i].transform.GetChild(0).gameObject.SetActive(false);
                            Save_Data[i].transform.GetChild(1).gameObject.SetActive(true);
                        }
                        Save_Data[i].transform.GetChild(0).Find("Select").gameObject.SetActive(false);
                        Save_Data[i].transform.GetChild(0).Find("SelectLoad").gameObject.SetActive(false);
                        Save_Data[i].transform.GetChild(0).Find("SelectDelete").gameObject.SetActive(false);
                    }
                }
                else if (gameload && !DL_Select)
                {

                    if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxisRaw("Vertical Trigger") == -1 && !axisInUse1) || (Input.GetAxisRaw("Vertical") <= -0.75f && !axisInUse3))
                    {
                        axisInUse1 = true;
                        axisInUse3 = true;
                        if (Select_Data_UI.transform.position == Save_Data[0].transform.position)
                        {
                            Select_Data_UI.SetActive(true);
                            Quit_Select_UI.SetActive(false);
                            Select_Data_UI.transform.position = Save_Data[1].transform.position;
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[1].transform.position)
                        {
                            Select_Data_UI.SetActive(true);
                            Quit_Select_UI.SetActive(false);
                            Select_Data_UI.transform.position = Save_Data[2].transform.position;
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[2].transform.position)
                        {
                            Select_Data_UI.SetActive(false);
                            Quit_Select_UI.SetActive(true);
                            Select_Data_UI.transform.position = Save_Data[3].transform.position;
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[3].transform.position)
                        {
                            Select_Data_UI.SetActive(true);
                            Quit_Select_UI.SetActive(false);
                            Select_Data_UI.transform.position = Save_Data[0].transform.position;
                        }
                        Select_Data_UI.GetComponent<Animator>().SetTrigger("reset");
                        SoundManager.SystemOnSE(true);
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxisRaw("Vertical Trigger") == +1 && !axisInUse1) || (Input.GetAxisRaw("Vertical") >= 0.75f && !axisInUse3))
                    {
                        axisInUse1 = true;
                        axisInUse3 = true;
                        if (Select_Data_UI.transform.position == Save_Data[0].transform.position)
                        {
                            Quit_Select_UI.SetActive(true);
                            Select_Data_UI.SetActive(false);
                            Select_Data_UI.transform.position = Save_Data[3].transform.position;
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[1].transform.position)
                        {
                            Quit_Select_UI.SetActive(false);
                            Select_Data_UI.SetActive(true);
                            Select_Data_UI.transform.position = Save_Data[0].transform.position;
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[2].transform.position)
                        {
                            Quit_Select_UI.SetActive(false);
                            Select_Data_UI.SetActive(true);
                            Select_Data_UI.transform.position = Save_Data[1].transform.position;
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[3].transform.position)
                        {
                            Quit_Select_UI.SetActive(false);
                            Select_Data_UI.SetActive(true);
                            Select_Data_UI.transform.position = Save_Data[2].transform.position;
                        }
                        Select_Data_UI.GetComponent<Animator>().SetTrigger("reset");
                        SoundManager.SystemOnSE(true);
                    }
                    else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button7))
                    {
                        if(Quit_Select_UI.activeSelf)
                        {
                            Application.Quit();
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[0].transform.position)
                        {
                            select_load_data = 0;
                            if (gamedata[0].hasdata == 0) // 슬롯0에 저장데이터 여부검사
                            {
                                gameload = false;
                                hasloaddata = false;
                                time = 0;
                                visible = false;
                                Title_UI.SetActive(false);
                            }
                            else
                            {
                                hasloaddata = true;
                                DL_Select = true;
                            }
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[1].transform.position)
                        {
                            select_load_data = 1;
                            if (gamedata[1].hasdata == 0)  // 슬롯1에 저장데이터 여부검사
                            {
                                gameload = false;
                                hasloaddata = false;
                                time = 0;
                                visible = false;
                                Title_UI.SetActive(false);
                            }
                            else
                            {
                                hasloaddata = true;
                                DL_Select = true;
                            }
                        }
                        else if (Select_Data_UI.transform.position == Save_Data[2].transform.position)
                        {
                            select_load_data = 2;
                            if (gamedata[2].hasdata == 0)  // 슬롯2에 저장데이터 여부검사
                            {
                                gameload = false;
                                hasloaddata = false;
                                time = 0;
                                visible = false;
                                Title_UI.SetActive(false);
                            }
                            else
                            {
                                hasloaddata = true;
                                DL_Select = true;
                            }
                        }
                        SoundManager.SystemOnSE(true);
                    }
                    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                    {
                       // time = 0;
                       // visible = false;
                       // SoundManager.SystemOffSE(true);
                    }
                }
                else if(DL_Select)
                {
                    if (Select_Data_UI.activeSelf)
                    {
                        Select_Data_UI.SetActive(false);
                        Save_Data[select_load_data].transform.GetChild(0).Find("PlayTime").gameObject.SetActive(false);
                        Save_Data[select_load_data].transform.GetChild(0).Find("Area").gameObject.SetActive(false);
                        Save_Data[select_load_data].transform.GetChild(0).Find("Select").gameObject.SetActive(true);
                        Save_Data[select_load_data].transform.GetChild(0).Find("SelectLoad").gameObject.SetActive(true);
                        Save_Data[select_load_data].transform.GetChild(0).Find("SelectDelete").gameObject.SetActive(false);
                        SelectDelete = false;
                    }
                    else if(!Select_Data_UI.activeSelf)
                    {
                        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetAxisRaw("Horizontal Trigger") == -1 && !axisInUse2) || (Input.GetAxisRaw("Horizontal Trigger") == 1 && !axisInUse2) || (Input.GetAxisRaw("Horizontal") <= -0.75f && !axisInUse4) || (Input.GetAxisRaw("Horizontal") >= 0.75f && !axisInUse4))
                        {
                            axisInUse2 = true;
                            axisInUse4 = true;
                            SelectDelete = !SelectDelete;
                            Save_Data[select_load_data].transform.GetChild(0).Find("SelectLoad").gameObject.SetActive(!SelectDelete);
                            Save_Data[select_load_data].transform.GetChild(0).Find("SelectDelete").gameObject.SetActive(SelectDelete);
                            SoundManager.SystemOnSE(true);
                        }
                        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button2))
                        {
                            if(!SelectDelete)
                            {
                                gameload = false;
                                DL_Select = false;
                                time = 0;
                                visible = false;
                                //Title_UI.SetActive(false);
                                SoundManager.SystemOnSE(true);
                            }
                            else
                            {
                                PlayerPrefs.SetString("SceneName" + select_load_data, "Stage1-0");
                                PlayerPrefs.SetFloat("PlayerPosX" + select_load_data, 13.39f);
                                PlayerPrefs.SetFloat("PlayerPosY" + select_load_data, 0);
                                PlayerPrefs.SetFloat("PlayerPosZ" + select_load_data,0);
                                PlayerPrefs.SetFloat("PlayTime" + select_load_data, 0);
                                PlayerPrefs.SetFloat("ItemData" + select_load_data, 0);
                                PlayerPrefs.SetInt("HasData" + select_load_data, 0);
                                PlayerPrefs.SetInt("GameLevel" + select_load_data, 1);
                                gamedata[select_load_data].scene_name = PlayerPrefs.GetString("SceneName" + select_load_data, "Stage1-0");
                                gamedata[select_load_data].player_pos = new Vector3(PlayerPrefs.GetFloat("PlayerPosX" + select_load_data, 0), PlayerPrefs.GetFloat("PlayerPosY" + select_load_data, 0), PlayerPrefs.GetFloat("PlayerPosZ" + select_load_data, 0));
                                gamedata[select_load_data].playtime = PlayerPrefs.GetInt("PlayTime" + select_load_data, 0);
                                gamedata[select_load_data].itemdata = PlayerPrefs.GetInt("ItemData" + select_load_data, 0);
                                int number = 0;
                                while (true)
                                {
                                    Vector3 vector = new Vector3(PlayerPrefs.GetFloat(number + "ItemPosX" + select_load_data, -100), PlayerPrefs.GetFloat(number + "ItemPosY" + select_load_data, -100), PlayerPrefs.GetFloat(number + "ItemPosZ" + select_load_data, - 100));
                                    if (vector == new Vector3(-100, -100, -100))
                                    {
                                        break;
                                    }
                                    gamedata[select_load_data].itempos.Add(vector);
                                    number++;
                                }
                                gamedata[select_load_data].hasdata = 0;
                                DL_Select = false;
                                start_load = false;
                                SoundManager.SystemOffSE(true);
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                        {
                            DL_Select = false;
                            start_load = false;
                            SoundManager.SystemOffSE(true);
                            Select_Data_UI.SetActive(true);
                        }
                    }
                }
                #endregion
            }
        }



        if(!gameload)
        {
            time += Time.deltaTime;
            if (true)
            {
                if(!hasloaddata)  // 저장 데이터없음 새로시작
                {
                    /*
                    if (!player_ani.GetBool("rise"))
                    {
                        //player_ani.SetBool("rise", true);
                        GameManager.time = 0;
                        GameManager.fadeout = false;
                        load_location = Vector3.zero;

                        PlayerPrefs.SetString("SceneName" + select_load_data, "Stage1-0");
                        PlayerPrefs.SetFloat("PlayerPosX" + select_load_data, 13.39f);
                        PlayerPrefs.SetFloat("PlayerPosY" + select_load_data, 9.829f);
                        PlayerPrefs.SetFloat("PlayerPosZ" + select_load_data, 0);
                        PlayerPrefs.SetFloat("PlayTime" + select_load_data, 0);
                        PlayerPrefs.SetFloat("ItemData" + select_load_data, 0);
                        int number = 0;
                        while(PlayerPrefs.GetFloat(number + "ItemPosX" + select_load_data, -100) != -100)
                        {
                            PlayerPrefs.SetFloat(number + "ItemPosX" + select_load_data, -100);
                            PlayerPrefs.SetFloat(number + "ItemPosY" + select_load_data, -100);
                            PlayerPrefs.SetFloat(number + "ItemPosZ" + select_load_data, -100);
                            number++;
                        }
                        PlayerPrefs.SetInt("HasData" + select_load_data, 1);
                    }
                    */
                    GameManager.time = 0;
                    GameManager.fadeout = false;
                    load_location = Vector3.zero;

                    PlayerPrefs.SetString("SceneName" + select_load_data, "Stage1-0");
                    PlayerPrefs.SetFloat("PlayerPosX" + select_load_data, 13.39f);
                    PlayerPrefs.SetFloat("PlayerPosY" + select_load_data, 9.829f);
                    PlayerPrefs.SetFloat("PlayerPosZ" + select_load_data, 0);
                    PlayerPrefs.SetFloat("PlayTime" + select_load_data, 0);
                    PlayerPrefs.SetFloat("ItemData" + select_load_data, 0);
                    PlayerPrefs.SetInt("HasData" + select_load_data, 1);
                    PlayerPrefs.SetInt("GameLevel" + select_load_data, 1);
                    int number = 0;
                    while (PlayerPrefs.GetFloat(number + "ItemPosX" + select_load_data, -100) != -100)
                    {
                        PlayerPrefs.SetFloat(number + "ItemPosX" + select_load_data, -100);
                        PlayerPrefs.SetFloat(number + "ItemPosY" + select_load_data, -100);
                        PlayerPrefs.SetFloat(number + "ItemPosZ" + select_load_data, -100);
                        number++;
                    }
                    GameManager.player_data = select_load_data;
                    SceneManager.LoadScene("Tutorial");
                    SoundManager.TitleBGM(false);
                }
                else   // 저장있음 이어서시작
                {

                    load_location = gamedata[select_load_data].player_pos;
                    play_time = gamedata[select_load_data].playtime;
                    itemdata = gamedata[select_load_data].itemdata;
                    GameManager.GameLevel = (TutorialScript.GameLevel)gamedata[select_load_data].level;
                    for (int i = 0; i < gamedata[select_load_data].itempos.Count; i++)
                    {
                        itempos.Add(gamedata[select_load_data].itempos[i]);
                    }
                    /*
                    if (!player_ani.GetBool("rise"))
                    {
                        player_ani.SetBool("rise", true);
                        GameManager.time = 0;
                        GameManager.fadeout = false;
                    }
                    */
                    GameManager.player_data = select_load_data;
                    SceneManager.LoadScene(gamedata[select_load_data].scene_name);
                    SoundManager.TitleBGM(false);
                }
            }
            else if(time >= 0.2f)
            {
                GameManager.player_data = select_load_data;
                SceneManager.LoadScene(gamedata[select_load_data].scene_name);
                SoundManager.TitleBGM(false);
            }
        }
    }
    #endregion

    #region[지역 이름]
    string AreaName(string name)
    {
        if(name.Equals("Stage1-0"))
            return "<size=75>墓地</size>(무덤)";
        else if (name.Equals("Stage1-1"))
            return "<size=75>洞窟</size>(동굴)";
        else if (name.Equals("Stage1-2"))
            return "<size=75>洞窟</size>(동굴)";
        else if (name.Equals("Stage1-3"))
            return "<size=75>洞窟</size>(동굴)";
        else if (name.Equals("Stage2-1"))
            return "<size=75>洞窟</size>(공중유적)";
        else if (name.Equals("Stage2-2"))
            return "<size=75>洞窟</size>(공중유적)";
        else if (name.Equals("Stage2-3"))
            return "<size=75>洞窟</size>(유적내부)";
        else
            return "공동 묘지";
    }
    #endregion

    #region[플레이시간]
    string PlayTime(float n)
    {
        int min = (int)n / 60;
        int hour = min/60;
        int sec = (int)n %60;
        min =min%60;
        return string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
    }
    #endregion

    #region[자식객체의 모든 알파값 조정]
    void ImageAlpha(GameObject obj, float a)
    {
        if (obj.activeSelf)
        {
            if (obj.GetComponent<Image>())
                obj.GetComponent<Image>().color = new Color(obj.GetComponent<Image>().color.r, obj.GetComponent<Image>().color.g, obj.GetComponent<Image>().color.b, a);
            if (obj.GetComponent<Text>())
                obj.GetComponent<Text>().color = new Color(obj.GetComponent<Text>().color.r, obj.GetComponent<Text>().color.g, obj.GetComponent<Text>().color.b, a);
            if (obj.GetComponent<Outline>())
                obj.GetComponent<Outline>().effectColor = new Color(obj.GetComponent<Outline>().effectColor.r, obj.GetComponent<Outline>().effectColor.g, obj.GetComponent<Outline>().effectColor.b, a);
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            ImageAlpha(obj.transform.GetChild(i).gameObject, a);
        }
    }
    #endregion
}
