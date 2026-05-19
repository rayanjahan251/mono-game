using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace first_game;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _squareTexture;
    private float _ground;

    private Player _player;

    private Texture2D _background;

    private Rectangle[] _platforms; 

    public Game1()
    {
        
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 800;

        _platforms = new  Rectangle[3];
        _platforms[0] = new Rectangle(200, 620, 150, 20);
        _platforms[1] = new Rectangle(400, 520, 150, 20);
        _platforms[2] = new Rectangle(600, 420, 150, 20);
    }

    protected override void Initialize()
    {
        _player = new Player(
            new Vector2(50, 335),
            new Vector2(40, 65)
            );

        _ground = 760;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("images/background");

        _squareTexture = new Texture2D(GraphicsDevice, 1, 1);
        _squareTexture.SetData(new[] { Color.Beige });
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
            _spriteBatch.Draw(_squareTexture, _platforms[i], Color.RosyBrown);
        }

        _player.Draw(_spriteBatch);

        _spriteBatch.Draw(
            _squareTexture,
            new Rectangle(
                (int)_player.Position.X,
                (int)_player.Position.Y,
                (int)_player.Size.X,
                (int)_player.Size.Y),
            Color.Beige);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void ResolveCollisions()
    {
        for (int i = 0; i < _platforms.Length; i++)
        {
            bool isCollidingLeft = (_player.Position.X + _player.Size.X)
                > _platforms[i].Left;
            bool isCollidingTop = (_player.Position.Y + _player.Size.Y)
                > _platforms[i].Top;
            bool isCollidingRight = _player.Position.X < _platforms[i].Right;
            bool isCollidingBottom = _player.Position.Y
                < _platforms[i].Bottom;
            bool isColliding = isCollidingLeft
                && isCollidingTop
                && isCollidingRight
                && isCollidingBottom;

            if (isColliding)
            {
                if ((isCollidingLeft || isCollidingRight)
                    && (!isCollidingTop && !isCollidingBottom))
                {
                    _player.Velocity.X *= -1;
                }

                if (isCollidingBottom)
                {
                    _player.Velocity.Y *= -1;
                }

                if (isCollidingTop)
                {
                    _player.Velocity.Y = 0;
                    _player.Position.Y = _platforms[i].Top - _player.Size.Y;
                }
            }
        }
    }
}
