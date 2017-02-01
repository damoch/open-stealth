using UnityEngine;
using UnityEditor;

using NUnit.Framework;

public class SaveTest {

	[Test]
	public void saveTest() {
        SaveDataDTO dto = new SaveDataDTO("toom");

        SaveController.saveGame("key", dto);
        Assert.AreEqual(SaveController.savedGameExists("key"), true);
    }

    [Test]
    public void loadTest()
    {
        var dto = new SaveDataDTO("toom");
        SaveController.saveGame("key", dto);

        var dto2 = SaveController.getSavedGame("key");

        Assert.AreEqual(dto.RoomName, dto2.RoomName);
    }
}
