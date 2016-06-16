using System;
using System.Collections.Generic;

namespace MonoChess
{
    class Computer
    {
        Random random;
        Board board;

        List<int[]> legalMoves = new List<int[]>();
        List<int[]> tmpMoves = new List<int[]>();

        int checkSquareX;
        int checkSquareY;
        int randomIndex;
        int piece;

        bool outOfBoard;

        public Computer(Board board)
        {
            this.board = board;
        }       

        public int[,] CalculateMove()
        {
            random = new Random();
            legalMoves.Clear();
            tmpMoves.Clear();

            for (int x = 0; x < board.boardLayout.Length; x++)
            {
                for (int y = 0; y < board.boardLayout[x].Length; y++)
                {
                    if (board.boardLayout[x][y] > 0 && board.boardLayout[x][y] <= 6)
                    {
                        for (int i = 0; i < (board.boardLayout[x][y] == 2 ? 2 : 3); i++)
                        {
                            for (int j = 0; j < (board.boardLayout[x][y] == 2 ? 4 : (i < 2 ? 3 : 2)); j++)
                            {
                                outOfBoard = false;

                                checkSquareX = x + (board.boardLayout[x][y] == 2 ? (i == 0 ? (j < 2 ? 1 : -1) : (j < 2 ? 2 : -2)) : (i < 2 ? (i == 0 ? -1 : 1) : 0));
                                checkSquareY = (board.boardLayout[x][y] == 2 ? y + (i == 0 ? (j % 2 == 0 ? 2 : -2) : (j % 2 == 0 ? 1 : -1)) : (i < 2 ? ((y - 1) + j) : (y + (j == 0 ? -1 : 1))));

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

            //foreach (int[] move in legalMoves)
            //{
            //    piece = board.boardLayout[move[0]][move[1]];

            //    switch(piece)
            //    {
            //        case 1:

            //            break;
            //        case 3:

            //            break;
            //        case 4:

            //            break;
            //        case 6:
            //            if (board.IsLegal(new int[] { move[0], move[1] }, new int[] { move[0] + 2, move[1]})) //these co-ordinate systems are so backwards lol
            //            {
            //                tmpMoves.Add(new int[] { move[1], move[0], move[1] + 2, move[0] });
            //            }
            //            break;
            //    }
            //}

            //legalMoves.AddRange(tmpMoves);

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