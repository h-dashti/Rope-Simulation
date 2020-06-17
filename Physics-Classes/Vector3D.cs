//************************************************************************
//
// by H.dashti 
// email : ebi_2000_9@yahoo.com
//**************************************************************************


using System;

namespace Physics
{
    public struct Vector3D
    {
        public static readonly Vector3D Zero = new Vector3D();
        public double x, y, z;

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        # region Properties
        //public double X
        //{
        //    get { return x; }
        //    set { x = value; }
        //}
        //public double Y
        //{
        //    get { return y; }
        //    set { y = value; }
        //}
        //public double Z
        //{
        //    get { return z; }
        //    set { z = value; }
        //}
        public double Lenght
        {
            get { return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z); }
        }
        # endregion

        # region Methods

        public bool Equals(Vector3D v)
        {
            if (this.x == v.x && this.y == v.y && this.z == v.z)
                return true;
            else
                return false;
        }

        public Vector3D Unit()
        {
            if (this.Lenght == 0)
                return this;
            return this / this.Lenght;
        }
        public void Normalize()
        {
            double lenght = this.Lenght;
            if (lenght == 0)
                return;
            this /= lenght;
        }

        public Vector3D Rotate(Vector3D rotationAxes, double rotationAngle)
        {
            if (rotationAxes == Vector3D.Zero)
                throw new ArgumentException("You can't rotate around a zero vector.", "rotationAxis");

            rotationAxes.Normalize();
            Vector3D parallelVector = (this * rotationAxes) * rotationAxes;
            Vector3D perpendicularVector = this - parallelVector;
            Vector3D mutualyPerpendicularector = rotationAxes ^ perpendicularVector;

            Vector3D rotatedPerpendicularVector = (perpendicularVector * Math.Cos(rotationAngle)) +
               (mutualyPerpendicularector * Math.Sin(rotationAngle));

            return parallelVector + rotatedPerpendicularVector;
        }

        public Vector2D GetOnScreenYZPosition(Vector3D cameraPosition)
        {
            double d1 = (this.y - cameraPosition.y) * cameraPosition.x / (cameraPosition.x - this.x) +
               cameraPosition.y;
            double d2 = (this.z - cameraPosition.z) * cameraPosition.x / (cameraPosition.x - this.x) +
                cameraPosition.z;

            return new Vector2D(d1, d2);

        }

        public  Vector3D CrossProduct(Vector3D v)
        {
            double p = this.y * v.z - this.z * v.y;
            double q = this.z * v.x - this.x * v.z;
            double w = this.x * v.y - this.y * v.x;
            return new Vector3D(p, q, w);
        }
        public  double DotProduct(Vector3D v)
        {
            return this.x * v.x + this.y * v.y + this.z * v.z;
        }


        # endregion



        # region overloaded Operators


        public static Vector3D operator -(Vector3D v)
        {
            return new Vector3D(-v.x, -v.y, -v.z);
        }
        public static Vector3D operator +(Vector3D v)
        {
            return v;
        }
        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static double operator *(Vector3D v1, Vector3D v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Vector3D operator *(Vector3D v, double d)
        {
            return new Vector3D(v.x * d, v.y * d, v.z * d);
        }
        public static Vector3D operator *(double d, Vector3D v)
        {
            return new Vector3D(v.x * d, v.y * d, v.z * d);
        }
        public static Vector3D operator /(Vector3D v, double d)
        {
            return new Vector3D(v.x / d, v.y / d, v.z / d);
        }
        public static Vector3D operator ^(Vector3D v1, Vector3D v2)
        {
            return v1.CrossProduct(v2);
        }
        public static bool operator ==(Vector3D v1, Vector3D v2)
        {
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector3D v1, Vector3D v2)
        {
            return !v1.Equals(v2);
        }

        # endregion


        # region overriden Methos

        public override string ToString()
        {
            return "(" + this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString() + ")";
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector3D)
                return ((Vector3D)obj == this);
            else
                return false;
        }
        public override int GetHashCode()
        {
            uint hashX = (uint)this.x.GetHashCode();
            uint hashY = (uint)this.y.GetHashCode();
            uint hashZ = (uint)this.z.GetHashCode();

            return (int)(hashX ^ ((hashY << 10) + (hashY >> 22)) ^ ((hashZ << 20) + (hashZ >> 12)));
        }
        # endregion

    }


    public struct Vector2D
    {
        public double x, y;
        public Vector2D(double x,double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
