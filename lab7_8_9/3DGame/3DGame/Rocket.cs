using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace _3DGame

{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Rocket : Microsoft.Xna.Framework.DrawableGameComponent
    {
        internal Vector3 position;            
        internal Vector3 speed, previousPosition, startPosition; 
        Matrix directionMatrix;            // direction transformations
        
        internal bool active;                     // visibility
        float boundary;                   // edge of world on X and Z
        Model model;
        internal ChaseCamera cam;

        public Rocket(Game game, float border) : base(game)
        {
            // TODO: Construct any child components here
            boundary = border;
            active = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("rocket");
        }


        public void Launch(Vector3 look, Vector3 start)
        {
            previousPosition = 
                position = startPosition = start;   // start at camera
            speed = 25.0f * Vector3.Normalize(look); // unitize direction
            active = true;                    // make visible
        }

        private void SetDirectionMatrix()
        {
            Vector3 Look = Vector3.Normalize(speed);

            Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f); // fake Up to get Right
            Vector3 Right = Vector3.Cross(Up, Look);
            Right.Normalize();

            Up = Vector3.Cross(Look, Right);               // calculate Up with
            Up.Normalize();                                // correct vectors

            Matrix matrix = Matrix.Identity;                 // compute direction matrix
            matrix.Right = Right;
            matrix.Up = Up;
            matrix.Forward = Vector3.Normalize(speed);
            directionMatrix = matrix;
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (active)
            {
                previousPosition = position;
                Vector3 distanceTravelled = position - startPosition;
                float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //if (distanceTravelled.Length() > 2.5f)
                //{
                    // gravity time starts ticking

                    const float GRAVITY = 9.8f;             // gravity constant and scaling
 
                    speed.Y -= GRAVITY * seconds;
                    position += speed * seconds;
                //}
                //else
                //{
                //    position += speed * seconds;
                //    //startPosition = position;
                //}
                
                SetDirectionMatrix();   // rocket direction considers speed
                if (position.Y < -0.5f) // de-activate if below ground
                    active = false;
                if (position.X > boundary || position.X < -boundary ||
                    position.Z > boundary || position.Z < -boundary)
                    active = false;

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (active)
            {
                Matrix world = Matrix.CreateScale(0.4f) *
                       Matrix.CreateRotationX(-MathHelper.PiOver2) *
                       directionMatrix * Matrix.CreateTranslation(position);
                model.Draw(world, cam.view, cam.project);
                base.Draw(gameTime);
            }
        }
    }
}
