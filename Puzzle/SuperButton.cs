using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Puzzle
{
    internal class SuperButton:Button
    {
        int posGrid;
        int posX;
        int posY;
        int correcteX;
        int correcteY;

        SuperGrid grid;

        public SuperButton(SuperGrid grid)
        {
            this.Grid = grid;
        }

        public int PosX
        {
            get
            {
                return posX;
            }
            set
            {
                posX = value;
            }
        }

        public int PosY
        {
            get
            {
                return posY;
            }
            set
            {
                posY = value;
            }
        }

        public int CorrecteX
        {
            get
            {
                return correcteX;
            }
            set
            {
                correcteX = value;
            }
        }

        public int CorrecteY
        {
            get
            {
                return correcteY;
            }
            set
            {
                correcteY = value;
            }
        }

        public bool PosicioCorrecta { 
            get
            {
                if (posX == correcteX && posY == correcteY)
                    return true;
                else
                    return false;
            } 
        }

        public bool Moure
        {
            get
            {
                if (PosX - 1 == this.grid.BtnVacio.PosX && PosY == this.grid.BtnVacio.PosY)
                {
                    return true;

                }
                else if (PosX + 1 == this.grid.BtnVacio.PosX && PosY == this.grid.BtnVacio.PosY)
                {
                    return true;

                }
                else if (PosY + 1 == this.grid.BtnVacio.PosY && PosX == this.grid.BtnVacio.PosX)
                {
                    return true;
                }
                else if (PosY - 1 == this.grid.BtnVacio.PosY && PosX == this.grid.BtnVacio.PosX)
                {
                    return true;

                }
                else if (PosX == this.grid.BtnVacio.PosX)
                {
                    return true;
                }
                else if (PosY== this.grid.BtnVacio.PosY)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public int PosGrid { get => posGrid; set => posGrid = value; }
        internal SuperGrid Grid { get => grid; set => grid = value; }
    }
}
