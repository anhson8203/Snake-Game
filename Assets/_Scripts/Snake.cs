using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    public class Snake : MonoBehaviour
    {
        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        private enum State
        {
            Alive,
            Dead
        }

        private State _state;
        private Direction _gridMoveDirection;
        
        private Vector2Int _gridPosition;
        private LevelGrid _levelGrid;
        
        private float _gridMoveTimer;
        private float _gridMoveTimerMax;
        private int _snakeBodySize;
        
        private List<SnakeMovePosition> _snakeMovePositionList;
        private List<SnakeBodyPart> _snakeBodyPartList;

        public void Setup(LevelGrid levelGrid) => _levelGrid = levelGrid;

        private void Awake()
        {
            _gridPosition = new Vector2Int(10, 10);
            _gridMoveTimerMax = .2f;
            _gridMoveTimer = _gridMoveTimerMax;
            _gridMoveDirection = Direction.Right;

            _snakeMovePositionList = new List<SnakeMovePosition>();
            _snakeBodySize = 0;

            _snakeBodyPartList = new List<SnakeBodyPart>();

            _state = State.Alive;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Alive:
                    HandleInput();
                    HandleGridMovement();
                    break;
                case State.Dead:
                    break;
            }
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_gridMoveDirection != Direction.Down)
                {
                    _gridMoveDirection = Direction.Up;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_gridMoveDirection != Direction.Up)
                {
                    _gridMoveDirection = Direction.Down;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_gridMoveDirection != Direction.Right)
                {
                    _gridMoveDirection = Direction.Left;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_gridMoveDirection != Direction.Left)
                {
                    _gridMoveDirection = Direction.Right;
                }
            }
        }

        private void HandleGridMovement()
        {
            _gridMoveTimer += Time.deltaTime;
            if (!(_gridMoveTimer >= _gridMoveTimerMax)) return;
            _gridMoveTimer -= _gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (_snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = _snakeMovePositionList[0];
            }

            var snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, _gridPosition, _gridMoveDirection);
            _snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch (_gridMoveDirection)
            {
                default:
                case Direction.Right:
                    gridMoveDirectionVector = new Vector2Int(+1, 0);
                    break;
                case Direction.Left:
                    gridMoveDirectionVector = new Vector2Int(-1, 0);
                    break;
                case Direction.Up:
                    gridMoveDirectionVector = new Vector2Int(0, +1);
                    break;
                case Direction.Down:
                    gridMoveDirectionVector = new Vector2Int(0, -1);
                    break;
            }

            _gridPosition += gridMoveDirectionVector;
            _gridPosition = _levelGrid.ValidateGridPosition(_gridPosition);

            var snakeAteFood = _levelGrid.TrySnakeEatFood(_gridPosition);
            if (snakeAteFood)
            {
                _snakeBodySize++;
                CreateSnakeBodyPart();
                SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
            }

            if (_snakeMovePositionList.Count >= _snakeBodySize + 1)
            {
                _snakeMovePositionList.RemoveAt(_snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (var snakeBodyPartGridPosition in _snakeBodyPartList.Select(snakeBodyPart => snakeBodyPart.GetGridPosition()).Where(snakeBodyPartGridPosition => _gridPosition == snakeBodyPartGridPosition))
            {
                _state = State.Dead;
                GameHandler.SnakeDied();
                SoundManager.PlaySound(SoundManager.Sound.SnakeDie);
            }

            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }

        private void CreateSnakeBodyPart() => _snakeBodyPartList.Add(new SnakeBodyPart(_snakeBodyPartList.Count));

        private void UpdateSnakeBodyParts()
        {
            for (var i = 0; i < _snakeBodyPartList.Count; i++)
            {
                _snakeBodyPartList[i].SetSnakeMovePosition(_snakeMovePositionList[i]);
            }
        }


        private float GetAngleFromVector(Vector2Int direction)
        {
            var n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            
            return n;
        }

        public List<Vector2Int> GetFullSnakeGridPositionList()
        {
            var gridPositionList = new List<Vector2Int>() { _gridPosition };
            gridPositionList.AddRange(_snakeMovePositionList.Select(snakeMovePosition => snakeMovePosition.GetGridPosition()));

            return gridPositionList;
        }
        
        private class SnakeBodyPart
        {
            private SnakeMovePosition _snakeMovePosition;
            private readonly Transform _transform;

            public SnakeBodyPart(int bodyIndex)
            {
                var snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
                snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.snakeBodySprite;
                snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
                _transform = snakeBodyGameObject.transform;
            }

            public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
            {
                _snakeMovePosition = snakeMovePosition;
                _transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

                float angle;
                switch (snakeMovePosition.GetDirection())
                {
                    default:
                    case Direction.Up:
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = 0;
                                break;
                            case Direction.Left:
                                angle = 0 + 45;
                                _transform.position += new Vector3(.2f, .2f);
                                break;
                            case Direction.Right:
                                angle = 0 - 45;
                                _transform.position += new Vector3(-.2f, .2f);
                                break;
                        }

                        break;
                    case Direction.Down:
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = 180;
                                break;
                            case Direction.Left:
                                angle = 180 - 45;
                                _transform.position += new Vector3(.2f, -.2f);
                                break;
                            case Direction.Right:
                                angle = 180 + 45;
                                _transform.position += new Vector3(-.2f, -.2f);
                                break;
                        }

                        break;
                    case Direction.Left:
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = +90;
                                break;
                            case Direction.Down:
                                angle = 180 - 45;
                                _transform.position += new Vector3(-.2f, .2f);
                                break;
                            case Direction.Up:
                                angle = 45;
                                _transform.position += new Vector3(-.2f, -.2f);
                                break;
                        }

                        break;
                    case Direction.Right:
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = -90;
                                break;
                            case Direction.Down:
                                angle = 180 + 45;
                                _transform.position += new Vector3(.2f, .2f);
                                break;
                            case Direction.Up:
                                angle = -45;
                                _transform.position += new Vector3(.2f, -.2f);
                                break;
                        }

                        break;
                }

                _transform.eulerAngles = new Vector3(0, 0, angle);
            }

            public Vector2Int GetGridPosition() => _snakeMovePosition.GetGridPosition();
        }

        private class SnakeMovePosition
        {
            private readonly SnakeMovePosition _previousSnakeMovePosition;
            private readonly Vector2Int _gridPosition;
            private readonly Direction _direction;

            public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
            {
                _previousSnakeMovePosition = previousSnakeMovePosition;
                _gridPosition = gridPosition;
                _direction = direction;
            }

            public Vector2Int GetGridPosition() => _gridPosition;

            public Direction GetDirection() => _direction;

            public Direction GetPreviousDirection() => _previousSnakeMovePosition?._direction ?? Direction.Right;
        }
    }
}