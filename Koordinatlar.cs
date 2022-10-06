using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labirentcözme
{
    internal class Koordinatlar
    {
        public int row;
        public int column;

        public Koordinatlar(int row, int col)
        {
            this.row = row;
            this.column = col;
        }
        public Koordinatlar()
        {
            row = 0;
            column = 0;
        }
    }
}
