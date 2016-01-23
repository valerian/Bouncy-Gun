using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class DataManager
{
    private static DataHandler<GameData> _gameData = null;
    public static DataHandler<GameData> gameData { get { return _gameData ?? (_gameData = new DataHandler<GameData>("saved_game.gd")); } }
}
