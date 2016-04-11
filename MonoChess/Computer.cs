using System;

namespace MonoChess
{
    class Computer
    {
        Random random;
        Board board;

        int[] moveFrom;
        int[] moveTo;

        public Computer(Board board)
        {
            this.board = board;
        }

        public int[,] CalculateMove()
        {
            if(board.PieceNumber(7, 5) != 0)
            {
                moveFrom = new int[] { 0, 1 };
                moveTo = new int[] { 0, 3 };
            }
            else
            {
                moveFrom = new int[] { 1, 1 };
                moveTo = new int[] { 1, 3 };
            }

            return new int[,] { { moveFrom[0], moveFrom[1] }, { moveTo[0], moveTo[1] } };
        }
    }
}