using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Prefabs.WorlsObjects;
using UnityEngine;
#if UNITY_EDITOR
#endif


namespace Assets.Scripts.Modules
{
    public class GameController : MonoBehaviour {



        public static GameState GameState { get; set; }

        public static Dictionary<string, bool> ItemsAquired { get; set; }

        private RoomManager _roomManager;
        public void Start ()
        {
            _roomManager = gameObject.GetComponent<RoomManager>();
            string continueFlag = SaveController.GetContinueFlag();
            if (!continueFlag.Equals("NONE"))
            {
                LoadGame(continueFlag);
            }

            if(ItemsAquired == null)
            {
                ItemsAquired = ItemsTools.GetItemsList();
            }
        }
	
  
        public static void RestartRoom()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }



        public void SaveGame(string saveName)
        {
            SaveController.SaveGame(saveName, GetGameData());
        }

        public void LoadGame(string saveName)
        {
            if (SaveController.SavedGameExists(saveName))
            {
                SetGameData(SaveController.GetSavedGame(saveName), saveName);
            }
            else
                throw new UnityException("No save data!");
        }



        public void SetGamePaused()
        {
            GameState = GameState.Paused;
            SoundManager.Instance.MusicSource.Pause();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;
        }

        public void SetGameRunning()
        {
            GameState = GameState.Running;
            SoundManager.Instance.MusicSource.UnPause();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }

        internal void GameFinished()
        {
            SetGamePaused();
        }

        SaveDataDto GetGameData()
        {
            var dto = _roomManager.GetRoomData();
            dto.ItemsAquired = ItemsAquired;
            return dto;
        }

        public void GameOver()
        {
            SetGamePaused();
        }

        void SetGameData(SaveDataDto dto, string saveName)
        {
            if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(dto.RoomName))
            {
                SaveController.SetContinueFlag(saveName);
                UnityEngine.SceneManagement.SceneManager.LoadScene(dto.RoomName);
            }
            else
            {
                SaveController.SetContinueFlag("NONE");
                ItemsAquired = dto.ItemsAquired;
                ItemsTools.SetupItemsForScene(ItemsAquired);
                _roomManager.SetRoomData(dto);
            }

        }

        public static void SetItemFlag(string key)
        {
            ItemsAquired[key] = true;
        }


        void Update()
        {
            if (Input.GetKey(KeyCode.F5))
            {
                SaveGame("QUICK");
                Debug.Log("Game saved");
            }

            if (Input.GetKey(KeyCode.F6))
            {
                LoadGame("QUICK");
                Debug.Log("Game loaded");
            }
        }
    }
}
