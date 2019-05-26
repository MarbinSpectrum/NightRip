using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    #region[잡다변수]
    public static bool existence = false;

    [Header("Type")]
    public Type this_type;
    public enum Type { Manager, Effect };

    float time = 0;

    [Header("Hit Air")]
    public GameObject Hit_Air;
    public static GameObject Hit_Air_Effect;

    [Header("Hit Large")]
    public GameObject Hit_Large;
    public static GameObject Hit_Large_Effect;

    [Header("Hit Medium")]
    public GameObject Hit_Medium;
    public static GameObject Hit_Medium_Effect;

    [Header("Hit Medium Dark")]
    public GameObject Hit_Medium_Dark;
    public static GameObject Hit_Medium_Dark_Effect;

    [Header("Hit Normal")]
    public GameObject Hit_Normal;
    public static GameObject Hit_Normal_Effect;

    [Header("Hit Small")]
    public GameObject Hit_Small;
    public static GameObject Hit_Small_Effect;

    [Header("Boom")]
    public GameObject[] Boom = new GameObject[3];
    public static GameObject[] Boom_Effect;

    [Header("Energy")]
    public GameObject Energy;
    public static GameObject Energy_Effect;
    #endregion

    #region[Awake]
    void Awake()
    {
        if(this_type == Type.Manager)
        {
            existence = true;
            DontDestroyOnLoad(gameObject);

            Hit_Air_Effect = Hit_Air;
            Hit_Large_Effect = Hit_Large;
            Hit_Medium_Effect = Hit_Medium;
            Hit_Medium_Dark_Effect = Hit_Medium_Dark;
            Hit_Normal_Effect = Hit_Normal;
            Hit_Small_Effect = Hit_Small;

            Boom_Effect = Boom;

            Energy_Effect = Energy;
        }
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (this_type == Type.Effect)
        {
            time += Time.deltaTime;
            if (time > 5f) Destroy(gameObject);
        }
    }
    #endregion

    #region[이펙트 생성]
    public static void Play_Hit_Air(int x , int y,bool filpX = false)
    {
        Play_Hit_Air(new Vector3(x, y, 0), filpX);
    }
    public static void Play_Hit_Air(Vector3 vector3, bool filpX = false)
    {
        if (Hit_Air_Effect)
        {
            GameObject emp = Instantiate(Hit_Air_Effect, vector3, Quaternion.identity);
            emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
        }
    }

    public static void Play_Hit_Large(int x, int y, bool filpX = false)
    {
        Play_Hit_Large(new Vector3(x, y, 0), filpX);
    }
    public static void Play_Hit_Large(Vector3 vector3, bool filpX = false)
    {
        if (Hit_Large_Effect)
        {
            GameObject emp = Instantiate(Hit_Large_Effect, vector3, Quaternion.identity);
            emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
        }
    }

    public static void Play_Hit_Medium(int x, int y, bool filpX = false)
    {
        Play_Hit_Medium(new Vector3(x, y, 0), filpX);
    }
    public static void Play_Hit_Medium(Vector3 vector3, bool filpX = false)
    {
        if (Hit_Medium_Effect)
        {
            GameObject emp = Instantiate(Hit_Medium_Effect, vector3, Quaternion.identity);
            emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
        }
    }

    public static void Play_Hit_Medium_Dark(int x, int y, bool filpX = false)
    {
        Play_Hit_Medium_Dark(new Vector3(x, y, 0), filpX);
    }
    public static void Play_Hit_Medium_Dark(Vector3 vector3, bool filpX = false)
    {
        if (Hit_Medium_Dark_Effect)
        {
            GameObject emp = Instantiate(Hit_Medium_Dark_Effect, vector3, Quaternion.identity);
            emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
        }
    }

    public static void Play_Hit_Normal(int x, int y, bool filpX = false)
    {
        Play_Hit_Normal(new Vector3(x, y, 0), filpX);
    }
    public static void Play_Hit_Normal(Vector3 vector3, bool filpX = false)
    {
        if(Hit_Normal_Effect)
        {
            GameObject emp = Instantiate(Hit_Normal_Effect, vector3, Quaternion.identity);
            emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
        }
    }

    public static void Play_Hit_Small(int x, int y, bool filpX = false)
    {
        Play_Hit_Small(new Vector3(x, y, 0), filpX);
    }
    public static void Play_Hit_Small(Vector3 vector3, bool filpX = false)
    {
        if (Hit_Small_Effect)
        {
            GameObject emp = Instantiate(Hit_Small_Effect, vector3, Quaternion.identity);
            emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
        }
    }

    public static void Play_Boom(int x, int y, bool filpX = false)
    {
        Play_Boom(new Vector3(x, y, 0), 0, filpX);
    }
    public static void Play_Boom(int x, int y , int type , bool filpX = false)
    {
        Play_Boom(new Vector3(x, y, 0),type, filpX);
    }
    public static void Play_Boom(Vector3 vector3, int type, bool filpX = false)
    {
        try
        {
            if (Boom_Effect[type])
            {
                GameObject emp = Instantiate(Boom_Effect[type], vector3, Quaternion.identity);
                emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
            }
        }
        catch { }
    }

    public static void Play_Energy(int x, int y, bool filpX = false)
    {
        Play_Energy(new Vector3(x, y, 0), filpX);
    }
    public static void Play_Energy(Vector3 vector3, bool filpX = false)
    {
        if (Energy_Effect)
        {
            GameObject emp = Instantiate(Energy_Effect, vector3, Quaternion.identity);
            emp.transform.localScale = filpX ? new Vector3(-Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z) : new Vector3(+Mathf.Abs(emp.transform.localScale.x), emp.transform.localScale.y, emp.transform.localScale.z);
        }
    }
    #endregion

}
