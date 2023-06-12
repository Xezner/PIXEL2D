using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector2 _move;
    private bool _isJumping = false;

    [Header("RigidBody")]
    [SerializeField] private Rigidbody2D _rigidBody;


    // Update is called once per frame
    void Update()
    {
        //Player input for Movement (left and right)
        _move.x = Input.GetAxisRaw("Move");
        if(Input.GetButtonUp("Move"))
        {
            Debug.Log("TEST");
            _move.x = 0;
        }


        //Jump Logic
        if(Input.GetButtonDown("Jump"))
        {
            _move.y = _jumpForce;
            _isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2( (_move.x * _moveSpeed), _rigidBody.velocity.y);
        
        if(_isJumping)
        {
            _isJumping = false;
            _rigidBody.AddForce(_move, ForceMode2D.Impulse);
        }
    }

}
