using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_RPG_Game___Nadim
{
    internal class Program
    {
        static char[,] map;
        static int playerX, playerY;
        static int enemyX, enemyY;
        static int playerHealth = 100;
        static int enemyHealth = 50;

        static void Main()
        {
            InitializeGame();
            while (playerHealth > 0)
            {
                DrawMap();
                DisplayStats();
                ProcessInput();
                MoveEnemy();
                CheckCollisions();
            }

            Console.WriteLine("Game Over. You were defeated!");
            Console.ReadLine();
        }

        static void InitializeGame()
        {
            ReadMapFromFile("Map.txt");
            FindStartingPositions();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == 0 || j == 0 || i == map.GetLength(0) - 1 || j == map.GetLength(1) - 1)
                    {
                        map[i, j] = '#';
                    }
                }
            }

            // Place the player and enemy on the map
            map[playerX, playerY] = 'P';
            map[enemyX, enemyY] = 'E';
        }

        static void ReadMapFromFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int rows = lines.Length;
            int cols = lines[0].Length;

            map = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    map[i, j] = lines[i][j];
                }
            }
        }

        static void FindStartingPositions()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 'P')
                    {
                        playerX = i;
                        playerY = j;
                    }
                    else if (map[i, j] == 'E')
                    {
                        enemyX = i;
                        enemyY = j;
                    }
                }
            }
        }

        static void DrawMap()
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void DisplayStats()
        {
            Console.WriteLine($"Player Health: {playerHealth}");
            Console.WriteLine($"Enemy Health: {enemyHealth}");
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

            if (map[newPlayerX, newPlayerY] != '#')
            {
                map[playerX, playerY] = ' ';
                playerX = newPlayerX;
                playerY = newPlayerY;
                map[playerX, playerY] = 'P';
            }
        }

        static void MoveEnemy()
        {
            int direction = (new Random()).Next(4); // 0: Up, 1: Left, 2: Down, 3: Right

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

            if (map[newEnemyX, newEnemyY] != '#')
            {
                map[enemyX, enemyY] = ' ';
                enemyX = newEnemyX;
                enemyY = newEnemyY;
                map[enemyX, enemyY] = 'E';
            }
        }

        static void CheckCollisions()
        {
            // Check if the player and enemy are on the same position
            if (playerX == enemyX && playerY == enemyY)
            {
                // Player attacks the enemy
                enemyHealth -= 10;

                // Enemy attacks the player
                playerHealth -= 20;

                // Check for player and enemy death
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
        }
    }
}