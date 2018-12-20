using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk
{
    static class Vector3D
    {
        public static double DistanceBetweenPoints(Point p1, Point  p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2) + Math.Pow(p1.z - p2.z, 2));
        }

    }

    struct Point
    {
        public double x;
        public double y;
        public double z;

        public Point(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static explicit operator Point(Ball entity)
        {
            return new Point(entity.x, entity.y, entity.z);
        }

        public static explicit operator Point(Robot entity)
        {
            return new Point(entity.x, entity.y, entity.z);
        }

        public static explicit operator Point(Goal entity)
        {
            return new Point(entity.x, entity.y, entity.z);
        }
    }
}
