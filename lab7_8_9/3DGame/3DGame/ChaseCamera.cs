using Microsoft.Xna.Framework;


namespace _3DGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ChaseCamera : Microsoft.Xna.Framework.GameComponent
    {
        internal Vector3 position, up, right, lookAt, direction;
        internal Vector3 targetPosition, targetDirection, targetUp;
        internal Vector3 positionOffset, lookAtOffset;
        internal float fieldOfView = MathHelper.PiOver4, 
            aspectRatio = 3f/4f, farClip = 1000f, nearClip = 0.1f;
        internal Matrix view, project;

        public ChaseCamera(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            UpdatePosition();
            UpdateMatrices();
            base.Initialize();
        }

        private void UpdatePosition()
        {
            direction = targetDirection;
            up = targetUp;
            right = Vector3.Cross(up, targetDirection);

            Matrix transform = Matrix.Identity;
            transform.Forward = targetDirection;
            transform.Up = up;
            transform.Right = Vector3.Cross(up, targetDirection);

            position = targetPosition + Vector3.TransformNormal(positionOffset, transform);
            lookAt = targetPosition + Vector3.TransformNormal(lookAtOffset, transform);
        }

        private void UpdateMatrices()
        {
            view = Matrix.CreateLookAt(position, lookAt, up);
            project = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClip, farClip);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            UpdatePosition();
            UpdateMatrices(); 
            base.Update(gameTime);
        }
    }
}
