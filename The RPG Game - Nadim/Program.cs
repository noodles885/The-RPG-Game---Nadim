using System;
using System.IO;

namespace The_RPG_Game
{
    internal class Program
    {
        static char[,] map;
        static int playerX, playerY;
        static int enemyX, enemyY;
        static int playerHealth = 100;
        static int enemyHealth = 100;

        static void Main()
        {
            InitializeGame();
            while (playerHealth > 0)
            {
                DrawMap();
                DisplayStats();
                ProcessInput();
                MoveEnemy();
                MovePlayer(0, 0);
                CheckCollisions();
            }

            Console.WriteLine("Game Over. You were defeated!");
            Console.ReadLine();
        }

        static void InitializeGame()
        {
            if (!ReadMapFromFile("Map.txt"))
            {
                GenerateMap(20, 20);
            }

            FindStartingPositions();
        }

        static bool ReadMapFromFile(string fileName)
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                int rows = lines.Length;
                int cols = lines[0].Length;

                map = new char[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        map[i, j] = ' ';
                    }
                }

                for (int i = 0; i < rows; i++)
                {
                    map[i, 0] = '¤';
                    map[i, cols - 1] = '¤';
                }

                for (int j = 0; j < cols; j++)
                {
                    map[0, j] = '¤';
                    map[rows - 1, j] = '¤';
                }

                Random random = new Random();
                for (int i = 0; i < rows * cols / 20; i++)
                {
                    int harmfulTileX, harmfulTileY;
                    do
                    {
                        harmfulTileX = random.Next(1, rows - 1);
                        harmfulTileY = random.Next(1, cols - 1);
                    } while (map[harmfulTileX, harmfulTileY] != ' ');

                    map[harmfulTileX, harmfulTileY] = 'H';
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File {fileName} not found. Generating a default map.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading map from file: {ex.Message}. Generating a default map.");
                return false;
            }
        }

        static void FindStartingPositions()
        {
            Random random = new Random();

            do
            {
                playerX = random.Next(1, map.GetLength(0) - 1);
                playerY = random.Next(1, map.GetLength(1) - 1);
            } while (map[playerX, playerY] != ' ');

            do
            {
                enemyX = random.Next(1, map.GetLength(0) - 1);
                enemyY = random.Next(1, map.GetLength(1) - 1);
            } while (map[enemyX, enemyY] != ' ');

            map[playerX, playerY] = 'P';
            map[enemyX, enemyY] = 'E';
        }

        static void GenerateMap(int rows, int cols)
        {
            map = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    map[i, j] = ' ';
                }
            }

            for (int i = 0; i < rows; i++)
            {
                map[i, 0] = '¤';               
                map[i, cols - 1] = '¤';        
            }

            for (int j = 0; j < cols; j++)
            {
                map[0, j] = '¤';               
                map[rows - 1, j] = '¤';        
            }

            Random random = new Random();
            for (int i = 0; i < rows * cols / 20; i++)
            {
                int harmfulTileX, harmfulTileY;
                do
                {
                    harmfulTileX = random.Next(1, rows - 1);
                    harmfulTileY = random.Next(1, cols - 1);
                } while (map[harmfulTileX, harmfulTileY] != ' ');

                map[harmfulTileX, harmfulTileY] = 'H';
            }
        }

        static void DrawMap()
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == playerX && j == playerY)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('P' + " ");
                        Console.ResetColor();
                    }
                    else if (i == enemyX && j == enemyY)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('E' + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        if (map[i, j] == 'H')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write('H' + " ");
                            Console.ResetColor();
                        }
                        else if (map[i, j] == '¤')
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('¤' + " ");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(map[i, j] + " ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        static void DisplayStats()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Player Health: {playerHealth}");
            Console.WriteLine($"Enemy Health: {enemyHealth}");
            Console.ResetColor();
        }

        static void ProcessInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    MovePlayer(-1, 0);
                    break;
                case ConsoleKey.A:
                    MovePlayer(0, -1);
                    break;
                case ConsoleKey.S:
                    MovePlayer(1, 0);
                    break;
                case ConsoleKey.D:
                    MovePlayer(0, 1);
                    break;
            }
        }

        static void MovePlayer(int deltaX, int deltaY)
        {
            int newPlayerX = playerX + deltaX;
            int newPlayerY = playerY + deltaY;

            if (map[newPlayerX, newPlayerY] != '¤')
            {
                if (map[newPlayerX, newPlayerY] == 'H')
                {
                    playerHealth -= 15;
                    map[newPlayerX, newPlayerY] = ' ';
                }

                map[playerX, playerY] = ' ';
                playerX = newPlayerX;
                playerY = newPlayerY;
                map[playerX, playerY] = 'P';
            }
        }

        static void MoveEnemy()
        {
            int direction = (new Random()).Next(4);

            int newEnemyX = enemyX;
            int newEnemyY = enemyY;

            switch (direction)
            {
                case 0:
                    newEnemyX--;
                    break;
                case 1:
                    newEnemyY--;
                    break;
                case 2:
                    newEnemyX++;
                    break;
                case 3:
                    newEnemyY++;
                    break;
            }

            if (newEnemyX >= 1 && newEnemyX < map.GetLength(0) - 1 && newEnemyY >= 1 && newEnemyY < map.GetLength(1) - 1)
            {
                if (map[newEnemyX, newEnemyY] != '¤')
                {
                    map[enemyX, enemyY] = ' ';
                    enemyX = newEnemyX;
                    enemyY = newEnemyY;
                    map[enemyX, enemyY] = 'E';
                }
            }

            if (playerX == enemyX && playerY == enemyY)
            {
                playerHealth -= 10;
            }
        }

        static void CheckCollisions()
        {
            
            if (playerX == enemyX && playerY == enemyY)
            {

                
                enemyHealth -= 20;

                
                if (playerHealth <= 0)
                {
                    Console.WriteLine("Game Over. You were defeated!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                else if (enemyHealth <= 0)
                {
                    Console.WriteLine("Enemy defeated! You win!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            else
            {
                
                if (map[playerX, playerY] == 'H')
                {
                    
                    playerHealth -= 15;
                    map[playerX, playerY] = ' '; 
                }

                
                if (playerHealth <= 0)
                {
                    Console.WriteLine("Game Over. You were defeated!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }
    }
}
