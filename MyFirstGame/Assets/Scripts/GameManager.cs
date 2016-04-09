using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Text speedCountText;

    private PlayerStats _playerStats;

    // Use this for initialization
    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        _playerStats = new PlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        speedCountText.text = string.Format("Speed Powerup Count: {0}",
            _playerStats.PermanentSpeedCount + _playerStats.TemporarySpeedCount);
    }
}
