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
    private float _jumpTimer;

    private Player _player;

    private Texture2D _background;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 800;


        
    }

    protected override void Initialize()
    {
        _player = new Player(
            new Vector2(50, 335),
            new Vector2(40, 65)
            );

        _jumpTimer = 0;
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
        GamePadState gamePad = GamePad.GetState (PlayerIndex.One);
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
        if (keyboard.IsKeyDown(Keys.Space) && (_jumpTimer <= 0))
        {
            direction.Y = -400;
            _jumpTimer = 1;
        }

        _player.Update(deltaTime);
        _player.SetDirection(direction);
        if (_player.Position.Y < (_ground - _player.Size.Y))
        {
            _player.Position.Y++;
        }
        if (_jumpTimer >= 0)
            _jumpTimer -= deltaTime;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, Vector2.Zero, Color.White);

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
}
