using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame
{
    class Inventory
    {
        public int[] itemsId = new int[9];
        public int[] itemsAmount = new int[9];

        private int maxAmount = 99;

        public int selectedItemPosition = 0;

        public Inventory()
        {
            for (int i = 0; i < itemsId.Length; i++)
            {
                // null
                itemsId[i] = -1;
            }
        }

        public void SelectItem(int moveDirection /*-1 = left, 1 = right*/)
        {
            selectedItemPosition += moveDirection;

            if (selectedItemPosition < 0)
            {
                selectedItemPosition = 8;
            }
            else if (selectedItemPosition > 8)
            {
                selectedItemPosition = 0;
            }
        }

        public void AddItem(int id, int amount)
        {
            bool found = false;

            for (int itemPos = 0; itemPos < itemsId.Length; itemPos++)
            {
                if (itemsId[itemPos] == id && itemsAmount[itemPos] < maxAmount)
                {
                    itemsAmount[itemPos] += amount;

                    found = true;

                    break;
                }
            }

            if (found == false)
            {
                for (int itemPos = 0; itemPos < itemsId.Length; itemPos++)
                {
                    if (itemsId[itemPos] == -1)
                    {
                        itemsId[itemPos] = id;
                        itemsAmount[itemPos] = amount;

                        break;
                    }
                }
            }
        }

        public bool RemoveItem(int id, int amount)
        {
            //bool found = false;

            if (itemsAmount[id] - amount >= 0)
            {
                itemsAmount[id] -= amount;

                if (itemsAmount[id] <= 0)
                {
                    itemsId[id] = -1;
                    itemsAmount[id] = 0;
                }

                return true;
            }
            else
            {
                return false;
            }

            /*for (int itemPos = 0; itemPos < itemsId.Length; itemPos++)
            {
                if (itemsId[itemPos] == id && itemsAmount[itemPos] >= 0 + amount)
                {
                    itemsAmount[itemPos] -= amount;

                    if (itemsAmount[itemPos] <= 0)
                    {
                        itemsId[itemPos] = -1;
                    }

                    found = true;

                    break;
                }
            }

            if (found == false)
            {
                for (int itemPos = 0; itemPos < itemsId.Length; itemPos++)
                {
                    if (itemsId[itemPos] == -1)
                    {
                        itemsId[itemPos] = id;
                        itemsAmount[itemPos] -= amount;

                        break;
                    }
                }
            }*/
        }
    }
}
