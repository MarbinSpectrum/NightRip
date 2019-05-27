using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    Rigidbody2D rigidbody2D;

    bool fly = false;

    float time = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        FlyCheck();
        Ani();
    }

    void FlyCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.05f) + new Vector2(boxCollider2D.offset.x, boxCollider2D.offset.y), new Vector2(boxCollider2D.size.x * 0.8f, boxCollider2D.size.y), 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Background"));
        fly = hit;
    }

    void Ani()
    {
        animator.SetBool("Fly", fly);
    }
}
