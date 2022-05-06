namespace SurvivalGame
{
    class Vector2
    {
        public float x;
        public float y;

        public Vector2(float x1, float y1)
        {
            x = x1;
            y = y1;
        }

        public string ToStringConvert()
        {
            string finalString = x + " - " + y;

            return finalString;
        }
    }
}
