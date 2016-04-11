using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoChess
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont smallFont;

        Board board;
        Computer computer;

        MouseState mouseState;
        KeyboardState oldKeyState;

        Texture2D boardTile;
        Texture2D chessPieces;
        Texture2D cursor;
        Texture2D squareSelected;
        Texture2D turn;

        //Effect blurEffect;

        int totalFrames = 0;
        int fps = 0;
        float timeElapsed = 0.0f;
        float keyTimeElapsed = 0.0f;
        float keyTimeMax = 150.0f;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1000;
            Content.RootDirectory = "Content";

            board = new Board();
            computer = new Computer(board);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("font");
            smallFont = Content.Load<SpriteFont>("smallFont");

            boardTile = Content.Load<Texture2D>("boardTile");
            chessPieces = Content.Load<Texture2D>("chessPieces");
            cursor = Content.Load<Texture2D>("cursor");
            squareSelected = Content.Load<Texture2D>("squareSelected");
            turn = Content.Load<Texture2D>("turn");

            //blurEffect = Content.Load<Effect>("blurEffect");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            keyTimeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed >= 1000.0f)
            {
                fps = totalFrames;
                totalFrames = 0;
                timeElapsed = 0.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && oldKeyState.IsKeyUp(Keys.S)) // switch turns
            {
                board.turn = !board.turn;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R) && oldKeyState.IsKeyUp(Keys.R)) // reset board
            {
                board.ResetBoard();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T) && oldKeyState.IsKeyUp(Keys.T)) // test button
            {
                board.singlePlayer = !board.singlePlayer;
                board.testString = board.singlePlayer.ToString();
            }
            if (board.maxMoveDisplay == 11)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && board.displayMoves[0] != board.moves[0] && keyTimeElapsed >= keyTimeMax)
                {
                    keyTimeElapsed = 0.0f;

                    Array.Copy(board.displayMoves, 0, board.displayMoves, 1, 10);
                    board.displayMoves[0] = board.moves[board.moveNumber + board.moveOffset - 12];

                    board.moveOffset--;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && board.displayMoves[10] != board.moves[board.moveNumber - 1] && keyTimeElapsed >= keyTimeMax)
                {
                    keyTimeElapsed = 0.0f;

                    Array.Copy(board.displayMoves, 1, board.displayMoves, 0, 10);
                    board.displayMoves[10] = board.moves[board.moveNumber + board.moveOffset];

                    board.moveOffset++;
                }
                // Fix this if I can be bothered lol
                //if (Keyboard.GetState().IsKeyDown(Keys.PageUp) && oldKeyState.IsKeyUp(Keys.PageUp))
                //{
                //    board.moves.CopyTo(0, board.displayMoves, 0, 11);
                //    board.moveOffset = -board.moveNumber;
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.PageDown) && oldKeyState.IsKeyUp(Keys.PageDown))
                //{
                //    board.moves.CopyTo(board.moveNumber - 11, board.displayMoves, 0, 11);
                //    board.moveOffset = 0;
                //}
            }
            oldKeyState = Keyboard.GetState();

            if(board.turn == false && board.singlePlayer)
            {
                board.MovePiece(computer.CalculateMove());
                board.turn = !board.turn;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            totalFrames++;

            // Game over screen thing, effects not working with Monogame...
            //if (board.gameOver) // if the game has ended, blur
            //{
            //    spriteBatch.Begin();
            //    {
            //        //blurEffect.CurrentTechnique.Passes[0].Apply();
            //        board.DrawBoard(spriteBatch, boardTile, squareSelected);
            //        board.DrawPieces(spriteBatch, chessPieces);
            //    }
            //    spriteBatch.End();
            //}
            //else // if the game is still in progress, no blur
            //{
            //    spriteBatch.Begin();
            //    {
            //        board.DrawBoard(spriteBatch, boardTile, squareSelected);
            //        board.DrawPieces(spriteBatch, chessPieces);
            //    }
            //    spriteBatch.End();
            //}

            if (!board.gameOver)
            {
                spriteBatch.Begin();
                {
                    board.DrawBoard(spriteBatch, boardTile, squareSelected);
                    board.DrawPieces(spriteBatch, chessPieces);
                }
                spriteBatch.End();

                spriteBatch.Begin();
                {
                    spriteBatch.DrawString(spriteFont, board.pieceString, new Vector2(810, 7), Color.White); // piece color + piece
                    spriteBatch.DrawString(spriteFont, fps.ToString(), new Vector2(965, 7), Color.White); // fps (top right hand corner)
                    for (int i = 0; i < 8; i++) // board layout in numbers
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            spriteBatch.DrawString(smallFont, board.boardLayout[j, i].ToString(), new Vector2(810 + (i * 24), 70 + (j * 24)), Color.White);
                        }
                    }
                    for (int i = 0; i < board.maxMoveDisplay; i++)
                    {
                        spriteBatch.DrawString(smallFont, board.displayMoves[i], new Vector2(810, 765 - ((board.maxMoveDisplay - i) * 45)), Color.White);
                    }
                    spriteBatch.DrawString(spriteFont, board.error, new Vector2(810, 750), Color.White); // any errors
                    spriteBatch.DrawString(spriteFont, board.testString, new Vector2(810, 770), Color.White);
                }
                spriteBatch.End();

                spriteBatch.Begin();
                {
                    if (board.turn)
                    {
                        spriteBatch.Draw(turn, new Vector2(950, 35), new Rectangle(0, 0, 15, 21), Color.White); // white highlighted
                        spriteBatch.Draw(turn, new Vector2(970, 35), new Rectangle(45, 0, 15, 21), Color.White); // black shaded out
                    }
                    else
                    {
                        spriteBatch.Draw(turn, new Vector2(950, 35), new Rectangle(15, 0, 15, 21), Color.White); // white shaded out
                        spriteBatch.Draw(turn, new Vector2(970, 35), new Rectangle(30, 0, 15, 21), Color.White); // black highlighted
                    }
                }
                spriteBatch.End();

                spriteBatch.Begin();
                {
                    spriteBatch.Draw(cursor, new Vector2(mouseState.X, mouseState.Y), Color.White);
                }
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}