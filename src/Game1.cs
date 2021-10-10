using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.BitmapFonts;
using Nez.Particles;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tweens;
using Nez.UI;
using SpiritSpeak.Combat;
using SpiritSpeak.Combat.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak
{
    public class Game1 : Nez.Core
    {
        private static int ScreenWidth = 1400;
        private static int ScreenHeight = 900;

        private Battle testBattle;

        private HumanCommander _player;

        private double Timer = 2d;
        private bool Animating = false;

        private Dictionary<string, ITween<Vector2>> _currentTweens;


        private int _gridTileSize = 80;
        private Vector2 _gridAnchor;
        private int gridSize = 5;




        public Game1()
        {
            _gridAnchor = new Vector2(ScreenWidth/2 - ((gridSize)*_gridTileSize / 2), _gridTileSize + 10);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Screen.SetSize(ScreenWidth, ScreenHeight);

            Scene = new Scene();
            //Scene.SetDesignResolution(100, 100, Scene.SceneResolutionPolicy.BestFit);

            var bg_sprite = new SpriteRenderer(Scene.Content.Load<Texture2D>("galaxy"));
            var bg = Scene.CreateEntity("__bg").AddComponent(bg_sprite);
            bg_sprite.Origin = Vector2.Zero;

            var sprite = new SpriteRenderer(Scene.Content.Load<Texture2D>("grid"));
            var grid = Scene.CreateEntity("Grid", _gridAnchor - new Vector2(10,10));
            grid.AddComponent(sprite);
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

            SetupUI(testBattle);

        }
        private void SetupUI(Battle testBattle)
        {
            var font = new BitmapFont();
            font.Load("Content/Georgia.fnt");
            font.Initialize(true);
            Graphics.Instance.BitmapFont = font;
            Table table = BuildTurnOrderUI(testBattle);
            BuildInfoUI(testBattle);
            BuildMoveUI(testBattle);

            // if creating buttons with just colors (PrimitiveDrawables) it is important to explicitly set the minimum size since the colored textures created
            // are only 1x1 pixels
            var button = new Button(ButtonStyle.Create(Color.Black, Color.DarkGray, Color.Green));
            button.OnClicked += (x => _player.ActionConfirmed = true);
            table.Add(button).SetMinWidth(100).SetMinHeight(70);
        }

        private static Table BuildInfoUI(Battle testBattle)
        {
            var texture = Content.Load<Texture2D>("Sprites");
            var sprites = Sprite.SpritesFromAtlas(texture, 84, 80);

            var entity = Scene.CreateEntity($"UI-InfoBox");
            var canvas = entity.AddComponent<UICanvas>();
            var table = canvas.Stage.AddElement(new Table());

            table.SetDebug(true);
            table.SetFillParent(false);
            table.SetSize(300, 300);

            table.Left().Top();
            table.SetBackground(new SpriteDrawable(Graphics.Instance.PixelTexture));
            table.SetColor(Color.LightGray);

            table.Add(new Label("Info goes here", Graphics.Instance.BitmapFont, Color.Black)).SetMinHeight(40).SetMinWidth(40);

            return table;
        }

        private static Table BuildMoveUI(Battle testBattle)
        {
            var texture = Content.Load<Texture2D>("Sprites");
            var sprites = Sprite.SpritesFromAtlas(texture, 84, 80);

            var entity = Scene.CreateEntity($"UI-MoveInfoBox");
            var canvas = entity.AddComponent<UICanvas>();
            var Outertable = canvas.Stage.AddElement(new Table());
            var table = new Table();

            Outertable.SetDebug(true);

            Outertable.SetFillParent(true);
            Outertable.Bottom();

            var defaults = Outertable.GetRowDefaults();
            defaults.SetMinHeight(80).SetMinWidth(80).SetPadRight(5).SetPadBottom(5);
            Outertable.Add("Move1");
            Outertable.Add("Move2");
            Outertable.Add("Move3");
            Outertable.Add("Move4");

            Outertable.Row();

            Outertable.Add(table).SetMinWidth(500).SetMinHeight(200).SetColspan(Outertable.GetColumns());
            table.SetDebug(true);
            table.SetFillParent(false);
            table.SetSize(500, 200);
            table.SetBackground(new SpriteDrawable(Graphics.Instance.PixelTexture));
            table.SetColor(Color.LightGreen);
            table.Top();

            table.Add(new Label("Move info goes here", Graphics.Instance.BitmapFont, Color.Black));

            return table;
        }

        private static Table BuildTurnOrderUI(Battle testBattle)
        {
            var texture = Content.Load<Texture2D>("Sprites");
            var sprites = Sprite.SpritesFromAtlas(texture, 84, 80);

            var entity = Scene.CreateEntity($"UI-TurnOrder");
            var canvas = entity.AddComponent<UICanvas>();
            var table = canvas.Stage.AddElement(new Table());

            table.SetDebug(true);
            table.SetFillParent(true);

            table.Right().Top();

            table.Add("Portrait");
            table.Add("Health");
            //table.Add("Spirit").SetMinHeight(30);
            table.Row();

            var sidx = 0;
            foreach (var spirit in testBattle.Spirits)
            {
                sidx++;
                var image = new Image(sprites[sidx]);
                table.Add(image);
                var health = new ProgressBar(0, 1, 0.025f, false, ProgressBarStyle.Create(Color.Green, Color.Red, 40));
                health.SetValue(1);
                //health.Tween("Value",0f, 5f).SetLoops(LoopType.PingPong, -1).SetEaseType(EaseType.Linear).Start();
                var mana = new ProgressBar(0, 1, 0.025f, false, ProgressBarStyle.Create(Color.Aqua, Color.Gray, 40));
                mana.SetValue(1);
                //mana.Tween("Value", 0f, 5f).SetLoops(LoopType.PingPong, -1).SetEaseType(EaseType.Linear).Start();
                var stack = new Table();
                stack.DebugAll();

                stack.Add(health).SetMinHeight(40);
                stack.Row();
                stack.Add(mana).SetMinHeight(40);
                table.Add(stack);

                table.Row();
            }

            return table;
        }

        private void SetupBattle(Battle testBattle)
        {
            var spirits = testBattle.Spirits;

            var sidx = 5;
            var idx = 0;
            foreach (var spirit in spirits)
            {
                var location = GetGridPositionInPixels(spirit.GridLocation);
                //var locationUI = GetGridPositionInPixels(new Point(7, idx));

                CreateSpriteEntity(sidx, spirit.Id.ToString(), location);
                //CreateSpriteEntity(sidx, $"{spirit.Id}-Faceplate", locationUI);

                idx++;
                sidx++;
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

                }
            }
            if (Input.IsKeyPressed(Keys.Space))
            {
                //_player.ActionConfirmed = true;
            }
        }

        private void UpdateCombatSprites(GameTime gameTime)
        {
            if (!Animating)
            {
                Timer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (Timer < 0)
                {
                    Timer = 3;

                    var battleResult = testBattle.TakeTurn();

                    if (battleResult == null)
                    {
                        return;
                    }

                    _currentTweens = new Dictionary<string, ITween<Vector2>>();

                    //First process movements that aren't Shoves
                    foreach (var a in battleResult.MovementResults.Where(x => !x.Shove))
                    {
                        HandleMoves(a);
                    }

                    //Calculate animations
                    foreach (var a in battleResult.AnimationResults)
                    {
                        HandleAnimations(a);
                    }

                    //Calculate if/how terrain changes
                    foreach (var a in battleResult.TerrainResults)
                    {
                        HandleTerrainChanges(a);
                    }

                    //Lastly process movements that are Shoves
                    foreach (var m in battleResult.MovementResults.Where(x=> x.Shove))
                    {
                        HandleShoves(m);
                    }

                    //Deal damage
                    foreach (var a in battleResult.DamageResults)
                    {
                        HandleDamage(a);
                    }
                }
            }
        }

        private void HandleDamage(DamageResult a)
        {
            var id = a.Target.Id.ToString();
            var sourceEntity = Scene.FindEntity(id);
            //var faceplate = Scene.FindEntity($"{id}-Faceplate");
            var newLocation = new Vector2(a.Target.GridLocation.X * _gridTileSize + _gridTileSize / 2 + 5, a.Target.GridLocation.Y * _gridTileSize + _gridTileSize / 2) + _gridAnchor;

            var tween = sourceEntity.TweenLocalPositionTo(newLocation,.05f).SetDelay(.3f).SetLoops(LoopType.PingPong,3);

            var white = new Vector4(1, 1, 1, 1);
            var red = new Vector4(1, 0, 0, 1);
            var blend = white * a.Target.PercentVitality + red * (1 - a.Target.PercentVitality);
            var blendedColor = new Color(blend);

            //faceplate.GetComponent<SpriteRenderer>().TweenColorTo(blendedColor).Start(); //Setup a known tween for color modulated objects and call jump to elapsed time when changing the color?

            AddPositionTweenToEntity(id, tween);
        }

        private void HandleShoves(MovementResult a)
        {
            throw new NotImplementedException();
        }

        private void HandleTerrainChanges(TerrainResult a)
        {
            throw new NotImplementedException();
        }

        private void HandleAnimations(AnimationResult a)
        {
            if  (a.Animation == AnimationType.Throw)
            {
                var origin = GetGridPositionInPixels(a.Source.GridLocation);
                foreach (var target in a.Targetting.DirectTargets)
                {
                    var entity = CreateSpriteEntity(a.SpriteId, Guid.NewGuid().ToString(), new Vector2(-10000,-10000));
                    
                    var destination = GetGridPositionInPixels(target.GridLocation);

                    entity.TweenLocalPositionTo(origin,0.0001f).SetDelay(a.DelayInSeconds)
                        .SetNextTween(entity.TweenLocalPositionTo(destination).SetEaseType(EaseType.BounceOut)
                        .SetCompletionHandler(x => entity.Destroy()))
                        .Start();
                }
            }
            else if (a.Animation == AnimationType.DoubleBonk)
            {

            }
            else if (a.Animation == AnimationType.Shove)
            {

            }
        }

        private void HandleMoves(MovementResult a)
        {
            var id = a.Source.Id.ToString();
            var sourceEntity = Scene.FindEntity(id);
            var newLocation = new Vector2(a.Source.GridLocation.X * _gridTileSize + _gridTileSize / 2, a.Source.GridLocation.Y * _gridTileSize + _gridTileSize / 2) + _gridAnchor;

            var tween = sourceEntity.TweenLocalPositionTo(newLocation);

            AddPositionTweenToEntity(id, tween);
        }

        private void AddPositionTweenToEntity(string id, ITween<Vector2> tween)
        {
            if (_currentTweens.TryGetValue(id, out var ogTween))
            {
                ogTween.SetNextTween(tween);
                _currentTweens[id] = tween;
            }
            else
            {
                _currentTweens.Add(id, tween);
                tween.Start();
            }
        }

        private Vector2 GetGridPositionInPixels(Point p)
        {
            return new Vector2(p.X * _gridTileSize + _gridTileSize / 2, p.Y * _gridTileSize + _gridTileSize / 2) + _gridAnchor;
        }
        private Entity CreateSpriteEntity(int spriteId, string entityId, Vector2 location)
        {
            var texture = Content.Load<Texture2D>("Sprites");
            var sprites = Sprite.SpritesFromAtlas(texture, 84, 80);

            var entity = Scene.CreateEntity($"{entityId}", location);
            var spriteRenderer = new SpriteRenderer(sprites[spriteId]);
            entity.AddComponent(spriteRenderer);
            return entity;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
