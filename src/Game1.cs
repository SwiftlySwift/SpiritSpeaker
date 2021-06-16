using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Particles;
using Nez.Sprites;
using System;

namespace SpiritSpeak
{
    public class Game1 : Nez.Core
    {
        private Entity myFirstEntity;
        private Entity mySecondEntity;
        private Entity rotationAnchor;
        private Effect myEffect;

        public Game1()
        {
            //_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            Scene = new Scene();

            mySecondEntity = Scene.CreateEntity("GoodByeWorld");

            myFirstEntity = Scene.CreateEntity("HelloWorld");
            //var noteTex = Scene.Content.Load<Texture2D>("notes");
            //var textComponent = new SpriteRenderer(noteTex);//new TextComponent(Graphics.Instance.BitmapFont, "Hello World", Vector2.Zero, Color.White);
            //myFirstEntity.AddComponent(textComponent);
            //textComponent.OriginNormalized = new Vector2(.5f, .5f);
            //textComponent.Material = Material.StencilWrite();

            var tex = Scene.Content.Load<Texture2D>("notes");
            var lava = new SpriteRenderer(tex)
            {
                Color = Color.White,
            };

            //lava.Material = Material.StencilRead();
            //lava.Material = new Material();

            myEffect = Scene.Content.Load<Effect>("spriteEffect");
            lava.Material.Effect = myEffect;
            var noteTexture = Scene.Content.Load<Texture>("notes");

            mySecondEntity.AddComponent(lava);
            lava.OriginNormalized = new Vector2(0, 0);



            //var pConfig = new ParticleEmitterConfig();
            //pConfig.Sprite = Graphics.Instance.PixelTexture;
            //pConfig.Duration = 3000;
            //pConfig.EmissionRate = 100;
            //pConfig.AngleVariance = 30;
            //pConfig.MaxParticles = 100;
            //pConfig.ParticleLifespan = 1;
            //pConfig.StartParticleSize = 5;
            //pConfig.FinishParticleSize = 0;
            //pConfig.Speed = 100;
            //pConfig.StartColor = Color.Blue;
            //pConfig.StartColorVariance = Color.Red;

            //var particleEmitter = new ParticleEmitter(pConfig);

        }

        protected override void LoadContent()
        {
            //_spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var noteTexture = Scene.Content.Load<Texture>("notes");

            myFirstEntity.Position = Input.MousePosition;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
