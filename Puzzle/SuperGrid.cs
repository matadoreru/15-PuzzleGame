using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Puzzle
{
    internal class SuperGrid:Grid
    {
        SuperButton btnVacio;
        int vacioPosX;
        int vacioPosY;
        int nCompletats;

        public int VacioPosX
        {
            get
            {
                return vacioPosX;
            }
            set
            {
                vacioPosX = value;
            }
        }

        public int VacioPosY
        {
            get
            {
                return vacioPosY;
            }
            set
            {
                vacioPosY = value;
            }
        }

        internal SuperButton BtnVacio { get => btnVacio; set => btnVacio = value; }

        public int NCompletats { get => nCompletats; set => nCompletats = value; }

        public SuperGrid()
        {
        }


    }
}
