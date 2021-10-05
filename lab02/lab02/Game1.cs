using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace lab02
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D bgTexture;
        private Rock[] rocks;
        private RunningMan rMan;
        private int rockCount = 5;

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
            rocks = new Rock[rockCount];

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
            if(CheckCollision())
                rMan.manColor = Color.Red;
            else 
            {
                rMan.manColor = Color.White;
                base.Update(gameTime);
            }
            
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
            Matrix personTransform = Matrix.CreateTranslation(new Vector3(rMan.position, 0));
            Rectangle personRect2 = new Rectangle((int)rMan.position.X - GraphicsDevice.Viewport.Width, (int)rMan.position.Y, rMan.frameRect.Width, rMan.frameRect.Height);
            Matrix personTransform2 = Matrix.CreateTranslation(new Vector3(rMan.position.X - GraphicsDevice.Viewport.Width, rMan.position.Y, 0));

            foreach (Rock r in rocks)
            {
                Rectangle rockRect = new Rectangle(0, 0, r.texture.Width, r.texture.Height);
                Matrix transform = Matrix.CreateTranslation(new Vector3(-r.center, 0)) * Matrix.CreateRotationZ(r.rotateAngle) * Matrix.CreateTranslation(new Vector3(r.position, 0));
                rockRect = CalculateBoundingRectangle(rockRect, transform);

                if (rockRect.Intersects(personRect))
                {
                    if (PixelCollision(personTransform, rMan.frameRect, rMan.texture.Width, ref rMan.data, transform, r.texture.Width, r.texture.Height, ref r.data))
                        return true;

                    if (PixelCollision(personTransform2, rMan.frameRect, rMan.texture.Width, ref rMan.data, transform, r.texture.Width, r.texture.Height, ref r.data))
                        return true;
                }
            }
            return false;
        }

        private bool PixelCollision(
            Matrix transformA, Rectangle rectA, int widthA, ref Color[] dataA,
            Matrix transformB, int widthB, int heightB, ref Color[] dataB)
        {
            Matrix AToB = transformA * Matrix.Invert(transformB);
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, AToB); Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, AToB);
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, AToB);
            for (int yA = 0; yA < rectA.Bottom; yA++)
            {   // For each row in A
                Vector2 posInB = yPosInB; // At the beginning of the row
                for (int xA = 0; xA < widthA; xA++)
                { // For each pixel in the row
                    int xB = (int)Math.Round(posInB.X); int yB = (int)Math.Round(posInB.Y);
                    if (0 <= xB && xB < widthB && 0 <= yB && yB < heightB)
                    {
                        Color colorA = dataA[xA + yA * widthA]; Color colorB = dataB[xB + yB * widthB];
                        if (colorA.A != 0 && colorB.A != 0) return true;
                    }
                    posInB += stepX; // Move to next pixel in the row
                }
                yPosInB += stepY; // Move to the next row
            }
            return false; // No intersection found
        }

    }
}
