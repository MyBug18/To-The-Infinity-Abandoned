using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidWrapper : CelestialBodyWrapper
{
    public Asteroid asteroid;
    [SerializeField]
    SpriteRenderer sr;

    [SerializeField]
    Texture2D[] textures;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        asteroid = (Asteroid)body;

        int idx = GameDataHolder.random.Next(0, textures.Length);
        Sprite sp = Sprite.Create(textures[idx], new Rect(0, 0, textures[idx].width, textures[idx].height), new Vector2(0.5f, 0.5f));

        sr.sprite = sp;
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
