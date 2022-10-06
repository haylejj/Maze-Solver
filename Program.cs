using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;


namespace labirentcözme
{
    public enum Nesne
    {
        path = 0,
        wall = 1,
        bomb = 2
    }
    internal class Program
    {
        static void Main(string[] args)

        {
            
            Maze labirent=new Maze();
            labirent.MakeMaze();
            labirent.ReadMaze();
            labirent.Randbomb();
            labirent.MazeOutPut();
            labirent.FindEntries();
            bool exit = false;

            labirent.EntryFromAllEntries();
            while (!exit)
            {
                
                Console.Write("L: LABİRENT\n");
                Console.Write("B: BOMBA\n");
                Console.Write("X: ÇÖZÜMLER\n");
                Console.Write("Q: ÇIKIŞ\n");
                string satir = Console.ReadLine().ToUpper();
               
                if (satir=="L")
                {
                    labirent.MazeOutPut();
                }
                else if (satir == "X")
                {
                    labirent.AllSolutionsOutPut(true);
                }
                else if (satir=="Q")
                {
                    exit = true;
                }
                else if (satir == "B")
                {
                    labirent.BombaKoordinat();
                }
            }
            Console.ReadKey();
        }

           


        }
    }



