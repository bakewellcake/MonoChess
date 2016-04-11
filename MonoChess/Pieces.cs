using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoChess
{
    class Pieces
    {
        public List<string> pieces = new List<string>() { "pawn", "knight", "bishop", "rook", "queen", "king" };
        public List<string> pieceColors = new List<string>() { "white", "black" };
        public string piece;
        public string pieceColor;

        public int[] pieceCoord;

        public Rectangle[,] chessSpriteRect = new Rectangle[,] {
            { new Rectangle(0, 0 , 55, 79), new Rectangle(55, 0 , 74, 79), new Rectangle(129, 0 , 77, 79), new Rectangle(206, 0 , 64, 79), new Rectangle(270, 0 , 86, 79), new Rectangle(356, 0 , 78, 79) },
            { new Rectangle(0, 79, 55, 79), new Rectangle(55, 79, 74, 79), new Rectangle(129, 79, 77, 79), new Rectangle(206, 79, 64, 79), new Rectangle(270, 79, 86, 79), new Rectangle(356, 79, 78, 79) }
        };

        public Pieces()
        {

        }

        public Vector2 PiecePos()
        {
            return new Vector2(pieceCoord[0] * 100 + ((100 - chessSpriteRect[pieceColors.IndexOf(pieceColor), pieces.IndexOf(piece)].Width) / 2), pieceCoord[1] * 100 + (100 - chessSpriteRect[pieceColors.IndexOf(pieceColor), pieces.IndexOf(piece)].Height) - 4);
        }

        public Rectangle PieceRect()
        {
            return chessSpriteRect[pieceColors.IndexOf(pieceColor), pieces.IndexOf(piece)];
        }
    }
}