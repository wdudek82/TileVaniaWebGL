using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float runSpeed = 7;

    [SerializeField]
    private float jumpSpeed = 5f;

    [SerializeField]
    private float climbSpeed = 3;

    [SerializeField]
    private Vector2 deathKick = new Vector2(10f, 10f);

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform gun;

    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _feetCollider;
    private float _initialGravityScale;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
    private static readonly int Dying = Animator.StringToHash("Dying");

    private bool _isAlive = true;

    public bool IsAlive => _isAlive;

    private bool PlayerHasHorizontalSpeed => Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
    private bool PlayerHasVerticalSpeed => Mathf.Abs(_rigidbody2D.velocity.y) > Mathf.Epsilon;


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _initialGravityScale = _rigidbody2D.gravityScale;
    }

    void Update()
    {
        if (!_isAlive) return;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!_isAlive) return;
        _moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (
            !_isAlive ||
            !_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncing", "Climbing"))
        ) return;
        if (value.isPressed)
        {
            _rigidbody2D.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!_isAlive) return;
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Run()
    {
        var xVelocity = _isAlive ? _moveInput.x * runSpeed : 0f;
        var playerVelocity = new Vector2(xVelocity, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = playerVelocity;
        _animator.SetBool(IsRunning, PlayerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        if (!PlayerHasHorizontalSpeed) return;
        transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
    }

    void ClimbLadder()
    {
        var isTouchingLadder = IsBodyTouchingLayer("Climbing");
        _animator.SetBool(IsClimbing, PlayerHasVerticalSpeed && isTouchingLadder);
        if (!isTouchingLadder)
        {
            _rigidbody2D.gravityScale = _initialGravityScale;
            return;
        }

        var climbVelocity = new Vector2(_rigidbody2D.velocity.x, _moveInput.y * climbSpeed);
        _rigidbody2D.velocity = climbVelocity;
        _rigidbody2D.gravityScale = 0f;
    }

    private bool IsBodyTouchingLayer(string layerName)
    {
        return IsColliderTouchingLayer(_bodyCollider, layerName);
    }

    private static bool IsColliderTouchingLayer(Collider2D col, string layerName)
    {
        return col.IsTouchingLayers(LayerMask.GetMask(layerName));
    }

    private void Die()
    {
        if (!_bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) return;
        _isAlive = false;
        _rigidbody2D.velocity = deathKick;
        _animator.SetTrigger(Dying);
        StartCoroutine(OnDeath());
    }

    private IEnumerator OnDeath()
    {
        yield return new WaitForSecondsRealtime(1);
        var gameSession = FindObjectOfType<GameSession>();
        gameSession.ProcessPlayerDeath();
    }
}
