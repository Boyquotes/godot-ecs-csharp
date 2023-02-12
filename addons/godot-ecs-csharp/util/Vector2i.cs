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

    }

}