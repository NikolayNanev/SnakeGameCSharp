namespace SnakeGame
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    class ProgramMain
    {
        //Console Window constants
        public const int Window_Width = 100;
        public const int Window_Height = 40;

        //Starting lenght of snake        
        public const int initialSnakeLength = 5;

        //Starting level
        public static int gameLevel = 1;
        
        //Speed constants
        public const int initialSnakeSpeed = 300;
        public const int maxSnakeSpeed = 50;
        public const int speedFactor = 1;

        //Starting heading of snake
        public const Direction initialDirection = Direction.East;

        //Random number generator 
        public static Random randNumGener = new Random();

        //String formats
        public static string curentLevel = String.Format("Level:{0}", gameLevel);
        public static string foodSymbol = "@";
        public static string snakeSymbol = "*";
        public static string spaceSymbol = " ";

        static void Main()
        {

            #region InitializeConsoleWindowSize

            Console.SetWindowSize(Window_Width, Window_Height);
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            #endregion

            #region InitializeGameElements

            int initialRowOfSnake = Window_Height / 2;
            int initialColoumOfSnake = Window_Width / 2;

            Direction currentDirection = initialDirection;
            int curentSnakeSpeed = initialSnakeSpeed;
            
            Queue<Position> snake = new Queue<Position>();

            for (int i = 0; i < initialSnakeLength; i++)
            {
                snake.Enqueue(new Position(initialColoumOfSnake + i, initialRowOfSnake));
            }

            Position snakeFood = GenerateFood();
            DrawSingleElement(snakeFood, foodSymbol);

            DrawBorderElement();

            Position snakeHead;
            Position snakeTail;
            Position headingOfSnake;
            Position newSnakeHead;

            
            #endregion

            #region Directions

            Position[] directions = new Position[]
            {
                new Position(-1, -1), //NorthWest
                new Position(0, -1), // North
                new Position(1, -1), //NorthtEast
                new Position(-1, 0), //West
                new Position(1, 0), //East
                new Position(-1, 1), //SouthWest
                new Position(0, 1), //South
                new Position(1, 1) //SouthEast
            };

            #endregion

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    currentDirection = ReadKeyFromKeyboard(currentDirection);
                    DrawSingleElement(new Position(1 + curentLevel.Length, 1), spaceSymbol);
                }

                snakeHead = snake.Last();
                headingOfSnake = directions[(int)currentDirection];

                newSnakeHead = new Position(
                    snakeHead.Column + headingOfSnake.Column, 
                    snakeHead.Row + headingOfSnake.Row
                );

                #region GameOverLogic

                if (newSnakeHead.Column < 1 || 
                    newSnakeHead.Row < 1 || 
                    newSnakeHead.Column >= Console.WindowWidth - 1 || 
                    newSnakeHead.Row >= Console.WindowHeight - 1 || 
                    snake.Contains(newSnakeHead))
                {
                    Console.Clear();

                    DrawSingleElement(new Position(initialColoumOfSnake - 3, initialRowOfSnake - 3),
                        "Game Over!");
                    DrawSingleElement(new Position(initialColoumOfSnake - 5, initialRowOfSnake - 1),
                        String.Format("Level reached: {0}", gameLevel));
                    DrawSingleElement(new Position(initialColoumOfSnake - 7, initialRowOfSnake),
                        "(Press Esc to exit)");

                    ConsoleKeyInfo exitKey = Console.ReadKey();

                    while (exitKey.Key != ConsoleKey.Escape)
                    {
                        exitKey = Console.ReadKey();
                    }
                    return;
                }

                #endregion

                #region SnakeGrowingLogic

                if (newSnakeHead == snakeFood)
                {
                    snakeFood = GenerateFood();
                    DrawSingleElement(snakeFood, foodSymbol);

                    if (curentSnakeSpeed > maxSnakeSpeed)
                    {
                        curentSnakeSpeed -= speedFactor;
                        gameLevel++;
                    }                   
                }
                
                else
                {
                    snakeTail = snake.Dequeue();
                    DrawSingleElement(snakeTail, spaceSymbol);

                }

                snake.Enqueue(newSnakeHead);

                #endregion

                DrawSnakeElement(snake);
                curentLevel = String.Format("Level:{0}", gameLevel);
                DrawSingleElement(new Position(1, 1), curentLevel);
                
                Thread.Sleep(curentSnakeSpeed);                            
            }                
        }

        private static void DrawBorderElement()
        {
            for (int i = 1; i < Window_Width - 1; i++)
            {
                DrawSingleElement(new Position(i, 0), "-");
            }

            for (int i = 1; i < Window_Height - 1; i++)
            {
                DrawSingleElement(new Position(0, i), "|");
            }

            for (int i = 1; i < Window_Height - 1; i++)
            {
                DrawSingleElement(new Position(Window_Width - 1, i), "|");
            }

            for (int i = 1; i < Window_Width - 1; i++)
            {
                DrawSingleElement(new Position(i, Window_Height - 1), "-");
            }
        }

        private static Position GenerateFood()
        {
            return new Position(randNumGener.Next(1, Console.WindowWidth -1), randNumGener.Next(1, Console.WindowHeight -1));
        }

        private static Direction ReadKeyFromKeyboard(Direction currentDirection)
        {
            ConsoleKeyInfo inputKey = Console.ReadKey();

            switch (inputKey.Key)
            {
                case ConsoleKey.NumPad7:
                    if (currentDirection != Direction.SouthEast){
                        currentDirection = Direction.NorthWest;  
                    }
                    break;
                case ConsoleKey.NumPad8:
                    if (currentDirection != Direction.South){
                        currentDirection = Direction.North;
                    }
                    break;
                case ConsoleKey.NumPad9:
                    if (currentDirection != Direction.SouthWest){
                       currentDirection = Direction.NorthEast; 
                    } 
                    break;
                case ConsoleKey.NumPad4:
                    if (currentDirection != Direction.East){
		                 currentDirection = Direction.West;
	                } 
                    break;
                case ConsoleKey.NumPad6:
                    if (currentDirection != Direction.West){
                        currentDirection = Direction.East;
                    }
                    break;
                case ConsoleKey.NumPad1:
                    if (currentDirection != Direction.NorthEast){
                        currentDirection = Direction.SouthWest;
                    }
                    break;
                case ConsoleKey.NumPad2:
                    if (currentDirection != Direction.North){
                        currentDirection = Direction.South;
                    }
                    break;
                case ConsoleKey.NumPad3:
                    if (currentDirection != Direction.NorthWest){
                        currentDirection = Direction.SouthEast;
                    }
                    break;
                default:
                    break;
            }
            return currentDirection;
        }

        private static void DrawSnakeElement(Queue<Position> inputQueue)
        {
            foreach (Position curentElementPosition in inputQueue)
            {
                DrawSingleElement(curentElementPosition, "*");
            }
        }

        private static void DrawSingleElement(Position positionOfElement, string str)
        {
            Console.SetCursorPosition(positionOfElement.Column, positionOfElement.Row);
            Console.Write(str);
        }
    }
}
