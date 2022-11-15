using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Wall : MonoBehaviour
    {
        public Sprite dmgSprite;
        public AudioClip chopSound1;				
		public AudioClip chopSound2;
        public int hp = 3;
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            //Get a component reference to the SpriteRenderer.
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //DamageWall is called when the player attacks a wall.
        public void DamageWall(int loss)
        {
            //SoundManager.instance.RandomizeSfx (chopSound1, chopSound2);
            //Set spriteRenderer to the damaged wall sprite.
            spriteRenderer.sprite = dmgSprite;
            hp -= loss;

            if (hp <= 0)
                gameObject.SetActive(false);
        }
    }
}