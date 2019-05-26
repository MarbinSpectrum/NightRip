using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region[잡다변수]
    [Header("GameManager")]
    public GameObject GameManager_prefab;
    [Header("SoundManager")]
    public GameObject SoundManager_prefab;
    [Header("Effectanager")]
    public GameObject Effectanager_prefab;
    [TextArea]
    [Header("스테이지이름")]
    public string stage_name;
    [Header("시작 씬 여부")]
    public bool start_scene;
    [Header("배경음시작여부")]
    public bool start_bgm;
    public int map_n = 0;
    public static int map_n_static = -1;

    [Header("플레이어")]
    public GameObject player;

    [Header("카메라")]
    public GameObject camera;

    public static GameObject player_static;
    public static GameObject camera_static;
    #endregion

    #region[Awake]
    void Awake()
    {
        if(!GameManager.existence)
            Instantiate(GameManager_prefab);
        if (!SoundManager.existence)
            Instantiate(SoundManager_prefab);
        if (!EffectManager.existence)
            Instantiate(Effectanager_prefab);
    }
    #endregion

    #region[Start]
    void Start()
    {
        GameManager.time = 0;
        GameManager.fadeout = true;
        player_static = player;
        camera_static = camera;
        if (TitleScript.play_time != 0)
        {
            RaycastHit2D[] findsave = Physics2D.BoxCastAll((Vector2)TitleScript.load_location, new Vector2(2, 2), 0, Vector2.zero);
            for (int i = 0; i < findsave.Length; i++)
            {
                if (findsave[i].transform.name.Equals("SavePoint"))
                {
                    Destroy(findsave[i].transform.gameObject);
                    break;
                }
            }
            for (int i = 0; i < TitleScript.itempos.Count; i++)
            {
                RaycastHit2D[] finditem = Physics2D.BoxCastAll((Vector2)TitleScript.itempos[i], new Vector2(2, 2), 0, Vector2.zero);
                for (int j = 0; j < finditem.Length; j++)
                {
                    if (finditem[j].transform.name.Equals("Item") || finditem[j].transform.name.Equals("Treasure_Box"))
                    {
                        Destroy(finditem[j].transform.gameObject);
                    }
                }
            }
            player.transform.position = TitleScript.load_location;
            camera.transform.position = new Vector3(TitleScript.load_location.x, TitleScript.load_location.y, camera.transform.position.z);
            TitleScript.load_location = Vector3.zero;
            GameManager.Has_Item.Item0_num = GameManager.Has_Item.Item0_maxnum;
            GameManager.PlayTime = TitleScript.play_time;
            start_scene = true;
            start_bgm = true;
            TitleScript.play_time = 0;
        }

        if (GameManager.MapNameLine_Image && start_scene)
            GameManager.MapNameLine_Image.SetActive(true);
        if (GameManager.Player_UI)
        {
            GameManager.Player_UI.SetActive(true);
            for(int i = 0; i < GameManager.Item_Pos.Count; i++)
            {
                GameManager.Item_Pos.Clear();
            }
        }
        if (GameManager.MapName_Text && start_scene)
        {
            GameManager.MapName_Text.text = stage_name;
        }

        if (TitleScript.itemdata == 0)
        {
            GameManager.Has_Item.Item0_Has = false;
            GameManager.Has_Item.Item0_maxnum = 0;
            GameManager.Has_Item.Item0_num = 0;
            GameManager.Has_Item.Item1_Has = false;
            GameManager.Has_Item.Item1_num = 0;
            GameManager.Equip_Item = GameManager.Item.없음;
        }
        else if (TitleScript.itemdata > 0 && TitleScript.itemdata < 32)
        {
            GameManager.Has_Item.Item0_Has = true;
            GameManager.Has_Item.Item0_maxnum = TitleScript.itemdata;
            GameManager.Has_Item.Item0_num = GameManager.Has_Item.Item0_maxnum;
            GameManager.Has_Item.Item1_Has = false;
            GameManager.Has_Item.Item1_num = 0;
        }
        else if (TitleScript.itemdata == 32)
        {
            GameManager.Has_Item.Item0_Has = false;
            GameManager.Has_Item.Item0_maxnum = 0;
            GameManager.Has_Item.Item0_num = 0;
            GameManager.Has_Item.Item1_Has = true;
            GameManager.Has_Item.Item1_num = 1;
            GameManager.Equip_Item = GameManager.Item.없음;
        }
        else if (TitleScript.itemdata > 32)
        {
            GameManager.Has_Item.Item0_Has = true;
            GameManager.Has_Item.Item0_maxnum = TitleScript.itemdata - 32;
            GameManager.Has_Item.Item0_num = GameManager.Has_Item.Item0_maxnum;
            GameManager.Has_Item.Item1_Has = true;
            GameManager.Has_Item.Item1_num = 1;
        }
        TitleScript.itemdata = -1;
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (GameManager.MapNameLine_Image && GameManager.MapNameLine_Image.activeSelf && GameManager.MapName_Text.color.a == 0)
        {
            GameManager.MapNameLine_Image.SetActive(false);
            if (map_n_static != map_n)
            {
                map_n_static = map_n;
                if(start_bgm)
                {
                    SoundManager.OffBGM();
                    if (map_n == 0)
                        SoundManager.Area1BGM(true);
                    else if (map_n == 1)
                        SoundManager.Area2BGM(true);
                    else if (map_n == 2)
                        SoundManager.Area3BGM(true);
                }
            }
        }
    }
    #endregion
}
