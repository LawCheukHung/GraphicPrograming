using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lab02
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D bgTexture;
        private Rock[] rocks;
        private RunningMan rMan;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000/24f);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            rocks = new Rock[5];
            for(int i = 0; i < rocks.Length; i++)
            {
                rocks[i] = new Rock(this);
                Components.Add(rocks[i]);
            }

            rMan = new RunningMan(this);
            Components.Add(rMan);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            bgTexture = Content.Load<Texture2D>("Images\\background");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if(!CheckCollision())
                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(bgTexture, GraphicsDevice.Viewport.Bounds, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {

            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private bool CheckCollision()
        {
            Rectangle personRect = new Rectangle((int)rMan.position.X, (int)rMan.position.Y, rMan.frameRect.Width, rMan.frameRect.Height);
            
            foreach(Rock r in rocks)
            {
                Rectangle rockRect = new Rectangle(0, 0, r.texture.Width, r.texture.Height);
                Matrix transform = Matrix.CreateTranslation(new Vector3(-r.center, 0)) * Matrix.CreateRotationZ(r.rotateAngle) * Matrix.CreateTranslation(new Vector3(r.position, 0));
                rockRect = CalculateBoundingRectangle(rockRect, transform);

                if(personRect.Intersects(rockRect)) 
                    return true;
            }
            return false;
        }
    }
}
