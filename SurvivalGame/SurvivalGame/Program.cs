using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SurvivalGame
{
    class Program
    {
        static Player player = new Player(new Vector2(600, 460), 0.3f, 0.7f);
        static System.Timers.Timer gameTimer = new System.Timers.Timer(15);
        //static System.Threading.Timer gameTimer = new System.Threading.Timer(Update, null, 50000, 5000);
        //static System.Threading.Timer drawTimer = new System.Threading.Timer(DrawFrame, null, 50000, 5000);
        static System.Timers.Timer drawTimer = new System.Timers.Timer(30);

        public static List<Enemy> Enemies = new List<Enemy>();

        public static Items[,] levelLayout = new Items[30, 360];
        //public static InteractiveBlocks[,] levelLayoutInteractiveBlocks = new InteractiveBlocks[30, 360];

        static Bitmap map;

        private static string frame;
        private static string oldFrame;

        //static char[,] screenBufferChar = new char[30, 120];
        static string[,] screenBufferChar = new string[30, 120];

        private static int canSave = -1;

        //private static int draw = 0;

        private static bool gameState = true;

        private static int winHeight = 30;
        private static int winWidth = 120;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys ArrowKeys);




        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        static void Main(string[] args)
        {
            gameTimer.Elapsed += new ElapsedEventHandler(Update);
            gameTimer.Interval = 15;
            drawTimer.Elapsed += new ElapsedEventHandler(DrawFrame);
            drawTimer.Interval = 35;

            GameSetup();

            Console.WindowHeight = winHeight;
            Console.WindowWidth = winWidth;

            Console.CursorVisible = false;

            while (true) ;
        }

        private static void Update(/*Object stateInfo*/object source, ElapsedEventArgs e)
        {
            if (gameState)
            {
                //DrawFrame();
                player.Update();

                //Console.ForegroundColor = ConsoleColor.Green;

                /*foreach (Enemy enemy in Enemies)
                {
                    enemy.Update();
                }*/

                Console.CursorLeft = 0;
                Console.CursorTop = 0;

                if (GetAsyncKeyState(Keys.P) < 0 && canSave <= 0)
                {
                    SaveGame();
                    Console.BackgroundColor = ConsoleColor.Red;
                }

                /*if (GetAsyncKeyState(Keys.Escape) < 0)
                {
                    gameTimer.Stop();
                    GameSetup();
                }*/

                if (canSave > -1)
                {
                    canSave--;
                }

                //CheckWin();
            }
        }

        private static void GameSetup()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                //return;
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Console.ReadKey();
                //return;
            }

            Console.Clear();
            Console.Write("Do you want do load saved game? [Y] / [N]");

            string gameLoad = Console.ReadLine();
            gameLoad = gameLoad.ToLower();

            if (gameLoad == "n")
            {
                LoadLevel();
            }
            else if (gameLoad == "y")
            {
                LoadLevelFromSave();
            }
            else
            {
                LoadLevel();
            }

            string pauseStr = "35";
            bool failed = true;
            int pauseBtwFrames = 35;

            while (failed)
            {
                Console.Write("Write pause between frames in milliseconds (35 is basic value): ");
                pauseStr = Console.ReadLine();

                try
                {
                    pauseBtwFrames = int.Parse(pauseStr);
                    failed = false;
                }
                catch (Exception)
                {
                    failed = true;
                    Console.WriteLine();
                }
            }

            drawTimer.Interval = pauseBtwFrames;

            screenBufferChar = new string[winHeight, winWidth];

            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;

            for (int y = 0; y < winHeight; y++)
            {
                for (int x = 0; x < winWidth; x++)
                {
                    oldFrame += "m";
                }
            }

            /*gameTimer.Change(0, 15);
            drawTimer.Change(0, 30);*/
            gameTimer.Start();
            drawTimer.Start();
        }

        private static void LoadLevel()
        {
            GenerateLevel();

            string location = Directory.GetCurrentDirectory();

            try
            {
                //map = new Bitmap($@"{location}\levelL.png");
                map = new Bitmap($@"genLevel.png");
            }
            catch (Exception)
            {
                Console.Write("Map not loaded [Enter]");
                Console.ReadLine();

                Environment.Exit(0);
            }

            levelLayout = new Items[map.Height, map.Width];
            //levelLayoutInteractiveBlocks = new InteractiveBlocks[map.Height, map.Width];

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Color color = map.GetPixel(x, y);

                    levelLayout[y, x] = new Items(color.R);

                    InteractiveBlock interactivityCheck = new InteractiveBlock(-1);
                    //if (interactivityCheck.IsBlockInteractive(new Item(color.R).id)) levelLayoutInteractiveBlocks[y, x] = new InteractiveBlocks(color.R);
                    if (interactivityCheck.IsBlockInteractive(new Items(color.R).id)) levelLayout[y, x] = new InteractiveBlock(color.R);

                    /*if (color == Color.FromArgb(0, 0, 0))
                    {
                        // stone
                        levelLayout[y, x] = new Block(1);
                    }
                    else if (color == Color.FromArgb(255, 0, 0))
                    {
                        // spikes
                        levelLayout[y, x] = "S";
                    }
                    else if (color == Color.FromArgb(0, 255, 0))
                    {
                        // target
                        levelLayout[y, x] = "T";
                    }
                    else if (color == Color.FromArgb(0, 0, 255))
                    {
                        // enemy
                        Enemies.Add(new Enemy(new Vector2(x, y), 0.25f));
                    }
                    else
                    {
                        // air
                        levelLayout[y, x] = new Block(0);
                    }*/
                }
            }

            Console.Write("\n" +
                "  World generated [ENTER] to continue");

            Console.ReadLine();
        }

        private static void LoadLevelFromSave()
        {
            string location = Directory.GetCurrentDirectory();
            string saveFilePlayer = "";

            try
            {
                saveFilePlayer = File.ReadAllText($@"{location}\Saves\savePlayer01.txt");
            }
            catch (Exception)
            {
                Console.WriteLine("No savefile found. ");

                LoadLevel();
                return;
            }

            Console.WriteLine("Loading world...");

            string[] saveFilePLines = saveFilePlayer.Split('\n');

            if (saveFilePlayer.Length > 0)
            {
                string[] playerPos = saveFilePLines[0].Split(' ');

                player.position = new Vector2(float.Parse(playerPos[0]), float.Parse(playerPos[1]));

                for (int i = 1; i < 10; i++)
                {
                    string[] inventoryItem = saveFilePLines[i].Split(' ');

                    player.inventory.itemsId[i - 1] = int.Parse(inventoryItem[0]);
                    player.inventory.itemsAmount[i - 1] = int.Parse(inventoryItem[1]);
                }
            }

            File.Delete($@"genLevel.png");
            File.Copy($@"Saves\map.png", $@"genLevel.png");

            try
            {
                map = new Bitmap($@"genLevel.png");
            }
            catch (Exception)
            {
                Console.Write("Map not loaded [Enter]");
                Console.ReadLine();

                Environment.Exit(0);
            }

            levelLayout = new Items[map.Height, map.Width];
            //levelLayoutInteractiveBlocks = new InteractiveBlocks[map.Height, map.Width];

            string saveFileBl = File.ReadAllText($@"{location}\Saves\saveBlocks01.txt");
            string[] saveFileBlocks = saveFileBl.Split('\n');

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Color color = map.GetPixel(x, y);

                    levelLayout[y, x] = new Items(color.R);

                    InteractiveBlock interactivityCheck = new InteractiveBlock(-1);
                    //if (interactivityCheck.IsBlockInteractive(new Item(color.R).id)) levelLayoutInteractiveBlocks[y, x] = new InteractiveBlocks(color.R);

                    if (interactivityCheck.IsBlockInteractive(new Items(color.R).id)) 
                    {
                        levelLayout[y, x] = new InteractiveBlock(color.R);

                        InteractiveBlock block = levelLayout[y, x] as InteractiveBlock;

                        string saveData = "";

                        for (int i = 0; i < saveFileBlocks.Length; i++)
                        {
                            if (saveFileBlocks[i][0] == '*')
                            {
                                i++;

                                string[] blPosStr = saveFileBlocks[i].Split(' ');


                                if (int.Parse(blPosStr[0]) == y && int.Parse(blPosStr[1]) == x)
                                {
                                    // found

                                    i++;

                                    while (saveFileBlocks[i][0] != '*' && i < saveFileBlocks.Length - 1)
                                    {
                                        saveData += saveFileBlocks[i];
                                        saveData += '\n';

                                        //Console.WriteLine(saveFileBlocks[i]);
                                        
                                        i++;
                                    }

                                    Console.WriteLine(saveData);
                                }
                            }
                        }

                        block.Load(saveData);
                    }
                }
            }

            Console.Write("  World loaded [ENTER] to continue");

            Console.ReadLine();
        }

        private static void DrawFrame(/*Object stateInfo*/object source, ElapsedEventArgs e)
        {
            if (gameState)
            {
                frame = "";

                for (int y = 0; y < winHeight; y++)
                {
                    for (int x = 0; x < winWidth; x++)
                    {
                        screenBufferChar[y, x] = " ";
                    }
                }

                string color = "";
                for (int y = 0; y < winHeight; y++)
                {
                    for (int x = 0; x < winWidth; x++)
                    {
                        if (color != levelLayout[y + (int)player.position.y - (winHeight / 2), x + (int)player.position.x - (winWidth / 2)].color)
                        {
                            screenBufferChar[y, x] = levelLayout[y + (int)player.position.y - (winHeight / 2), x + (int)player.position.x - (winWidth / 2)].color + levelLayout[y + (int)player.position.y - (winHeight / 2), x + (int)player.position.x - (winWidth / 2)].renderChar;
                            color = levelLayout[y + (int)player.position.y - (winHeight / 2), x + (int)player.position.x - (winWidth / 2)].color;
                        }
                        else
                        {
                            screenBufferChar[y, x] = levelLayout[y + (int)player.position.y - (winHeight / 2), x + (int)player.position.x - (winWidth / 2)].renderChar.ToString();
                        }
                    }
                }

                screenBufferChar[winHeight / 2, winWidth / 2] = "\u001b[38;5;123m☻";



                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < winWidth; x++)
                    {
                        screenBufferChar[y, x] = " ";
                    }
                }

                int startInvRender = (winWidth / 2) - 16;
                int it = startInvRender;

                for (int inventoryItem = 0; inventoryItem < 9; inventoryItem++)
                {
                    string intConverter = player.inventory.itemsAmount[inventoryItem].ToString();

                    screenBufferChar[1, it] = new Items(player.inventory.itemsId[inventoryItem]).color + new Items(player.inventory.itemsId[inventoryItem]).renderChar.ToString();
                    screenBufferChar[2, it] = intConverter[0].ToString();

                    if (intConverter.Length > 1)
                    {
                        screenBufferChar[2, it + 1] = intConverter[1].ToString();
                    }

                    if (player.inventory.selectedItemPosition == inventoryItem)
                    {
                        screenBufferChar[0, it] = "☻";
                    }

                    it += 4;
                }

                if (player.buildMode)
                {
                    screenBufferChar[1, 36] = "█";
                }
                else
                {
                    screenBufferChar[1, 36] = "Ҁ";
                }

                for (int p = 0; p < new Items(player.inventory.itemsId[player.inventory.selectedItemPosition]).name.Length; p++)
                {
                    screenBufferChar[1, 84 + p] = new Items(player.inventory.itemsId[player.inventory.selectedItemPosition]).name[p].ToString();
                }



                for (int y = 0; y < winHeight; y++)
                {
                    for (int x = 0; x < winWidth; x++)
                    {
                        frame += screenBufferChar[y, x];
                    }
                }

                Console.SetCursorPosition(0, 0);

                Console.Write(frame);
            }
        }

        private static void GenerateLevel()
        {
            Console.Write("Generating new world...");

            Bitmap generatedBmp = new Bitmap(1140, 1140);

            int height = (generatedBmp.Height - 140) / 2;

            Random rand = new Random();

            int prewiousBlock = rand.Next(-1, 2);

            for (int x = 70; x < generatedBmp.Width - 70; x++)
            {
                if (rand.Next(0, 4) == 0)
                {
                    height += prewiousBlock;
                }
                else
                {
                    prewiousBlock = rand.Next(-1, 2);
                    height += prewiousBlock;
                }



                generatedBmp.SetPixel(x, height, Color.FromArgb(2, 0, 0));

                int dirt = rand.Next(3, 6);

                for (int y = height + 1; y < height + dirt; y++)
                {
                    generatedBmp.SetPixel(x, y, Color.FromArgb(3, 0, 0));
                }

                for (int y = height + dirt; y < generatedBmp.Height - 70; y++)
                {
                    generatedBmp.SetPixel(x, y, Color.FromArgb(1, 0, 0));
                }
            }



            int numOfCaves = rand.Next(2, 6);

            for (int cave = 0; cave < numOfCaves; cave++)
            {
                int numOfBends = rand.Next(5, 13);

                List<int> caveBendsX = new List<int>();
                for (int bendX = 0; bendX < numOfBends; bendX++)
                {
                    caveBendsX.Add(rand.Next(60, generatedBmp.Width - 60));
                }

                List<int> caveBendsY = new List<int>();
                for (int bendY = 0; bendY < numOfBends; bendY++)
                {
                    caveBendsY.Add(rand.Next(60, generatedBmp.Height - 60));
                }

                int x = caveBendsX[0];
                int y = caveBendsY[0];

                for (int bend = 1; bend < caveBendsX.Count; bend++)
                {
                    while (x != caveBendsX[bend] || y != caveBendsY[bend])
                    {
                        if (x < caveBendsX[bend])
                        {
                            x += rand.Next(-1, 3);
                        }
                        else if (x > caveBendsX[bend])
                        {
                            x -= rand.Next(-1, 3);
                        }
                        else if (x == caveBendsX[bend])
                        {
                            x += rand.Next(-2, 3);
                        }

                        if (y < caveBendsY[bend])
                        {
                            y += rand.Next(-1, 3);
                        }
                        else if (y > caveBendsY[bend])
                        {
                            y -= rand.Next(-1, 3);
                        }
                        else if (y == caveBendsY[bend])
                        {
                            y += rand.Next(-2, 3);
                        }

                        for (int pixX = x - rand.Next(2, 4); pixX < x + rand.Next(2, 4); pixX++)
                        {
                            for (int pixY = y - rand.Next(2, 4); pixY < y + rand.Next(2, 4); pixY++)
                            {
                                generatedBmp.SetPixel(pixX, pixY, Color.FromArgb(0, 0, 0));
                            }
                        }
                    }
                }
            }

            int[] suitableBlocksSurface = new int[2] { 1, 3 };
            int[] suitableBlocksUnderground = new int[1] { 1 };
            int[] noBlockCheck = new int[1] { -1 };

            generatedBmp = GenStructure(generatedBmp, rand.Next(10, 26), 0, false, suitableBlocksSurface);
            generatedBmp = GenStructure(generatedBmp, rand.Next(7, 16), 1, false, suitableBlocksSurface);
            generatedBmp = GenStructure(generatedBmp, rand.Next(3, 11), 2, false, suitableBlocksSurface);
            generatedBmp = GenStructure(generatedBmp, rand.Next(30, 51), 3, true, suitableBlocksUnderground);
            generatedBmp = GenStructure(generatedBmp, rand.Next(1, 3), 4, false, suitableBlocksSurface);

            generatedBmp = GenOreVein(generatedBmp, 8, 2, 5, 450, 260);  // silver
            generatedBmp = GenOreVein(generatedBmp, 9, 1, 4, 370, 150);  // gold
            generatedBmp = GenOreVein(generatedBmp, 10, 1, 3, 150, 70); // damanite


            generatedBmp.Save("genLevel.png", System.Drawing.Imaging.ImageFormat.Png);
            generatedBmp.Dispose();
        }

        private static void SaveGame()
        {
            canSave = 66;

            string dir = Directory.GetCurrentDirectory();

            float playerX = player.position.x;
            float playerY = player.position.y;

            Bitmap saveMap = map;

            for (int y = 0; y < saveMap.Height; y++)
            {
                for (int x = 0; x < saveMap.Width; x++)
                {
                    saveMap.SetPixel(x, y, Color.FromArgb(levelLayout[y, x].id, 0, 0));
                }
            }

            Directory.CreateDirectory($@"{dir}\Saves");

            File.Delete($@"{dir}\Saves\map.png");
            File.Delete($@"{dir}\Saves\savePlayer01.txt");
            File.Delete($@"{dir}\Saves\saveBlocks01.txt");

            //File.Copy($@"{dir}\genLevel.png", $@"{dir}\Saves\map.png");
            saveMap.Save($@"{dir}\Saves\map.png", System.Drawing.Imaging.ImageFormat.Png);

            FileStream saveFileP = File.Create($@"{dir}\Saves\savePlayer01.txt");
            saveFileP.Close();

            FileStream saveFileB = File.Create($@"{dir}\Saves\saveBlocks01.txt");
            saveFileB.Close();

            string saveDataPlayer = $@"{playerX} {playerY}
{player.inventory.itemsId[0]} {player.inventory.itemsAmount[0]}
{player.inventory.itemsId[1]} {player.inventory.itemsAmount[1]}
{player.inventory.itemsId[2]} {player.inventory.itemsAmount[2]}
{player.inventory.itemsId[3]} {player.inventory.itemsAmount[3]}
{player.inventory.itemsId[4]} {player.inventory.itemsAmount[4]}
{player.inventory.itemsId[5]} {player.inventory.itemsAmount[5]}
{player.inventory.itemsId[6]} {player.inventory.itemsAmount[6]}
{player.inventory.itemsId[7]} {player.inventory.itemsAmount[7]}
{player.inventory.itemsId[8]} {player.inventory.itemsAmount[8]}";

            File.WriteAllText($@"{dir}\Saves\savePlayer01.txt", saveDataPlayer);

            string saveDataBlocks = "";

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (levelLayout[y, x] is InteractiveBlock)
                    {
                        saveDataBlocks += $@"
*****
{y} {x}";
                        InteractiveBlock block = levelLayout[y, x] as InteractiveBlock;

                        saveDataBlocks += block.Save();
                    }
                }
            }

            saveDataBlocks += "\nend";

            File.WriteAllText($@"{dir}\Saves\saveBlocks01.txt", saveDataBlocks);
        }

        private static Bitmap GenStructure(Bitmap map, int amount, int id, bool isUnderground, int[] blockType)
        {
            Random rand = new Random();

            for (int structNum = 0; structNum < amount; structNum++)
            {
                Structures structuresGen = new Structures();
                string st = structuresGen.GenerateStructure(id);
                string[] structure = st.Split('\n');

                int structLine = 0;

                int currentHeight = 0;

                int xSt = rand.Next(70, map.Width - 70);

                if (isUnderground)
                {
                    for (int y = 0; y < map.Height - 70; y++)
                    {
                        if (blockType[0] == -1)
                        {
                            if (new Items(map.GetPixel(xSt, y).R).collide)
                            {
                                currentHeight = rand.Next(y + 15, map.Height - 85);
                                break;
                            }
                        }
                        else
                        {
                            if (new Items(map.GetPixel(xSt, y).R).collide)
                            {
                                bool foundBlock = false;
                                currentHeight = rand.Next(y + 15, map.Height - 85);

                                for (int type = 0; type < blockType.Length; type++)
                                {
                                    if (new Items(map.GetPixel(xSt, currentHeight).R).id == blockType[type])
                                    {
                                        foundBlock = true;
                                        break;
                                    }
                                }

                                if (foundBlock)
                                {
                                    break;
                                }
                                else
                                {
                                    xSt = rand.Next(70, map.Width - 70);
                                    y = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < map.Height - 70; y++)
                    {
                        if (blockType[0] == -1)
                        {
                            if (new Items(map.GetPixel(xSt, y).R).collide)
                            {
                                currentHeight = y;
                                break;
                            }
                        }
                        else
                        {
                            if (new Items(map.GetPixel(xSt, y).R).collide)
                            {
                                bool foundBlock = false;

                                for (int type = 0; type < blockType.Length; type++)
                                {
                                    if (new Items(map.GetPixel(xSt, y).R).id == blockType[type])
                                    {
                                        currentHeight = y;
                                        foundBlock = true;
                                        break;
                                    }
                                }

                                if (foundBlock)
                                {
                                    break;
                                }
                                else
                                {
                                    xSt = rand.Next(70, map.Width - 70);
                                    y = 0;
                                }
                            }
                        }
                    }

                    int structureLength = 0;

                    for (int i = 0; i < structure.Length; i++)
                    {
                        if (structure[i].Length > structureLength)
                        {
                            structureLength = structure[i].Length;
                        }
                    }

                    xSt += structureLength / 2;
                }


                for (int yS = currentHeight - structure.Length; yS < currentHeight; yS++)
                {
                    string structureLine = structure[structLine];
                    char[] structureLineSplit = structureLine.ToCharArray();

                    int xPosInStructure = 0;

                    for (int xS = xSt - structureLineSplit.Length; xS < xSt; xS++)
                    {
                        if (structureLineSplit[xPosInStructure] != ' ' && structureLineSplit[xPosInStructure] != '\r')
                        {
                            map.SetPixel(xS, yS, Color.FromArgb((int)(char.GetNumericValue(structureLineSplit[xPosInStructure])), 0, 0));
                            InteractiveBlock interactivityCheck = new InteractiveBlock(-1);
                            //if (interactivityCheck.IsBlockInteractive(new Item(structureLineSplit[xPosInStructure]).id)) levelLayoutInteractiveBlocks[xS, yS] = new InteractiveBlocks(new Item(structureLineSplit[xPosInStructure]).id);
                            if (interactivityCheck.IsBlockInteractive(new Items(structureLineSplit[xPosInStructure]).id)) levelLayout[xS, yS] = new InteractiveBlock(new Items(structureLineSplit[xPosInStructure]).id);
                        }

                        xPosInStructure++;
                    }

                    structLine++;
                }
            }
            
            return map;
        }

        private static Bitmap GenOreVein(Bitmap map, int id, int amountMin, int amountMax, int maxHeight, int generateTimes)
        {
            Random rand = new Random();
            maxHeight = map.Height - 70 - maxHeight;

            for (int t = 0; t < generateTimes; t++)
            {
                int amount = rand.Next(amountMin, amountMax);

                Vector2 pos = new Vector2(0, 0);

                for (int i = 0; i < 10; i++)
                {
                    pos = new Vector2(rand.Next(70, map.Width - 70), rand.Next(maxHeight, map.Height - 70));

                    if (new Items(map.GetPixel((int)pos.x, (int)pos.y).R).collide)
                    {
                        break;
                    }
                }

                for (int n = 0; n < amount; n++)
                {
                    map.SetPixel((int)pos.x, (int)pos.y, Color.FromArgb(id, 0, 0));

                    pos.x += rand.Next(-1, 2);
                    pos.y += rand.Next(-1, 2);
                }
            }

            return map;
        }

        public static void ChangeGameState(bool changeTo)
        {
            gameState = changeTo;
        }
    }
}
