using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace labirentcözme
{
    internal class Maze
    {
        string file="LabYol0Test.txt";
        int[,] maze_= new int[30,30];
        string[,] make_maze = new string[30, 33];
        int bomb_ = 3;
        bool[,] steps= new bool[30,30];
        public int size=30; 
        List<Koordinatlar> entries= new List<Koordinatlar>();
        List<List<Koordinatlar>> solutions= new List<List<Koordinatlar>>();
        char wall = '1'; // duvarlar yani 1 leri temsil ediyor
        char path = '0'; // yolları yani 0 ları temsil ediyor
        char bomb = 'B'; // bomba


        public void MakeMaze()
        {
            string[,] make_maze = new string[30, 33];
            Random r = new Random();
            using (var wr = new StreamWriter(file,true))
            {
                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (j == 0)
                        {
                            wr.Write(make_maze[i, j] = "{");
                        }
                        else if (j == 31)
                        {
                            wr.Write(make_maze[i, j] = "}");
                        }
                        else
                        {
                            make_maze[i, j] = r.Next(0, 2).ToString();
                            wr.Write(make_maze[i, j] + ",");
                        }

                    }
                    wr.WriteLine();

                }
                wr.Flush();
                wr.Close();
            }
        }
        public void ReadMaze()
        {

            String textkonum = file;

            StreamReader streamReader = new StreamReader(textkonum);
            string lab = streamReader.ReadToEnd();
            //Console.WriteLine(lab);
            char[] chars = { ',', '{', '}', ' ', '\r', '\n' };
            string[] lab2 = lab.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            //foreach (var item in lab2)
            //{
            //    Console.Write(item);
            //}
            //Console.WriteLine(lab2.Length);
            int k = 0;
            string[,] lab3 = new string[30, 30];
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    maze_[i, j] = int.Parse(lab2[k]);
                    k++;
                }
            }

        }
        public void MazeOutPut()
        {
            Console.WriteLine("{0}: DUVAR\n{1}:YOL\n{2}:BOMBA\n\n", wall, path, bomb);
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    char charr = maze_[i, j] == (int)Nesne.path ? path : wall;
                    if (maze_[i, j] == (int)Nesne.bomb) charr = bomb;
                    Console.Write(charr);
                }
                Console.WriteLine();
            }
        }

        public void Randbomb()
        {

            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                Koordinatlar kordinat = new Koordinatlar();
                kordinat.row = r.Next(0, size);
                kordinat.column = r.Next(0, size);
                if (Pathcont(kordinat) && Bordercont(kordinat))
                {
                    maze_[kordinat.row, kordinat.column] = (int)Nesne.bomb;
                }
                else i--;

            }
        }

        // kontroller yapılacak ona göre çözüm bulunacak
        public bool Pathcont(Koordinatlar koordinat)
        {
            return maze_[koordinat.row, koordinat.column] == (int)Nesne.path;
        }
        public bool Bordercont(Koordinatlar koordinat)
        {
            return koordinat.row >= 0 && koordinat.row < 30 && koordinat.column >= 0 && koordinat.column < 30;
        }
        public bool Bombcont(Koordinatlar koordinat)
        {
            return maze_[koordinat.row, koordinat.column] == (int)Nesne.bomb;
        }
        public bool Wallcont(Koordinatlar koordinat)
        {
            return maze_[koordinat.row, koordinat.column] == (int)Nesne.wall;
        }
        public bool Exitcont(Koordinatlar koordinat)
        {
            return !entries.Any(n => n.row == koordinat.row && n.column == koordinat.column) && koordinat.column == 30 - 1;
        }

        public void FindEntries()
        {
            for (int i = 0; i < size; i++)
            {
                Koordinatlar koordinat = new Koordinatlar(i, 0);
                if (Pathcont(koordinat) && !entries.Any(n => n.row == koordinat.row && n.column == koordinat.column))
                {
                    entries.Add(koordinat);
                }
            }
        }
        public bool Visitmaze(Koordinatlar koordinat, List<Koordinatlar> route, bool[,] visit, Koordinatlar entries)
        {
            int row = koordinat.row;
            int column = koordinat.column;
            visit[row, column] = true;
            route.Add(koordinat);
            if (Exitcont(koordinat) && ((entries.row != koordinat.row) && (entries.column != koordinat.column)))
            {
                return true;
            }
            if (Bombcont(koordinat))
            {
                Console.Beep(800, 800);
                Console.WriteLine("Bomb Exploded !!");
                return false;
            }
            int[] directionRow = { 0, 1, -1, 0 };
            int[] directionColumn = { 1, 0, 0, -1 };
            for (int j = 0; j < directionRow.Length; j++)
            {
                Koordinatlar target = new Koordinatlar(koordinat.row + directionRow[j], koordinat.column + directionColumn[j]);
                if ((Bordercont(target)) && !Wallcont(target) && !visit[target.row, target.column] && Visitmaze(target, route, visit, entries))
                {
                    return true;
                }
            }
            visit[koordinat.row, koordinat.column] = false; // true 
            route.Remove(koordinat);
            return false; // true
        }
        void RouteOutPut(List<Koordinatlar> route, bool mazeoutput)
        {
            if (!mazeoutput)
            {
                Console.WriteLine("\nÇözüm Adımları: ");
                for (int i = 0; i < route.Count; i++)
                {
                    Console.WriteLine("{0}) {1} x {2}", i + 1, route[i].row, route[i].column);
                }
            }
            else
            {
                Console.WriteLine("\n LABİRENT");
                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (route.Any(n => n.row == i && n.column == j))
                        {
                            Console.Write(path);
                        }
                        else
                        {
                            Console.Write(wall);
                        }
                    }
                    Console.WriteLine();
                }
            }

        }
        public void EntryFromAllEntries()
        {
            foreach (Koordinatlar giris in entries)
            {
                steps = new bool[30, 30];
                List<Koordinatlar> izlenenyol = new List<Koordinatlar>();
                Console.WriteLine(giris.row + " x " + giris.column + " 'den girildi");
                if (Visitmaze(giris, izlenenyol, steps, giris))
                {
                    Console.Write("Çıkış bulundu.");
                    RouteOutPut(izlenenyol, false);
                    solutions.Add(izlenenyol);
                }
            }
        }
        public void AllSolutionsOutPut(bool mazeoutput)
        {
            foreach (var cozum in solutions)
            {
                RouteOutPut(cozum, mazeoutput);
            }
        }
       
        public void BombaKoordinat()
        {
            Console.WriteLine("Bomba koordinatları:");
            int sayac = 1;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (maze_[i, j] == (int)Nesne.bomb)
                    {
                        Console.WriteLine("{0}. Bomba: {1} x {2}", sayac, i, j);
                        sayac++;
                    }
                }
            }
        }
        

    }
}

