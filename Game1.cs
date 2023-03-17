﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Explosion
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Particle> _particles;
        private MouseState _prevMouseState;

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
            _particles = new List<Particle>();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) 
                return;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 origin = mouseState.Position.ToVector2();

                for (int i = 0; i < 1000; i++)
                {
                    Vector2 velocity = Vector2.Normalize(new Vector2(Random.Shared.NextSingle() * 2 - 1, Random.Shared.NextSingle() * 2 - 1));
                    float rotation = GetRotationAngle(velocity);
                    velocity *= (2f + Random.Shared.NextSingle() * 8f);

                    _particles.Add(new Particle(Content.Load<Texture2D>("particle"), origin, velocity, Color.BlueViolet, rotation, 1f / (float.Epsilon + Random.Shared.NextSingle())));
                }
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                var particle = _particles[i];   
                if (particle.IsActive)
                {
                    particle.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    _particles.RemoveAt(i);
                    i--;
                }
            }

            _prevMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(blendState: BlendState.Additive);
            foreach (Particle p in _particles)
            {
                p.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private float GetRotationAngle(Vector2 direction)
        {
            return MathF.Atan2(direction.Y, direction.X);
        }
    }
}