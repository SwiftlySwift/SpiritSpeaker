using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SpiritSpeak.Combat.Test
{
    [TestClass]
    public class BattleTests
    {
        [TestMethod]
        public void TakeTurnsTestBasic()
        {
            var battle = new Battle()
            {
                Commanders = new List<Commander>()
                {
                    new Commander(1){ Initiative = 1 },
                    new Commander(2){ Initiative = 2 },
                }
            };

            battle.StartCombat();

            Assert.AreEqual(1, battle.CurrentInitiative);
            Assert.AreEqual(2, battle.MaxInitiative);

            battle.TakeTurn();
            Assert.AreEqual(2, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(1, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(2, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(1, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(2, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(1, battle.CurrentInitiative);
        }

        [TestMethod]
        public void TakeTurnsTestAdvanced()
        {
            var battle = new Battle()
            {
                Commanders = new List<Commander>()
                {
                    new Commander(1){ Initiative = 14 },
                    new Commander(0){ Initiative = 7 },
                }
            };

            battle.StartCombat();

            Assert.AreEqual(7, battle.CurrentInitiative);
            Assert.AreEqual(14, battle.MaxInitiative);

            battle.TakeTurn();
            Assert.AreEqual(14, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(7, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(14, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(7, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(14, battle.CurrentInitiative);
            battle.TakeTurn();
            Assert.AreEqual(7, battle.CurrentInitiative);
        }

        [TestMethod]
        public void DifferentCommanderTest()
        {
            var battle = new Battle()
            {
                Commanders = new List<Commander>()
                {
                    new Commander(1){ Initiative = 14 },
                    new DumbCommander(){ Initiative = 7 },
                }
            };

            battle.StartCombat();

            var dumbResult = battle.TakeTurn();
            var angryResult = battle.TakeTurn();
        }

        [TestMethod]
        public void SlowCommanderTest()
        {
            var battle = new Battle()
            {
                Commanders = new List<Commander>()
                {
                    new SlowCommander(){ Initiative = 7 },
                    new Commander(0){ Initiative = 14 },
                }
            };

            battle.StartCombat();

            Assert.AreEqual(7, battle.CurrentInitiative);

            var noResult = battle.TakeTurn();
            Assert.AreEqual(7, battle.CurrentInitiative);
            Assert.IsNull(noResult);

            var stillNoResult = battle.TakeTurn();
            Assert.AreEqual(7, battle.CurrentInitiative);
            Assert.IsNull(stillNoResult);

            var finallyAResult = battle.TakeTurn();
            Assert.AreEqual(14, battle.CurrentInitiative);

            var angryResult = battle.TakeTurn();
        }


        [TestMethod]
        public void AllyEnemyCheckTest()
        {
            var battle = new Battle()
            {
                Commanders = new List<Commander>()
                {
                    new Commander(0) { Initiative = 1 },
                    new Commander(1) { Initiative = 2 },
                    new Commander(-1) { Initiative = 3 },
                }
            };
            var spirit1 = new Spirit(battle, 1, 0, 0);
            var spirit2 = new Spirit(battle, 2, 0, 1);
            var spirit1friend = new Speaker(battle, 1);
            var spirit2friend = new Speaker(battle, 2);
            var jerk1 = new Spirit(battle, -1, 0, 2);
            var jerk2 = new Speaker(battle, -1);

            battle.Commanders[0].Spirits.Add(spirit1);
            battle.Commanders[0].Speakers.Add(spirit1friend);

            battle.Commanders[1].Spirits.Add(spirit2);
            battle.Commanders[1].Speakers.Add(spirit2friend);

            //battle.Commanders[2].Spirits.Add(jerk1);
            battle.Commanders[2].Speakers.Add(jerk2);


            var friends1 = battle.GetAllyTargets(1);
            var friends2 = battle.GetAllyTargets(2);
            var friends3 = battle.GetAllyTargets(-1);
            var enemies1 = battle.GetEnemyTargets(1);
            var enemies2 = battle.GetEnemyTargets(2);
            var enemies3 = battle.GetEnemyTargets(3);

            Assert.AreEqual(2, friends1.Count);
            Assert.AreEqual(2, friends2.Count);
            Assert.AreEqual(0, friends3.Count);

            Assert.AreEqual(4, enemies1.Count);
            Assert.AreEqual(4, enemies2.Count);
            Assert.AreEqual(6, enemies3.Count);

            Assert.IsTrue(friends1.Contains(spirit1));
            //Assert.IsTrue(friends1.Contains(spirit1friend));

            Assert.IsTrue(friends2.Contains(spirit2));
            //Assert.IsTrue(friends2.Contains(spirit2friend));

            Assert.IsTrue(enemies2.Contains(spirit1));
            //Assert.IsTrue(enemies2.Contains(spirit1friend));
            Assert.IsTrue(enemies2.Contains(jerk1));
            //Assert.IsTrue(enemies2.Contains(jerk2));

            Assert.IsTrue(enemies1.Contains(spirit2));
            //Assert.IsTrue(enemies1.Contains(spirit2friend));
            Assert.IsTrue(enemies1.Contains(jerk1));
            //Assert.IsTrue(enemies1.Contains(jerk2));

            Assert.IsTrue(enemies3.Contains(spirit2));
            //Assert.IsTrue(enemies3.Contains(spirit2friend));
            Assert.IsTrue(enemies3.Contains(jerk1));
            //Assert.IsTrue(enemies3.Contains(jerk2));
            Assert.IsTrue(enemies3.Contains(spirit1));
            //Assert.IsTrue(enemies3.Contains(spirit1friend));
        }

        [TestMethod]
        public void CombatTest()
        {
            var battle = new Battle()
            {
                Commanders = new List<Commander>()
                {
                    new Commander(1) { Initiative = 1 },
                    new Commander(2) { Initiative = 2 },
                }
            };
            var spirit1 = new Spirit(battle, 1, 0, 0) { Strength = 10, MaxVitality = 20, Vitality = 20};
            var spirit2 = new Spirit(battle, 2, 0, 1) { Strength = 5, MaxVitality = 30, Vitality = 30 }; ;
            var spirit1friend = new Speaker(battle, 1) { Vitality = 20 };
            var spirit2friend = new Speaker(battle, 2) { Vitality = 20 };

            battle.Commanders[0].Spirits.Add(spirit1);
            battle.Commanders[0].Speakers.Add(spirit1friend);

            battle.Commanders[1].Spirits.Add(spirit2);
            battle.Commanders[1].Speakers.Add(spirit2friend);

            battle.StartCombat();

            var turn1 = battle.TakeTurn();
            var turn2 = battle.TakeTurn();
        }

    }
}
