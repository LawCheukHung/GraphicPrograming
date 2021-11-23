using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DGame
{
    class Windmill : DrawableGameComponent
    {
        internal Model baseModel, fanModel;
        internal Vector3 position;
        float rotateAngle, rotateSpeed = 0.02f;
        internal Matrix view, project;

        public Windmill(Game game, Vector3 pos) : base(game)
        {
            position = pos;
        }

        protected override void LoadContent()
        {
            baseModel = Game.Content.Load<Model>("base");
            fanModel = Game.Content.Load<Model>("fan");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            rotateAngle = (rotateAngle + rotateSpeed) % MathHelper.TwoPi;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix world;
            world = Matrix.CreateScale(10) * Matrix.CreateTranslation(position);
            baseModel.Draw(world, view, project);
            world = Matrix.CreateScale(10) * Matrix.CreateRotationZ(rotateAngle) * Matrix.CreateTranslation(position);
            fanModel.Draw(world, view, project);

            base.Draw(gameTime);
        }
    }
}
