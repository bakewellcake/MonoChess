using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoChess
{
    class Board
    {
        Pieces pieces;

        public MouseState mouseState;
        public MouseState prevMouseState;

        public bool loaded = false;
        public bool selected = false;
        public bool turn = true; // true = WHITE MOVE false = BLACK MOVE
        public bool gameOver = false;
        public bool singlePlayer = false;
        public bool displayMenu = true;

        public int moveOffset = 0;
        public int moveNumber = 1;
        public int maxMoveDisplay = 0;
        public int selectedNumber;
        public int[] selectedPos;
        public int[] prevSelectedPos;

        public string pieceString = "";
        public string error = "NULL";
        public string testString;
        public List<string> moves = new List<string>();
        public string[] displayMoves = new string[10];

        public Pieces[,] piecesList = new Pieces[8, 8];
        public Rectangle[,] boardRect = new Rectangle[8, 8];

        public int[][] boardLayout = new int[][] {
            new int [] { 1,  2,  3,  4,  5,  3,  2,  1 }, // 1 = bRook, 2 = bKnight, 3 = bBishop
            new int [] { 6,  6,  6,  6,  6,  6,  6,  6 }, // 4 = bQueen, 5 = bKing, 6 = bPawn
            new int [] { 0,  0,  0,  0,  0,  0,  0,  0 }, //
            new int [] { 0,  0,  0,  0,  0,  0,  0,  0 }, //
            new int [] { 0,  0,  0,  0,  0,  0,  0,  0 }, //
            new int [] { 0,  0,  0,  0,  0,  0,  0,  0 }, //
            new int [] { 7,  7,  7,  7,  7,  7,  7,  7 }, // 7 = wPawn, 8 = wRook, 9 = wKnight
            new int [] { 8,  9,  10, 11, 12, 10, 9,  8 }  // 10 = wBishop, 11 = wQueen, 12 = wKing
        };

        public Board()
        {
            pieces = new Pieces();
            testString = singlePlayer.ToString();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boardRect[i, j] = new Rectangle(i * 100, j * 100, 100, 100);
                }
            }
        }

        public void ResetBoard()
        {
            moves.Clear();
            moveOffset = 0;
            moveNumber = 1;
            maxMoveDisplay = 0;
            turn = true;

            boardLayout = new int[][] {
                new int [] { 1,  2,  3,  4,  5,  3,  2,  1 },
                new int [] { 6,  6,  6,  6,  6,  6,  6,  6 },
                new int [] { 0,  0,  0,  0,  0,  0,  0,  0 },
                new int [] { 0,  0,  0,  0,  0,  0,  0,  0 },
                new int [] { 0,  0,  0,  0,  0,  0,  0,  0 },
                new int [] { 0,  0,  0,  0,  0,  0,  0,  0 },
                new int [] { 7,  7,  7,  7,  7,  7,  7,  7 },
                new int [] { 8,  9,  10, 11, 12, 10, 9,  8 }
            };
        }

        public int PieceNumber(int x, int y)
        {
            return boardLayout[y][x];
        }

        public void MovePiece(int[] moveFrom, int[] moveTo)
        {
            moves.Add(
                moveNumber.ToString() + ": " +
                piecesList[moveFrom[0], moveFrom[1]].pieceColor + " " +
                piecesList[moveFrom[0], moveFrom[1]].piece +
                "\n   (" + moveFrom[1].ToString() + ", " + moveFrom[0].ToString() + ") " +
                "to " +
                "(" + moveTo[1].ToString() + ", " + moveTo[0].ToString() + ")"
            );

            if (maxMoveDisplay < 10)
            {
                moves.CopyTo(0, displayMoves, 0, moveNumber);
                maxMoveDisplay++;
            }
            else
            {
                if (moveOffset != 0)
                {
                    moves.CopyTo(moveNumber - 9, displayMoves, 0, 10);
                    moveOffset = 0;
                }
                else
                {
                    Array.Copy(displayMoves, 1, displayMoves, 0, 9);
                    displayMoves[9] = moves[moveNumber - 1];
                }
            }
            moveNumber++;

            boardLayout[moveTo[0]][moveTo[1]] = boardLayout[moveFrom[0]][moveFrom[1]];
            boardLayout[moveFrom[0]][moveFrom[1]] = 0;
        }

        public void MovePiece(int[,] move)
        {
            MovePiece(new int[] { move[0, 1], move[0, 0] }, new int[] { move[1, 1], move[1, 0] });
        }

        public bool IsLegal(int[] moveFrom, int[] moveTo) // moveFrom[y, x], moveTo[y, x] because I suck at coding
        {
            if (boardLayout[moveFrom[0]][moveFrom[1]] == 1 || boardLayout[moveFrom[0]][moveFrom[1]] == 8) // rook
            {
                if ((moveTo[1] < moveFrom[1] && moveTo[0] == moveFrom[0]) ||
                    (moveTo[1] > moveFrom[1] && moveTo[0] == moveFrom[0]) ||
                    (moveTo[1] == moveFrom[1] && moveTo[0] < moveFrom[0]) ||
                    (moveTo[1] == moveFrom[1] && moveTo[0] > moveFrom[0]))
                {
                    for (int i = 1; i < Math.Abs(moveTo[0] - moveFrom[0]); i++)
                    {
                        if (boardLayout[moveFrom[0] + ((moveTo[0] - moveFrom[0]) < 0 ? -i : i)][moveFrom[1]] != 0)
                        {
                            return false;
                        }
                    }
                    for (int i = 1; i < Math.Abs(moveTo[1] - moveFrom[1]); i++)
                    {
                        if (boardLayout[moveFrom[0]][moveFrom[1] + ((moveTo[1] - moveFrom[1]) < 0 ? -i : i)] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (boardLayout[moveFrom[0]][moveFrom[1]] == 2 || boardLayout[moveFrom[0]][moveFrom[1]] == 9) // knight
            {
                if ((moveTo[1] == moveFrom[1] + 1 && moveTo[0] == moveFrom[0] - 2) ||
                    (moveTo[1] == moveFrom[1] + 1 && moveTo[0] == moveFrom[0] + 2) ||
                    (moveTo[1] == moveFrom[1] - 1 && moveTo[0] == moveFrom[0] - 2) ||
                    (moveTo[1] == moveFrom[1] - 1 && moveTo[0] == moveFrom[0] + 2) ||
                    (moveTo[1] == moveFrom[1] + 2 && moveTo[0] == moveFrom[0] - 1) ||
                    (moveTo[1] == moveFrom[1] + 2 && moveTo[0] == moveFrom[0] + 1) ||
                    (moveTo[1] == moveFrom[1] - 2 && moveTo[0] == moveFrom[0] - 1) ||
                    (moveTo[1] == moveFrom[1] - 2 && moveTo[0] == moveFrom[0] + 1))
                {
                    return true;
                }
            }
            if (boardLayout[moveFrom[0]][moveFrom[1]] == 3 || boardLayout[moveFrom[0]][moveFrom[1]] == 10) // bishop
            {
                if (((moveTo[1] < moveFrom[1] && moveTo[0] < moveFrom[0]) ||
                    (moveTo[1] > moveFrom[1] && moveTo[0] > moveFrom[0]) ||
                    (moveTo[1] > moveFrom[1] && moveTo[0] < moveFrom[0]) ||
                    (moveTo[1] < moveFrom[1] && moveTo[0] > moveFrom[0])) &&
                    (Math.Abs(moveTo[1] - moveFrom[1]) == Math.Abs(moveTo[0] - moveFrom[0])))
                {
                    for (int i = 1; i < Math.Abs(moveTo[0] - moveFrom[0]); i++) // Only need to check one co-ordinate for distance here
                    {
                        if (boardLayout[moveFrom[0] + ((moveTo[0] - moveFrom[0]) < 0 ? -i : i)][moveFrom[1] + ((moveTo[1] - moveFrom[1]) < 0 ? -i : i)] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (boardLayout[moveFrom[0]][moveFrom[1]] == 4 || boardLayout[moveFrom[0]][moveFrom[1]] == 11) //queen
            {
                if ((moveTo[1] < moveFrom[1] && moveTo[0] == moveFrom[0]) ||
                    (moveTo[1] > moveFrom[1] && moveTo[0] == moveFrom[0]) ||
                    (moveTo[1] == moveFrom[1] && moveTo[0] < moveFrom[0]) ||
                    (moveTo[1] == moveFrom[1] && moveTo[0] > moveFrom[0]))
                {
                    for (int i = 1; i < Math.Abs(moveTo[0] - moveFrom[0]); i++)
                    {
                        if (boardLayout[moveFrom[0] + ((moveTo[0] - moveFrom[0]) < 0 ? -i : i)][moveFrom[1]] != 0)
                        {
                            return false;
                        }
                    }
                    for (int i = 1; i < Math.Abs(moveTo[1] - moveFrom[1]); i++)
                    {
                        if (boardLayout[moveFrom[0]][moveFrom[1] + ((moveTo[1] - moveFrom[1]) < 0 ? -i : i)] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (((moveTo[1] < moveFrom[1] && moveTo[0] < moveFrom[0]) ||
                    (moveTo[1] > moveFrom[1] && moveTo[0] > moveFrom[0]) ||
                    (moveTo[1] > moveFrom[1] && moveTo[0] < moveFrom[0]) ||
                    (moveTo[1] < moveFrom[1] && moveTo[0] > moveFrom[0])) &&
                    (Math.Abs(moveTo[1] - moveFrom[1]) == Math.Abs(moveTo[0] - moveFrom[0])))
                {
                    for (int i = 1; i < Math.Abs(moveTo[0] - moveFrom[0]); i++) // Only need to check one co-ordinate for distance here
                    {
                        if (boardLayout[moveFrom[0] + ((moveTo[0] - moveFrom[0]) < 0 ? -i : i)][moveFrom[1] + ((moveTo[1] - moveFrom[1]) < 0 ? -i : i)] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (boardLayout[moveFrom[0]][moveFrom[1]] == 5 || boardLayout[moveFrom[0]][moveFrom[1]] == 12) // king
            {
                if ((moveTo[1] == moveFrom[1] + 1 && moveTo[0] == moveFrom[0] - 1) ||
                    (moveTo[1] == moveFrom[1] + 1 && moveTo[0] == moveFrom[0] + 1) ||
                    (moveTo[1] == moveFrom[1] - 1 && moveTo[0] == moveFrom[0] - 1) ||
                    (moveTo[1] == moveFrom[1] - 1 && moveTo[0] == moveFrom[0] + 1) ||
                    (moveTo[1] == moveFrom[1] + 0 && moveTo[0] == moveFrom[0] - 1) ||
                    (moveTo[1] == moveFrom[1] + 0 && moveTo[0] == moveFrom[0] + 1) ||
                    (moveTo[1] == moveFrom[1] - 1 && moveTo[0] == moveFrom[0] + 0) ||
                    (moveTo[1] == moveFrom[1] + 1 && moveTo[0] == moveFrom[0] + 0))
                {
                    return true;
                }
            }
            if (boardLayout[moveFrom[0]][moveFrom[1]] == 6 || boardLayout[moveFrom[0]][moveFrom[1]] == 7) // pawn
            {
                if (turn == true)
                {
                    if ((moveTo[1] == moveFrom[1]) &&
                        (moveTo[0] == moveFrom[0] - 1) &&
                        (piecesList[moveTo[0], moveTo[1]].piece == "")) // moving forwards
                    {
                        return true;
                    }
                    if ((moveTo[1] == moveFrom[1]) &&
                        (moveTo[0] == moveFrom[0] - 2) &&
                        (moveFrom[0] == 6) &&
                        (piecesList[moveTo[0], moveTo[1]].piece == "") &&
                        (piecesList[moveTo[0] + 1, moveTo[1]].piece == "")) // moving forwards 2 places on first move
                    {
                        return true;
                    }
                    if ((moveTo[1] == moveFrom[1] + 1 || moveTo[1] == moveFrom[1] - 1) &&
                        (moveTo[0] == moveFrom[0] - 1) &&
                        (piecesList[moveTo[0], moveTo[1]].piece != "")) // taking a piece diagonally
                    {
                        return true;
                    }
                    if ((moveTo[1] == moveFrom[1] + 1 || moveTo[1] == moveFrom[1] - 1) &&
                        (moveTo[0] == moveFrom[0] - 1 && piecesList[moveTo[0], moveTo[1]].piece == "") &&
                        (piecesList[moveTo[0] + 1, moveTo[1]].pieceColor == "black") &&
                        (piecesList[moveTo[0] + 1, moveTo[1]].piece == "pawn") &&
                        (piecesList[moveTo[0] + 1, moveTo[1]].pieceCoord[1] == 3)) // en passant
                    {
                        boardLayout[moveTo[0] + 1][moveTo[1]] = 0;
                        return true;
                    }
                }
                if (turn == false)
                {
                    if ((moveTo[1] == moveFrom[1]) &&
                        (moveTo[0] == moveFrom[0] + 1) &&
                        (piecesList[moveTo[0], moveTo[1]].piece == ""))
                    {
                        return true;
                    }
                    if ((moveTo[1] == moveFrom[1]) &&
                        (moveTo[0] == moveFrom[0] + 2) &&
                        (moveFrom[0] == 1) &&
                        (piecesList[moveTo[0], moveTo[1]].piece == "") &&
                        (piecesList[moveTo[0] - 1, moveTo[1]].piece == ""))
                    {
                        return true;
                    }
                    if ((moveTo[1] == moveFrom[1] + 1 || moveTo[1] == moveFrom[1] - 1) &&
                        (moveTo[0] == moveFrom[0] + 1) &&
                        (piecesList[moveTo[0], moveTo[1]].piece != ""))
                    {
                        return true;
                    }
                    if ((moveTo[1] == moveFrom[1] + 1 || moveTo[1] == moveFrom[1] - 1) &&
                        (moveTo[0] == moveFrom[0] + 1) &&
                        (piecesList[moveTo[0], moveTo[1]].piece == "") &&
                        (piecesList[moveTo[0] - 1, moveTo[1]].pieceColor == "white") &&
                        (piecesList[moveTo[0] - 1, moveTo[1]].piece == "pawn") &&
                       (piecesList[moveTo[0] - 1, moveTo[1]].pieceCoord[1] == 4))
                    {
                        boardLayout[moveTo[0] - 1][moveTo[1]] = 0;
                        return true;
                    }
                }
            }
            return false;
        }

        public void IllegalMove()
        {
            Console.WriteLine("Illegal Move!");
        }

        public void DrawBoard(SpriteBatch spriteBatch, Texture2D boardTile, Texture2D squareSelected)
        {
            mouseState = Mouse.GetState();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    spriteBatch.Draw(boardTile, new Vector2(i * 100, j * 100), new Rectangle(((i + j) % 2) * 100, 0, 100, 100), Color.White);

                    if (boardRect[i, j].Contains(new Rectangle(mouseState.X, mouseState.Y, 1, 1)) && loaded)
                    {
                        spriteBatch.Draw(squareSelected, new Vector2(i * 100, j * 100), new Rectangle(0, 0, 100, 100), Color.White);
                        pieceString =
                                    "(" + piecesList[j, i].pieceCoord[0].ToString() + ", " + piecesList[j, i].pieceCoord[1].ToString() + ") " +
                                    boardLayout[j][i].ToString() + "\n" + piecesList[j, i].pieceColor + " " + piecesList[j, i].piece;

                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //spriteBatch.Draw(squareSelected, new Vector2(i * 100, j * 100), new Rectangle(200, 0, 100, 100), Color.White); // possibly temporary -- click makes selection go green (kinda buggy)
                            if (prevMouseState.LeftButton == ButtonState.Released)
                            {
                                if (boardLayout[j][i] != 0)
                                {
                                    if (turn == true && boardLayout[j][i] >= 7)
                                    {
                                        selected = true;
                                        selectedNumber = boardLayout[j][i]; // number of piece when square is selected (indicated by red box)
                                        selectedPos = new int[] { j, i };
                                    }
                                    if (turn == false && boardLayout[j][i] <= 6 && singlePlayer == false)
                                    {
                                        selected = true;
                                        selectedNumber = boardLayout[j][i]; // number of piece when square is selected (indicated by red box)
                                        selectedPos = new int[] { j, i };
                                    }
                                }
                                if (selected && selectedPos == prevSelectedPos)
                                {
                                    if (IsLegal(selectedPos, new int[] { j, i }))
                                    {
                                        if (boardLayout[j][i] == 5 || boardLayout[j][i] == 12) // When king is captured... Display "game over"?
                                        {
                                            gameOver = true;
                                            Console.WriteLine("Game over!");
                                            ResetBoard();
                                        }
                                        else
                                        {
                                            MovePiece(selectedPos, new int[] { j, i });
                                            turn = !turn;
                                        }
                                    }
                                    else
                                    {
                                        IllegalMove();
                                    }
                                    selected = false;
                                }
                            }
                        }
                        prevMouseState = Mouse.GetState();
                        prevSelectedPos = selectedPos;
                    }
                }
            }
            if (selected)
            {
                spriteBatch.Draw(squareSelected, new Vector2(selectedPos[1] * 100, selectedPos[0] * 100), new Rectangle(100, 0, 100, 100), Color.White);
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                selected = false;
            }
        }

        public void DrawPieces(SpriteBatch spriteBatch, Texture2D chessPieces)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (boardLayout[i][j])
                    {
                        case 0: piecesList[i, j] = new Pieces { piece = "", pieceColor = "none", pieceCoord = new int[] { j, i } }; break; // piece = "" because I print color then piece so this is to avoid "none none"
                        case 1: piecesList[i, j] = new Pieces { piece = "rook", pieceColor = "black", pieceCoord = new int[] { j, i } }; break;
                        case 2: piecesList[i, j] = new Pieces { piece = "knight", pieceColor = "black", pieceCoord = new int[] { j, i } }; break;
                        case 3: piecesList[i, j] = new Pieces { piece = "bishop", pieceColor = "black", pieceCoord = new int[] { j, i } }; break;
                        case 4: piecesList[i, j] = new Pieces { piece = "queen", pieceColor = "black", pieceCoord = new int[] { j, i } }; break;
                        case 5: piecesList[i, j] = new Pieces { piece = "king", pieceColor = "black", pieceCoord = new int[] { j, i } }; break;
                        case 6: piecesList[i, j] = new Pieces { piece = "pawn", pieceColor = "black", pieceCoord = new int[] { j, i } }; break;
                        case 7: piecesList[i, j] = new Pieces { piece = "pawn", pieceColor = "white", pieceCoord = new int[] { j, i } }; break;
                        case 8: piecesList[i, j] = new Pieces { piece = "rook", pieceColor = "white", pieceCoord = new int[] { j, i } }; break;
                        case 9: piecesList[i, j] = new Pieces { piece = "knight", pieceColor = "white", pieceCoord = new int[] { j, i } }; break;
                        case 10: piecesList[i, j] = new Pieces { piece = "bishop", pieceColor = "white", pieceCoord = new int[] { j, i } }; break;
                        case 11: piecesList[i, j] = new Pieces { piece = "queen", pieceColor = "white", pieceCoord = new int[] { j, i } }; break;
                        case 12: piecesList[i, j] = new Pieces { piece = "king", pieceColor = "white", pieceCoord = new int[] { j, i } }; break;
                        default: break;
                    }
                    if (boardLayout[i][j] != 0)
                    {
                        spriteBatch.Draw(chessPieces, piecesList[i, j].PiecePos(), piecesList[i, j].PieceRect(), Color.White);
                    }
                    else
                    {
                        // empty space
                    }
                }
            }
            if (!loaded)
            {
                loaded = true;
            }
        }
    }
}