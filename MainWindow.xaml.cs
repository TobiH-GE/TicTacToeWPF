﻿using System;
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
        Label[,] labelArray = new Label[3,3];
        public MainWindow()
        {
            InitializeComponent();

            labelArray[0, 0] = Label0;
            labelArray[0, 1] = Label1;
            labelArray[0, 2] = Label2;
            labelArray[1, 0] = Label3;
            labelArray[1, 1] = Label4;
            labelArray[1, 2] = Label5;
            labelArray[2, 0] = Label6;
            labelArray[2, 1] = Label7;
            labelArray[2, 2] = Label8;

            DrawBoard();
        }
        public void PrintHint(sbyte x, sbyte y)
        {
            labelArray[(int)y, (int)x].Content = "H";
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point point = new Point();

            switch (((Label)sender).Name.ToString())
            {
                case "Label0":
                    {
                        point.y = 0; point.x = 0; game.Turn(point); DrawBoard();  return;
                    }
                case "Label1":
                    {
                        point.y = 0; point.x = 1; game.Turn(point); DrawBoard(); return;
                    }
                case "Label2":
                    {
                        point.y = 0; point.x = 2; game.Turn(point); DrawBoard(); return;
                    }
                case "Label3":
                    {
                        point.y = 1; point.x = 0; game.Turn(point); DrawBoard(); return;
                    }
                case "Label4":
                    {
                        point.y = 1; point.x = 1; game.Turn(point); DrawBoard(); return;
                    }
                case "Label5":
                    {
                        point.y = 1; point.x = 2; game.Turn(point); DrawBoard(); return;
                    }
                case "Label6":
                    {
                        point.y = 2; point.x = 0; game.Turn(point); DrawBoard(); return;
                    }
                case "Label7":
                    {
                        point.y = 2; point.x = 1; game.Turn(point); DrawBoard(); return;
                    }
                case "Label8":
                    {
                        point.y = 2; point.x = 2; game.Turn(point); DrawBoard(); return;
                    }

                default:
                    lblStatus.Content = "Error";
                    DrawBoard();
                    return;
            }
  
        }

        private void DrawBoard()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    labelArray[y, x].Content = game.board[y, x].ToString();
                }
            }

            lblRound.Content = $"round: {game.turnNumber}";
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