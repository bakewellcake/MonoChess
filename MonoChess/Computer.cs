using System;
using System.Collections.Generic;

namespace MonoChess
{
    class Computer
    {
        Random random;
        Board board;

        int[] moveFrom;
        int[] moveTo;

        List<int[]> legalMoves = new List<int[]>();

        int checkSquareX;
        int checkSquareY;
        int randomIndex;

        bool outOfBoard;

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
            random = new Random();
            legalMoves.Clear();

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
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < (i < 2 ? 3 : 2); j++)
                                {
                                    outOfBoard = false;

                                    checkSquareX = x + (i < 2 ? (i == 0 ? -1 : 1) : 0);
                                    checkSquareY = (i < 2 ? ((y - 1) + j) : (y + (j == 0 ? -1 : 1)));

                                    if (checkSquareX < 0 || checkSquareX > 7 || checkSquareY < 0 || checkSquareY > 7)
                                    {
                                        outOfBoard = true;
                                    }

                                    if (outOfBoard == false && board.IsLegal(new int[] { x, y }, new int[] { checkSquareX, checkSquareY })) //check move is legal to that space
                                    {
                                        legalMoves.Add(new int[] { x, y, checkSquareX, checkSquareY });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            randomIndex = random.Next(0, legalMoves.Count);
            foreach (int[] value in legalMoves)
            {
                Console.WriteLine("(" + value[1].ToString() + ", " + value[0].ToString() + ")" + " (" + value[3].ToString() + ", " + value[2].ToString() + ")");
            }
            Console.WriteLine("\n(" + legalMoves[randomIndex][1].ToString() + ", " + legalMoves[randomIndex][0] + ")" + " (" + legalMoves[randomIndex][3].ToString() + ", " + legalMoves[randomIndex][2].ToString() + ")\n=============");

            return new int[,] { { legalMoves[randomIndex][1], legalMoves[randomIndex][0] }, { legalMoves[randomIndex][3], legalMoves[randomIndex][2] } };
        }
    }
}