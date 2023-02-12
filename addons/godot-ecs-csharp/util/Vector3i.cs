namespace GdEcs
{

    public struct Vector3i
    {

        public int x { get; }
        public int y { get; }
        public int z { get; }

        public Vector3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3i Clone()
        {
            return new Vector3i(x, y, z);
        }

    }

}