using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtObjectScript : MonoBehaviour
{
    public int damage;
    [Header("몬스터 죽는여부")]
    public bool monster_dead = true;
    [Header("무적 무시여부")]
    public bool Immediately_damage = false;
}
