using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Game game = new Game();
        Button[,] buttonArray = new Button[3,3];
        TurnResult tResult = TurnResult.NotSet;
        private MediaPlayer music = new MediaPlayer();
        private MediaPlayer clickSound = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            clickSound.Open(new Uri(@"./click.mp3", UriKind.Relative));

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
        private void PrintHint(sbyte x, sbyte y)
        {
            buttonArray[y, x].Foreground = Brushes.Gray;
            buttonArray[y, x].Content = "H";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tResult == TurnResult.Win || tResult == TurnResult.Tie)
                return;

            Point point = new Point();

            for (sbyte y = 0; y < 3; y++)
            {
                for (sbyte x = 0; x < 3; x++)
                {
                    clickSound.Position = TimeSpan.Zero;
                    clickSound.Play();
                    if (buttonArray[y, x] == ((Button)sender)) { point.y = y; point.x = x; tResult = game.Turn(point); DrawBoard(); return; };
                }
            }
        }

        private void DrawBoard()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
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
                    else
                    {
                        buttonArray[y, x].Content = "";
                    }
                }
            }

            //lblRound.Content = $"round: {game.turnNumber}";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("game.turnNumber"));
            lblPlayer.Foreground = game.currentPlayerID ? Brushes.Red : Brushes.Blue;
            lblPlayer.Content = $"{game.playerNames[game.currentPlayerID ? 1 : 0]}, it's your turn!";

            Point hint = game.DrawHint();
            if (hint.x != -1)
                PrintHint(hint.x, hint.y);

            if (tResult == TurnResult.Tie)
            {
                lblStatus.Foreground = Brushes.Gray;
                lblStatus.Content = "tie! press restart for a new game!";
            }
            else if (tResult == TurnResult.Win)
            {
                lblStatus.Foreground = game.currentPlayerID ? Brushes.Red : Brushes.Blue;
                lblStatus.Content = $"{game.playerNames[game.currentPlayerID ? 1 : 0]}, won the game!"; ;
            }
            else if (tResult == TurnResult.NotSet)
            {
                lblStatus.Foreground = Brushes.Gray;
                lblStatus.Content = "game started!";
            }
            else if (tResult == TurnResult.Valid)
            {
                lblStatus.Foreground = Brushes.Gray;
                lblStatus.Content = "valid turn, next player!";
            }
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            tResult = TurnResult.NotSet;
            game.ResetBoard();
            DrawBoard();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            music.Open(new Uri("./music.mp3", UriKind.Relative));
            music.Play();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            music.Stop();
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tResult = TurnResult.NotSet;
            game.ResetBoard();
            DrawBoard();
        }
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
