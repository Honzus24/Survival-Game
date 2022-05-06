using System;

namespace SurvivalGame
{
    class Chest
    {
        public int[] itemsId = new int[18];
        public int[] itemsAmount = new int[18];

        private int maxAmount = 99;

        public int selectedItemPosition = 0;
        public int selectedPlayerItemPosition = 0;

        private int selectedId = -1;
        private int selectedAmount = 0;

        private bool browsingPlayer = false;

        public Chest()
        {
            for (int i = 0; i < itemsId.Length; i++)
            {
                // null
                itemsId[i] = -1;
            }
        }

        public void Interact(Player player)
        {
            Program.ChangeGameState(false);

            Console.Clear();
            Console.Clear();
            Console.Write("      press [ENTER]");
            Console.ReadLine();
            Console.Clear();

            bool inUI = true;

            while (inUI)
            {
                int itemNum = 0;

                Console.Write("\n\n\n");

                for (int x = 0; x < 9; x++)
                {
                    Console.Write($"          {new Items(player.inventory.itemsId[itemNum]).color + new Items(player.inventory.itemsId[itemNum]).renderChar}");
                    if (selectedPlayerItemPosition == itemNum)
                    {
                        Console.Write('☻');
                    }
                    itemNum++;
                }
                itemNum = 0;
                Console.WriteLine();
                for (int x = 0; x < 9; x++)
                {
                    string num = player.inventory.itemsAmount[itemNum].ToString();

                    Console.Write($"          {num}");
                    itemNum++;
                }

                Console.SetCursorPosition(0, 7);

                itemNum = 0;
                int itemNumber = 0;

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        Console.Write($"          {new Items(itemsId[itemNum]).color + new Items(itemsId[itemNum]).renderChar}");
                        if (selectedItemPosition == itemNum)
                        {
                            Console.Write('☻');
                        }
                        itemNum++;
                    }

                    Console.WriteLine();

                    for (int x = 0; x < 9; x++)
                    {
                        string num = itemsAmount[itemNumber].ToString();

                        Console.Write($"          {num}");
                        itemNumber++;
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine();
                    }
                }

                Console.WriteLine($"    {new Items(selectedId).color + new Items(selectedId).renderChar}");
                Console.Write($"    {selectedAmount}");

                Console.Write("\n\n");

                Console.Write($"Chest: {new Items(itemsId[selectedItemPosition]).name} - Player: {new Items(player.inventory.itemsId[selectedPlayerItemPosition]).name}");

                Console.Write("\n\n\n\n");

                Console.Write(@"
WASD = move
Space = take all items
M = take one item
N = drop one item
Q = exit");
                





                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Q)
                {
                    inUI = false;
                    Program.ChangeGameState(true);
                }
                else if (key.Key == ConsoleKey.W)
                {
                    if (!browsingPlayer)
                    {
                        selectedItemPosition -= 9;

                        if (selectedItemPosition < 0)
                        {
                            selectedItemPosition = 0;
                        }
                    }
                }
                else if (key.Key == ConsoleKey.S)
                {
                    if (!browsingPlayer)
                    {
                        selectedItemPosition += 9;

                        if (selectedItemPosition > 17)
                        {
                            selectedItemPosition = 17;
                        }
                    }

                }
                else if (key.Key == ConsoleKey.A)
                {
                    if (browsingPlayer)
                    {
                        selectedPlayerItemPosition -= 1;

                        if (selectedPlayerItemPosition < 0)
                        {
                            selectedPlayerItemPosition = 0;
                        }
                    }
                    else
                    {
                        selectedItemPosition -= 1;

                        if (selectedItemPosition < 0)
                        {
                            selectedItemPosition = 0;
                        }
                    }
                }
                else if (key.Key == ConsoleKey.D)
                {
                    if (browsingPlayer)
                    {
                        selectedPlayerItemPosition += 1;

                        if (selectedPlayerItemPosition > 8)
                        {
                            selectedPlayerItemPosition = 8;
                        }
                    }
                    else
                    {
                        selectedItemPosition += 1;

                        if (selectedItemPosition > 17)
                        {
                            selectedItemPosition = 17;
                        }
                    }
                }
                else if (key.Key == ConsoleKey.P)
                {
                    browsingPlayer = !browsingPlayer;
                }
                else if (key.Key == ConsoleKey.M)
                {
                    if (browsingPlayer)
                    {
                        if (player.inventory.itemsId[selectedPlayerItemPosition] == selectedId || selectedId == -1)
                        {
                            if (player.inventory.itemsAmount[selectedPlayerItemPosition] > 0)
                            {
                                selectedId = player.inventory.itemsId[selectedPlayerItemPosition];
                                player.inventory.itemsAmount[selectedPlayerItemPosition] -= 1;
                                selectedAmount += 1;

                                if (player.inventory.itemsAmount[selectedPlayerItemPosition] <= 0)
                                {
                                    player.inventory.itemsId[selectedPlayerItemPosition] = -1;
                                    player.inventory.itemsAmount[selectedPlayerItemPosition] = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (itemsId[selectedItemPosition] == selectedId || selectedId == -1)
                        {
                            if (itemsAmount[selectedItemPosition] > 0)
                            {
                                selectedId = itemsId[selectedItemPosition];
                                itemsAmount[selectedItemPosition] -= 1;
                                selectedAmount += 1;

                                if (itemsAmount[selectedItemPosition] <= 0)
                                {
                                    itemsId[selectedItemPosition] = -1;
                                    itemsAmount[selectedItemPosition] = 0;
                                }
                            }
                        }
                    }
                }
                else if (key.Key == ConsoleKey.Spacebar)
                {
                    if (browsingPlayer)
                    {
                        if (player.inventory.itemsId[selectedPlayerItemPosition] == selectedId || selectedId == -1)
                        {
                            if (player.inventory.itemsAmount[selectedPlayerItemPosition] > 0)
                            {
                                selectedId = player.inventory.itemsId[selectedPlayerItemPosition];
                                selectedAmount += player.inventory.itemsAmount[selectedPlayerItemPosition];

                                player.inventory.itemsId[selectedPlayerItemPosition] = -1;
                                player.inventory.itemsAmount[selectedPlayerItemPosition] = 0;
                            }
                        }
                    }
                    else
                    {
                        if (itemsId[selectedItemPosition] == selectedId || selectedId == -1)
                        {
                            if (itemsAmount[selectedItemPosition] > 0)
                            {
                                selectedId = itemsId[selectedItemPosition];
                                selectedAmount += itemsAmount[selectedItemPosition];

                                itemsId[selectedItemPosition] = -1;
                                itemsAmount[selectedItemPosition] = 0;
                            }
                        }
                    }
                }
                else if (key.Key == ConsoleKey.N)
                {
                    if (browsingPlayer)
                    {
                        if (player.inventory.itemsId[selectedPlayerItemPosition] == selectedId || player.inventory.itemsId[selectedPlayerItemPosition] == -1)
                        {
                            if (player.inventory.itemsAmount[selectedPlayerItemPosition] < 99)
                            {
                                player.inventory.itemsId[selectedPlayerItemPosition] = selectedId;
                                player.inventory.itemsAmount[selectedPlayerItemPosition]++;

                                selectedAmount -= 1;

                                if (selectedAmount <= 0)
                                {
                                    selectedAmount = 0;
                                    selectedId = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (itemsId[selectedItemPosition] == selectedId || itemsId[selectedItemPosition] == -1)
                        {
                            if (itemsAmount[selectedItemPosition] < 99)
                            {
                                itemsId[selectedItemPosition] = selectedId;
                                itemsAmount[selectedItemPosition]++;

                                selectedAmount -= 1;

                                if (selectedAmount <= 0)
                                {
                                    selectedAmount = 0;
                                    selectedId = -1;
                                }
                            }
                        }
                    }
                }

                Console.Clear();
            }
        }

        public string WriteToSaveFile()
        {
            string finalOutput = "";

            for (int i = 0; i < itemsId.Length; i++)
            {
                finalOutput += $"\n{itemsId[i]} {itemsAmount[i]}";
            }

            return finalOutput;
        }

        public void LoadDataToObject(string data)
        {
            string[] dataLines = data.Split('\n');
            Console.WriteLine(data);

            for (int i = 0; i < itemsId.Length; i++)
            {
                string[] item = dataLines[i].Split(' ');

                try
                {
                    itemsId[i] = int.Parse(item[0]);
                    itemsAmount[i] = int.Parse(item[1]);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
