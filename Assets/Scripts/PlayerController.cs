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
    [SerializeField] private Vector2 _move;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("RigidBody")]
    [SerializeField] private Rigidbody2D _rigidBody;

    [Header("Player Sprite")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Player States")]
    [SerializeField] private bool _isJumping = false;
    [SerializeField] private bool _isGrounded = false;
    [SerializeField] private bool _isFalling = false;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private bool _isControllable = true;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    // Update is called once per frame
    void Update()
    {
        if(!_isControllable)
        {
            _move = Vector2.zero;
            return;
        }
        _animator.ResetTrigger("IsControllable");

        if(_isDead)
        {
            _rigidBody.velocity = Vector2.zero;
            _move = Vector2.zero;
            return;
        }

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

        _animator.SetBool("IsGrounded", _isGrounded);

        if(_isGrounded)
        {
            _animator.SetFloat("MoveX", Mathf.Abs(_move.x));
        }
        else
        {
            _animator.SetFloat("VelocityY", _rigidBody.velocity.y);

            _isFalling = _rigidBody.velocity.y < 0;
            _animator.SetBool("IsFalling", _isFalling);
        }
    }

    private void FixedUpdate()
    {
        //Logic for grounded ->
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _enemyLayer))
        {
            Debug.Log("Enemy dead");
            collision.gameObject.GetComponent<Animator>().SetTrigger("IsDead");
            _move.y = _gravity / 2;
            _rigidBody.AddForce(_move, ForceMode2D.Impulse);
        }
        //Checks if the player enters the "death zone" or the enemy
        else if (collision.gameObject.CompareTag("DeathZone") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Dead");
            _isDead = true;
            _animator.SetTrigger("IsDead");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Reason why it's trigger and not collision because we still want to be able to pass through the object
        if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("GOAL!");
            _isControllable = false;
            _animator.SetTrigger("IsControllable");
        }
    }

    //Visually represents the ground checker
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
