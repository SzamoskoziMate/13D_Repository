namespace ConsoleApp1
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;

    class Program
    {
        static string currentFileName = null;
        static bool IsRunningDrawing = true;
        static List<ConsoleCharacter> savedContent1 = new List<ConsoleCharacter>();
        /*
        static void SaveToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var item in savedContent1)
                {
                    string line = $"{item.X},{item.Y},{(int)item.Szín},{item.Karakter}";
                    writer.WriteLine(line);
                }
            }
            Console.SetCursorPosition(1, 1);
            Console.WriteLine($"Content saved to {fileName}");
        }
        static void ListAndDeleteFiles()
        {
            string[] files = Directory.GetFiles(".", "*.txt");
            if (files.Length == 0)
            {
                Console.SetCursorPosition(1, 1);
                Console.WriteLine("No files found.");
                return;
            }

            int selectedFileIndex = 0;
            DrawFileButtons(files, selectedFileIndex);
            Console.SetCursorPosition(1, 1);
            Console.Write("Press double ESC to leave");
            Console.SetCursorPosition(1, 2);
            Console.Write("Press double Enter to choose the file");
            bool selecting = true;
            while (selecting)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedFileIndex--;
                        if (selectedFileIndex < 0) selectedFileIndex = files.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedFileIndex++;
                        if (selectedFileIndex >= files.Length) selectedFileIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        File.Delete(files[selectedFileIndex]);
                        Console.SetCursorPosition(1, 3);
                        Console.WriteLine($"File {files[selectedFileIndex]} has been deleted.");
                        selecting = false;
                        break;
                    case ConsoleKey.Escape:
                        selecting = false;
                        break;
                }
                DrawFileButtons(files, selectedFileIndex);
            }
        }
        
        static void ListAndSelectFilesForEdit()
        {
            string[] files = Directory.GetFiles(".", "*.txt");
            if (files.Length == 0)
            {
                Console.SetCursorPosition(1, 1);
                Console.WriteLine("No drawings found.");
                return;
            }

            int selectedFileIndex = 0;
            DrawFileButtons(files, selectedFileIndex);
            Console.SetCursorPosition(1, 1);
            Console.Write("Press double ESC to leave");
            Console.SetCursorPosition(1, 2);
            Console.Write("Press double Enter to choose the file");
            bool selecting = true;
            while (selecting)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedFileIndex--;
                        if (selectedFileIndex < 0) selectedFileIndex = files.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedFileIndex++;
                        if (selectedFileIndex >= files.Length) selectedFileIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        LoadDrawing(files[selectedFileIndex]);
                        selecting = false;
                        IsRunningDrawing = true;
                        Drawing();
                        break;
                    case ConsoleKey.Escape:
                        selecting = false;
                        break;
                }
                DrawFileButtons(files, selectedFileIndex);
            }
        }
        static void DrawFileButtons(string[] files, int selectedFileIndex)
        {

            DrawEdges();
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileName(files[i]);
                Console.SetCursorPosition((Console.WindowWidth - fileName.Length) / 2, i + 5);
                if (i == selectedFileIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(fileName);
                Console.ResetColor();
            }
        }
        static void LoadFromFile(string fileName)
        {
            savedContent1.Clear();
            currentFileName = fileName;
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            int x = int.Parse(parts[0]);
                            int y = int.Parse(parts[1]);
                            ConsoleColor color = (ConsoleColor)int.Parse(parts[2]);
                            char character = parts[3][0];
                            savedContent1.Add(new ConsoleCharacter
                            {
                                X = x,
                                Y = y,
                                Szín = color,
                                Karakter = character
                            });
                        }
                    }
                }
                LoadContent();
                Drawing();
                Console.CursorVisible = true;
            }
            else
            {
                Console.SetCursorPosition(1, 1);
                Console.WriteLine($"File {fileName} not found. Press ESC to leave");
            }
        }
        */
        static string PromptFileName()
        {
            Console.SetCursorPosition(1, 1);
            Console.Write("Enter a file name: ");
            string fileName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "default.txt";
            }


            if (!fileName.EndsWith(".txt"))
            {
                fileName += ".txt";
            }

            return fileName;
        }
        
        static List<Drawing> GetDrawings()
        {
            using (var context = new DrawingDbContext())
            {
                return context.Drawings.ToList();
            }
        }
        static void ListAndSelectDrawings()
        {
            var drawings = GetDrawings();

            if (drawings.Count == 0)
            {
                Console.WriteLine("Nincsenek mentett rajzok.");
                return;
            }

            int selectedIndex = 0;
            bool selecting = true;

            while (selecting)
            {
                Console.Clear();
                Console.WriteLine("Válaszd ki a rajzot (Enter betöltéshez, ESC kilépéshez):");

                for (int i = 0; i < drawings.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine($"{i + 1}. {drawings[i].Name}");
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : drawings.Count - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex < drawings.Count - 1) ? selectedIndex + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        LoadDrawing(drawings[selectedIndex].Id);
                        selecting = false;
                        break;
                    case ConsoleKey.Escape:
                        selecting = false;
                        break;
                }
            }
        }

        static void SaveDrawing(string drawingName)
        {
            using (var context = new DrawingDbContext())
            {
                var newDrawing = new Drawing
                {
                    Name = drawingName,
                    ConsoleCharacters = savedContent1.Select(c => new ConsoleCharacter
                    {
                        Character = c.Character,
                        X = c.X,
                        Y = c.Y,
                        Szín = (ConsoleColor)Enum.ToObject(typeof(ConsoleColor), c.Szín);
                    }).ToList()
                }; 

                context.Drawings.Add(newDrawing);
                context.SaveChanges();
                Console.SetCursorPosition(1, 1);
                Console.WriteLine($"Drawing '{drawingName}' saved to database.");
            }
        }


        static void LoadDrawing(int drawingId)
        {
            using (var context = new DrawingDbContext())
            {
                var drawing = context.Drawings
                    .Include(d => d.ConsoleCharacters)
                    .FirstOrDefault(d => d.Id == drawingId);

                if (drawing == null)
                {
                    Console.WriteLine("Nem található a rajz.");
                    return;
                }

                Console.Clear();
                Console.WriteLine($"Rajz betöltve: {drawing.Name}");

                foreach (var character in drawing.ConsoleCharacters)
                {
                    Console.SetCursorPosition(character.X, character.Y);
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), character.Szín);
                    Console.Write(character.Character);
                }

                Console.ResetColor();
                Drawing(); 
            }
        }
        static void ListAndDeleteDrawings()
        {
            var drawings = GetDrawings();

            if (drawings.Count == 0)
            {
                Console.WriteLine("Nincsenek mentett rajzok.");
                return;
            }

            int selectedIndex = 0;
            bool deleting = true;

            while (deleting)
            {
                Console.Clear();
                Console.WriteLine("Válaszd ki a törlendő rajzot (Enter törléshez, ESC kilépéshez):");

                for (int i = 0; i < drawings.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine($"{i + 1}. {drawings[i].Name}");
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : drawings.Count - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex < drawings.Count - 1) ? selectedIndex + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        DeleteDrawing(drawings[selectedIndex].Id);
                        deleting = false;
                        break;
                    case ConsoleKey.Escape:
                        deleting = false;
                        break;
                }
            }
        }

        static void DeleteDrawing(int drawingId)
        {
            using (var context = new DrawingDbContext())
            {
                var drawing = context.Drawings
                    .Include(d => d.ConsoleCharacters)
                    .FirstOrDefault(d => d.Id == drawingId);

                if (drawing == null)
                {
                    Console.WriteLine("Nem található a rajz.");
                    return;
                }

                context.Drawings.Remove(drawing);
                context.SaveChanges();

                Console.WriteLine("A rajz sikeresen törölve.");
            }
        }

        static void ButtonNavigation()
        {
            ConsoleKeyInfo gombInfo;
            int választottGomb = 0;
            string[] gombok = { "Új szinezés", "Mentés", "Betöltés", "Törlés", " Kilépés" };
            int gombSzélesség = 14;
            int gombMagasság = 3;

            Console.CursorVisible = false;
            int consoleSzélesség = Console.WindowWidth;
            int consoleMagasság = Console.WindowHeight;

            int gombKezdésY = (consoleMagasság - (gombMagasság + 1) * gombok.Length) / 2;
            int centerX = (consoleSzélesség - (gombSzélesség + 2)) / 2;

            DrawButtons(gombok, választottGomb, gombSzélesség, gombMagasság, gombKezdésY, centerX);

            bool isRunning = true;
            while (isRunning)
            {
                gombInfo = Console.ReadKey(true);

                int előzőGomb = választottGomb;
                switch (gombInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        választottGomb--;
                        if (választottGomb < 0)
                        {
                            választottGomb = gombok.Length - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        választottGomb++;
                        if (választottGomb >= gombok.Length)
                        {
                            választottGomb = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (választottGomb == 0)
                        {
                            Console.Clear();
                            DrawEdges();
                            ClearButtons(gombok.Length, gombSzélesség, gombMagasság, gombKezdésY, centerX);
                            isRunning = false;
                            IsRunningDrawing = true;
                            savedContent1.Clear();
                            Drawing();
                        }
                        else if (választottGomb == 1)
                        {
                            if (savedContent1.Count == 0)
                            {
                                Console.SetCursorPosition(1, 1);
                                Console.WriteLine("There is nothing to save.");
                            }
                            else
                            {
                                if (currentFileName != null)
                                {
                                    SaveDrawing(currentFileName);
                                    Console.SetCursorPosition(1, 1);
                                    Console.WriteLine($"Changes saved to {currentFileName}");
                                }
                                else
                                {
                                    string fileName = PromptFileName();
                                    SaveDrawing(fileName);
                                }
                            }
                        }
                        else if (választottGomb == 2)
                        {
                            ClearButtons(gombok.Length, gombSzélesség, gombMagasság, gombKezdésY, centerX);
                            ListAndSelectDrawings();
                        }
                        else if (választottGomb == 3)
                        {
                            ClearButtons(gombok.Length, gombSzélesség, gombMagasság, gombKezdésY, centerX);
                            string fileName = PromptFileName();
                            ListAndDeleteDrawings();
                        }
                        else if (választottGomb == 4)
                        {
                            Environment.Exit(0);
                        }
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        DrawEdges();
                        ButtonNavigation();
                        break;
                }

                if (előzőGomb != választottGomb)
                {
                    DrawButtons(gombok, választottGomb, gombSzélesség, gombMagasság, gombKezdésY, centerX);
                }
            }

            Console.SetCursorPosition(0, gombKezdésY + gombok.Length * (gombMagasság + 1));
            Console.CursorVisible = true;
        }
        static void DrawEdges()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            Console.SetCursorPosition(Console.WindowWidth - 1, 0);
            Console.Write("╗");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("╝");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("╚");

            int keretX = Console.WindowWidth - 2;
            int keretY = Console.WindowHeight - 2;
            Console.SetCursorPosition(keretX, keretY);

            for (int i = 0; i < Console.WindowWidth - 2; i++)
            {
                Console.SetCursorPosition(i + 1, 0);
                Console.Write("═");

            }
            for (int i = 0; i < Console.WindowWidth - 2; i++)
            {
                Console.SetCursorPosition(i + 1, Console.WindowHeight - 1);
                Console.Write("═");

            }

            for (int j = 1; j < Console.WindowHeight - 1; j++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, j);
                Console.Write("║");

            }

            for (int j = 1; j < Console.WindowHeight - 1; j++)
            {
                Console.SetCursorPosition(0, j);
                Console.Write("║");

            }
        }
        static void DrawButtons(string[] gombok, int választottGomb, int gombSzélesség, int gombMagasság, int startY, int centerX)
        {
            for (int i = 0; i < gombok.Length; i++)
            {
                DrawButton(gombok[i], i == választottGomb, startY + i * (gombMagasság + 1), gombSzélesség, centerX);
            }
        }
        static void DrawButton(string text, bool isSelected, int topOffset, int gombSzélesség, int centerX)
        {
            Console.SetCursorPosition(centerX, topOffset);
            if (isSelected)
            {
                Console.Write('█' + new string('█', gombSzélesség) + '█');
            }
            else
            {
                Console.Write('┌' + new string('─', gombSzélesség) + '┐');
            }
            Console.SetCursorPosition(centerX, topOffset + 1);
            if (isSelected)
            {
                Console.Write('█' + CenterText(text, gombSzélesség) + '█');
            }
            else
            {
                Console.Write('│' + CenterText(text, gombSzélesség) + '│');
            }
            Console.SetCursorPosition(centerX, topOffset + 2);
            if (isSelected)
            {
                Console.Write('█' + new string('█', gombSzélesség) + '█');
            }
            else
            {
                Console.Write('└' + new string('─', gombSzélesség) + '┘');
            }
        }
        
        class ConsoleCharacter
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Character { get; set; }
            public ConsoleColor Szín { get; set; }
        }
        
        static void SaveCharacter(int x, int y, string character, ConsoleColor color)
        {

            savedContent1.Add(new ConsoleCharacter
            {
                X = x,
                Y = y,
                Character = character[0],
                Szín = color 
            });

        }
        static void LoadContent()
        {
            Console.Clear();
            DrawEdges();

            foreach (var item in savedContent1)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.ForegroundColor = item.Szín;
                Console.Write(item.Character);
            }
        }
        static void ClearButtons(int gombokSzáma, int gombSzélesség, int gombMagasság, int startY, int centerX)
        {
            for (int i = 0; i < gombokSzáma; i++)
            {

                for (int j = 0; j < gombMagasság; j++)
                {
                    Console.SetCursorPosition(centerX, startY + i * (gombMagasság + 1) + j);
                    Console.Write(new string(' ', gombSzélesség + 2));
                }
            }
        }
        static string CenterText(string text, int szélesség)
        {
            int padding = (szélesség - text.Length) / 2;
            return new string(' ', padding) + text + new string(' ', szélesség - padding - text.Length);
        }
        static void Drawing()
        {
            int x = Console.WindowWidth / 2;
            int y = Console.WindowHeight / 2;
            ConsoleKeyInfo gombInfo;
            string jelenlegiKarakter = "█";

            while (IsRunningDrawing)
            {

                Console.SetCursorPosition(Console.WindowLeft + 2, 1);
                Console.WriteLine($"A karaktered: {jelenlegiKarakter}");
                Console.SetCursorPosition(Console.WindowLeft + 2, 3);
                Console.Write("A karaktereid szinesen: █,▓,▒,░ ");
                Console.SetCursorPosition(x, y);
                Console.CursorVisible = true;


                gombInfo = Console.ReadKey(true);


                switch (gombInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (y > 0)
                        {
                            y--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (y < Console.WindowHeight - 1)
                        {
                            y++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x > 0)
                        {
                            x--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (x < Console.WindowWidth - 1)
                        {
                            x++;
                        }
                        break;
                    case ConsoleKey.D1:
                        Console.ForegroundColor = ConsoleColor.Red;

                        break;
                    case ConsoleKey.D2:
                        Console.ForegroundColor = ConsoleColor.Green;

                        break;
                    case ConsoleKey.D3:
                        Console.ForegroundColor = ConsoleColor.Blue;

                        break;
                    case ConsoleKey.D4:
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        break;
                    case ConsoleKey.D5:
                        Console.ForegroundColor = ConsoleColor.Cyan;

                        break;
                    case ConsoleKey.D6:
                        Console.ForegroundColor = ConsoleColor.Magenta;

                        break;
                    case ConsoleKey.D7:
                        Console.ForegroundColor = ConsoleColor.White;

                        break;
                    case ConsoleKey.D8:
                        jelenlegiKarakter = "▓";
                        break;
                    case ConsoleKey.D9:
                        jelenlegiKarakter = "▒";
                        break;
                    case ConsoleKey.D0:
                        jelenlegiKarakter = "░";
                        break;
                    case ConsoleKey.D:
                        jelenlegiKarakter = "█";
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Write(jelenlegiKarakter);
                        SaveCharacter(x, y, jelenlegiKarakter, Console.ForegroundColor);
                        break;
                    case ConsoleKey.Escape:
                        IsRunningDrawing = false;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        DrawEdges();
                        ButtonNavigation();
                        break;
                }
            }
        }


        static void Main(string[] args)
        {

            DrawEdges();

            ButtonNavigation();




        }
    }

}
