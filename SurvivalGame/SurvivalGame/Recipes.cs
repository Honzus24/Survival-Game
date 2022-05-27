using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame
{
    class Recipes
    {
        public int id;
        public List<int> resources = new List<int>();
        public List<int> item = new List<int>();

        public int Rcount;
        public int Icount;

        public Recipes(int Id)
        {
            id = Id;

            switch (id)
            {
                case 0:
                    break;
                case 1:
                    resources.Add(1); // stone
                    resources.Add(1); // stone
                    resources.Add(1); // stone

                    Rcount = 3;
                    Icount = 2;

                    item.Add(5); // leaf
                    item.Add(5); // leaf

                    Icount = 2;

                    break;
                case 2:
                    resources.Add(5); // leaf

                    Rcount = 1;

                    item.Add(1); // stone

                    Icount = 1;

                    break;
                case 3:
                    resources.Add(4); // wood

                    Rcount = 1;

                    item.Add(11); // table

                    Icount = 1;

                    break;
                case 4:
                    resources.Add(4); // wood

                    Rcount = 1;

                    item.Add(1); // stone

                    Icount = 1;

                    break;
                case 5:
                    resources.Add(4); // wood
                    resources.Add(4); // wood

                    Rcount = 2;

                    item.Add(5); // leaf

                    Icount = 1;

                    break;
            }
        }

        public List<Recipes> SearchRecipe(List<int> Resources)
        {
            List<Recipes> recipes = new List<Recipes>();
            bool recipeValid = false;

            // number of recipes

            for (int it = 0; it < 6; it++)
            {
                Recipes rec = new Recipes(it);

                for (int i = 0; i < rec.resources.Count; i++)
                {
                    for (int j = 0; j < Resources.Count; j++)
                    {
                        if (rec.resources.Contains(Resources[j]))
                        {
                            recipes.Add(rec);
                            recipeValid = true;
                            break;
                        }
                    }

                    if (recipeValid)
                    {
                        break;
                    }
                }
            }

            return recipes;
        }
    }
}
