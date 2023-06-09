using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using Vector2 = System.Numerics.Vector2;

namespace Explosion;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ParticleManager _particleMgr;
    private Texture2D _texture;
    private MouseState _mouseState;
    private MouseState _prevMouseState;

    private float[] _fpsHistory = new float[4];
    private int _fpsIndex = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _particleMgr = new ParticleManager(5000);
        _texture = Content.Load<Texture2D>("particle");
    }

    protected override void Update(GameTime gameTime)
    {
        if (!IsActive)
        {
            return;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        ShowFPS(gameTime);

        _particleMgr.Update(deltaTime);

        _mouseState = Mouse.GetState();
        if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
        {
            GenerateNewParticles();
        }

        _prevMouseState = _mouseState;

        base.Update(gameTime);
    }

    private void ShowFPS(GameTime gameTime)
    {
        float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _fpsHistory[_fpsIndex] = 1 / elapsedSeconds;
        _fpsIndex = (_fpsIndex + 1) % _fpsHistory.Length;

        float smoothedFps = (_fpsHistory[0] + _fpsHistory[1] + _fpsHistory[2] + _fpsHistory[3]) / 4;

        Window.Title = $"Particles {smoothedFps:F2}";
    }

    private void GenerateNewParticles()
    {
        Vector2 position = _mouseState.Position.ToVector2().ToNumerics();

        for (int i = 0; i < 1000; i++)
        {
            Vector2 velocity = Vector2.Normalize(NextVector2(-1, 1)) * NextSingle(160, 800);
            float lifeTime = NextSingle(0.3f, 1);

            _particleMgr.AddParticle(new Particle
            {
                Texture = _texture,
                Position = position,
                Velocity = velocity,
                Color = Color.BlueViolet,
                Size = NextSingle(0.2f, 1),
                Rotation = RotationAngle(velocity),
                InitialLifetime = lifeTime,
                RemainingLifetime = lifeTime
            });
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(blendState: BlendState.Additive);
        _particleMgr.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private static float RotationAngle(Vector2 direction)
    {
        return MathF.Atan2(direction.Y, direction.X);
    }

    private static float NextSingle(float min, float max)
    {
        return Random.Shared.NextSingle() * (max - min) + min;
    }

    private static Vector2 NextVector2(float min, float max)
    {
        return new Vector2(NextSingle(min, max), NextSingle(min, max));
    }
}