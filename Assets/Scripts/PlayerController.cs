using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic 2D Platformer Game made for PIXEL 2D as Webinar Master June 16,2023 Software Engineering Day, Polytechnic University of the Philippines, College of Computer Engineering
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _freefallForce;
    [SerializeField] private Vector2 _move;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayer;

    [Header("RigidBody")]
    [SerializeField] private Rigidbody2D _rigidBody;

    [Header("Player Sprite")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Player States")]
    [SerializeField] private bool _isJumping = false;
    [SerializeField] private bool _isGrounded = false;

    // Update is called once per frame
    void Update()
    {
        //Player input for Movement (left and right)
        _move.x = Input.GetAxisRaw("Move");

        //Flip the sprite based on the movement direction
        if (_move.x > 0.01f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_move.x < -0.01f)
        {
            _spriteRenderer.flipX = true;
        }

        //Jump Logic
        if (Input.GetButtonDown("Jump"))
        {
            _move.y = _gravity;
            _isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        //Logic for grounded
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        //Movemeent logic
        _rigidBody.velocity = new Vector2( (_move.x * _moveSpeed), _rigidBody.velocity.y);

        //Jump physics logic
        if (_isJumping && _isGrounded)
        {
            _isJumping = false;
            _rigidBody.AddForce(_move, ForceMode2D.Impulse);
        }
    }

    //Visually represents the ground checker
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
