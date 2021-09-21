﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lab02
{
    class Rock : DrawableGameComponent
    {
        private Texture2D texture;
        private Vector2 position, center, velocity;
        private float rotateAngle, rotateSpeed;
        private SpriteBatch _spriteBatch;
        public static Random r = new Random();

        public Rock(Game g) : base(g) { }

        public override void Initialize()
        {
            position.X = r.Next(GraphicsDevice.Viewport.Width);
            position.Y = 0;
            velocity.X = 0;
            velocity.Y = (float)(r.NextDouble() * 2 + 0.5f);
            rotateSpeed = velocity.Y / 10;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("Images\\asteroid");
            center.X = texture.Width / 2;
            center.Y = texture.Height / 2;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            rotateAngle = (rotateAngle + rotateSpeed) % MathHelper.TwoPi;
            if(position.Y > 225)
            {
                Initialize();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position, null, Color.White, rotateAngle, center, 1, SpriteEffects.None, 0);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
