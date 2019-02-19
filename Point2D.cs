namespace Codefarts.VectorGrid
{
    public partial struct Point2D  
    {
        public bool Equals(Point2D other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X * 397) ^ this.Y;
            }
        }

        public int X;
        public int Y;
                         
        public Point2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {     
            return obj is Point2D && this.Equals((Point2D)obj);
        }                                                                                                                 

        /// <summary>
        /// Converts a <see cref="Point2D"/> type into a unity Vector2 without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The <see cref="Point2D"/> to convert</param>
        /// <returns>Returns a new unity Vector2 type.</returns>
        public static implicit operator UnityEngine.Vector2(Point2D v)
        {
            return new UnityEngine.Vector2(v.X, v.Y);
        }

        /// <summary>
        /// Converts a unity Vector2 type into a <see cref="Point2D"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The unity Vector2 to convert</param>
        /// <returns>Returns a new unity Vector2 type.</returns>
        public static implicit operator Point2D(UnityEngine.Vector2 v)
        {
            return new Point2D((int)v.x, (int)v.y);
        }

        /// <summary>
        /// The +.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Point2D operator +(Point2D value1, Point2D value2)
        {
            Point2D x;
            x.X = value1.X + value2.X;
            x.Y = value1.Y + value2.Y;
            return x;
        }

        /// <summary>
        /// The +.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator +(Point2D value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X + value2.x;
            x.y = value1.Y + value2.y;
            return x;
        }

        /// <summary>
        /// The +.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator +(UnityEngine.Vector2 value2, Point2D value1)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X + value2.x;
            x.y = value1.Y + value2.y;
            return x;
        }

        /// <summary>
        /// The /.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator /(Point2D value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X / value2.x;
            x.y = value1.Y / value2.y;
            return x;
        }

        /// <summary>
        /// The /.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator /(UnityEngine.Vector2 value2, Point2D value1)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X / value2.x;
            x.y = value1.Y / value2.y;
            return x;
        }

        /// <summary>
        /// The ==.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator ==(Point2D value1, UnityEngine.Vector2 value2)
        {
            return value1.X == value2.x && value1.Y == value2.y;
        }

        /// <summary>
        /// The !=.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator !=(Point2D value1, UnityEngine.Vector2 value2)
        {
            return value1.X != value2.x || value1.Y != value2.y;
        }

        /// <summary>
        /// The *.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator *(Point2D value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X * value2.x;
            x.y = value1.Y * value2.y;
            return x;
        }

        /// <summary>
        /// The -.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator -(Point2D value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = -value2.x;
            x.y = -value2.y;
            return x;
        }

        /// <summary>
        /// The -.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator -(UnityEngine.Vector2 value2, Point2D value1)
        {
            UnityEngine.Vector2 x;
            x.x = -value2.x;
            x.y = -value2.y;
            return x;
        }     
    }
}