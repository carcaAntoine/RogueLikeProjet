using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class GameManager : MonoBehaviour
    {
        public float levelStartDelay = 2f;
        public float turnDelay = 0.1f;
        public static GameManager instance = null;
        private BoardManager boardScript;
        public int playerFoodPoints = 100;
        [HideInInspector] public bool playersTurn = true;

        private Text levelText;
        private GameObject levelImage;
        private int level = 1;
        private List<Enemy> enemies;
        private bool enemiesMoving;
        private bool doingSetup;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            enemies = new List<Enemy>();
            boardScript = GetComponent<BoardManager>();
            InitGame();
        }

        private void OnLevelWasLoaded(int index)
        {
            level++;
            InitGame();
        }

        void InitGame()
        {
            doingSetup = true;

            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelText.text = "Day " + level;
            levelImage.SetActive(true);
            Invoke("HideLevelImage", levelStartDelay);

            enemies.Clear();
            boardScript.SetupScene(level);
        }
        void Update()
        {
            if (playersTurn || enemiesMoving || doingSetup)
            {
                return;
            }

            StartCoroutine(MoveEnemies());
        }

        private void HideLevelImage()
        {
            levelImage.SetActive(false);
            doingSetup = false;
        }

        public void GameOver()
        {
            levelText.text = "After " + level + " days, you starved.";
            levelImage.SetActive(true);
            enabled = false;
        }


        public void AddEnemyToList(Enemy script)
        {
            enemies.Add(script);
        }

        IEnumerator MoveEnemies()
        {
            enemiesMoving = true;
            yield return new WaitForSeconds(turnDelay);
            if (enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].MoveEnemy();
                yield return new WaitForSeconds(enemies[i].moveTime);
            }

            playersTurn = true;
            enemiesMoving = false;
        }

        /*
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            //Add one to our level number.
            level++;
            Debug.Log("Level Finish Loading");
            //Call InitGame to initialize our level.
            InitGame();
        }
        void OnEnable()
        {
            //Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        void OnDisable()
        {
            //Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled.
            //Remember to always have an unsubscription for every delegate you subscribe to!
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        */
    }
}
