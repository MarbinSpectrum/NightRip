using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class GameManager : MonoBehaviour
{
    #region[잡다변수]
    public static bool existence = false;

    [Header("플레이중인 난이도")]
    public static TutorialScript.GameLevel GameLevel;
    public TutorialScript.GameLevel GameLv;
    [Header("플레이중인 데이터")]
    public int play_data;
    public static int player_data = 0;

    [Header("페이드인 아웃")]
    public Image Fade;
    static public Image Static_Fade;

    [Header("지역표시")]
    public GameObject MapNameLine;
    public Text MapName;
    public static Text MapName_Text;
    public static GameObject MapNameLine_Image;

    [Header("저장표시 UI")]
    public GameObject Save_Text;
    public static GameObject SaveObj;
    [HideInInspector] public static float save_time = 0;
    [HideInInspector] public static float save_fadetime = 0;

    [Header("게임오버 UI")]
    public GameObject GameOver_UI;
    public static GameObject GameOver_UI_static;
    public GameObject GameOverSelectBar;

    [Header("정지 UI")]
    public GameObject Stop_UI;
    public static GameObject Stop_UI_static;
    public GameObject StopSelectBar;

    [Header("표지판 UI")]
    public GameObject Sign;
    public static GameObject Sign_UI;
    public static GameObject Read_UI;
    public static GameObject View_UI;

    static Vector3 Sign_UI_Pos = new Vector3(0, 0, 0);

    /// ////////////////////////////////////

    [HideInInspector] public static float time = 0;
    [HideInInspector] public static bool fadeout = true;

    [HideInInspector] public static float PlayTime = 0;

    [Header("플레이어 UI")]
    public GameObject Play_UI;
    public static GameObject Player_UI;

    /// ////////////////////////////////////

    [Header("플레이어 체력 UI")]
    public GameObject PlayHp_UI;
    public static GameObject PlayerHp_UI;
    public GameObject PlayHpAni_UI;
    public static GameObject PlayerHpAni_UI;
    public static int player_max_hp = 100;
    public static int player_now_hp = player_max_hp;
    public static float hp_bar_width;
    static float hpbaranitime = 0;
    /// ////////////////////////////////////

    [Header("보스 UI")]
    public Text Bos_name;
    public static Text Boss_name;
    public GameObject Bos_UI;
    public static GameObject Boss_UI;
    public GameObject BosHp_UI;
    public static GameObject BossHp_UI;
    public GameObject BosHpAni_UI;
    public static GameObject BossHpAni_UI;
    public static int Boss_max_hp = 100;
    public static int Boss_now_hp = Boss_max_hp;
    public static float Boss_hp_bar_width;
    static float bosshpbaranitime = 0;
    /// ////////////////////////////////////

    [Header("검 데미지")]
    public int Sword_Damage_View;
    public static int Sword_Damage;

    [Header("활 데미지")]
    public int Arrow_Damage_View;
    public static int Arrow_Damage;

    [Header("검 경직 여부")]
    public bool Stun_Sword_View;
    public static bool Stun_Sword;

    [Header("화살 경직 여부")]
    public bool Stun_Arrow_View;
    public static bool Stun_Arrow;

    [Header("화살 관통 여부")]
    public bool Penetrate_Arrow_View;
    public static bool Penetrate_Arrow;

    [Header("동작 속도 강화 여부")]
    public bool SpeedUp_View;
    public static bool SpeedUp;

    [System.Serializable]
    public struct Has_Item_List
    {
        [Header("회복포션")]
        public bool Item0_Has;
        public int Item0_num;
        public int Item0_maxnum;
        [Header("응급처치키트")]
        public bool Item1_Has;
        public int Item1_num;
    }
    [Header("소비 아이템(보유)")]
    public static Has_Item_List Has_Item;

    [System.Serializable]
    public struct Has_Weapon_List
    {
        [Header("날카로운화살")]
        public bool Weapon0_Has;
        [Header("무거운화살")]
        public bool Weapon1_Has;
        [Header("가벼운화살")]
        public bool Weapon2_Has;
        [Header("날개옷")]
        public bool Weapon3_Has;
    }
    public static List<Vector3> Item_Pos = new List<Vector3>();

    [Header("장착 아이템(보유)")]
    public static Has_Weapon_List Has_Weapon;

    [Header("소비 아이템(장착)")]
    public static Item Equip_Item;
    public enum Item { 없음, 회복포션, 응급처치키트 };
    public Sprite[] Item_img;
    public GameObject Item_ui;
    [Header("장착 아이템(장착)")]
    public static Weapon Equip_Weapon0;
    public static Weapon Equip_Weapon1;
    public enum Weapon { 없음, 날카로운화살, 무거운화살, 가벼운화살, 날개옷 };
    public Sprite[] Weapon_img;
    /// ////////////////////////////////////
    /// 
    public static bool vibration = false;
    public static Vector3 camera_default_vector = Vector3.zero;
    public static float vibrationpower = 0.01f;
    static float vibrationtime = 0;
    static float vibrationcounttime = 0;

    bool axisInUse1 = false;
    bool axisInUse2 = false;
    bool axisInUse3 = false;
    #endregion

    #region[Awake]
    void Awake()
    {
        existence = true;
        DontDestroyOnLoad(gameObject);

        Static_Fade = Fade;

        MapName_Text = MapName;
        MapNameLine_Image = MapNameLine;

        Player_UI = Play_UI;
        PlayerHp_UI = PlayHp_UI;
        PlayerHpAni_UI = PlayHpAni_UI;
        hp_bar_width = PlayerHp_UI.GetComponent<RectTransform>().sizeDelta.x;

        Boss_UI = Bos_UI;
        Boss_name = Bos_name;
        BossHp_UI = BosHp_UI;
        BossHpAni_UI = BosHpAni_UI;
        Boss_hp_bar_width = BossHp_UI.GetComponent<RectTransform>().sizeDelta.x;

        GameOver_UI_static = GameOver_UI;
        GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(1).transform.position;

        Stop_UI_static = Stop_UI;
        StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(2).transform.position;

        Sign_UI = Sign;
        Read_UI = Sign.transform.GetChild(0).gameObject;
        View_UI = Sign.transform.GetChild(1).gameObject;

    }
    #endregion

    #region[Start]
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Game"))
            SceneManager.LoadScene("Title");
        time = 0;
    }
    #endregion

    #region[Update]
    void Update()
    {

        #region[기타반복]
        if (StageManager.player_static)
        {
            PlayTime += Time.deltaTime;
        }
        play_data = player_data;

        if (Input.GetAxisRaw("Vertical Trigger") == 0)
        {
            axisInUse1 = false;
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            axisInUse2 = false;
        }
        #endregion

        #region[페이드아웃]
        time += Time.deltaTime;
        if (fadeout)
        {
            Fade.color = new Color(0, 0, 0, 1 - time * 0.75f < 0 ? 0 : 1 - time * 0.75f);
            MapName.color = new Color(1, 1, 1, 2.5f - time < 0 ? 0 : 2.5f - time);
            MapNameLine.GetComponent<Image>().color = new Color(1, 1, 1, 2.5f - time < 0 ? 0 : 2.5f - time);
        }
        else
        {
            Fade.color = new Color(0, 0, 0, time * 0.75f > 1 ? 1 : time * 1.5f);
            MapName.color = new Color(1, 1, 1, 0);
            MapNameLine.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        #endregion

        #region[표지판]
        if (Camera.main)
            Sign_UI.transform.position = Camera.main.WorldToScreenPoint(Sign_UI_Pos);
        #endregion

        #region[저장표시]
        if (save_time > 0)
        {
            save_time -= Time.deltaTime;
            Save_Text.SetActive(true);
        }
        else
        {
            Save_Text.SetActive(false);
        }
        #endregion

        #region[저장소 소멸]
        if (SaveObj)
        {
            save_fadetime += Time.deltaTime;
            if (save_fadetime > 0.2f)
            {
                if (SaveObj.GetComponent<SpriteRenderer>().color.a > 0)
                {
                    for (int i = 0; i < SaveObj.transform.childCount; i++)
                    {
                        if (SaveObj.transform.GetChild(i).GetComponent<SpriteRenderer>())
                            SaveObj.transform.GetChild(i).GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.02f);
                    }
                    SaveObj.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.02f);
                }
                else
                {
                    Destroy(SaveObj);
                    SaveObj = null;
                    save_fadetime = 0;
                }
            }
        }
        #endregion

        #region[체력바]
        hpbaranitime += Time.deltaTime;
        if (PlayerHp_UI && PlayerHpAni_UI && PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.x > PlayerHp_UI.GetComponent<RectTransform>().sizeDelta.x && hpbaranitime > 0.5f)
        {
            hpbaranitime = 1;
            PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta -= new Vector2(5, 0);
        }

        bosshpbaranitime += Time.deltaTime;
        if (BossHp_UI && BossHpAni_UI && BossHpAni_UI.GetComponent<RectTransform>().sizeDelta.x > BossHp_UI.GetComponent<RectTransform>().sizeDelta.x && bosshpbaranitime > 0.5f)
        {
            bosshpbaranitime = 1;
            BosHpAni_UI.GetComponent<RectTransform>().sizeDelta -= new Vector2(5, 0);
        }
        #endregion

        #region[진동]
        vibrationtime += Time.deltaTime;
        vibrationcounttime += Time.deltaTime;
        if (Camera.main && vibration && vibrationtime > 0.05f)
        {
            vibrationtime = 0;
            Camera.main.transform.position = camera_default_vector + new Vector3(Random.Range(-vibrationpower, +vibrationpower), Random.Range(-vibrationpower, +vibrationpower), 0);
            GamePad.SetVibration(0, vibrationpower*3, vibrationpower*3);
            if (vibrationcounttime > 0.25f)
            {
                vibration = false;
                Camera.main.transform.position = camera_default_vector;
                GamePad.SetVibration(0, 0f, 0f);
            }
        }
        #endregion

        #region[옵션 동기화]
        if (Stun_Sword != Stun_Sword_View)
            Stun_Sword = Stun_Sword_View;
        if (Stun_Arrow != Stun_Arrow_View)
            Stun_Arrow = Stun_Arrow_View;
        if (Penetrate_Arrow != Penetrate_Arrow_View)
            Penetrate_Arrow = Penetrate_Arrow_View;
        if (SpeedUp != SpeedUp_View)
            SpeedUp = SpeedUp_View;
        if (Sword_Damage != Sword_Damage_View)
            Sword_Damage = Sword_Damage_View;
        if (Arrow_Damage != Arrow_Damage_View)
            Arrow_Damage = Arrow_Damage_View;
        #endregion

        #region[게임 오버 UI]
        if (GameOver_UI_static.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxisRaw("Vertical Trigger") == -1 && !axisInUse1) || (Input.GetAxisRaw("Vertical") <= -0.75f && !axisInUse2))
            {
                axisInUse1 = true;
                axisInUse2 = true;
                if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(1).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(2).transform.position;
                }
                else if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(2).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(3).transform.position;
                }
                else if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(3).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(1).transform.position;
                }
                SoundManager.SystemOnSE(true);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxisRaw("Vertical Trigger") == +1 && !axisInUse1) || (Input.GetAxisRaw("Vertical") >= +0.75f && !axisInUse2))
            {
                axisInUse1 = true;
                axisInUse2 = true;
                if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(1).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(3).transform.position;
                }
                else if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(2).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(1).transform.position;
                }
                else if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(3).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(2).transform.position;
                }
                SoundManager.SystemOnSE(true);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(1).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(1).transform.position;
                    TitleScript.load_location = new Vector3(PlayerPrefs.GetFloat("PlayerPosX" + player_data, 0), PlayerPrefs.GetFloat("PlayerPosY" + player_data, 0), PlayerPrefs.GetFloat("PlayerPosZ" + player_data, 0));
                    TitleScript.play_time = PlayerPrefs.GetFloat("PlayTime" + player_data, 0);
                    SoundManager.OffSE();
                    GameOver_UI_static.SetActive(false);
                    player_now_hp = player_max_hp;
                    GameManager.MapNameLine_Image.SetActive(false);
                    TitleScript.itemdata = PlayerPrefs.GetInt("ItemData" + player_data, 0);
                    PlayerHp_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                    PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                    SceneManager.LoadScene(PlayerPrefs.GetString("SceneName" + player_data, "Stage1-0"));
                    StageManager.map_n_static = -1;
                }
                else if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(2).transform.position)
                {
                    GameOverSelectBar.transform.position = GameOver_UI_static.transform.GetChild(1).transform.position;
                    SceneManager.LoadScene("Title");
                    SoundManager.OffSE();
                    player_now_hp = player_max_hp;
                    GameManager.MapNameLine_Image.SetActive(false);
                    PlayerHp_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                    PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                    GameOver_UI_static.SetActive(false);
                    StageManager.map_n_static = -1;
                }
                else if (GameOverSelectBar.transform.position == GameOver_UI_static.transform.GetChild(3).transform.position)
                {
                    Application.Quit();
                }
            }
        }
        #endregion

        #region[정지 UI]
        if (StageManager.player_static && StageManager.player_static.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6))
            {
                Stop_UI_static.SetActive(!Stop_UI_static.activeSelf);
                StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(2).transform.position;
            }
        }
        else if (StageManager.player_static && !StageManager.player_static.activeSelf)
        {
            if (Stop_UI_static.activeSelf)
            {
                Stop_UI_static.SetActive(false);
                StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(2).transform.position;
            }
        }

        if (Stop_UI_static.activeSelf)
        {
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxisRaw("Vertical Trigger") == -1 && !axisInUse1) || (Input.GetAxisRaw("Vertical") <= -0.75f && !axisInUse2))
            {
                axisInUse1 = true;
                axisInUse2 = true;
                if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(2).transform.position)
                {
                    StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(3).transform.position;
                }
                else if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(3).transform.position)
                {
                    StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(4).transform.position;
                }
                else if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(4).transform.position)
                {
                    StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(2).transform.position;
                }
                SoundManager.SystemOnSE(true);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxisRaw("Vertical Trigger") == +1 && !axisInUse1) || (Input.GetAxisRaw("Vertical") >= +0.75f && !axisInUse2))
            {
                axisInUse1 = true;
                axisInUse2 = true;
                if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(2).transform.position)
                {
                    StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(4).transform.position;
                }
                else if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(3).transform.position)
                {
                    StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(2).transform.position;
                }
                else if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(4).transform.position)
                {
                    StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(3).transform.position;
                }
                SoundManager.SystemOnSE(true);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(2).transform.position)
                {
                    Stop_UI_static.SetActive(false);
                    StopSelectBar.transform.position = Stop_UI_static.transform.GetChild(2).transform.position;
                }
                else if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(3).transform.position)
                {
                    if (Read_UI) Read_UI.SetActive(false);
                    if (View_UI) View_UI.SetActive(false);
                    if (Player_UI) Player_UI.SetActive(false);
                    SoundManager.OffSE();
                    SoundManager.OffBGM();
                    SceneManager.LoadScene("Title");
                    player_now_hp = player_max_hp;
                    TitleScript.itemdata = 0;
                    GameManager.MapNameLine_Image.SetActive(false);
                    PlayerHp_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                    PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                    Stop_UI_static.SetActive(false);
                    StageManager.map_n_static = -1;
                }
                else if (StopSelectBar.transform.position == Stop_UI_static.transform.GetChild(4).transform.position)
                {
                    Application.Quit();
                }
            }
        }
        else
        {
            Time.timeScale = 1;
        }
        #endregion

        #region[아이템]
        ChangeItem();
        #endregion

        if(GameManager.GameLevel != GameLv)
        {
             GameLv = GameManager.GameLevel;
        }
    }
    #endregion

    #region[표지판 UI 좌표이동]
    public static void ViewSign(Vector3 pos)
    {
        Sign_UI_Pos = pos;
    }
    #endregion

    #region[게임저장]
    public static void SaveGame(GameObject obj)
    {
        SaveObj = obj;
        save_time = 3f;
        Has_Item.Item0_num = Has_Item.Item0_maxnum;
        Has_Item.Item1_num = 1;
        string SaveArea = SceneManager.GetActiveScene().name;
        Vector3 SaveLocation = obj.transform.position;
        PlayerPrefs.SetString("SceneName" + player_data, SaveArea);
        PlayerPrefs.SetFloat("PlayerPosX" + player_data, SaveLocation.x);
        PlayerPrefs.SetFloat("PlayerPosY" + player_data, SaveLocation.y);
        PlayerPrefs.SetFloat("PlayerPosZ" + player_data, SaveLocation.z);
        PlayerPrefs.SetFloat("PlayTime" + player_data, PlayTime);
        PlayerPrefs.SetInt("ItemData" + player_data, Has_Item.Item0_maxnum + (Has_Item.Item1_Has ? 32 : 0));
        for (int i = 0; i < Item_Pos.Count; i++)
        {
            PlayerPrefs.SetFloat(i + "ItemPosX" + player_data, Item_Pos[i].x);
            PlayerPrefs.SetFloat(i + "ItemPosY" + player_data, Item_Pos[i].y);
            PlayerPrefs.SetFloat(i + "ItemPosZ" + player_data, Item_Pos[i].z);
        }
        PlayerPrefs.SetInt("HasData" + player_data, 1);
        PlayerPrefs.SetInt("GameLevel" + player_data, (int)GameManager.GameLevel);
        player_now_hp = player_max_hp;
        PlayerHp_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
        PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width, PlayerHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
    }
    #endregion

    #region[체력바 상태변경]
    public static void UpdateHpBar()
    {
        if (!PlayerHp_UI) return;
        PlayerHp_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(hp_bar_width * (player_now_hp / (float)(player_max_hp)), PlayerHp_UI.GetComponent<RectTransform>().sizeDelta.y);
        hpbaranitime = 0;

        if (!BossHp_UI) return;
        BossHp_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(Boss_hp_bar_width * (Boss_now_hp / (float)(Boss_max_hp)), BossHp_UI.GetComponent<RectTransform>().sizeDelta.y);
        bosshpbaranitime = 0;
    }
    #endregion

    #region[진동효과]
    public static void VibrationCamera(float power)
    {
        if(Camera.main)
        {
            vibrationpower = power;
            camera_default_vector = Camera.main.transform.position;
            vibrationtime = 0;
            vibrationcounttime = 0;
            vibration = true;
        }
    }
    #endregion

    #region[아이템]
    void Update_Item()
    {
        Item_ui.transform.Find("Equip Item").GetComponent<Image>().sprite = Item_img[(int)Equip_Item];
        Item_ui.transform.Find("n").gameObject.SetActive(Equip_Item == Item.없음 ? false : true);
        Item_ui.transform.Find("n").GetComponent<Text>().text = "" + (Equip_Item == Item.회복포션 ? Has_Item.Item0_num : Has_Item.Item1_num);
    }
    void ChangeItem()
    {
        if (Input.GetAxisRaw("Right Trigger") == 0)
        {
            axisInUse3 = false;
        }
        if (Time.timeScale != 0 && StageManager.player_static)
        {
            if (Equip_Item == Item.없음)
            {
                if (Has_Item.Item0_Has)
                {
                    Equip_Item = Item.회복포션;
                }
                else if (Has_Item.Item1_Has)
                {
                    Equip_Item = Item.응급처치키트;
                }
            }
            else if (Equip_Item == Item.회복포션)
            {
                if (Input.GetKeyDown(KeyCode.Q) || (Input.GetAxisRaw("Right Trigger") == -1 && !axisInUse3))
                {
                    axisInUse3 = true;
                    if (Has_Item.Item1_Has)
                    {
                        Equip_Item = Item.응급처치키트;
                    }
                }
            }
            else if (Equip_Item == Item.응급처치키트)
            {
                if (Input.GetKeyDown(KeyCode.Q) || (Input.GetAxisRaw("Right Trigger") == -1 && !axisInUse3))
                {
                    axisInUse3 = true;
                    if (Has_Item.Item0_Has)
                    {
                        Equip_Item = Item.회복포션;
                    }
                }
            }
            Update_Item();
        }
    }
    public static bool Can_Use_Item()
    {
        if (Equip_Item == Item.회복포션)
        {
            if (Has_Item.Item0_Has)
            {
                return Has_Item.Item0_num > 0 ? true : false;
            }
        }
        else if(Equip_Item == Item.응급처치키트)
        {
            if (Has_Item.Item1_Has)
            {
                return Has_Item.Item1_num > 0 ? true : false;
            }
        }
        return false;
    }
    public static void Use_Item()
    {
        if (Can_Use_Item())
        {
            if (Has_Item.Item0_Has && Equip_Item == Item.회복포션)
            {
                Has_Item.Item0_num--;
                player_now_hp += 20;
                player_now_hp = player_now_hp > player_max_hp ? player_max_hp : player_now_hp;
                UpdateHpBar();
            }
            else if (Has_Item.Item1_Has && Equip_Item == Item.응급처치키트)
            {
                Has_Item.Item1_num--;
                player_now_hp = player_max_hp;
                UpdateHpBar();
            }
        }
    }
    #endregion

}
