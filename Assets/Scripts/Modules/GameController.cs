using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;


public class GameController : MonoBehaviour {

    static GameObject[] _guards;


    public static GameState GameState { get; set; }
    public static GameObject Player { get; set; }

    public static Dictionary<string, bool> ItemsAquired { get; set; }


    public void Start () {
        _guards = GameObject.FindGameObjectsWithTag("Guard");
        Player = GameObject.FindGameObjectsWithTag("PlayerObject")[0];

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
	
    public void SetGlobalAlert()
    {
        if (_guards != null)
        {
            foreach (GameObject guard in _guards)
            {
                var guardController = guard.GetComponent<GuardController>();
                if (!guardController.State.Equals(GuardState.Alerted))
                    guardController.SetAlert(Player.transform.position);

            }
        }

    }

    public void CheckForOtherGuardsState()
    {

        if (AnyGuardSeesPlayer())
        {
            SetGlobalAlert();
        }
        else
        {
            SetGlobalSuspicious();
        }
        
    }
    bool AnyGuardSeesPlayer()
    {
        if (_guards != null)
        {
            foreach (GameObject guard in _guards)
            {
                var guardController = guard.GetComponent<GuardController>();
                if (guardController.FieldOfView.PlayerInRange)
                {
                    return true;

                }
            }
        }
        return false;
    }


    
    public void SetGlobalSuspicious()
    {
        StartCoroutine("Evasion");
        if (_guards != null)
        {
            foreach (GameObject guard in _guards)
            {
                var guardController = guard.GetComponent<GuardController>();
                if (!guardController.State.Equals(GuardState.Suspicious))
                    guardController.SetSuspicious(Player.transform.position, true);

            }
        }
    }

    public static void RestartRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void SetGlobalCalm()
    {
        if (_guards != null)
        {
            foreach (GameObject guard in _guards)
            {
                var guardController = guard.GetComponent<GuardController>();
                if (!guardController.State.Equals(GuardState.Calm))
                    guardController.SetCalm(guardController.NavPoint != null ? guardController.NavPoint.transform.position 
                        : guardController.transform.position);

            }
        }
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

    IEnumerator Evasion()
    {
        yield return new WaitForSeconds(5f);
        if (AnyGuardSeesPlayer())
        {
            SetGlobalAlert();

        }
        else
        {
            SetGlobalCalm();
        }

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
        var dto = new SaveDataDto();
        dto.RoomName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        dto.PlayerPosition = Player.transform.position;
        dto.ItemsAquired = ItemsAquired;
        dto.Items = Player.GetComponent<PlayerController>().Keys;
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
            Player.transform.position = dto.PlayerPosition;
            Player.GetComponent<PlayerController>().Keys = dto.Items;
            ItemsAquired = dto.ItemsAquired;
            ItemsTools.SetupItemsForScene(ItemsAquired);
        }

    }

    public static void SetItemFlag(string key)
    {
        ItemsAquired[key] = true;
    }
}
