using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame
{
    class Recipes
    {
        private int id;
        public List<int> resources = new List<int>();
        public List<int> item = new List<int>();

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

                    item.Add(5); // leaf
                    item.Add(5); // leaf

                    break;
                case 2:
                    resources.Add(5); // leaf

                    item.Add(1); // stone

                    break;
                case 3:
                    resources.Add(4); // wood

                    item.Add(11); // table

                    break;
                case 4:
                    resources.Add(4); // wood

                    item.Add(1); // stone

                    break;
                case 5:
                    resources.Add(4); // wood
                    resources.Add(4); // wood

                    item.Add(5); // leaf

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
