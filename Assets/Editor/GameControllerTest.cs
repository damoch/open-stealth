using System.Collections.Generic;
using Assets.Scripts.Modules;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor
{
    public class GameControllerTest
    {
    
        [Test]
        public void alertTest()
        {
            RoomManager gc = new RoomManager();
            gc.SetGlobalAlert();
        }

        [Test]
        [ExpectedException(typeof(UnityException))]
        public void loadNullGameTest()
        {
            GameController gc = new GameController();
            gc.LoadGame(null);
        }

        [Test]
        public void getScoreTest()
        {
            Dictionary<string, bool> items = new Dictionary<string, bool>();
            items.Add("foo", false);
            items.Add("bar", true);
            var result = ItemsTools.CountScore(items);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void getScoreEmptyTest()
        {
            Dictionary<string, bool> items = new Dictionary<string, bool>();
            var result = ItemsTools.CountScore(items);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void getScoreNullTest()
        {
            var result = ItemsTools.CountScore(null);
            Assert.AreEqual(0, result);
        }
    }
}

