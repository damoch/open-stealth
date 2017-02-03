using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveDataDto
{
    public SaveDataDto()
    {
    }

    public SaveDataDto (string roomName)
    {
        this.RoomName = roomName;
    }
    [SerializeField]
    private string _roomName;

    [SerializeField]
    private Vector3 _playerPosition;

    [SerializeField]
    private Dictionary<string, bool> _itemsAquired;

    [SerializeField]
    private List<KeyItemController> _items;


    //Unity ma problem z serializacją prywatnych propertiesów, dlatego tak jest
    public string RoomName
    {
        get
        {
            return _roomName;
        }

        set
        {
            _roomName = value;
        }
    }

    public Vector3 PlayerPosition
    {
        get
        {
            return _playerPosition;
        }

        set
        {
            _playerPosition = value;
        }
    }

    public Dictionary<string, bool> ItemsAquired
    {
        get
        {
            return _itemsAquired;
        }
        set
        {
            _itemsAquired = value;
        }
    }

    public List<KeyItemController> Items
    {
        get
        {
            return _items;
        }
        set
        {
            _items = value;
        }
    }
}