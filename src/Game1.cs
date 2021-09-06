﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
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
        private Battle testBattle;

        private HumanCommander _player;

        private double Timer = 2d;
        private bool Animating = false;

        private Dictionary<string, ITween<Vector2>> _currentTweens;


        private int _gridTileSize = 80;
        private Vector2 _gridAnchor = new Vector2(10, 10);

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

            var leftCommander = new Commander(0)
            {
                Initiative = 1
            };
            //_player = leftCommander;
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

                }
            }
            if (Input.IsKeyPressed(Keys.Space))
            {
                //_player.ActionConfirmed = true;
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
            var newLocation = new Vector2(a.Target.GridLocation.X * _gridTileSize + _gridTileSize / 2, a.Target.GridLocation.Y * _gridTileSize + _gridTileSize / 2 + 5) + _gridAnchor;

            var tween = sourceEntity.TweenLocalPositionTo(newLocation).SetLoops(LoopType.PingPong,3);

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
            //throw new NotImplementedException();
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
