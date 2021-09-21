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
    }
}
