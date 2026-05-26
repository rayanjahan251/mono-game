using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;



namespace my_game
{
    public class Enemy
    {
        private const float _gravity = 9.8f;

        public Vector2 Position;
        public Vector2 Size;

        private float _movementSpeed;


        public Vector2 Velocity;

        public Enemy(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;

            _movementSpeed = 300;
        }

        public void Update(float deltaTime)
        {
            Velocity.Y += _gravity;

            Position.X += Velocity.X * _movementSpeed * deltaTime;
            Position.Y += Velocity.Y * deltaTime;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void SetDirection(Vector2 direction)
        {
            Velocity.X = direction.X;
        }
    }
}
