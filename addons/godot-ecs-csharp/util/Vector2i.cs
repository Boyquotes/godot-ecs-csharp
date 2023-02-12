namespace GdEcs
{

    public struct Vector2i
    {

        public int x { get; }
        public int y { get; }

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2i Clone()
        {
            return new Vector2i(x, y);
        }

    }

}