﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TicTacToeWPF
{
    class Game
    {
        public FieldState[,] board = new FieldState[3, 3];
        public bool currentPlayerID = false;
        public string[] playerNames = new string[] { "Player 1", "Player 2" };
        public byte turnNumber = 1;

        public Game ()
        {
            ResetBoard();
        }
        public FieldState[,] GetBoard()
        {
            return board;
        }
        public bool GetPlayerID()
        {
            return currentPlayerID;
        }
        public TurnResult Turn(Point point)             // Spielzug ausführen und TurnResult zurückgeben
        {
            if (board[point.y, point.x] != FieldState.E)// Brett an y.x nicht leer?
            {
                return TurnResult.Invalid;              // dann zurück
            }

            board[point.y, point.x] = (currentPlayerID ? FieldState.X : FieldState.O); // Brett mit Spielstein des Spielers belegen

            if (CheckWin(board[point.y, point.x]))      // Gewonnen?
            {
                return TurnResult.Win;
            }

            if (turnNumber == 9)                        // unentschieden
                return TurnResult.Tie;

            turnNumber++;
            currentPlayerID = !currentPlayerID;         // Spieler wechseln
            return TurnResult.Valid;                    // zurück, Spielzug ok
        }

        private bool CheckWin(FieldState playerFieldState, byte checkValue = 3) // zum Test ob 2 oder 3 Steine in Reihe liegen
        {
            byte counterX;                              // für den horizontalen Test  
            byte counterY;                              // für den vertikalen Test
            byte counterDiag1;                          // für den Test von oben links nach unten rechts
            byte counterDiag2;                          // für den Test von oben rechts nach unten links

            for (byte y = 0; y <= 2; y++)               // Haupt-Testschleife, wir testen alles in einem Rutsch
            {
                counterX = 0;
                counterY = 0;
                counterDiag1 = 0;
                counterDiag2 = 0;

                for (byte x = 0; x <= 2; x++)           // wir erhöhen x in jedem Durchgang
                {
                    if (board[y, x] == playerFieldState)// horizontaler Test, fängt an bei [0,0] [0,1] [0,2] ...
                    {
                        counterX++;                     // Anzahl der gleichen Steine in dieser Reihe um 1 erhöhen
                    }
                    if (board[x, y] == playerFieldState)// vertikaler Test (wir tauschen einfach x und y), Test fängt an bei [0,0] [1,0] [2,0] ...
                    {
                        counterY++;                     // Anzahl der gleichen Steine in dieser Spalte um 1 erhöhen
                    }
                    #region Hack - Game läuft auch ohne nachfolgende Zeile
                    if (checkValue == 3 && y == 0)      // Diagonalen nur einmal testen
                    {
                        #endregion
                        if (board[x, x] == playerFieldState) // Test diagonal von oben links, x wird immer um 1 erhöht daher setzen wir einfach [x,x] ein, das ergibt den Test für [0,0] [1,1] [2,2]
                        {
                            counterDiag1++;             // Anzahl der gleichen Steine in dieser Diagonalen um 1 erhöhen
                        }
                        if (board[2 - x, x] == playerFieldState) // Test diagonal von oben rechts, x wird immer um 1 erhöht, mit [2 - x, x] erhalten wir also [2,0] [1,1] [0,2]
                        {
                            counterDiag2++;             // Anzahl der gleichen Steine in dieser Diagonalen um 1 erhöhen
                        }
                        #region
                    }
                    #endregion
                    if (counterX == checkValue || counterY == checkValue || counterDiag1 == checkValue || counterDiag2 == checkValue) // Sobald irgendwo checkValue gleiche Steine gezählt wurden, dann ...
                    {
                        return true;                    // raus aus der Funktion, Gewinner steht fest, kein weiteres Prüfen notwendig
                    }
                }
                #region Hack - Game läuft auch ohne diesen Bereich
                if (checkValue == 3 && counterX == 0 && counterY == 0) // dirty hack, wenn Rand oben und Rand links nicht belegt, dann brauchen wir gar nicht weiter testen
                {
                    return false;
                }
                #endregion
            }
            return false;
        }
        public Point DrawHint()                         // KI, Vorschlag für Spielzug holen
        {
            Point hint;

            hint = GetHint(3, (currentPlayerID ? FieldState.X : FieldState.O)); // prüfe mit welchem 3. Zug Spieler gewinnen kann
            if (hint.x != -1)
            {
                return hint;                            // Gewinn möglich, also zurück
            }

            hint = GetHint(3, (!currentPlayerID ? FieldState.X : FieldState.O)); // prüfe mit welchem 3. Zug der Gegner gewinnen kann (um dies zu verhindern)
            if (hint.x != -1)
            {
                return hint;                            // Gewinn möglich, also zurück
            }

            hint = GetHint(2, (currentPlayerID ? FieldState.X : FieldState.O)); // prüfe ob Spieler einen zweiten Stein legen kann und dann mit einem dritten in der selben Reihe gewinnt
            if (hint.x != -1)
            {
                return hint;                            // gibt dann die Position des möglichen dritten Steins zurück (wirkt sehr intelligent)
            }

            hint = GetHint(3, FieldState.E);            // prüfe ob überhaupt noch eine 3er Reihe frei ist
            if (hint.x != -1)
            {
                return hint;                            // gibt die Position zurück
            }

            hint.x = -1;
            return hint;                                // Rückgabewert -1 steht für "kein Hint vorhanden"
        }

        public Point GetHint(byte checkValue, FieldState fState)
        {
            Point returnHint = new Point();
            Point firstHint = new Point();
            firstHint.x = -1;

            for (sbyte y = 0; y < 3; y++)
            {
                for (sbyte x = 0; x < 3; x++)
                {
                    if (board[y, x] == FieldState.E)    // prüfen des Spielfelds auf einen freien Platz
                    {
                        board[y, x] = fState;           // ein Teststein setzen

                        if (CheckWin(fState, checkValue))   // testen auf 2er oder 3er (checkValue) Reihe
                        {
                            if (checkValue == 3)        // es wurde auf 3 Steine getestet
                            {
                                returnHint.x = x;       // Werte x y merken
                                returnHint.y = y;
                                board[y, x] = FieldState.E; // Teststein wieder vom Feld löschen
                                return returnHint;      // Werte zurückgeben
                            }
                            if (checkValue == 2)        // es wurde auf 2 Steine getestet
                            {
                                returnHint = GetHint(3, fState); // testen ob mit 3. Stein Gewinn möglich, Funktion ruft sich selbst auf
                                if (returnHint.x != -1)
                                {
                                    firstHint = returnHint; // Werte speichern
                                    board[y, x] = FieldState.Blocked;
                                    returnHint = GetHint(3, fState); // testen, ob es noch eine bessere, unschlagbare Position gibt
                                    if (returnHint.x != -1)
                                    {
                                        board[y, x] = FieldState.E;
                                        returnHint.x = x;
                                        returnHint.y = y;
                                        return returnHint; // Gewinn mit 3. garantiert
                                    }
                                }
                            }
                        }
                        board[y, x] = FieldState.E;     // Teststein wieder vom Feld löschen
                    }
                }
            }
            if (firstHint.x != -1) return firstHint;
            returnHint.x = -1;                          // kein Gewinn möglich
            return returnHint;
        }
        public void ResetBoard()
        {
            turnNumber = 1;
            currentPlayerID = false;

            for (byte y = 0; y <= 2; y++)
            {
                for (byte x = 0; x <= 2; x++)
                {
                    board[y, x] = FieldState.E;
                }
            }
        }
    }
}
