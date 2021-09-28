using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lab02
{
    class RunningMan : DrawableGameComponent
    {
        private Vector2 position, velocity;
        private Texture2D texture;
        private int frameCount, currentFrame;
        private double frameElapsedTime, frameTimeStep;
        private Rectangle frameRect;
        private SpriteBatch _spriteBatch;

        public RunningMan(Game g) : base(g) { }

        public override void Initialize()
        {
            position.X = GraphicsDevice.Viewport.Width/2;
            position.Y = 160;
            velocity.X = 6f;
            velocity.Y = 0;
            frameCount = 10;
            frameTimeStep = 1000 / 25f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("Images\\Run");
            frameRect = new Rectangle(0, 0, texture.Width/10, texture.Height);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // increment elapsed frame time
            frameElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // is it time to move on to the next frame?

            if (frameElapsedTime >= frameTimeStep)
            {
                position += velocity;
                currentFrame = (currentFrame + 9) % frameCount;
                // update the frame rectangle (only x-coordinate needed)
                frameRect.X = currentFrame * frameRect.Width;
	            frameElapsedTime = 0; // reset the elapsed counter
                          // checking for screen edge
            }

            if(position.X > GraphicsDevice.Viewport.Width)
                position.X = 0;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position, frameRect, Color.White);
            _spriteBatch.Draw(texture, new Vector2(position.X - GraphicsDevice.Viewport.Width, position.Y), frameRect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
