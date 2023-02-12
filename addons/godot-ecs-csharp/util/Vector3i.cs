using Godot;

namespace GdEcs
{

    public struct Vector3i
    {

        public int x { get; }
        public int y { get; }
        public int z { get; }

        public Vector3 AsVector3 => new Vector3(x, y, z);

        public Vector3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3i(Vector3i vec) : this(vec.x, vec.y, vec.z) { }

        public Vector3i(Vector3 vec) : this((int)vec.x, (int)vec.y, (int)vec.z) { }

        public Vector3i Clone()
        {
            return new Vector3i(x, y, z);
        }

        public override bool Equals(object? obj) => obj is Vector3i other && this.Equals(other);

        public bool Equals(Vector3i obj) => x == obj.x && y == obj.y && z == obj.z;

        public override int GetHashCode() => (x, y, z).GetHashCode();

        public static bool operator ==(Vector3i a, Vector3i b) => a.Equals(b);

        public static bool operator !=(Vector3i a, Vector3i b) => !(a == b);

    }

}