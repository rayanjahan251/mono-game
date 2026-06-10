using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Platform
{
    private Vector2 _position;
    private Rectangle _collider;
    private Texture2D _texture;

    public Platform(Vector2 position, Vector2 size)
    {
        _position = position;
        _collider = new Rectangle(
         position.ToPoint(), 
         size.ToPoint()
         );
    }
    public void LoadContent(Texture2D texture)
    {
        _texture = texture;
    }
}


