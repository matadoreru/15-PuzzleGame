using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Puzzle
{
    /// <summary>
    /// Interaction logic for wndJugar.xaml
    /// </summary>
    public partial class wndJugar : Window
    {
        SolidColorBrush pinzellIncorrecte = Brushes.AliceBlue;
        SolidColorBrush pinzellCorrecte = Brushes.Aqua;
        SuperGrid myGrid;
        Random random = new Random();
        DispatcherTimer rellotge;
        TimeSpan tempsTranscorregut = TimeSpan.Zero;
        int? nFila;
        int? nCol;
        double porcentajeComp;
        bool rellotgeEnces = false;

        public wndJugar(MainWindow main)
        {
            InitializeComponent();
            nFila = main.nFila.Value;
            nCol = main.nCol.Value;
            int index = 0;
            int? totalNumeros = nCol * nFila;
            Random random = new Random();
            List<int> nombresAleatoris = new List<int>();

            #region Numeros Aleatorios
            for (int i = 1; i < nFila*nCol;i++)
            {
                nombresAleatoris.Add(i);
            }

            for (int i = 1; i < (nFila * nCol) * (nFila * nCol); i++)
            {
                index = random.Next(0, Convert.ToInt32(totalNumeros - 1));
                Intercambiar(nombresAleatoris, index);

            }
            #endregion

            #region Desordres 
            int nDesordres = 0;
            int nombreActual;
            for(int i = 0; i < nombresAleatoris.Count - 1; i++)
            {
                nombreActual = nombresAleatoris[i];
                for (int j = i + 1; j < nombresAleatoris.Count - 1; j++)
                {
                    if (nombreActual > nombresAleatoris[j])
                    {
                        nDesordres++;
                    }
                }
            }
            if (nDesordres % 2 != 0)
                Intercambiar(nombresAleatoris, nombresAleatoris.Count - 1);

            #endregion

            #region SuperGrid Creation && AddButton

            #region Crear SuperGrid
            myGrid = new SuperGrid();
            myGrid.VacioPosX = Convert.ToInt32(nCol - 1);
            myGrid.VacioPosY = Convert.ToInt32(nFila - 1);
            myGrid.NCompletats = 0;
            myGrid.ShowGridLines = true;

            for (int i = 0; i < nCol; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(colDef);
            }
            for (int i = 0; i < nFila; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef);
            }
            #endregion

            #region Colocar botones
            index = 0;
            for (int i = 0; i < nCol; i++)
            {
                for(int j = 0; j < nFila; j++)
                {
                    SuperButton btn = new SuperButton(myGrid);
                    Grid.SetRow(btn,i);
                    Grid.SetColumn(btn, j);
                    if(index < nCol*nFila - 1)
                    {
                        btn.Content = nombresAleatoris[index];
                        PosicioCorrecte(btn,Convert.ToInt32(nCol));
                    }
                    btn.FontSize = 25;
                    btn.PosX = i;
                    btn.PosGrid = index;
                    btn.PosY = j;
                    btn.Click += btnMoure_Click;
                    if (btn.PosicioCorrecta)
                    {
                        btn.Background = pinzellCorrecte;
                        myGrid.NCompletats++;
                    }
                    else
                        btn.Background = pinzellIncorrecte; 
                    btn.Margin = new Thickness(5);
                    myGrid.Children.Add(btn);
                    index++;
                }
            }
            SuperButton final = (SuperButton)myGrid.Children[myGrid.Children.Count - 1];
            final.Content = "";
            final.Visibility = Visibility.Hidden;
            myGrid.BtnVacio = final;
            #endregion

            dockPanel.Children.Add(myGrid);
            #endregion

            #region Rellotge
            rellotge = new DispatcherTimer();
            rellotge.Interval = TimeSpan.FromSeconds(1);
            rellotge.Tick += Rellotge_Tick;
            rellotge.Start();
            rellotgeEnces = true;
            #endregion

            numeroCompletado.Text = myGrid.NCompletats + "/" + Convert.ToInt32(nCol * nFila);
            porcentajeComp = Math.Round((myGrid.NCompletats / Convert.ToDouble(nCol * nFila)) * 100,2);
            porcentajeCompletado.Text = porcentajeComp + " %";
        }

        #region Funcions

        #region Intercambiar dades
        static void Intercambiar(List<int> dades, int index)
        {
            if(index == 0)
            {
                dades.Reverse(index, 2);
            }
            else
            {
                dades.Reverse(index - 1, 2);
            }
        }
        #endregion

        #region Moure Button

        private void btnMoure_Click(object sender, RoutedEventArgs e)
        {
            SuperButton btnClick = (SuperButton)sender;
            SuperGrid myGrid = btnClick.Grid;
            SuperButton vacio = myGrid.BtnVacio;
            MoureVacio(btnClick, vacio, myGrid);
        }

        private void MoureVacio(SuperButton btnClick, SuperButton vacio, SuperGrid myGrid)
        {
            if(btnClick.Moure)
            {
                if (btnClick.PosX - 1 == vacio.PosX && btnClick.PosY == vacio.PosY
                    || btnClick.PosX + 1 == vacio.PosX && btnClick.PosY == vacio.PosY
                    || btnClick.PosY + 1 == vacio.PosY && btnClick.PosX == vacio.PosX
                    || btnClick.PosY - 1 == vacio.PosY && btnClick.PosX == vacio.PosX)
                {
                    vacio.Content = btnClick.Content;
                    btnClick.Content = "";
                    if (btnClick.PosicioCorrecta)
                        myGrid.NCompletats--;
                    vacio.CorrecteX = btnClick.CorrecteX;
                    vacio.CorrecteY = btnClick.CorrecteY;
                    vacio.Visibility = Visibility.Visible;
                    btnClick.Visibility = Visibility.Hidden;
                    myGrid.BtnVacio = btnClick;
                }
                else if (btnClick.PosX == vacio.PosX)
                {
                    MoureX(myGrid, btnClick, vacio);
                }
                else if (btnClick.PosY == vacio.PosY)
                {
                    MoureY(myGrid, btnClick, vacio);
                }
                if (vacio.PosicioCorrecta)
                {
                    vacio.Background = pinzellCorrecte;
                    myGrid.NCompletats++;
                }
                else
                {
                    vacio.Background = pinzellIncorrecte;

                }
                if (btnClick.PosicioCorrecta)
                {
                    btnClick.Background = pinzellCorrecte;
                }
                else
                {
                    btnClick.Background = pinzellIncorrecte;
                }
                numeroCompletado.Text = myGrid.NCompletats + "/" + Convert.ToInt32(nCol * nFila);
                porcentajeComp = Math.Round((myGrid.NCompletats / Convert.ToDouble(nCol * nFila)) * 100, 2);
                porcentajeCompletado.Text = porcentajeComp + " %";
            }
        }

        private void MoureX(SuperGrid myGrid, SuperButton btnClick, SuperButton btnVacio)
        {
            if(btnVacio.PosY > btnClick.PosY)
            {
                for (int i = 0; i < btnVacio.PosY - btnClick.PosY; i++)
                {
                    MoureBtn((SuperButton)myGrid.Children[((int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY) - 1], (SuperButton)myGrid.Children[(int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY]);
                }
            }
            else if (btnVacio.PosY < btnClick.PosY)
            {
                for (int i = 0; i < btnClick.PosY - btnVacio.PosY; i++)
                {
                    MoureBtn((SuperButton)myGrid.Children[((int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY) + 1], (SuperButton)myGrid.Children[(int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY]);
                }
            }
        }

        private void MoureY(SuperGrid myGrid, SuperButton btnClick, SuperButton btnVacio)
        {
            if (btnVacio.PosX > btnClick.PosX)
            {
                for (int i = 0; i < btnVacio.PosX - btnClick.PosX; i++)
                {
                    MoureBtn((SuperButton)myGrid.Children[((int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY) - (int)nCol], (SuperButton)myGrid.Children[((int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY)]);
                }
            }
            else if (btnVacio.PosX < btnClick.PosX)
            {
                for (int i = 0; i < btnClick.PosX - btnVacio.PosX; i++)
                {
                    MoureBtn((SuperButton)myGrid.Children[((int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY) + (int)nCol], (SuperButton)myGrid.Children[((int)nCol * myGrid.BtnVacio.PosX + myGrid.BtnVacio.PosY)]);
                }
            }
        }

        private void MoureBtn(SuperButton btnNormal, SuperButton btnVacio)
        {
            btnVacio.Content = btnNormal.Content;
            btnNormal.Content = "";
            if (btnNormal.PosicioCorrecta)
                myGrid.NCompletats--;
            btnVacio.CorrecteX = btnNormal.CorrecteX;
            btnVacio.CorrecteY = btnNormal.CorrecteY;
            btnVacio.Visibility = Visibility.Visible;
            btnNormal.Visibility = Visibility.Hidden;
            myGrid.BtnVacio = btnNormal;
        }

        #endregion

        private void PosicioCorrecte(SuperButton btn, int nCol)
        {
            int numPos = Convert.ToInt32(btn.Content);
            int posX = 0;
            int index = 1;
            int posY = 0;
            while(index <= numPos)
            {
                if(numPos == index)
                {
                    if (posX == nCol)
                    {
                        posY++;
                        posX = 0;
                    }
                    btn.CorrecteX = posX;
                    btn.CorrecteY = posY;
                }
                else
                {
                    posX++;
                    if (posX == nCol)
                    {
                        posY++;
                        posX = 0;
                    }
                }
                index++;
            }
        }

        #endregion

        #region Funcio Rellotge
        private void Rellotge_Tick(object? sender, EventArgs e)
        {
            tempsTranscorregut = tempsTranscorregut.Add(rellotge.Interval);
            ActualitzarRellotge(sbItemRellotge1, tempsTranscorregut);
        }

        private void ActualitzarRellotge(TextBlock tbText, TimeSpan periode)
        {
            String cadena = String.Format("{0:00}:{1:00}:{2:00}", periode.Hours, periode.Minutes, periode.Seconds);
            tbText.Text = cadena;
        }
        #endregion

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P && rellotgeEnces)
            {
                rellotge.Stop();
                rellotgeEnces = false;

                foreach(SuperButton btn in myGrid.Children)
                {
                    btn.Visibility = Visibility.Hidden;
                }

            }
            else if(e.Key == Key.P && !rellotgeEnces)
            {
                rellotge.Start();
                rellotgeEnces = true;
                foreach (SuperButton btn in myGrid.Children)
                {
                    btn.Visibility = Visibility.Visible;
                }
                myGrid.BtnVacio.Visibility = Visibility.Hidden;
            }
        }
    }
}
