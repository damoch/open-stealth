using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;


public class GameController : MonoBehaviour {

    static GameObject[] guards;
    GuardController guard;


    public static GameState gameState { get; set; }
    public static GameObject player { get; set; }

    public static Dictionary<string, bool> itemsAquired { get; set; }

    public AudioClip alarmSound;
    public AudioClip backgroundMusic;

    public GameObject gameOverPanel;
    public GameObject gameFinishedPanel;

    public void Start () {
        guards = GameObject.FindGameObjectsWithTag("Guard");
        player = GameObject.FindGameObjectsWithTag("PlayerObject")[0];

        string continueFlag = SaveController.getContinueFlag();
        if (!continueFlag.Equals("NONE"))
        {
            loadGame(continueFlag);
        }

        if(itemsAquired == null)
        {
            itemsAquired = ItemsTools.getItemsList();
        }
        SoundManager.instance.ChangeMusic(backgroundMusic);
	}
	
    public void setGlobalAlert()
    {
        if (guards != null)
        {
            foreach (GameObject guard in guards)
            {
                GuardController guardController = guard.GetComponent<GuardController>();
                if (!guardController.state.Equals(GuardState.ALERTED))
                    guardController.setAlert(player.transform.position);

            }
        SoundManager.instance.ChangeMusic(alarmSound);
        }

    }

    public void checkForOtherGuardsState()
    {

        if (anyGuardSeesPlayer())
        {
            setGlobalAlert();
        }
        else
        {
            setGlobalSuspicious();
        }
        
    }

    IEnumerator checkFieldOfViesAgain()
    {
        yield return new WaitForSeconds(5f);
        checkForOtherGuardsState();

    }

    bool anyGuardSeesPlayer()
    {
        if (guards != null)
        {
            foreach (GameObject guard in guards)
            {
                GuardController guardController = guard.GetComponent<GuardController>();
                if (guardController.fieldOfView.playerInRange)
                {
                    return true;

                }
            }
        }
        return false;
    }


    
    public void setGlobalSuspicious()
    {
        StartCoroutine("evasion");
        if (guards != null)
        {
            foreach (GameObject guard in guards)
            {
                GuardController guardController = guard.GetComponent<GuardController>();
                if (!guardController.state.Equals(GuardState.SUSPICIOUS))
                    guardController.setSuspicious(player.transform.position, true);

            }
        }
    }

    public static void RestartRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public void setGlobalCalm()
    {
        if (guards != null)
        {
            foreach (GameObject guard in guards)
            {
                GuardController guardController = guard.GetComponent<GuardController>();
                if (!guardController.state.Equals(GuardState.CALM))
                    guardController.setCalm(guardController.navPoint.transform.position);

            }
            SoundManager.instance.ChangeMusic(backgroundMusic);
        }
    }

    public void saveGame(string saveName)
    {
        SaveController.saveGame(saveName, getGameData());
    }

    public void loadGame(string saveName)
    {
        if (SaveController.savedGameExists(saveName))
        {
            setGameData(SaveController.getSavedGame(saveName), saveName);
        }
        else
            throw new UnityException("Brak zapisu!");
    }

    IEnumerator evasion()
    {
        Debug.Log("Uspokojenie");
        yield return new WaitForSeconds(5f);
        if (anyGuardSeesPlayer())
        {
            setGlobalAlert();

        }
        else
        {
            setGlobalCalm();
        }

    }

    public void setGamePaused()
    {
        gameState = GameState.PAUSED;
        SoundManager.instance.musicSource.Pause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;
    }

    public void setGameRunning()
    {
        gameState = GameState.RUNNING;
        SoundManager.instance.musicSource.UnPause();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    internal void GameFinished()
    {
        setGamePaused();
        gameFinishedPanel.SetActive(true);
    }

    SaveDataDTO getGameData()
    {
        SaveDataDTO dto = new SaveDataDTO();
        dto.RoomName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        dto.PlayerPosition = player.transform.position;
        dto.ItemsAquired = itemsAquired;
        dto.Items = player.GetComponent<PlayerController>().keys;
        return dto;
    }

    public void GameOver()
    {
        setGamePaused();
        gameOverPanel.SetActive(true);
    }

    void setGameData(SaveDataDTO dto, string saveName)
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(dto.RoomName))
        {
            SaveController.setContinueFlag(saveName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(dto.RoomName);
        }
        else
        {
            SaveController.setContinueFlag("NONE");
            player.transform.position = dto.PlayerPosition;
            player.GetComponent<PlayerController>().keys = dto.Items;
            itemsAquired = dto.ItemsAquired;
            ItemsTools.setupItemsForScene(itemsAquired);
        }

    }

    public static void setItemFlag(string key)
    {
        itemsAquired[key] = true;
    }
}
