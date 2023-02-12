using Godot;

namespace GdEcs
{

    public struct Vector2i
    {

        public int x { get; }
        public int y { get; }

        public Vector2 AsVector2 => new Vector2(x, y);

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2i(Vector2i vec) : this(vec.x, vec.y) { }

        public Vector2i(Vector2 vec) : this((int)vec.x, (int)vec.y) { }

        public Vector2i Clone()
        {
            return new Vector2i(x, y);
        }

        public override bool Equals(object? obj) => obj is Vector2i other && this.Equals(other);

        public bool Equals(Vector2i obj) => x == obj.x && y == obj.y;

        public override int GetHashCode() => (x, y).GetHashCode();

        public static bool operator ==(Vector2i a, Vector2i b) => a.Equals(b);

        public static bool operator !=(Vector2i a, Vector2i b) => !(a == b);

    }

}