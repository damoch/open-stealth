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
        gc.SetGlobalAlert();
    }

    [Test]
    public void pauseTest()
    {
        GameController gc = new GameController();
        gc.Start();
        Assert.AreEqual(GameState.Paused, GameController.GameState);
    }

    [Test]
    public void runTest()
    {
        GameController gc = new GameController();
        gc.Start();
        gc.SetGameRunning();
        Assert.AreEqual(GameState.Running, GameController.GameState);
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

