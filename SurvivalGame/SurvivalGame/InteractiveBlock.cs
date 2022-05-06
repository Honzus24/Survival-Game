namespace SurvivalGame
{
    class InteractiveBlock:Items
    {
        private Chest chest = null;
        private Table table = null;
        //public int saveSlots = 0;

        public InteractiveBlock(int Id):base(Id)
        {
            id = Id;

            switch (id)
            {
                case 7:
                    chest = new Chest();
                    //saveSlots = 18;
                    break;
                case 11:
                    table = new Table();
                    //saveSlots = 18;
                    break;
            }
        }

        public void InteractWBlock(Player player)
        {
            switch (id)
            {
                case 7:
                    chest.Interact(player);
                    break;
                case 11:
                    table.Interact(player);
                    break;
            }
        }

        public bool IsBlockInteractive(int idCheck)
        {
            bool isInteractive = false;

            if (idCheck == 7 || idCheck == 11)
            {
                isInteractive = true;
            }

            return isInteractive;
        }

        public string Save()
        {
            string output = "";

            switch (id)
            {
                case 7:
                    output = chest.WriteToSaveFile();
                    break;
                case 11:
                    output = table.WriteToSaveFile();
                    break;
            }

            return output;
        }

        public void Load(string data)
        {
            switch (id)
            {
                case 7:
                    chest.LoadDataToObject(data);
                    break;
                case 11:
                    table.LoadDataToObject(data);
                    break;
            }
        }
    }
}
