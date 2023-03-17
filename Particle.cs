﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Explosion
{
    public struct Particle
    {
        public readonly Texture2D Texture;
        public readonly Vector2 Velocity;
        public readonly Vector2 Origin;
        public readonly float Rotation;
        public readonly float LifeTime;
        public readonly float Scale;
        public Vector2 Position;
        public Color Color;
        public bool IsActive;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, Color color, float rotation, float lifeTime)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Color = color;
            Rotation = rotation;
            LifeTime = lifeTime;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Scale = 0.1f + Random.Shared.NextSingle();
            IsActive = true;
        }

        public void Update(float deltaTime)
        {
            Position += Velocity;
            Color = Color.Lerp(Color, Color.Black, deltaTime * LifeTime);
            IsActive = Color != Color.Black;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}