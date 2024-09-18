using System;

class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false; 
        int x = Console.WindowWidth / 2;
        int y = Console.WindowHeight / 2;
        ConsoleKeyInfo gombInfo;
        ConsoleColor[] szinek = new ConsoleColor[]
        {
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Blue,
            ConsoleColor.Yellow,
            ConsoleColor.Cyan,
            ConsoleColor.Magenta,
            ConsoleColor.White
        };
        int jelenlegiSzín = 0;

        while (true)
        {
            // A karakter színváltozása
            Console.ForegroundColor = szinek[jelenlegiSzín];

            // Egy "█" rajzolása a porzícióban
            Console.SetCursorPosition(x, y);
            Console.Write("█");

            // Gomb lenyomás 
            gombInfo = Console.ReadKey(true);

            // Mozgás
            switch (gombInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (y > 0) y--;
                    break;
                case ConsoleKey.DownArrow:
                    if (y < Console.WindowHeight - 1) y++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (x > 0) x--;
                    break;
                case ConsoleKey.RightArrow:
                    if (x < Console.WindowWidth - 1) x++;
                    break;
                case ConsoleKey.Escape:
                    return;
            }
            // Szinek asszociálása a számokhoz
            if (gombInfo.Key >= ConsoleKey.D1 && gombInfo.Key <= ConsoleKey.D7)
            {
                int választottSzám = gombInfo.Key - ConsoleKey.D1 + 1;

                for (int i = 1; i <= választottSzám; i++)
                {
                    jelenlegiSzín = (i - 1) % szinek.Length;
                }
            }

        }
    }
}
