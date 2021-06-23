using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Particles;
using Nez.Sprites;
using Nez.Textures;
using SpiritSpeak.Combat;
using System;

namespace SpiritSpeak
{
    public class Game1 : Nez.Core
    {
        private Entity myFirstEntity;
        private Entity mySecondEntity;

        private Battle testBattle;

        private double Timer = 2d;
        private bool Animating = false;

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

            testBattle = new Battle();

            var leftCommander = new Commander(0)
            {
                Initiative = 1
            };
            leftCommander.Spirits.Add(new Spirit(testBattle, 0,0,0)
            {
                MaxVitality = 20,
                Vitality = 20,
                Strength = 5,
            });
            //leftCommander.Spirits.Add(new Spirit(testBattle, 0, 0, 1)
            //{
            //    MaxVitality = 20,
            //    Vitality = 20,
            //    Strength = 6,
            //});
            testBattle.Commanders.Add(leftCommander);

            var rightCommander = new Commander(1);
            rightCommander.Spirits.Add(new Spirit(testBattle, 1, 4, 4)
            {
                MaxVitality = 20,
                Vitality = 20,
                Strength = 15,
            });
            //rightCommander.Spirits.Add(new Spirit(testBattle, 1, 4, 1)
            //{
            //    MaxVitality = 20,
            //    Vitality = 20,
            //    Strength = 25,
            //});
            testBattle.Commanders.Add(rightCommander);

            testBattle.StartCombat();

            SetupBattle(testBattle);
        }
        private void SetupBattle(Battle testBattle)
        {
            var texture = Content.Load<Texture2D>("Sprites");
            var sprites = Sprite.SpritesFromAtlas(texture, 84, 80);

            var spirits = testBattle.Spirits;

            var gridAnchor = new Vector2(100, 100);
            var sidx = 5;
            var gridTileSize = 90;
            foreach (var spirit in spirits)
            {
                var location = new Vector2(spirit.GridLocation.X * gridTileSize, spirit.GridLocation.Y * gridTileSize) + gridAnchor;
                var entity = Scene.CreateEntity($"{spirit.Id}", location);
                var spriteRenderer = new SpriteRenderer(sprites[sidx++]);
                entity.AddComponent(spriteRenderer);
            }
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

            var gridAnchor = new Vector2(100, 100);
            var gridTileSize = 90;


            if (!Animating)
            {
                Timer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (Timer < 0)
                {
                    Timer = 1;

                    var battleResult = testBattle.TakeTurn();
                    if (battleResult.Source != null)
                    {
                        var sourceEntity = Scene.FindEntity(battleResult.Source.Id.ToString());
                        var newLocation = new Vector2(battleResult.Source.GridLocation.X * gridTileSize, battleResult.Source.GridLocation.Y * gridTileSize) + gridAnchor;

                        sourceEntity.TweenLocalPositionTo(newLocation).Start();
                    }
                }
            }

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
