using Assets.Scripts.Data;
using Assets.Scripts.Modules;
using NUnit.Framework;

namespace Assets.Editor
{
    public class SaveTest {

        [Test]
        public void SaveTest_() {
            SaveDataDto dto = new SaveDataDto("toom");

            SaveController.SaveGame("key", dto);
            Assert.AreEqual(SaveController.SavedGameExists("key"), true);
        }

        [Test]
        public void LoadTest()
        {
            var dto = new SaveDataDto("toom");
            SaveController.SaveGame("key", dto);

            var dto2 = SaveController.GetSavedGame("key");

            Assert.AreEqual(dto.RoomName, dto2.RoomName);
        }
    }
}
