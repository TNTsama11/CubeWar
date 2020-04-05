using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnime : MonoBehaviour
{
    public Sprite[] loadSprites;
    private Image img;

    float timer;
    int spriteIndex;
	
	void Awake()
	{
        img = this.GetComponent<Image>();
	}
	
    void Start()
    {
        
    }

    void Update()
    {
        if (timer < 0.1f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if(spriteIndex<9)
            {
                SetSprite(spriteIndex);
                spriteIndex++;
            }
            else
            {
                spriteIndex = 0;
            }

            timer = 0f;
        }
    }

    private void SetSprite(int index)
    {
        img.sprite = loadSprites[index];
    }
}
