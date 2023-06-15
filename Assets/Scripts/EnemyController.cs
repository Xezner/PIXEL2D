using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private float _startPosX;
    [SerializeField] private float _endPosX;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isPatrolling = true;

    private Vector2 _direction;
    private Vector2 _targetPosition;
    private bool _isMovingTowardsEndPos = true;

    private Vector2 _startPos;
    private Vector2 _endPos;

    private void Start()
    {
        //spawn game object
        transform.position = new Vector3(_startPosX, transform.position.y, 0);

        //initialize the start and end position
        _startPos = new Vector2(_startPosX, 0);
        _endPos = new Vector2(_endPosX, 0);

        //assign first target position
        _targetPosition = _endPos;
        
        //move towards target position
        _isMovingTowardsEndPos = true;
    }


    private void FixedUpdate()
    {
        if (_isPatrolling)
        {
            //Get current x position
            Vector2 _currentPosition = new Vector2(_rigidBody.position.x, 0);

            //Main formula for getting the direction
            //Normalize allows us to normalize all values with maximum value of 1
            _direction = (_targetPosition - _currentPosition).normalized;

            //Applies velocity to the game object
            _rigidBody.velocity = new Vector2(_direction.x * _speed, _rigidBody.velocity.y);

            //If destination is reached
            if (Vector2.Distance(_currentPosition, _targetPosition) < 0.1f)
            {
                //change target position if destination is reached
                if (_isMovingTowardsEndPos)
                {
                    _targetPosition = _startPos;
                }
                else
                {
                    _targetPosition = _endPos;
                }

                //toggle states
                _isMovingTowardsEndPos = !_isMovingTowardsEndPos;
            }
        }
    }
}
