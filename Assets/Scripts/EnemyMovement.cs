using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;

    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipSprite();
    }

    void Move()
    {
        _rigidbody2D.velocity = new Vector2(moveSpeed, 0f);
    }

    void FlipSprite()
    {
        var transformCached = transform;
        var localScale = transformCached.localScale;
        transformCached.localScale = new Vector2(-localScale.x, localScale.y);
    }
}
