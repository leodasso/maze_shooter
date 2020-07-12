using UnityEngine;
using ShootyGhost;
using UnityEngine.Events;

public class HauntModeTutorial : MonoBehaviour
{
    [Tooltip("How much haunt juice does the player need before showing this tutorial?")]
    public int minHauntJuice = 5;
    public UnityEvent showTutorial;

    Haunter _playerHaunter;
    bool _alreadyShowed;
    
    public void GetPlayer()
    {
        var player = GameMaster.GetPlayerInstance();
        if (!player) return;
        
        _playerHaunter = player.GetComponent<Haunter>();
    }

    public void TryShowTutorial()
    {
        if (!_playerHaunter) return;
        if (_playerHaunter.hauntJuice < minHauntJuice) return;
        ShowTutorial();
    }

    void ShowTutorial()
    {
        if (_alreadyShowed) return;
        _alreadyShowed = true;
        showTutorial.Invoke();
    }
}
