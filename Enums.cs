using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeWPF
{
    enum FieldState
    {
        E, X, O, Hint, Blocked
    }
    enum TurnResult
    {
        NotSet, Valid, Invalid, Tie, Win
    }
    enum Status
    {
        Started, Stopped, Win, Tie
    }
    enum Direction
    {
        Up, Down, Left, Right
    }
}
