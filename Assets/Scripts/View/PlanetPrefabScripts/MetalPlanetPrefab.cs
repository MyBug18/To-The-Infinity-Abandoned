using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlanetPrefab : MonoBehaviour
{
    [SerializeField]
    Texture2D[] atmos, dusts, auroras, oceans, types, rings;

    // Start is called before the first frame update
    void Start()
    {
        _ApplyTexture(0, atmos);
        _ApplyTexture(1, dusts);
        _ApplyTexture(2, auroras);
        _ApplyTexture(3, oceans);
        _ApplyTexture(4, types);
        _ApplyTexture(5, rings);

        if (GameManager.r.Next(0, 3) <= 1)
            transform.GetChild(5).gameObject.SetActive(false);
    }

    private void _ApplyTexture(int childNum, Texture2D[] array)
    {
        Texture2D tex = array[GameManager.r.Next(0, array.Length)];
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        transform.GetChild(childNum).GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
    }
}
