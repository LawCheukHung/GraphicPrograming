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
        //Matrix view, project;
        ChaseCamera cam;
        Windmill mill;
        Helicopter copter;
        HelicopterControllable player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            cam = new ChaseCamera(this);
            cam.up = Vector3.Up;
            cam.fieldOfView = MathHelper.PiOver4;
            cam.aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            cam.nearClip = 0.1f;
            cam.farClip = 1000;
            cam.positionOffset = new Vector3(0, 0, -4); // cam position relative to windmill
            cam.lookAtOffset = Vector3.Zero; // cam look at position relative to windmill
            cam.targetUp = Vector3.Up;
            cam.targetPosition = new Vector3(0.0f, 0.9f, -4.0f); // windmill position
            cam.targetDirection = -Vector3.Forward;
            Components.Add(cam);

            // TODO: Add your initialization logic here
            //view = Matrix.CreateLookAt((new Vector3(0, 0.9f, 0)), new Vector3(0, 0.9f, -0.5f), Vector3.Up);
            //project = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100.0f);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            mill = new Windmill(this, new Vector3(0, 0.9f, -4));
            mill.view = cam.view;
            mill.project = cam.project;
            Components.Add(mill);

            copter = new Helicopter(this, new Vector3(0, 0.9f, -4));
            copter.view = cam.view;
            copter.project = cam.project;
            Components.Add(copter);

            player = new HelicopterControllable(this, new Vector3(0, 1, -6));
            player.cam = cam;
            Components.Add(player);

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
            mill.view = cam.view;
            mill.project = cam.project;
            copter.view = cam.view;
            copter.project = cam.project;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            groundModel.Draw(Matrix.CreateScale(0.001f), cam.view, cam.project);

            base.Draw(gameTime);
        }
    }
}
