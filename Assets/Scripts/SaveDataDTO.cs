using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveDataDTO
{
    public SaveDataDTO()
    {
    }

    public SaveDataDTO (string roomName)
    {
        this.RoomName = roomName;
    }
    [SerializeField]
    private string roomName;

    [SerializeField]
    private Vector3 playerPosition;

    [SerializeField]
    private Dictionary<string, bool> itemsAquired;

    [SerializeField]
    private List<KeyItemController> items;


    //Unity ma problem z serializacją prywatnych propertiesów, dlatego tak jest
    public string RoomName
    {
        get
        {
            return roomName;
        }

        set
        {
            roomName = value;
        }
    }

    public Vector3 PlayerPosition
    {
        get
        {
            return playerPosition;
        }

        set
        {
            playerPosition = value;
        }
    }

    public Dictionary<string, bool> ItemsAquired
    {
        get
        {
            return itemsAquired;
        }
        set
        {
            itemsAquired = value;
        }
    }

    public List<KeyItemController> Items
    {
        get
        {
            return items;
        }
        set
        {
            items = value;
        }
    }
}