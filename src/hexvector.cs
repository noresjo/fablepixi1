
    [ProtoContract]
    public struct HexVector : IEquatable<HexVector>
    {

        [ProtoMember(1, DataFormat = DataFormat.TwosComplement)]
        public int HX;
        [ProtoMember(2, DataFormat = DataFormat.TwosComplement)]
        public int HY;
        [ProtoMember(3, DataFormat = DataFormat.TwosComplement)]
        public int HZ;

        public HexVector(int hx, int hy, int hz)
        {
            // We could assert here as well for the MAX_VALUE
            // Although as this should be a cheap structure it might be
            // a little to much
            this.HX = hx;
            this.HY = hy;
            this.HZ = hz;
        }

        public static implicit operator HexVector(string coord)
        {
            var parts = coord.Trim('(', ')').Split(',');
            return new HexVector(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }

        public HexVector(string s)
        {
            Trace.TraceInformation(s);
            if (s != null)
            {
                var parts = s.Trim('(', ')').Split(',');
                this.HX = int.Parse(parts[0]);
                this.HY = int.Parse(parts[1]);
                this.HZ = int.Parse(parts[2]);

            }
            else
            {
                this.HX = 0;
                this.HY = 0;
                this.HZ = 0;

            }
        }

        public bool IsValid { get { return HX == (HY + HZ); } }

        public int DistanceTo(HexVector other)
        {
            return HexVector.GetDistance(this, other);
        }

        public HexVector Add(HexVector other)
        {
            return new HexVector(this.HX + other.HX, this.HY + other.HY, this.HZ + other.HZ);
        }

        public HashSet<HexVector> GetNeighbours()
        {
            var result = new HashSet<HexVector>();

            var coord = this.Add(UP);
            foreach (var direction in DIRECTIONS)
            {
                coord = coord.Add(direction);
                result.Add(coord);
            }

            return result;
        }

        public IEnumerable<HexVector> GetNeighbours(IGameState state)
        {
            return this.GetNeighbours().Where(vector => state.HexagonByVector.ContainsKey(vector));
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + HX.GetHashCode();
                hash = hash * 23 + HY.GetHashCode();
                hash = hash * 23 + HZ.GetHashCode();
                return hash;
            }
        }

        public bool Equals(HexVector other)
        {
            return other == this;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.HX, this.HY, this.HZ);
        }

        public static readonly HexVector ORIGO = new HexVector(0, 0, 0);
        public static readonly HexVector UP = new HexVector(0, -1, 1);
        public static readonly HexVector UP_RIGHT = new HexVector(1, 0, 1);
        public static readonly HexVector DOWN_RIGHT = new HexVector(1, 1, 0);
        public static readonly HexVector DOWN = new HexVector(0, 1, -1);
        public static readonly HexVector DOWN_LEFT = new HexVector(-1, 0, -1);
        public static readonly HexVector UP_LEFT = new HexVector(-1, -1, 0);
        public static readonly HexVector NONE = new HexVector(int.MaxValue, int.MaxValue, int.MaxValue);

        public static HexVector[] DIRECTIONS = { DOWN_RIGHT, DOWN, DOWN_LEFT, UP_LEFT, UP, UP_RIGHT };

        /// <summary>
        /// Calculate distance between two vectors
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>Distance in number of coordes</returns>
        public static int GetDistance(HexVector source, HexVector destination)
        {
            int distance = Math.Max(Math.Abs(destination.HX - source.HX), Math.Abs(destination.HY - source.HY));
            distance = Math.Max(distance, Math.Abs(destination.HZ - source.HZ));

            Debug.Assert(distance >= 0);
            return distance;
        }

        public static HashSet<HexVector> GenerateSet(int radius)
        {
            HashSet<HexVector> result = new HashSet<HexVector>();
            result.Add(HexVector.ORIGO);
            HexVector coord = HexVector.ORIGO;
            for (int r = 1; r <= radius; r++)
            {
                coord = coord.Add(UP);
                foreach (var direction in DIRECTIONS)
                {
                    for (var i = 0; i < r; i++)
                    {
                        coord = coord.Add(direction);
                        result.Add(coord);
                    }
                }
            }

            return result;
        }

        public static bool operator ==(HexVector left, HexVector right)
        {
            return left.HX == right.HX && left.HY == right.HY && left.HZ == right.HZ;
        }
        public static bool operator !=(HexVector left, HexVector right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is HexVector ? (this == (HexVector)obj) : base.Equals(obj);
        }
    }