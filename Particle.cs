using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vector2 = System.Numerics.Vector2;

namespace Explosion;

public struct Particle
{
    public Texture2D Texture;
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;
    public float Size;
    public float Rotation;
    public float InitialLifetime;
    public float RemainingLifetime;
}