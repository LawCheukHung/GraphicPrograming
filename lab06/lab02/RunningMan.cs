﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lab02
{
    class RunningMan : DrawableGameComponent
    {
        public const float MAN_SPEED = 4.5f;
        public Color[] data;
        public Vector2 position, velocity;
        public Rectangle frameRect;
        public Texture2D texture;
        public Color manColor = Color.White;
        private int frameCount, currentFrame;
        private double frameElapsedTime, frameTimeStep;
        private SpriteBatch _spriteBatch;

        public RunningMan(Game g) : base(g) { }

        public override void Initialize()
        {
            position.X = GraphicsDevice.Viewport.Width/2;
            position.Y = 160;
            velocity.X = 0.0f;
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
                if(velocity.X != 0)
                    currentFrame = (currentFrame + 9) % frameCount;
                // update the frame rectangle (only x-coordinate needed)
                frameRect.X = currentFrame * frameRect.Width;
                data = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(data);
	            frameElapsedTime = 0; // reset the elapsed counter
                          // checking for screen edge
            }

            if(position.X > GraphicsDevice.Viewport.Width)
                position.X = 0;
            if (position.X < 0)
                position.X = GraphicsDevice.Viewport.Width;

            base.Update(gameTime);
        }

        SpriteEffects direction = SpriteEffects.None;

        public override void Draw(GameTime gameTime)
        {
            if(velocity.X < 0)
                direction = SpriteEffects.FlipHorizontally;
            else if(velocity.X > 0)
                direction = SpriteEffects.None;

            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position, frameRect, manColor);
            _spriteBatch.Draw(texture, new Vector2(position.X - GraphicsDevice.Viewport.Width, position.Y), frameRect, manColor);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
