using UnityEngine;
using UnityEditor;

using NUnit.Framework;

public class SaveTest {

	[Test]
	public void saveTest() {
        SaveDataDto dto = new SaveDataDto("toom");

        SaveController.SaveGame("key", dto);
        Assert.AreEqual(SaveController.SavedGameExists("key"), true);
    }

    [Test]
    public void loadTest()
    {
        var dto = new SaveDataDto("toom");
        SaveController.SaveGame("key", dto);

        var dto2 = SaveController.GetSavedGame("key");

        Assert.AreEqual(dto.RoomName, dto2.RoomName);
    }
}
