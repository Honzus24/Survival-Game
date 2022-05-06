using System;

namespace SurvivalGame
{
    class Enemy
    {
        /*public Vector2 position;
        private float origSpeed;
        private float speed;

        private float gravity = 0f;
        private float gravityNow;
        private float gravityIndex = 0.025f;

        public Enemy(Vector2 pos, float spd)
        {
            position = pos;
            origSpeed = spd;

            speed = origSpeed;
            gravityNow = gravity;
        }

        public void Update()
        {
            int collided = CheckCollision();

            if (collided == 1)
            {
                speed *= -1;
            }

            if (collided == 2)
            {
                gravityNow = gravity;
            }
            else
            {
                ApplyGravity();
            }

            Move();
        }

        private void Move()
        {
            position.x += speed;
        }

        public int CheckCollision()
        {
            string[,] levelLayout = Program.levelLayout;

            /*if (levelLayout[Convert.ToInt32(position.y), Convert.ToInt32(position.x)] == "S" ||
                levelLayout[Convert.ToInt32(position.y - 0.4f), Convert.ToInt32(position.x)] == "S")
            {
                return true;
            }*

            if (levelLayout[(int)position.y, (int)position.x] == "B")
            {
                // sides
                Collide(new Vector2((int)position.x, (int)position.y));

                return 1;
            }

            if (levelLayout[Convert.ToInt32(position.y + 0.4f), Convert.ToInt32(position.x)] == "B")
            {
                // below
                return 2;
            }

            return 0;
        }

        public void Collide(Vector2 block)
        {
            if (block.x - position.x >= -0.5)
            {
                position.x -= 0.4f;
            }
            else if (block.x - position.x < -0.5)
            {
                position.x += 0.4f;
            }

            //CheckCollision();
        }

        public void ApplyGravity()
        {
            position.y += gravityNow;

            gravityNow += gravityIndex;

            if (gravityNow > 1.5f)
            {
                gravityNow = 1.5f;
            }
        }*/
    }
}
