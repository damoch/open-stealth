using UnityEngine;
using UnityEditor;

using NUnit.Framework;
using System.Collections.Generic;

public class GameControllerTest
{
    
    [Test]
    public void alertTest()
    {
        GameController gc = new GameController();
        gc.setGlobalAlert();
    }

    [Test]
    public void pauseTest()
    {
        GameController gc = new GameController();
        gc.Start();
        Assert.AreEqual(GameState.PAUSED, GameController.gameState);
    }

    [Test]
    public void runTest()
    {
        GameController gc = new GameController();
        gc.Start();
        gc.setGameRunning();
        Assert.AreEqual(GameState.RUNNING, GameController.gameState);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void loadNullGameTest()
    {
        GameController gc = new GameController();
        gc.loadGame(null);
    }

    [Test]
    public void getScoreTest()
    {
        Dictionary<string, bool> items = new Dictionary<string, bool>();
        items.Add("foo", false);
        items.Add("bar", true);
        var result = ItemsTools.countScore(items);
        Assert.AreEqual(1, result);
    }

    [Test]
    public void getScoreEmptyTest()
    {
        Dictionary<string, bool> items = new Dictionary<string, bool>();
        var result = ItemsTools.countScore(items);
        Assert.AreEqual(0, result);
    }

    [Test]
    public void getScoreNullTest()
    {
        var result = ItemsTools.countScore(null);
        Assert.AreEqual(0, result);
    }
}

