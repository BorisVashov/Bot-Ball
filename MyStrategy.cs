using Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk
{
    public sealed class MyStrategy : IStrategy
    {
        public enum RobotRole
        {
            ATTACKER_SINGLE,
            DEFENDER_SINGLE,
            ATTACKER_LEFT,
            ATTACKER_RIGHT,
            DEFENDER_LEFT,
            DEFENDER_RIGHT
        }

        private bool isStartOne = true;
        private bool isStartTwo = true;

        private int idRobotOne;
        private int idRobotTwo;

        private RobotRole roleRobotOne = RobotRole.DEFENDER_SINGLE;
        private RobotRole roleRobotTwo = RobotRole.ATTACKER_SINGLE;

        private double distanceBallToMyGoal;
        private double distanceMeToBall;
        private double distanceMeToMyGoal;

        private Goal myGoal = new Goal(0, 0, -40);



        public void Act(Robot me, Rules rules, Game game, Action action)
        {
            if (isStartOne || isStartTwo) Initialize(me);

            CalculateDistances(me, game.ball);

            //DebugShow(me);

            if (me.id == idRobotOne)
            {
                PlaySelfRole(roleRobotOne, me, rules, game, action);
            }
            else if (me.id == idRobotTwo)
            {
                PlaySelfRole(roleRobotTwo, me, rules, game, action);
            }
        }


        private void PlaySelfRole(RobotRole roleRobot, Robot me, Rules rules, Game game, Action action)
        {

            switch (roleRobot)
            {
                case RobotRole.DEFENDER_SINGLE:
                    if (IsBallNearMyGoal() || (IsBallNearMe() && IsMeNearMyGoal())) // < 15
                    {
                        if (distanceMeToBall > 8)
                            MoveToBall(me, game.ball, action, rules);
                        else
                            HitBall(me, game.ball, action, rules);
                    }
                    else
                    {
                        GoDefendGoal(me, game.ball, action, rules.arena, rules);
                    }

                    break;

                case RobotRole.ATTACKER_SINGLE:
                    if (IsBallNearMe())
                        HitBall(me, game.ball, action, rules);
                    else
                        MoveToBall(me, game.ball, action, rules);

                    break;
            }
        }

        private bool IsBallNearMe()
        {
            if (distanceMeToBall < 5)
                return true;
            else
                return false;
        }

        private bool IsBallNearMyGoal()
        {
            if (distanceBallToMyGoal < 16)
                return true;
            else
                return false;
        }

        private bool IsMeNearMyGoal()
        {
            if (distanceMeToMyGoal < 20)
                return true;
            else
                return false;
        }

        private void HitBall(Robot me, Ball ball, Action action, Rules rules)
        {
            System.Console.WriteLine("HitBall");

            if (IsRightSideToHitBall(me, ball, action, rules) == true)
            {
                if (ball.x < me.x)
                    action.target_velocity_x = rules.ROBOT_ACCELERATION * -1;
                else if (ball.x > me.x)
                    action.target_velocity_x = rules.ROBOT_ACCELERATION;

                if (ball.z > me.z)
                    action.target_velocity_z = rules.ROBOT_ACCELERATION;

                if (distanceMeToBall - rules.BALL_RADIUS < 2 || ((ball.y - me.y) > 3 && distanceMeToBall < 8))
                {
                    action.jump_speed = rules.ROBOT_MAX_JUMP_SPEED;
                }
            }
        }

        private void MoveToBall(Robot me, Ball ball, Action action, Rules rules)
        {
            System.Console.WriteLine("MoveToBall");

            action.target_velocity_x = rules.ROBOT_ACCELERATION * (ball.x - me.x);
            action.target_velocity_z = rules.ROBOT_ACCELERATION * (ball.z - me.z);
        }

        private bool IsRightSideToHitBall(Robot me, Ball ball, Action action, Rules rules)
        {
            bool isRightSide = false;

            if (me.z < (ball.z - rules.BALL_RADIUS / 2))
                isRightSide = true;

            if (isRightSide == false)
            {
                if (ball.y - rules.BALL_RADIUS - 2 > me.y)
                {
                    action.target_velocity_z = -rules.ROBOT_ACCELERATION;
                }
                else
                {
                    action.target_velocity_z = -rules.ROBOT_ACCELERATION;

                    if (me.x > 0)
                        action.target_velocity_x = -rules.ROBOT_ACCELERATION;
                    else
                        action.target_velocity_x = rules.ROBOT_ACCELERATION;
                }
            }

            return isRightSide;
        }

        private void GoDefendGoal(Robot me, Ball ball, Action action, Arena arena, Rules rules)
        {
            System.Console.WriteLine("GoDefendGoal");

            action.target_velocity_z = rules.ROBOT_ACCELERATION * ((myGoal.z + 5) - me.z);

            if (ball.x < 5 || ball.x > -5)
                action.target_velocity_x = rules.ROBOT_ACCELERATION * (ball.x - me.x);
            else
                action.target_velocity_x = rules.ROBOT_ACCELERATION * (myGoal.x - me.x);
        }

        private void CalculateDistances(Robot me, Ball ball)
        {
            distanceMeToBall = Vector3D.DistanceBetweenPoints((Point)ball, (Point)me);
            distanceMeToMyGoal = Vector3D.DistanceBetweenPoints((Point)me, (Point)myGoal);
            distanceBallToMyGoal = Vector3D.DistanceBetweenPoints((Point)ball, (Point)myGoal);
        }

        private void Initialize(Robot me)
        {
            if (isStartOne)
            {
                idRobotOne = me.id;
                isStartOne = false;
            }
            else if (isStartTwo)
            {
                idRobotTwo = me.id;
                isStartTwo = false;
            }
        }

        private void ShowArena(Arena arena)
        {
            System.Console.WriteLine("=== ARENA ===");
            System.Console.WriteLine("arena.depth = " + arena.depth);
            System.Console.WriteLine("arena.height = " + arena.height);
            System.Console.WriteLine("arena.width = " + arena.width);
            System.Console.WriteLine("=== GOAL ===");
            System.Console.WriteLine("arena.goal_depth = " + arena.goal_depth);
            System.Console.WriteLine("arena.goal_height = " + arena.goal_height);
            System.Console.WriteLine("arena.goal_width = " + arena.goal_width);
            System.Console.WriteLine("===========================");
        }


        private int countTiks = 59;
        private void DebugShow(Robot robot)
        {
            countTiks++;

            if (countTiks % 60 == 0 && robot.id == idRobotOne)
            {
                System.Console.WriteLine("Robot #{0} --> X: [{1}] Y: [{2}] Z:[{3}]", robot.id, robot.x, robot.y, robot.z);

                System.Console.WriteLine("distanceMeToBall = {0}\ndistanceMeToMyGoal = {1}", distanceMeToBall, distanceMeToMyGoal);
                countTiks = 1;
            }
        }
    }
}
