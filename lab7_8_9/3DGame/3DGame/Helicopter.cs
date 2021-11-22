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
    class Helicopter : DrawableGameComponent
    {
        Model model;
        Vector3 rotateCenter;
        float rotateY, rotateZ = MathHelper.ToRadians(30), rotateSpeed = 0.05f;
        Vector3 offset = new Vector3(0, 0, -1);
        internal Matrix view, project;

        public Helicopter(Game g, Vector3 pos) : base(g)
        {
            rotateCenter = pos;
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("Helicopter");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            rotateY = (rotateY + rotateSpeed) % MathHelper.TwoPi;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix world = Matrix.CreateScale(0.05f) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateTranslation(offset) * Matrix.CreateRotationY(rotateY) * Matrix.CreateRotationZ(rotateZ) * Matrix.CreateTranslation(rotateCenter);
            model.Draw(world, view, project);

            base.Draw(gameTime);
        }
    }
}
