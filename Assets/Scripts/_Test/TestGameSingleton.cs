using UnityEngine;
using System.Collections;

public class TestGameSingleton : MonoBehaviour
{
    private Game game;

    void Start()
    {
        game = Game.instance;
        var toto = game;
        game = toto;
    }
}
