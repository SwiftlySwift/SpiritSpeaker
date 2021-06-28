using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Particles;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tweens;
using Nez.UI;
using SpiritSpeak.Combat;
using System;

namespace SpiritSpeak
{
    public class Game1 : Nez.Core
    {
        private Battle testBattle;

        private HumanCommander _player;

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

            var sprite = new SpriteRenderer(Scene.Content.Load<Texture2D>("grid"));
            Scene.CreateEntity("Grid").AddComponent(sprite);
            sprite.Origin = Vector2.Zero;

            testBattle = new Battle();

            var leftCommander = new HumanCommander(0)
            {
                Initiative = 1
            };
            _player = leftCommander;
            leftCommander.Spirits.Add(new Spirit(testBattle, 0, 0, 0)
            {
                MaxVitality = 25,
                Vitality = 25,
                Strength = 3,
                Movement = 2
            });
            //leftCommander.Spirits.Add(new Spirit(testBattle, 0, 0, 1)
            //{
            //    MaxVitality = 20,
            //    Vitality = 20,
            //    Strength = 6,
            //});
            testBattle.Commanders.Add(leftCommander);

            var rightCommander = new Commander(1);
            rightCommander.Spirits.Add(new Spirit(testBattle, 1, 2, 3)
            {
                MaxVitality = 20,
                Vitality = 20,
                Strength = 4,
            });
            //rightCommander.Spirits.Add(new Spirit(testBattle, 1, 4, 1)
            //{
            //    MaxVitality = 20,
            //    Vitality = 20,
            //    Strength = 3,
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

            var gridAnchor = new Vector2(10, 10);
            var sidx = 5;
            var gridTileSize = 80;
            foreach (var spirit in spirits)
            {
                var location = new Vector2(spirit.GridLocation.X * gridTileSize + gridTileSize/2, spirit.GridLocation.Y * gridTileSize + gridTileSize / 2) + gridAnchor;
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

            ReadPlayerInput();

            UpdateCombatSprites(gameTime);

            base.Update(gameTime);
        }

        private void ReadPlayerInput()
        {
            if (Input.LeftMouseButtonPressed)
            {
                var targetLocation = ((Input.MousePosition - new Vector2(10, 10)) / 80).ToPoint();

                if (Battle.OnTheGrid(targetLocation))
                {
                    var spirit = _player.Spirits[0];
                    var vector = spirit.GetApproachPath(targetLocation);
                    var targetSpirit = testBattle.Grid[targetLocation.X, targetLocation.Y].Spirit;

                    _player.BattleAction.Source = spirit;

                    if (targetSpirit != null && targetSpirit != spirit) //Stop hitting yourself
                    {
                        _player.BattleAction.Damage = spirit.Strength;
                        _player.BattleAction.Target = targetSpirit;

                        if (_player.BattleAction.Movements.Count == 0)
                        {
                            var approach = spirit.GetApproachPath(targetSpirit);
                            if (approach != null)
                            {
                                _player.BattleAction.Movements = approach.Movements;
                            }
                        }
                    }
                    else
                    {
                        _player.BattleAction.Movements = vector.Movements;
                    }
                }
            }
            if (Input.IsKeyPressed(Keys.Space))
            {
                _player.ActionConfirmed = true;
            }
        }

        private void UpdateCombatSprites(GameTime gameTime)
        {
            var gridAnchor = new Vector2(10, 10);
            var gridTileSize = 80;

            if (!Animating)
            {
                Timer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (Timer < 0)
                {
                    Timer = 1;

                    var battleResult = testBattle.TakeTurn();
                    if (battleResult != null && battleResult.Source != null)
                    {
                        var sourceEntity = Scene.FindEntity(battleResult.Source.Id.ToString());
                        var newLocation = new Vector2(battleResult.Source.GridLocation.X * gridTileSize + gridTileSize / 2, battleResult.Source.GridLocation.Y * gridTileSize + gridTileSize / 2) + gridAnchor;

                        var tween = sourceEntity.TweenLocalPositionTo(newLocation);

                        foreach (var damage in battleResult.DamageResults)
                        {
                            var source = Scene.FindEntity(damage.Source.Id.ToString());
                            var target = Scene.FindEntity(damage.Target.Id.ToString());

                            var damagePercent = ((float)damage.Target.Vitality / damage.Target.MaxVitality);
                            var color = new Color(1, damagePercent, damagePercent, 1);


                            tween.SetNextTween(source.TweenLocalPositionTo(target.LocalPosition).SetLoops(LoopType.PingPong));

                            target.GetComponent<SpriteRenderer>().TweenColorTo(color).SetDelay(.6f).Start();

                        }
                        tween.Start();
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
