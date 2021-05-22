using UnityEngine;
using ShootyGhost;
using UnityEngine.Events;

public class HauntModeTutorial : MonoBehaviour
{
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
        ShowTutorial();
    }

    void ShowTutorial()
    {
        if (_alreadyShowed) return;
        _alreadyShowed = true;
        showTutorial.Invoke();
    }
}
