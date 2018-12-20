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
        

        public void Act(Robot me, Rules rules, Game game, Action action)
        {
            if (isStartOne || isStartTwo) Initialize(me);

            if (me.id == idRobotOne)
            {
                ChoosePosition(roleRobotOne, me, rules, game, action);
            }
            else if (me.id == idRobotTwo)
            {
                ChoosePosition(roleRobotTwo, me, rules, game, action);
            }
        }


        private void ChoosePosition(RobotRole roleRobot, Robot me, Rules rules, Game game, Action action)
        {
            double distanceToBall = Vector3D.DistanceBetweenBallAndRobot(game.ball, me);
            double distanceToMyGoal = Vector3D.DistanceBetweenRobotAndMyGoal(me, new Point(0, 0, -40));

            switch (roleRobot)
            {
                case RobotRole.DEFENDER_SINGLE:
                    if (distanceToMyGoal < 15 && distanceToBall < 3)
                    {
                        HitBall(me, game.ball, action, rules, distanceToBall);
                    }
                    else //if (distanceToBall > 15)
                    {
                        GoDefendGoal(me, game.ball, action, rules.arena, rules);
                    }

                    break;

                case RobotRole.ATTACKER_SINGLE:
                    //HitBall(me, game.ball, action, distanceToBall);
                    break;
            }
        }

        private void HitBall(Robot me, Ball ball, Action action, Rules rules, double distance)
        {
            if (GoToRightSideBall(me, ball, action, rules) == true || distance > 6)
            {
                if (ball.x < me.x)
                    action.target_velocity_x = rules.ROBOT_ACCELERATION * -1;
                else if (ball.x > me.x)
                    action.target_velocity_x = rules.ROBOT_ACCELERATION;

                if (ball.z > me.z)
                    action.target_velocity_z = rules.ROBOT_ACCELERATION;

                if (distance < 2 || ((ball.y - me.y) > 2 && distance < 4))
                {
                    action.jump_speed = rules.ROBOT_MAX_JUMP_SPEED;
                }
            }
        }

        private bool GoToRightSideBall(Robot me, Ball ball, Action action, Rules rules)
        {
            bool isRightSide = false;
            if (me.z < (ball.z - rules.BALL_RADIUS / 2))
                isRightSide = true;

            if (isRightSide == false)
            {
                if (ball.y - 2 > me.y)
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
            if (me.z > -(arena.depth / 2) + 10)
            {
                action.target_velocity_z = -rules.ROBOT_ACCELERATION;
            }
            else if (me.z < -(arena.depth / 2) + 5)
            {
                action.target_velocity_z = rules.ROBOT_ACCELERATION;
            }
            else
            {
                action.target_velocity_z = 0;
            }


            if (me.x - 1 > ball.x && me.x > -13)
            {
                action.target_velocity_x = -rules.ROBOT_ACCELERATION;
            }
            else if (me.x + 1 < ball.x && me.x < 13)
            {
                action.target_velocity_x = rules.ROBOT_ACCELERATION;
            }
            else
            {
                action.target_velocity_x = 0;
            }

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

    }
}
