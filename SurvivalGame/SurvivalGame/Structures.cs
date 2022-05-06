namespace SurvivalGame
{
    class Structures
    {
        public string GenerateStructure(int id)
        {
            string structureStr = "";

            switch (id)
            {
                case 0:
                    structureStr = @"
 555 
 545 
55455
55455
  4  
  4   ";

                    break;
                case 1:
                    structureStr = @"
 55555 
 55455 
5554555
5554555
  545  
   4   
   4    ";

                    break;
                case 2:
                    structureStr = @"
  444  
 44444 
4444444
4000004
4000004
4000004
4444444 ";

                    break;
                case 3:
                    structureStr = @"
  60006  
 6000006 
600000006
600000006
600000006
 6000006 
  60006   ";

                    break;
                case 4:
                    structureStr = @"

         44  
        4444 
       400004
       400004
       400004
       400004
   4444400004
4444444400004
4000000400004
4000000400074
4444444444444 ";

                    break;
            }

            return structureStr;
        }
    }
}
