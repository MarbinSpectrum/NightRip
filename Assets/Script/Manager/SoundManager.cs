using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region[잡다변수]
    public static bool existence = false;

    [Header("BGM")]
    public GameObject BGM;
    static GameObject BGM_Group;

    [Header("타이틀 배경음")]
    public AudioSource Title;
    static AudioSource Title_Sound;

    [Header("조작법 배경음")]
    public AudioSource HowToPlay;
    static AudioSource HowToPlay_Sound;

    [Header("게임 로드 배경음")]
    public AudioSource LoadZone;
    static AudioSource LoadZone_Sound;

    [Header("첫번째 지역 배경음")]
    public AudioSource Area1;
    static AudioSource Area1_Sound;

    [Header("첫번째 지역 보스 배경음")]
    public AudioSource Boss1;
    static AudioSource Boss1_Sound;

    [Header("두번째 지역 배경음")]
    public AudioSource Area2;
    static AudioSource Area2_Sound;

    [Header("세번째 지역 배경음")]
    public AudioSource Area3;
    static AudioSource Area3_Sound;

    [Header("엔딩 배경음")]
    public AudioSource Ending;
    static AudioSource Ending_Sound;

    [HideInInspector]
    public static bool IsBgmOff = true;

    [Header("SE")]

    [Space]
    [Space]
    [Space]

    public GameObject SE;
    static GameObject SE_Group;

    [Header("점프때 기합소리")]
    public AudioSource[] Yap = new AudioSource[4];
    static AudioSource[] Yap_Sound = new AudioSource[4];

    [Header("화살 날리는 소리")]
    public AudioSource Arrow;
    static AudioSource Arrow_Sound;

    [Header("화살 맞는 소리")]
    public AudioSource ArrowDamage;
    static AudioSource ArrowDamage_Sound;

    [Header("공격 소리")]
    public AudioSource Attack;
    static AudioSource Attack_Sound;

    [Header("점프 소리")]
    public AudioSource Jump;
    static AudioSource Jump_Sound;

    [Header("발자국 소리")]
    public AudioSource Step;
    static AudioSource Step_Sound;

    [Header("맞는 소리")]
    public AudioSource Hurt;
    static AudioSource Hurt_Sound;

    [Header("죽는 소리")]
    public AudioSource Death;
    static AudioSource Death_Sound;

    [Header("힐 소리")]
    public AudioSource Heal;
    static AudioSource Heal_Sound;

    [Header("폭발 소리")]
    public AudioSource Explosion;
    static AudioSource Explosion_Sound;


    [Header("마녀소리")]
    public AudioSource[] Witch = new AudioSource[3];
    static AudioSource[] Witch_Sound = new AudioSource[3];

    [Header("시스템 키는소리")]
    public AudioSource SystemOn;
    static AudioSource SystemOnSound;

    [Header("시스템 끄는소리")]
    public AudioSource SystemOff;
    static AudioSource SystemOffSound;

    [Header("종 소리")]
    public AudioSource Bell;
    static AudioSource Bell_Sound;

    [HideInInspector]
    public static bool IsSEOff = true;
    #endregion

    #region[Awake]
    void Awake()
    {
        existence = true;
        DontDestroyOnLoad(gameObject);

        BGM_Group = BGM;

        Title_Sound = Title;
        LoadZone_Sound = LoadZone;
        HowToPlay_Sound = HowToPlay;
        Area1_Sound = Area1;
        Area2_Sound = Area2;
        Area3_Sound = Area3;
        Boss1_Sound = Boss1;
        Ending_Sound = Ending;

        SE_Group = SE;

        Yap_Sound = Yap;
        Arrow_Sound = Arrow;
        ArrowDamage_Sound = ArrowDamage;
        Attack_Sound = Attack;
        Jump_Sound = Jump;
        Step_Sound = Step;
        Hurt_Sound = Hurt;
        Death_Sound = Death;
        Heal_Sound = Heal;
        Explosion_Sound = Explosion;

        Witch_Sound = Witch;

        SystemOnSound = SystemOn;
        SystemOffSound = SystemOff;
        Bell_Sound = Bell;
    }
    #endregion

    #region[타이틀 배경음]
    public static void TitleBGM(bool onoff)
    {
        if (!Title_Sound) return;
        if (onoff)
        {
            IsBgmOff = false;
            Title_Sound.Play();
        }
        else
            Title_Sound.Stop();
    }
    #endregion

    #region[로드존 배경음]
    public static void LoadZoneBGM(bool onoff)
    {
        if (!LoadZone_Sound) return;
        if (onoff)
        {
            IsBgmOff = false;
            LoadZone_Sound.Play();
        }
        else
            LoadZone_Sound.Stop();
    }
    #endregion

    #region[조작법 배경음]
    public static void HowToPlayBGM(bool onoff)
    {
        if (!HowToPlay_Sound) return;
        if (onoff)
        {
            IsBgmOff = false;
            HowToPlay_Sound.Play();
        }
        else
            HowToPlay_Sound.Stop();
    }
    #endregion

    #region[첫번째지역 배경음]
    public static void Area1BGM(bool onoff)
    {
        if (!Area1_Sound) return;
        if (onoff)
        {
            Area1_Sound.Play();
            IsBgmOff = false;
        }
        else
            Area1_Sound.Stop();
    }
    #endregion

    #region[첫번째지역 보스배경음]
    public static void Boss1BGM(bool onoff)
    {
        if (!Boss1_Sound) return;
        if (onoff)
        {
            Boss1_Sound.Play();
            IsBgmOff = false;
        }
        else
            Boss1_Sound.Stop();
    }
    #endregion

    #region[두번째지역 배경음]
    public static void Area2BGM(bool onoff)
    {
        if (!Area2_Sound) return;
        if (onoff)
        {
            Area2_Sound.Play();
            IsBgmOff = false;
        }
        else
            Area2_Sound.Stop();
    }
    #endregion

    #region[세번째지역 배경음]
    public static void Area3BGM(bool onoff)
    {
        if (!Area3_Sound) return;
        if (onoff)
        {
            Area3_Sound.Play();
            IsBgmOff = false;
        }
        else
            Area3_Sound.Stop();
    }
    #endregion

    #region[엔딩 배경음]
    public static void EndBGM(bool onoff)
    {
        if (!Ending_Sound) return;
        if (onoff)
        {
            Ending_Sound.Play();
            IsBgmOff = false;
        }
        else
            Ending_Sound.Stop();
    }
    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////

    #region[기합소리]
    public static void YapSE(bool onoff,int type = 0)
    {
        if (!Yap_Sound[type]) return;
        if (onoff)
        {
            Yap_Sound[type].Play();
            IsSEOff = false;
        }
        else
            Yap_Sound[type].Stop();

    }
    #endregion

    #region[화살쏘는소리]
    public static void ArrowSE(bool onoff)
    {
        if (!Arrow_Sound) return;
        if (onoff)
        {
            Arrow_Sound.Play();
            IsSEOff = false;
        }
        else
            Arrow_Sound.Stop();
    }
    #endregion

    #region[화살맞는소리]
    public static void ArrowDamaageSE(bool onoff)
    {
        if (!ArrowDamage_Sound) return;
        if (onoff)
        {
            ArrowDamage_Sound.Play();
            IsSEOff = false;
        }
        else
            ArrowDamage_Sound.Stop();
    }
    #endregion

    #region[공격소리]
    public static void AttackSE(bool onoff)
    {
        if (!Attack_Sound) return;
        if (onoff)
        {
            Attack_Sound.Play();
            IsSEOff = false;
        }
        else
            Attack_Sound.Stop();
    }
    #endregion

    #region[점프소리]
    public static void JumpSE(bool onoff)
    {
        if (!Jump_Sound) return;
        if (onoff)
        {
            Jump_Sound.Play();
            IsSEOff = false;
        }
        else
            Jump_Sound.Stop();
    }
    #endregion

    #region[발자국소리]
    public static void StepSE(bool onoff)
    {
        if (!Step_Sound) return;
        if (onoff)
        {
            Step_Sound.Play();
            IsSEOff = false;
        }
        else
            Step_Sound.Stop();
    }
    #endregion

    #region[맞는소리]
    public static void HurtSE(bool onoff)
    {
        if (!Hurt_Sound) return;
        if (onoff)
        {
            Hurt_Sound.Play();
            IsSEOff = false;
        }
        else
            Hurt_Sound.Stop();
    }
    #endregion

    #region[죽는소리]
    public static void DeathSE(bool onoff)
    {
        if (!Death_Sound) return;
        if (onoff)
        {
            Death_Sound.Play();
            IsSEOff = false;
        }
        else
            Death_Sound.Stop();
    }
    #endregion

    #region[회복소리]
    public static void HealSE(bool onoff)
    {
        if (!Heal_Sound) return;
        if (onoff)
        {
            Heal_Sound.Play();
            IsSEOff = false;
        }
        else
            Heal_Sound.Stop();
    }
    #endregion

    #region[폭발소리]
    public static void ExplosionSE(bool onoff)
    {
        if (!Explosion_Sound) return;
        if (onoff)
        {
            Explosion_Sound.Play();
            IsSEOff = false;
        }
        else
            Explosion_Sound.Stop();
    }
    #endregion


    #region[마녀소리]
    public static void WitchSE(bool onoff, int type = 0)
    {
        if (!Witch_Sound[type]) return;
        if (onoff)
        {
            Witch_Sound[type].Play();
            IsSEOff = false;
        }
        else
            Witch_Sound[type].Stop();
    }
    #endregion


    #region[시스템 키는 소리]
    public static void SystemOnSE(bool onoff)
    {
        if (!SystemOnSound) return;
        if (onoff)
        {
            SystemOnSound.Play();
            IsSEOff = false;
        }
        else
            SystemOnSound.Stop();
    }
    #endregion

    #region[시스템 끄는 소리]
    public static void SystemOffSE(bool onoff)
    {
        if (!SystemOffSound) return;
        if (onoff)
        {
            SystemOffSound.Play();
            IsSEOff = false;
        }
        else
            SystemOffSound.Stop();
    }
    #endregion

    #region[종 소리]
    public static void BellSE(bool onoff)
    {
        if (!Bell_Sound) return;
        if (onoff)
        {
            Bell_Sound.Play();
            IsSEOff = false;
        }
        else
            Bell_Sound.Stop();
    }
    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////

    #region[배경음 모두 끄기]
    public static void OffBGM()
    {
        if (!BGM_Group) return;
        for (int i = 0; i < BGM_Group.transform.childCount; i++)
        {
            BGM_Group.transform.GetChild(i).GetComponent<AudioSource>().Stop();
        }
        IsBgmOff = true;
    }
    #endregion

    #region[효과음 모두 끄기]
    public static void OffSE()
    {
        if(SE_Group)
        {
            for (int i = 0; i < SE_Group.transform.childCount; i++)
            {
                SE_Group.transform.GetChild(i).GetComponent<AudioSource>().Stop();
            }
        }
        IsSEOff = true;
    }
    #endregion
}
