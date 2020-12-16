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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToeWPF
{
    struct Point
    {
        public sbyte x;
        public sbyte y;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game = new Game();
        Button[,] buttonArray = new Button[3,3];
        public MainWindow()
        {
            InitializeComponent();

            buttonArray[0, 0] = Button0;
            buttonArray[0, 1] = Button1;
            buttonArray[0, 2] = Button2;
            buttonArray[1, 0] = Button3;
            buttonArray[1, 1] = Button4;
            buttonArray[1, 2] = Button5;
            buttonArray[2, 0] = Button6;
            buttonArray[2, 1] = Button7;
            buttonArray[2, 2] = Button8;

            DrawBoard();
        }
        public void PrintHint(sbyte x, sbyte y)
        {
            buttonArray[(int)y, (int)x].Content = "H";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Point point = new Point();

            for (sbyte y = 0; y < 3; y++)
            {
                for (sbyte x = 0; x < 3; x++)
                {
                    if (buttonArray[y, x] == ((Button)sender)) { point.y = y; point.x = x; game.Turn(point); DrawBoard(); return; };
                }
            }
        }

        private void DrawBoard()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    buttonArray[y, x].Foreground = Brushes.Gray;

                    if (game.board[y, x] == FieldState.O)
                    {
                        buttonArray[y, x].Foreground = Brushes.Blue;
                        buttonArray[y, x].Content = "O";
                    }
                    else if (game.board[y, x] == FieldState.X)
                    {
                        buttonArray[y, x].Foreground = Brushes.Red;
                        buttonArray[y, x].Content = "X";
                    }
                    else if (game.board[y, x] == FieldState.E)
                    {
                        buttonArray[y, x].Content = "";
                    }
                }
            }

            lblRound.Content = $"round: {game.turnNumber}";
            lblPlayer.Foreground = game.currentPlayerID ? Brushes.Red : Brushes.Blue;
            lblPlayer.Content = $"{game.playerNames[game.currentPlayerID ? 1 : 0]}, it's your turn!";

            Point hint = game.DrawHint();
            if (hint.x != -1)
                PrintHint(hint.x, hint.y);

            if (game.turnNumber == 10)
                lblStatus.Content = "draw";
            else if (game.turnNumber == 11)
                lblStatus.Content = $"{game.playerNames[game.currentPlayerID ? 1 : 0]}, won the game!"; ;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            game.ResetBoard();
            DrawBoard();
        }
    }
}
