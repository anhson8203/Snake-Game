using UnityEngine;

namespace _Scripts
{
    public class LevelGrid
    {
        private Vector2Int _foodGridPosition;
        private GameObject _foodGameObject;
        private Snake _snake;
        
        private readonly int _width;
        private readonly int _height;

        public LevelGrid(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Setup(Snake snake)
        {
            _snake = snake;
            SpawnFood();
        }

        private void SpawnFood()
        {
            do
            {
                _foodGridPosition = new Vector2Int(Random.Range(0, _width), Random.Range(0, _height));
            } while (_snake.GetFullSnakeGridPositionList().IndexOf(_foodGridPosition) != -1);

            _foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
            _foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.foodSprite;
            _foodGameObject.transform.position = new Vector3(_foodGridPosition.x, _foodGridPosition.y);
        }

        public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
        {
            if (snakeGridPosition != _foodGridPosition) return false;
            
            Object.Destroy(_foodGameObject);
            SpawnFood();
            Score.AddScore();
            
            return true;
        }

        public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
        {
            if (gridPosition.x < 0)
            {
                gridPosition.x = _width - 1;
            }

            if (gridPosition.x > _width - 1)
            {
                gridPosition.x = 0;
            }

            if (gridPosition.y < 0)
            {
                gridPosition.y = _height - 1;
            }

            if (gridPosition.y > _height - 1)
            {
                gridPosition.y = 0;
            }

            return gridPosition;
        }
    }
}