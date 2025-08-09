using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    [SerializeField] private Sprite[] sprite;

    public void ChangeSprite()
    {
        if (sprite.Length < 2) return;
        GetComponent<SpriteRenderer>().sprite = sprite[1];
    }
    
    public void ChangeSpriteBack()
    {
        if (sprite.Length < 2) return;
        GetComponent<SpriteRenderer>().sprite = sprite[0];
    }
}
