using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using my_game;
using System.Reflection.Metadata.Ecma335;

namespace first_game;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _squareTexture;
    private float _ground;

    private Player _player;

    private Texture2D _background;
    private Texture2D _platform;

    private Rectangle[] _platforms;

    private Enemy _enemy;
    public Game1()
    {
        
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 800;

        _platforms = new  Rectangle[5];
        _platforms[0] = new Rectangle(200, 620, 150, 50);
        _platforms[1] = new Rectangle(400, 520, 150, 50);
        _platforms[2] = new Rectangle(600, 420, 150, 50);
        _platforms[3] = new Rectangle(200, 320, 150, 50);
        _platforms[4] = new Rectangle(600, 220, 150, 50);
    }

    protected override void Initialize()
    {
        _player = new Player(
            new Vector2(50, 335),
            new Vector2(90, 90)
            );

        _ground = 760;

        base.Initialize();

        _enemy = new Enemy
            (new Vector2(500, 400),
            new Vector2(40, 65)
            );
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("images/background");

        _squareTexture = new Texture2D(GraphicsDevice, 1, 1);
        _squareTexture.SetData(new[] { Color.Beige });

        Texture2D playerTexture = Content.Load<Texture2D>("main-character-sqr");
        _player.LoadContent(playerTexture);

        _platform = Content.Load<Texture2D>("images/platforms");
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
        KeyboardState keyboard = Keyboard.GetState();

        if (gamePad.Buttons.Back == ButtonState.Pressed
            || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        Vector2 direction = new Vector2();
        if (keyboard.IsKeyDown(Keys.A))
        {       
            direction.X = -1;
        }
        if (keyboard.IsKeyDown(Keys.D))
        {
            direction.X = 1;
        }
        if (keyboard.IsKeyDown(Keys.Space) && (_player.Velocity.Y == 0))
        {
            _player.Jump();
        }

        _player.Update(deltaTime);
        _player.SetDirection(direction);
        if (_player.Position.Y < (_ground - _player.Size.Y))
        {
            _player.Position.Y++;
        }

        if ((_player.Position.Y + _player.Size.Y) >= _ground)
        {
            _player.Velocity.Y = 0;
            _player.Position.Y = _ground - _player.Size.Y;
        }

        _enemy.Update(deltaTime);
 
        if ((_enemy.Position.Y + _enemy.Size.Y) >= _ground)
        {
            _enemy.Velocity.Y = 0;
            _enemy.Position.Y = _ground - _enemy.Size.Y;
        }

        ResolveCollisions();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, Vector2.Zero, Color.White);

        for (int i = 0; i < _platforms.Length; ++i)
        {
            _spriteBatch.Draw(_platform, _platforms[i], Color.RosyBrown);
        }

        _player.Draw(_spriteBatch);
        _enemy.Draw(_spriteBatch);

        _spriteBatch.Draw(
        _squareTexture,
            new Rectangle(
                (int)_enemy.Position.X,
                (int)_enemy.Position.Y,
                (int)_enemy.Size.X,
                (int)_enemy.Size.Y),
            Color.Red);
                
                

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void ResolveCollisions()
    {
        for (int i = 0; i < _platforms.Length; i++)
        {
            Vector2 collisionData = GetCollisionData(_player.Collider, _platforms[i]);
            if (collisionData == Vector2.Zero)
                continue;
            _player.Position += collisionData;
            if(collisionData.X != 0)
            {
                _player.Velocity.X = 0;
            }
            else
            {
                if (collisionData.Y < 0)
                {
                    _player.Velocity.Y = 0;
                }
                else
                {
                    _player.Velocity.Y = 0.1f;
                }
            }

        }
    }
    private Vector2 GetCollisionData(Rectangle a, Rectangle b)
    {
        Vector2 result = Vector2.Zero;
        if(a.Intersects(b))
        {
            Rectangle overlap = Rectangle.Intersect(a, b);
            if(overlap.Width < overlap.Height)
            {
                int direction = a.Center.X <b.Center.X ? -overlap.Width : overlap.Width;
                result.X = direction;
            }
            else
            {
                int direction = a.Center.Y < b.Center.Y ? -overlap.Height : overlap.Height;
                result.Y = direction;
            }
        }
        return result;
    }
}
