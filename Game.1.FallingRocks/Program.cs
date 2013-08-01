//* Implement the "Falling Rocks" game in the text console. 
//A small dwarf stays at the bottom of the screen and can move left and
//right (by the arrows keys). A number of rocks of different sizes and 
//forms constantly fall down and you need to avoid a crash.
//Rocks are the symbols ^, @, *, &, +, %, $, #, !, ., ;, - 
//distributed with appropriate density. The dwarf is (O). 
//Ensure a constant game speed by Thread.Sleep(150).
//Implement collision detection and scoring system.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

//Here is the main structure for program to see defined arguments everywhere

struct Object
{
    public int x1;
    public int x2;
    public int x3;
    public int y;
    public string str;
    public char firstSymbol;
    public char c;
    public char thirdSymbol;
    public ConsoleColor color;
}

//Structure ends here
class GameFallRocks
{
    static void PrintOnPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }
    static void PrintStringOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }




    static void Main()
    {
        // This is the dwarf
        int playfieldWidht = 60;//Console playing field
        int score = 0;
        int livesCount = 5;
        int rockWidth;
        char randomRockSymbol;
        //The size of the console
        Console.BufferHeight = Console.WindowHeight = 30;
        Console.BufferWidth = Console.WindowWidth = 80;
        Object dwarf = new Object();
        dwarf.x1 = 29;
        dwarf.x2 = 30;
        dwarf.x3 = 31;
        dwarf.y = Console.WindowHeight - 1;
        dwarf.firstSymbol = '(';
        dwarf.c = 'O';
        dwarf.thirdSymbol = ')';
        dwarf.color = ConsoleColor.White;
        Random randomGenerator = new Random();
        List<Object> rocks = new List<Object>();



        while (true)
        {
            bool hitted = false;
            {
                //The falling symbols we use in the game for rocks
                char[] c = new char[11] { '^', '@', '*', '&', '+', '%', '$', '#', '!', '.', ';' };
                int all = randomGenerator.Next(0, c.Length);
                int chance = randomGenerator.Next(0, 100);




                if (chance < 2)//Bonus - doubles the lives
                {
                    Object newRock = new Object();
                    newRock.color = ConsoleColor.Yellow;
                    newRock.c = '?';
                    newRock.x1 = randomGenerator.Next(0, playfieldWidht);
                    newRock.y = 0;
                    rocks.Add(newRock);
                }
                else if (chance < 20)//Bonus + 1 live
                {
                    Object newRock = new Object();
                    newRock.color = ConsoleColor.Cyan;
                    newRock.c = 'B';
                    newRock.x1 = randomGenerator.Next(0, playfieldWidht);
                    newRock.y = 0;
                    rocks.Add(newRock);

                }
                else
                {
                    //Here we create random row of falling rocks
                    rockWidth = randomGenerator.Next(1, 5);
                    randomRockSymbol = c[randomGenerator.Next(c.Length)];
                    Object newRock = new Object();
                    newRock.color = (ConsoleColor)randomGenerator.Next(5, 15);
                    newRock.x1 = randomGenerator.Next(0, playfieldWidht);
                    newRock.y = 0;
                    newRock.c = randomRockSymbol;
                    for (int i = 0; i < rockWidth; i++)//and multiple rocks
                    {
                        rocks.Add(newRock);
                        if (newRock.x1 < playfieldWidht - 1)
                        {
                            newRock.x1++;
                        }
                    }


                }
            }

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedkey = Console.ReadKey(true);
                //Moving dwarf left 
                if (pressedkey.Key == ConsoleKey.LeftArrow)
                {
                    if (dwarf.x1 - 1 >= 0)
                    {
                        dwarf.x1 = dwarf.x1 - 1;
                        dwarf.x2 = dwarf.x2 - 1;
                        dwarf.x3 = dwarf.x3 - 1;
                    }
                }
                //Moving dwarf right
                else if (pressedkey.Key == ConsoleKey.RightArrow)
                {
                    if (dwarf.x1 - 1 < playfieldWidht)
                    {
                        dwarf.x1 = dwarf.x1 + 1;
                        dwarf.x2 = dwarf.x2 + 1;
                        dwarf.x3 = dwarf.x3 + 1;
                    }
                }
            }
            List<Object> newList = new List<Object>();

            for (int i = 0; i < rocks.Count; i++)
            {
                //Replacing the old rocks with new for new row
                Object oldRock = rocks[i];
                Object newRock = new Object();
                newRock.x1 = oldRock.x1;
                newRock.y = oldRock.y + 1;
                newRock.c = oldRock.c;
                newRock.color = oldRock.color;
                //The bonus collision hit check
                if (newRock.c == 'B' && newRock.y == dwarf.y && newRock.x1 == dwarf.x1)
                {
                    livesCount++;
                }
                if (newRock.c == '?' && newRock.y == dwarf.y && newRock.x1 == dwarf.x1)
                {
                    livesCount = (livesCount * 2);
                }
                //We check when the hit is true and dwarf loose live
                if (newRock.c != '~' && newRock.c != 'B' && newRock.c != '?' && newRock.y == dwarf.y && newRock.x1 == dwarf.x1)
                {
                    livesCount--;
                    hitted = true;
                    if (livesCount <= 0)
                    {
                        //If lives hit 0 we stop the game
                        PrintStringOnPosition(25, 10, "GAME OVER!!!", ConsoleColor.Red);
                        PrintStringOnPosition(25, 12, "Press [enter] to exit", ConsoleColor.Red);

                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                //We check coordinates of y to add rocks and get score 
                if (newRock.y < Console.WindowHeight)
                {
                    newList.Add(newRock);
                }
                else
                {
                    score++;
                }
            }

            rocks = newList;
            Console.Clear();

            if (hitted)
            {
                //We get a"red "X" when collision happened
                rocks.Clear();
                PrintOnPosition(dwarf.x1, dwarf.y, 'X', ConsoleColor.Red);
            }
            else
            {
                PrintOnPosition(dwarf.x1, dwarf.y, dwarf.firstSymbol, dwarf.color);
                PrintOnPosition(dwarf.x2, dwarf.y, dwarf.c, dwarf.color);
                PrintOnPosition(dwarf.x3, dwarf.y, dwarf.thirdSymbol, dwarf.color);
            }
            foreach (Object rock in rocks)
            {
                PrintOnPosition(rock.x1, rock.y, rock.c, rock.color);

            }
            //The writting table possition for results in the right of the console
            PrintStringOnPosition(66, 5, "Lives: " + livesCount, ConsoleColor.White);
            PrintStringOnPosition(66, 10, "Score: " + score, ConsoleColor.White);

            //We have some sound (hertz value, we use a less louder frequence)
            Console.Beep(223, 35);
            //The speed of the game as in condition
            Thread.Sleep(150);
            //There are still some things to fix for smooth movement and correct 
            //collision/bonus taking because of dwarf size structure, 
            //I'm sorry, will be fixed in the next version :)
            //There is a hidden BONUS column at corner right where your
            //dwarf can rest if things get crowded. Enjoy and thank you
            //for playing :)!
        }
    }
}




