using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    [SerializeField, ReadonlyField] private float _inputHorizontal;
    [SerializeField, ReadonlyField] private float _inputVertical;
    [SerializeField, ReadonlyField] private bool _isPlayerControlling = false;
    [SerializeField, ReadonlyField] private float _currentMovementSpeed;

    [SerializeField] private float _movementSpeed = 6f;
    [SerializeField] private float _movementStopingSpeed = 0.35f;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    [Inject] private GameManager _gameManager;
    
    private void Start()
    {
        _currentMovementSpeed = _movementSpeed;
    }
    
    void Update()
    {
        if (_gameManager.IsGameStarted)
        {
            _inputHorizontal = SimpleInput.GetAxis(Horizontal);
            _inputVertical = SimpleInput.GetAxis(Vertical);
            CheckIfPlayerControlling();
        }
    }
    
    private void CheckIfPlayerControlling()
    {
        bool controllingHorizontal = MyUtilities.Utilities.IsNotZero(_inputHorizontal);
        bool controllingVertical = MyUtilities.Utilities.IsNotZero(_inputVertical);
        _isPlayerControlling = controllingHorizontal || controllingVertical;
    }
    
    private void FixedUpdate()
    {
        if (_isPlayerControlling && _gameManager.IsGameStarted)
        {
            MoveAndRotateByJoystick();
        }
        else
        {
            StopMovement();
        }
    }
    
    private void MoveAndRotateByJoystick()
    {
        Vector2 moveDirection = new Vector2(_inputHorizontal, _inputVertical);
        moveDirection.Normalize();
        _rigidbody.velocity = moveDirection * _currentMovementSpeed;
        RotatePlayer(moveDirection);
    }

    private void RotatePlayer(Vector2 normalisedMoveDirection)
    {
        float x = normalisedMoveDirection.x;
        bool directionIsNotZero = MyUtilities.Utilities.IsNotZero(x);
        if (directionIsNotZero)
        {
            _spriteRenderer.flipX = x < 0;
        }
    }

    private void StopMovement()
    {
        bool ifCharacterStillMoving = _rigidbody.velocity.sqrMagnitude > 0.0001f;

        if (ifCharacterStillMoving)
        {
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, _movementStopingSpeed);
        }
    }
}
