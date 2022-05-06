using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SurvivalGame
{
    class Player
    {
        public Vector2 position;
        //public Vector2 prewiousPosition;

        private float gravity = 0f;
        private float gravityNow;
        //private float gravityIndex = 0.05f;
        private float gravityIndex = 0.025f;
        private float maxFallingSpeed = 0.9f;

        private float speed;
        private float jumpHeight;

        private bool jumped = false;

        private int collisionCycle = 3;
        //public int maxDistance = 119;

        public Inventory inventory = new Inventory();
        private bool canScrollInventory = true;

        public bool buildMode = false;
        private bool canChangeBuildMode = true;

        private int useTimer; //build or mine
        private int maxUseTimer = 2;

        private bool canUse = true;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys ArrowKeys);

        public Player(Vector2 pos, float spd, float jmp)
        {
            position = pos;
            speed = spd;
            jumpHeight = jmp;

            gravityNow = gravity;
        }

        public void Update()
        {
            //Clear();

            //prewiousPosition = position;

            if (GetAsyncKeyState(Keys.A) < 0)
            {
                position.x -= speed;
            }

            if (GetAsyncKeyState(Keys.D) < 0)
            {
                position.x += speed;
            }

            if (GetAsyncKeyState(Keys.W) < 0)
            {
                if (jumped == false)
                {
                    if (Program.levelLayout[(int)(position.y + 0.45f), (int)(position.x)].collide &&
                        Program.levelLayout[(int)(position.y - 1f), (int)(position.x)].collide == false)
                    {
                        jumped = true;
                        Jump();
                    }
                }
            }
            else
            {
                jumped = false;
            }

            if (useTimer <= 0)
            {
                if (buildMode)
                {
                    if (inventory.itemsId[inventory.selectedItemPosition] != -1 && new Items(inventory.itemsId[inventory.selectedItemPosition]).placeable)
                    {
                        if (GetAsyncKeyState(Keys.Up) < 0)
                        {
                            Build(0, inventory.itemsId[inventory.selectedItemPosition]);
                        }
                        else if (GetAsyncKeyState(Keys.Right) < 0)
                        {
                            Build(1, inventory.itemsId[inventory.selectedItemPosition]);
                        }
                        else if (GetAsyncKeyState(Keys.Down) < 0)
                        {
                            Build(2, inventory.itemsId[inventory.selectedItemPosition]);
                        }
                        else if (GetAsyncKeyState(Keys.Left) < 0)
                        {
                            Build(3, inventory.itemsId[inventory.selectedItemPosition]);
                        }
                    }
                }
                else
                {
                    if (GetAsyncKeyState(Keys.Up) < 0)
                    {
                        Mine(0);
                    }
                    else if (GetAsyncKeyState(Keys.Right) < 0)
                    {
                        Mine(1);
                    }
                    else if (GetAsyncKeyState(Keys.Down) < 0)
                    {
                        Mine(2);
                    }
                    else if (GetAsyncKeyState(Keys.Left) < 0)
                    {
                        Mine(3);
                    }
                }
            }

            if (GetAsyncKeyState(Keys.RControlKey) < 0 && canChangeBuildMode)
            {
                canChangeBuildMode = false;
                buildMode = !buildMode;
            }
            else if (GetAsyncKeyState(Keys.RControlKey) >= 0 && canChangeBuildMode == false)
            {
                canChangeBuildMode = true;
            }

            if (GetAsyncKeyState(Keys.K) < 0 && canScrollInventory)
            {
                inventory.SelectItem(-1);
                canScrollInventory = false;
            }
            else if (GetAsyncKeyState(Keys.L) < 0 && canScrollInventory)
            {
                inventory.SelectItem(1);
                canScrollInventory = false;
            }

            if (GetAsyncKeyState(Keys.K) >= 0 && GetAsyncKeyState(Keys.L) >= 0 && canScrollInventory == false)
            {
                canScrollInventory = true;
            }

            if (GetAsyncKeyState(Keys.E) < 0 && canUse)
            {
                Interact();
            }
            else if (GetAsyncKeyState(Keys.E) >= 0 && canUse == false)
            {
                canUse = true;
            }

            bool collided = CheckCollision();

            if (collided)
            {
                gravityNow = gravity;
                //position = prewiousPosition;
                //position.y -= 1f;
            }
            else
            {
                ApplyGravity();
            }

            if (useTimer > -1)
            {
                useTimer--;
            }

            ResetCollisionCycle();

            //Console.Title = position.ToStringConvert() + " " + (int)position.x + " " + Convert.ToInt32(position.x);
            //Console.Title = inventory.itemsId[inventory.selectedItemPosition].ToString() + " " + inventory.itemsAmount[inventory.selectedItemPosition].ToString();

            //Draw();
        }

        private void Jump()
        {
            position.y -= 0.8f;
            gravityNow = -0.55f * jumpHeight;
        }


        public bool CheckCollision()
        {
            Items[,] levelLayout = Program.levelLayout;
            //List<Enemy> enemies = Program.Enemies;

            /*if (levelLayout[Convert.ToInt32(position.y), Convert.ToInt32(position.x)] == "S" ||
                levelLayout[Convert.ToInt32(position.y - 0.4f), Convert.ToInt32(position.x)] == "S")
            {
                // spikes
                Die();

                return true;
            }

            foreach (Enemy enemy in enemies)
            {
                // enemies
                Vector2 playerPosConverted = new Vector2((int)position.x, (int)position.y);
                Vector2 enemyPosConverted = new Vector2((int)enemy.position.x, (int)enemy.position.y);

                if (playerPosConverted.x == enemyPosConverted.x && playerPosConverted.y == enemyPosConverted.y)
                {
                    Die();

                    return true;
                }
            }*/

            if (levelLayout[(int)(position.y), (int)(position.x)].collide)
            {
                // sides
                Collide(new Vector2((int)(position.x), (int)(position.y)));

                if (levelLayout[(int)(position.y + 0.5f), (int)(position.x)].collide)
                {
                    // below
                    return true;
                }

                return false;
            }

            if (levelLayout[(int)(position.y + 0.45f), (int)(position.x)].collide)
            {
                // below
                return true;
            }

            if (levelLayout[(int)(position.y + 1.45f), (int)(position.x)].collide)
            {
                // 2 below
                if (gravityNow >= 0.5)
                {
                    gravityNow = 0.05f;
                    return false;
                }
            }

            if (levelLayout[(int)(position.y - 1f), (int)(position.x)].collide)
            {
                // top
                gravityNow = 0.05f;
                ApplyGravity();
            }

            return false;
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

            if (collisionCycle > 0)
            {
                collisionCycle--;
                CheckCollision();
            }

            //Draw();
        }

        public void ApplyGravity()
        {
            position.y += gravityNow;

            gravityNow += gravityIndex;

            if (gravityNow > maxFallingSpeed)
            {
                gravityNow = maxFallingSpeed;
            }
        }

        private void Mine(int direction /*0 up, 1 right, 2 down, 3 left*/)
        {
            useTimer = maxUseTimer;

            if (direction == 0)
            {
                if (Program.levelLayout[(int)(position.y - 1), (int)position.x].destroyable)
                {
                    inventory.AddItem(Program.levelLayout[(int)(position.y - 1), (int)position.x].id, Program.levelLayout[(int)(position.y - 1), (int)position.x].dropAmount);
                    Program.levelLayout[(int)(position.y - 1), (int)position.x] = new Items(0);
                    //Program.levelLayoutInteractiveBlocks[(int)(position.y - 1), (int)position.x] = new InteractiveBlocks(0);
                }
            }
            else if (direction == 1)
            {
                if (Program.levelLayout[(int)position.y, (int)(position.x + 1)].destroyable)
                {
                    inventory.AddItem(Program.levelLayout[(int)position.y, (int)(position.x + 1)].id, Program.levelLayout[(int)position.y, (int)(position.x + 1)].dropAmount);
                    Program.levelLayout[(int)position.y, (int)(position.x + 1)] = new Items(0);
                    //Program.levelLayoutInteractiveBlocks[(int)(position.y), (int)position.x + 1] = new InteractiveBlocks(0);
                }
            }
            else if (direction == 2)
            {
                if (Program.levelLayout[(int)(position.y + 1), (int)position.x].destroyable)
                {
                    inventory.AddItem(Program.levelLayout[(int)(position.y + 1), (int)position.x].id, Program.levelLayout[(int)(position.y + 1), (int)position.x].dropAmount);
                    Program.levelLayout[(int)(position.y + 1), (int)position.x] = new Items(0);
                    //Program.levelLayoutInteractiveBlocks[(int)(position.y + 1), (int)position.x] = new InteractiveBlocks(0);
                }
            }
            else if (direction == 3)
            {
                if (Program.levelLayout[(int)position.y, (int)(position.x - 1)].destroyable)
                {
                    inventory.AddItem(Program.levelLayout[(int)position.y, (int)(position.x - 1)].id, Program.levelLayout[(int)position.y, (int)(position.x - 1)].dropAmount);
                    Program.levelLayout[(int)position.y, (int)(position.x - 1)] = new Items(0);
                    //Program.levelLayoutInteractiveBlocks[(int)(position.y), (int)position.x - 1] = new InteractiveBlocks(0);
                }
            }
        }

        private void Build(int direction /*0 up, 1 right, 2 down, 3 left*/, int id)
        {
            useTimer = maxUseTimer;

            if (direction == 0)
            {
                if (Program.levelLayout[(int)(position.y - 1), (int)position.x].id == 0)
                {
                    if (inventory.RemoveItem(inventory.selectedItemPosition, 1))
                    {
                        Program.levelLayout[(int)(position.y - 1), (int)position.x] = new Items(id);

                        InteractiveBlock interactivityCheck = new InteractiveBlock(-1);
                        //if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayoutInteractiveBlocks[(int)(position.y - 1), (int)position.x] = new InteractiveBlocks(id);
                        if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayout[(int)(position.y - 1), (int)position.x] = new InteractiveBlock(id);
                    }
                }
            }
            else if (direction == 1)
            {
                if (Program.levelLayout[(int)position.y, (int)(position.x + 1)].id == 0)
                {
                    if (inventory.RemoveItem(inventory.selectedItemPosition, 1))
                    {
                        Program.levelLayout[(int)position.y, (int)(position.x + 1)] = new Items(id);

                        InteractiveBlock interactivityCheck = new InteractiveBlock(-1);
                        //if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayoutInteractiveBlocks[(int)(position.y), (int)position.x + 1] = new InteractiveBlocks(id);
                        if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayout[(int)(position.y), (int)position.x + 1] = new InteractiveBlock(id);
                    }
                }
            }
            else if (direction == 2)
            {
                if (Program.levelLayout[(int)(position.y + 1), (int)position.x].id == 0)
                {
                    if (inventory.RemoveItem(inventory.selectedItemPosition, 1))
                    {
                        Program.levelLayout[(int)(position.y + 1), (int)position.x] = new Items(id);

                        InteractiveBlock interactivityCheck = new InteractiveBlock(-1);
                        //if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayoutInteractiveBlocks[(int)(position.y + 1), (int)position.x] = new InteractiveBlocks(id);
                        if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayout[(int)(position.y + 1), (int)position.x] = new InteractiveBlock(id);
                    }
                }
            }
            else if (direction == 3)
            {
                if (Program.levelLayout[(int)position.y, (int)(position.x - 1)].id == 0)
                {
                    if (inventory.RemoveItem(inventory.selectedItemPosition, 1))
                    {
                        Program.levelLayout[(int)position.y, (int)(position.x - 1)] = new Items(id);

                        InteractiveBlock interactivityCheck = new InteractiveBlock(-1);
                        //if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayoutInteractiveBlocks[(int)(position.y), (int)position.x - 1] = new InteractiveBlocks(id);
                        if (interactivityCheck.IsBlockInteractive(id)) Program.levelLayout[(int)(position.y), (int)position.x - 1] = new InteractiveBlock(id);
                    }
                }
            }
        }

        private void Interact()
        {
            try
            {
                if (Program.levelLayout[(int)(position.y - 0.45f), (int)position.x].interactive)
                {
                    //Program.levelLayoutInteractiveBlocks[(int)(position.y - 0.45f), (int)position.x].InteractWBlock(this);
                    InteractiveBlock bl = Program.levelLayout[(int)(position.y - 0.45f), (int)position.x] as InteractiveBlock;
                    bl.InteractWBlock(this);
                }
            }
            catch (Exception)
            {

            }
        }

        public void Die()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();

            Console.CursorLeft = 0;
            Console.CursorTop = 0;

            Console.Write(" LOSE [Enter]");

            Console.ReadLine();

            Environment.Exit(0);
        }

        private void ResetCollisionCycle()
        {
            collisionCycle = 3;
        }
    }
}
