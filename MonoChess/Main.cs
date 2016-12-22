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
        SpriteFont menuFont;

        Board board;

        MouseState mouseState;
        KeyboardState oldKeyState;

        Texture2D boardTile;
        Texture2D chessPieces;
        Texture2D cursor;
        Texture2D squareSelected;
        Texture2D turn;

        int totalFrames = 0;
        int fps = 0;

        float timeElapsed = 0.0f;
        float keyTimeElapsed = 0.0f;
        //float keyTimeMax = 150.0f;
        float menuTimeElapsed = 0.0f;
        float menuTimeMax = 150.0f;

        bool menuClicked = false;

        string title = "MonoChess";
        string selected = null;
        string[] menuText;

        Rectangle[] menuRect;

        public enum GameState { mainMenu, gameOver, options, play, ip };
        public enum PlayState { computer, local, online };

        public GameState gameState = GameState.mainMenu;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1000;
            Content.RootDirectory = "Content";

            board = new Board();
            this.Window.Title = title;
        }

        public void DrawMenu(String[] list, int x, int y)
        {
            GraphicsDevice.Clear(Color.Gray);
            menuRect = new Rectangle[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                spriteBatch.Begin();
                menuRect[i] = new Rectangle(x - (int)(menuFont.MeasureString(list[i]).X / 2), y + (i * 50), (int)menuFont.MeasureString(list[i]).X, (int)menuFont.MeasureString(list[i]).Y);
                if (menuRect[i].Contains(mouseState.X, mouseState.Y))
                {
                    spriteBatch.DrawString(menuFont, list[i], new Vector2(x - (menuFont.MeasureString(list[i]).X / 2), y + (i * 50)), Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(menuFont, list[i], new Vector2(x - (menuFont.MeasureString(list[i]).X / 2), y + (i * 50)), Color.Black);
                }
                spriteBatch.End();

                if (menuRect[i].Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && menuClicked == false)
                {
                    selected = list[i];
                    menuClicked = true;
                }
            }
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
            menuFont = Content.Load<SpriteFont>("font"); //make a menuFont font file, convert to .xnb

            boardTile = Content.Load<Texture2D>("boardTile");
            chessPieces = Content.Load<Texture2D>("chessPieces");
            cursor = Content.Load<Texture2D>("cursor");
            squareSelected = Content.Load<Texture2D>("squareSelected");
            turn = Content.Load<Texture2D>("turn");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            keyTimeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (menuClicked)
            {
                menuTimeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (menuTimeElapsed >= menuTimeMax)
                {
                    menuClicked = false;
                    menuTimeElapsed = 0.0f;
                }
            }

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
                //
            }

            #region move history
            if (board.maxMoveDisplay == 10)
            {
                //Moving a piece after pressing up breaks the game for some reason
                //if (Keyboard.GetState().IsKeyDown(Keys.Up) && board.displayMoves[0] != board.moves[0] && keyTimeElapsed >= keyTimeMax)
                //{
                //    keyTimeElapsed = 0.0f;

                //    Array.Copy(board.displayMoves, 0, board.displayMoves, 1, 9);
                //    board.displayMoves[0] = board.moves[board.moveNumber + board.moveOffset - 11];

                //    board.moveOffset--;
                //}
                //Pressing down breaks the game because of board.moveNumber not actually relating to the position of the list
                //if (Keyboard.GetState().IsKeyDown(Keys.Down) && board.displayMoves[9] != board.moves[board.moveNumber - 1] && keyTimeElapsed >= keyTimeMax)
                //{
                //    keyTimeElapsed = 0.0f;

                //    Array.Copy(board.displayMoves, 1, board.displayMoves, 0, 9);
                //    board.displayMoves[9] = board.moves[board.moveNumber + board.moveOffset];

                //    board.moveOffset++;
                //}
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
            #endregion

            if (board.gameOver)
            {
                gameState = GameState.gameOver;
                board.gameOver = false;
            }

            oldKeyState = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            totalFrames++;

            switch (gameState)
            {
                case GameState.mainMenu:
                    menuText = new String[] { "HUMAN VS COMPUTER", "HUMAN VS HUMAN", "ONLINE", "EXIT" };
                    DrawMenu(menuText, graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 2) - 100);

                    switch (selected)
                    {
                        case "HUMAN VS COMPUTER":
                            gameState = GameState.play;
                            board.playState = PlayState.computer;

                            this.Window.Title = title + " - Human VS Computer";
                            break;
                        case "HUMAN VS HUMAN":
                            gameState = GameState.play;
                            board.playState = PlayState.local;

                            this.Window.Title = title + " - Human VS Human";
                            break;
                        case "ONLINE":
                            //gameState = GameState.ip;
                            //board.playState = PlayState.online;

                            //this.Window.Title = title + " - Online";
                            break;
                        case "EXIT":
                            this.Exit();
                            break;
                        default:
                            break;
                    }
                    selected = null;
                    break;
                case GameState.gameOver:
                    this.Window.Title = title;
                    menuText = new String[] { "PLAY AGAIN", "MAIN MENU", "EXIT" };
                    DrawMenu(menuText, graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 2) - 100);

                    spriteBatch.Begin();
                    {
                        spriteBatch.DrawString(spriteFont, "GAME OVER!", new Vector2((graphics.PreferredBackBufferWidth / 2) - 50, (graphics.PreferredBackBufferHeight / 2) - 200), Color.White);
                    }
                    spriteBatch.End();

                    switch (selected)
                    {
                        case "PLAY AGAIN":
                            gameState = GameState.play;
                            break;
                        case "MAIN MENU":
                            gameState = GameState.mainMenu;
                            break;
                        case "EXIT":
                            this.Exit();
                            break;
                        default:
                            break;
                    }
                    selected = null;
                    break;
                case GameState.options:

                    break;
                case GameState.play:
                    menuText = new String[] { "MAIN MENU" };
                    DrawMenu(menuText, 900, 7);

                    if (selected == "MAIN MENU")
                    {
                        this.Window.Title = title;
                        board.ResetBoard();
                        gameState = GameState.mainMenu;
                    }

                    spriteBatch.Begin();
                    {
                        board.DrawBoard(spriteBatch, boardTile, squareSelected);
                        board.DrawPieces(spriteBatch, chessPieces);
                    }
                    spriteBatch.End();

                    spriteBatch.Begin();
                    {
                        spriteBatch.DrawString(spriteFont, board.pieceString, new Vector2(810, 37), Color.White); // piece color + piece
                        spriteBatch.DrawString(spriteFont, fps.ToString(), new Vector2(965, 37), Color.White); // fps (top right hand corner)
                        for (int i = 0; i < 8; i++) // board layout in numbers
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                spriteBatch.DrawString(smallFont, board.boardLayout[j][i].ToString(), new Vector2(810 + (i * 24), 100 + (j * 24)), Color.White);
                            }
                        }
                        for (int i = 0; i < board.maxMoveDisplay; i++)
                        {
                            spriteBatch.DrawString(smallFont, board.displayMoves[i], new Vector2(810, 755 - ((board.maxMoveDisplay - i) * 45)), Color.White);
                        }
                        spriteBatch.DrawString(spriteFont, board.error, new Vector2(810, 750), Color.White); // any errors
                        //spriteBatch.DrawString(spriteFont, board.testString, new Vector2(810, 770), Color.White);
                    }
                    spriteBatch.End();

                    spriteBatch.Begin();
                    {
                        if (board.turn)
                        {
                            spriteBatch.Draw(turn, new Vector2(950, 65), new Rectangle(0, 0, 15, 21), Color.White); // white highlighted
                            spriteBatch.Draw(turn, new Vector2(970, 65), new Rectangle(45, 0, 15, 21), Color.White); // black shaded out
                        }
                        else
                        {
                            spriteBatch.Draw(turn, new Vector2(950, 65), new Rectangle(15, 0, 15, 21), Color.White); // white shaded out
                            spriteBatch.Draw(turn, new Vector2(970, 65), new Rectangle(30, 0, 15, 21), Color.White); // black highlighted
                        }
                    }
                    spriteBatch.End();
                    break;
                case GameState.ip:

                    break;
                default:
                    break;
            }

            spriteBatch.Begin();
            {
                spriteBatch.Draw(cursor, new Vector2(mouseState.X, mouseState.Y), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}