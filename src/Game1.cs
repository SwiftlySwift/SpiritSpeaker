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
            leftCommander.Spirits.Add(new Spirit(testBattle, 0)
            {
                MaxVitality = 20,
                Vitality = 20,
                Strength = 5,
            });
            leftCommander.Spirits.Add(new Spirit(testBattle, 0)
            {
                MaxVitality = 20,
                Vitality = 20,
                Strength = 6,
            });
            testBattle.Commanders.Add(leftCommander);

            var rightCommander = new Commander(1);
            rightCommander.Spirits.Add(new Spirit(testBattle, 1)
            {
                MaxVitality = 20,
                Vitality = 20,
                Strength = 15,
            });
            rightCommander.Spirits.Add(new Spirit(testBattle, 1)
            {
                MaxVitality = 20,
                Vitality = 20,
                Strength = 25,
            });
            testBattle.Commanders.Add(rightCommander);

            testBattle.StartCombat();

            SetupBattle(testBattle);
        }
        private void SetupBattle(Battle testBattle)
        {
            var texture = Content.Load<Texture2D>("Sprites");
            var sprites = Sprite.SpritesFromAtlas(texture, 84, 80);

            var leftSideSpirits = testBattle.Commanders[0].Spirits;
            var rightSideSpirits = testBattle.Commanders[1].Spirits;

            var leftAnchor = new Vector2(100, 100);
            var rightAnchor = new Vector2(300, 100);
            var sidx = 5;
            foreach (var spirit in leftSideSpirits)
            {
                var entity = Scene.CreateEntity($"{spirit.Id}", leftAnchor);
                var spriteRenderer = new SpriteRenderer(sprites[sidx++]);
                entity.AddComponent(spriteRenderer);
                leftAnchor = leftAnchor + new Vector2(0, 85);
            }

            foreach (var spirit in rightSideSpirits)
            {
                var entity = Scene.CreateEntity($"{spirit.Id}", rightAnchor);
                var spriteRenderer = new SpriteRenderer(sprites[sidx++]);
                entity.AddComponent(spriteRenderer);
                rightAnchor = rightAnchor + new Vector2(0, 85);
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

            if (!Animating)
            {
                Timer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (Timer < 0)
                {
                    Timer = 4;
                    //Animating = true;

                    var battleResult = testBattle.TakeTurn();

                    foreach(var damageResult in battleResult.DamageResults)
                    {
                        var sourceId = damageResult.Source.Id;
                        var targetId = damageResult.Target.Id;

                        var sourceEntity = Scene.FindEntity(sourceId.ToString());
                        var targetEntity = Scene.FindEntity(targetId.ToString());

                        sourceEntity.Tween("LocalPosition", targetEntity.LocalPosition, 2).SetLoops(Nez.Tweens.LoopType.PingPong).Start();
                        targetEntity.Tween("LocalPosition", targetEntity.LocalPosition + new Vector2(0, -50), 1).SetLoops(Nez.Tweens.LoopType.PingPong).SetDelay(1).Start();
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
