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
    class HelicopterControllable : DrawableGameComponent
    {
        internal Vector3 direction, position, right, up; // additional directions is added
        Vector3 initPosition; // stores the initial position, for resetting the position
        Model model;
        internal ChaseCamera cam; // variable to store the chase camera
        float yaw, pitch, speed; // rotate angle along y and x axes, and the speed of airship
        const float pitchRate = 0.01f;
        const float yawRate = 0.025f;
        const float forwardSpeed = 0.025f;

        public HelicopterControllable(Game g, Vector3 pos) : base(g)
        {
            position = initPosition = pos;
            up = Vector3.Up;
            direction = -Vector3.Forward;
            right = Vector3.Cross(direction, up);
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("Helicopter");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Left)) yaw = yawRate;
            else if (kb.IsKeyDown(Keys.Right)) yaw = -yawRate;
            else yaw = 0;

            if (kb.IsKeyDown(Keys.Up)) pitch = pitchRate;
            else if (kb.IsKeyDown(Keys.Down)) pitch = -pitchRate;
            else pitch = 0;

            if (kb.IsKeyDown(Keys.Space)) speed = 4 * forwardSpeed;
            else speed = forwardSpeed;

            Matrix transform = Matrix.CreateFromAxisAngle(right, pitch) * Matrix.CreateRotationY(yaw);

            up = Vector3.Normalize(Vector3.TransformNormal(up, transform));
            direction = Vector3.Normalize(Vector3.TransformNormal(direction, transform));
            right = Vector3.Normalize(Vector3.Cross(direction, up));
            up = Vector3.Normalize(Vector3.Cross(right, direction));

            position += direction * speed;

            if (position.Y <= 0) position.Y = 0;

            if (kb.IsKeyDown(Keys.R))
            {
                position = initPosition;
                direction = -Vector3.Forward;
                up = Vector3.Up;
            }

            base.Update(gameTime);
        }

        private Matrix YDirection()
        {
            Matrix rotationY = Matrix.Identity;
            rotationY.Forward = direction;
            rotationY.Up = up;
            rotationY.Right = right;
            rotationY.Translation = position;
            return rotationY;
        }


        public override void Draw(GameTime gameTime)
        {
            Matrix transform = Matrix.CreateScale(0.05f) * YDirection() * Matrix.CreateTranslation(position);
            model.Draw(transform, cam.view, cam.project);

            base.Draw(gameTime);
        }
    }
}
