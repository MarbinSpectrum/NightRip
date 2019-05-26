using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    #region[잡다변수]

    [Header("활성화 오브젝트")]
    public GameObject[] TriggerList;

    [Header("활성화검사 오브젝트")]
    public GameObject Awake_Trigger_Obj;

    public enum Type{enter , onoff , awake};

    [Header("발동종류")]
    public Type this_type;

    [Header("보스전여부")]
    public bool bossbgm;
    public string bossname;

    [Header("진동여부")]
    public bool vibration;

    [Header("음소거여부")]
    public bool stopbgm;

    [Header("딜레이")]
    public float delay;

    #endregion

    #region[Awake]
    void Awake()
    {
        if (this_type == Type.enter)
            transform.name = "Enter Trigger";
    }
    #endregion

    #region[Update]
    private void Update()
    {
        if (Awake_Trigger_Obj && Awake_Trigger_Obj.activeSelf && this_type == Type.awake)
        {
            StartCoroutine(Set_Trigger());
        }
    }
    #endregion

    #region[작동코드]
    public IEnumerator Set_Trigger()
    {
        yield return new WaitForSeconds(delay);
        if (bossbgm)
        {
            SoundManager.Boss1BGM(true);
            if(GameManager.Boss_UI)
            {
                GameManager.Boss_UI.SetActive(true);
                GameManager.Boss_now_hp = GameManager.Boss_max_hp;
                GameManager.BossHp_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Boss_hp_bar_width, GameManager.BossHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                GameManager.BossHpAni_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Boss_hp_bar_width, GameManager.BossHpAni_UI.GetComponent<RectTransform>().sizeDelta.y);
                GameManager.Boss_name.text = bossname + "";
                Camera_Maker[] cam = StageManager.camera_static.GetComponents<Camera_Maker>();
                try
                {
                    cam[0].enabled = false;
                    cam[1].enabled = true;
                }
                catch { Debug.Log("0번이나 , 1번카메라 없음"); }
            }
        }
        if(stopbgm)
        {
            SoundManager.OffBGM();
        }
        if (vibration)
        {
            GameManager.VibrationCamera(0.5f);
        }
        for (int i = 0; i < TriggerList.Length; i++)
        {
            if (TriggerList[i])
            {
                TriggerList[i].SetActive(true);
                if(TriggerList[i].transform.tag.Equals("Item") && TriggerList[i].GetComponent<Rigidbody2D>())
                    TriggerList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-20,20), 75));
            }
        }
        gameObject.SetActive(false);
    }
    #endregion


}
