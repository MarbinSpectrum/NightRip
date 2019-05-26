using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject Monster;

    public float cycle = 0;
    public float time = 0;
    BoxCollider2D boxCollider2d;

    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time > cycle)
        {
            Instantiate(Monster, transform.position + new Vector3(Random.Range(-boxCollider2d.size.x * 0.5f, +boxCollider2d.size.x * 0.5f), Random.Range(-boxCollider2d.size.y * 0.5f, +boxCollider2d.size.y * 0.5f), 0), Quaternion.identity);
            time = 0;
        }
    }
}
