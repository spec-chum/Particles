using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vector2 = System.Numerics.Vector2;

namespace Explosion;

/// <summary>
/// A simple particle manager using a circular buffer
/// </summary>
public class ParticleManager
{
    private readonly Particle[] _particles;
    private int _particleCount;
    private int _startIndex;

    public ParticleManager(int maxParticles)
    {
        _particles = new Particle[maxParticles];
        _particleCount = 0;
        _startIndex = 0;
    }

    /// <summary>
    /// Adds a new particle to the particle system.
    /// If the buffer is full, the oldest particle in the array is overwritten with the new one.
    /// </summary>
    /// <param name="particle">The particle to add.</param>
    public void AddParticle(Particle particle)
    {
        if (_particleCount < _particles.Length)
        {
            _particles[_particleCount] = particle;
            _particleCount++;
        }
        else
        {
            _particles[_startIndex] = particle;
            _startIndex = (_startIndex + 1) % _particles.Length;
        }
    }

    /// <summary>
    /// Update position of particles based on their veclocity and removes any expired particles.
    /// </summary>
    /// <param name="deltaTime">The elapsed time since the last update.</param>
    public void Update(float deltaTime)
    {
        int removedCount = 0;
        for (int i = 0; i < _particleCount; i++)
        {
            ref var particle = ref _particles[i];
            float lifetimeLeft = particle.RemainingLifetime -= deltaTime;

            particle.Position += particle.Velocity * deltaTime;

            // Start moving deleted particles to the end ready for deletion
            (particle, _particles[i - removedCount]) = (_particles[i - removedCount], particle);

            // Mark particle for deletion if expired
            if (lifetimeLeft <= 0f)
            {
                removedCount++;
            }
        }

        if (removedCount != 0)
        {
            // Delete expired particles
            _particleCount -= removedCount;

            // Reset startIndex when buffer becomes empty
            if (_particleCount == 0)
            {
                _startIndex = 0;
            }
        }
    }

    /// <summary>
    /// Renders the particles using the specified sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for rendering particles.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _particleCount; i++)
        {
            ref var particle = ref _particles[i];
            float alpha = particle.RemainingLifetime / particle.InitialLifetime;
            Color color = particle.Color;
            color.A = (byte)(255 * alpha);
            spriteBatch.Draw(particle.Texture, particle.Position, null, color, particle.Rotation, Vector2.Zero, particle.Size, SpriteEffects.None, 0f);
        }
    }
}