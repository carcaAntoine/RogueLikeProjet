using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class Player : MovingObject
    {

        public int wallDamage = 1;
        public int pointsPerFood = 10;
        public int pointsPerSoda = 20;
        public float restartLevelDelay = 1f;
        //private bool isMoving;//
        private Animator animator;
        private int food;
        public Text foodText;
        public AudioClip moveSound1;                //when player moves.
        public AudioClip moveSound2;                //when player moves.
        public AudioClip eatSound1;                 //when player collects a food object.
        public AudioClip eatSound2;                 //when player collects a food object.
        public AudioClip drinkSound1;               //when player collects a soda object.
        public AudioClip drinkSound2;               //when player collects a soda object.
        public AudioClip gameOverSound;				//when player dies.

        protected override void Start()
        {
            animator = GetComponent<Animator>();
            food = GameManager.instance.playerFoodPoints;

            foodText.text = "Food : " + food;

            base.Start();
        }

        private void OnDisable()
        {
            GameManager.instance.playerFoodPoints = food;
        }

        void Update()
        {
            if (!GameManager.instance.playersTurn) return;

            int horizontal = 0;
            int vertical = 0;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");

            //horizontal = Mathf.RoundToInt (Input.GetAxisRaw("Horizontal"));
            //vertical = Mathf.RoundToInt (Input.GetAxisRaw("Vertical"));

            if (horizontal != 0)
                vertical = 0;

            if (horizontal != 0 || vertical != 0)
                AttemptMove<Wall>(horizontal, vertical);

        }

        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            food--;
            foodText.text = "Food : " + food;
            base.AttemptMove<T>(xDir, yDir);
            RaycastHit2D hit;
            if (Move(xDir, yDir, out hit))
            {
                //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
                SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
            }
            checkIfGameOver();
            GameManager.instance.playersTurn = false;

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Exit")
            {
                Debug.Log(other.name);
                Invoke("Restart", restartLevelDelay);
                enabled = false;
            }
            else if (other.tag == "Food")
            {
                food += pointsPerFood;
                foodText.text = "+ " + pointsPerFood + " Food";
                SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
                other.gameObject.SetActive(false);
            }
            else if (other.tag == "Soda")
            {
                food += pointsPerSoda;
                foodText.text = "+ " + pointsPerSoda + " Food";
                SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
                other.gameObject.SetActive(false);
            }
        }

        protected override void OnCantMove<T>(T component)
        {
            Wall hitWall = component as Wall;
            hitWall.DamageWall(wallDamage);
            animator.SetTrigger("PlayerChop");

        }

        private void Restart()
        {
            //Application.LoadLevel(Application.loadedLevel);
            //SceneManager.LoadScene(0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        public void LoseFood(int loss)
        {
            animator.SetTrigger("PlayerHit");
            food -= loss;
            foodText.text = "- " + loss + " Food";
            checkIfGameOver();
        }

        private void checkIfGameOver()
        {
            if (food <= 0)
            {
                SoundManager.instance.PlaySingle (gameOverSound);
                SoundManager.instance.musicSource.Stop();
                GameManager.instance.GameOver();
            }
        }
    }
}