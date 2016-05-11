using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        
        //public Text primaryPowerupText;

        //private PlayerStats _playerStats;
        private PlayerController player;
        private Scene CurrentScene;
        private CustomSceneManager _customSceneManager;

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
            //_playerStats = new PlayerStats();

            _customSceneManager = new CustomSceneManager();

            InitializeLevel();
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null && player.CurrentPlayerEffects.CheckForExistenceOfBit((int) PlayerEffects.EndOfLevelReached))
            {
                LevelDestinationReached();
            }
        }

        private void LevelDestinationReached()
        {
            player = null;

            _customSceneManager.LoadNextScene();
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }
}
