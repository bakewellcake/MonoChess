using System;

namespace MonoChess
{
    class Computer
    {
        Random random;
        Board board;

        int[] moveFrom;
        int[] moveTo;

        int[,] legalMoves;

        public Computer(Board board)
        {
            this.board = board;
        }

        public bool CheckSurroundingSquares()
        {
            return false;
        }

        public int[,] CalculateMove()
        {
            for (int x = 0; x < board.boardLayout.Length; x++)
            {
                for (int y = 0; y < board.boardLayout[x].Length; y++)
                {
                    if (board.boardLayout[x][y] > 0 && board.boardLayout[x][y] <= 6)
                    {
                        if (board.boardLayout[x][y] == 2) //knight
                        {
                            //
                        }
                        else
                        {
                            if (x == 0 || x == 7 || y == 0 || y == 7)
                            {
                                if (x == 0 && y == 0) //top left
                                {

                                }
                                else if (x == 7 && y == 0) //top right
                                {

                                }
                                else if (x == 0 && y == 7) //bottom left
                                {

                                }
                                else if (x == 7 && y == 7) //bottom right
                                {

                                }
                                else
                                {
                                    for (int i = 1; i < 9; i++)
                                    {
                                        //calculate legal move here
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new int[,] { { 0, 0 }, { 1, 1 } };
        }
    }
}