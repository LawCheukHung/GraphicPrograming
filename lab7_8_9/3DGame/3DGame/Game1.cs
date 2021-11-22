using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Model groundModel;
        Matrix view, project;

        Windmill mill;
        Helicopter copter;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            view = Matrix.CreateLookAt((new Vector3(0, 0.9f, 0)), new Vector3(0, 0.9f, -0.5f), Vector3.Up);
            project = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000);
            mill = new Windmill(this, new Vector3(0, 0.9f, -4));
            mill.view = view;
            mill.project = project;
            Components.Add(mill);

            copter = new Helicopter(this, new Vector3(0, 0.9f, -4));
            copter.view = view;
            copter.project = project;
            Components.Add(copter);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            groundModel = Content.Load<Model>("ground");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            groundModel.Draw(Matrix.CreateScale(0.001f), view, project);

            base.Draw(gameTime);
        }
    }
}
