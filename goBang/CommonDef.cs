namespace goBang
{
    partial class mainClass
    {   
        //Definitions
        private const short BOARD_WIDTH = 15;
        private const short BOUNDRY = 5;
        private const short BOARD_SIZE = 225; //? Width ^ 2
        private const short MAX_PATTERN_LEN = 15;
        private const short MAX_EST_DEEP = 10;
        private const int MAX_INT = (int)1e9;
        private const int MIN_INT = (int)-1e9;
        private const bool AI_PLAYER = false;
        private const bool USER = true;
        private const short BLACK = 0;
        private const short WHITE = 1;
        private const bool ATTACK = true;
        private const bool DEFEND = false;
        private const short NONE = 0xFF;

        private const short SEARCH_RANGE = 1;
        static private short[] ComDir = { -1, -1, -1, 0, -1, +1,
                            0, -1,         0, +1,
                           +1, -1, +1, 0, +1, +1 };

        private enum Score
        {
            AAAAA = 1000000,
            AAAAO = 50000,
            AAAAB = 10000,
            AAOAA = 10000, //? AAOAA AAAOA AOAAA
            AAAOO = 30000,
            AAABO = 3000,
            AOAAO = 5000, //? AOAAO AAOAO OAOAA OAAOA 
            AOAAB = 3000, //? AOAAB AAOAB
            AOOAA = 2000,
            AOAOA = 2000,
            AAOOO = 1000,
            AABOO = 100,
            AOAOO = 500,
            AOABO = 100,
            AOOAO = 200,
            AOOAB = 40,
            AOOOA = 40,
            AOOOO = 10,
            ABOOO = 2
        }

        static void memset(int[] array, int value, int len)
        {
            for (int i = 0; i < len; i++)
                array[i] = value;
        }
    }    
}
