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
        public static double DistanceBetweenBallAndRobot(Ball ball, Robot robot)
        {
            return Math.Sqrt(Math.Pow(ball.x - robot.x, 2) + Math.Pow(ball.y - robot.y, 2) + Math.Pow(ball.z - robot.z, 2));
        }

        public static double DistanceBetweenRobotAndMyGoal(Robot robot, Point goalCenter)
        {
            return Math.Sqrt(Math.Pow(robot.x - goalCenter.x, 2) + Math.Pow(robot.y - goalCenter.y, 2) + Math.Pow(robot.z - goalCenter.z, 2));
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
    }
}
