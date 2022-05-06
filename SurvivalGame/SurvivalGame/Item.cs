namespace SurvivalGame
{
    class Items
    {
        public int id;
        public int dropAmount;

        public string name;

        public char renderChar;

        public bool destroyable = true;
        public bool collide = true;
        public bool placeable = true;
        public bool interactive = false;

        public string color = "\u001b[32;1m";

        public Items(int Id)
        {
            id = Id;

            switch (id)
            {
                case -1:
                    name = "nothing";

                    collide = false;
                    destroyable = false;
                    renderChar = ' ';
                    dropAmount = 0;
                    placeable = false;

                    break;
                case 0:
                    name = "air";

                    collide = false;
                    destroyable = false;
                    renderChar = ' ';
                    dropAmount = 0;
                    placeable = false;
                    color = "\u001b[30m"; // black

                    break;
                case 1:
                    name = "stone";

                    collide = true;
                    destroyable = true;
                    renderChar = '█';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[38;5;242m"; // gray

                    break;
                case 2:
                    name = "grass";

                    collide = false;
                    destroyable = true;
                    renderChar = 'M';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[32;1m"; // bright green

                    break;
                case 3:
                    name = "dirt";

                    collide = true;
                    destroyable = true;
                    renderChar = '▓';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[38;5;130m"; // brown

                    break;
                case 4:
                    name = "wood";

                    collide = true;
                    destroyable = true;
                    renderChar = '║';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[38;5;130m"; // brown

                    break;
                case 5:
                    name = "leaf";

                    collide = true;
                    destroyable = true;
                    renderChar = '▒';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[32;1m"; // bright green

                    break;
                case 6:
                    name = "crystal";

                    collide = true;
                    destroyable = true;
                    renderChar = '░';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[38;5;135m"; // purple

                    break;
                case 7:
                    name = "chest";

                    collide = false;
                    destroyable = true;
                    renderChar = '□';
                    dropAmount = 1;
                    placeable = true;
                    interactive = true;
                    color = "\u001b[38;5;130m"; // brown

                    break;
                case 8:
                    name = "silver ore";

                    collide = true;
                    destroyable = true;
                    renderChar = '▓';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[38;5;252m"; // silver

                    break;
                case 9:
                    name = "gold ore";

                    collide = true;
                    destroyable = true;
                    renderChar = '▓';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[38;5;184m"; // gold

                    break;
                case 10:
                    name = "damanite ore";

                    collide = true;
                    destroyable = true;
                    renderChar = '▓';
                    dropAmount = 1;
                    placeable = true;
                    color = "\u001b[38;5;199m"; // pink

                    break;
                case 11:
                    name = "table";

                    collide = false;
                    destroyable = true;
                    renderChar = '◘';
                    dropAmount = 1;
                    placeable = true;
                    interactive = true;
                    color = "\u001b[38;5;130m"; // brown

                    break;
            }
        }
    }
}
