using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

public class Player

{
    private const  float _gravity = 98;

    private float _movementSpeed;

    public Vector2 Position;
    public Vector2 Size;

    private Vector2 _velocity;

    public Player(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;

        _movementSpeed = 300;
    }

    public void Update (float deltaTime)
    {
        _velocity.Y += _gravity;

        Position.X += _velocity * _movementSpeed * deltaTime;
        Position.Y+= _velocity *_gravity * deltaTime;

    }

    public void Draw()
    {

    }

    public void SetDirection(Vector2 direction)
    {
        _velocity.X += direction.X;
    }
}