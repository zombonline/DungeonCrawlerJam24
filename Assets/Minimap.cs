using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] Image minimapImage, playerIconImage;    
    void Awake()
    {
        // Create a new texture with the same dimensions as the original texture
        Texture2D newTexture = new Texture2D(200, 200, TextureFormat.RGBA32, false);
        newTexture.filterMode = FilterMode.Point;

        for(int i = 0; i < newTexture.width; i++)
        {
            for(int j = 0; j < newTexture.height; j++)
            {
                if(i%2 == j%2)
                    newTexture.SetPixel(i, j, Color.white);
                else
                    newTexture.SetPixel(i, j, Color.grey);
            }
        }
        newTexture.Apply();
        // Replace the original texture with the new texture
        minimapImage.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
    }
    public void UpdateMinimap(Vector3 discoveredTile)
    {

        int x = (int)discoveredTile.x + 100;
        int y = (int)discoveredTile.z + 100;

        minimapImage.sprite.texture.SetPixel(x, y, Color.blue);
        minimapImage.sprite.texture.Apply();

        //move the anchored position of the minimap so that the player is always in the center
        //the image is 1500 x 1500, so the player is always in the center
        minimapImage.rectTransform.anchoredPosition = 
        new Vector2(-discoveredTile.x*(minimapImage.rectTransform.rect.width/minimapImage.sprite.texture.width),
         -discoveredTile.z* (minimapImage.rectTransform.rect.height/minimapImage.sprite.texture.height)); 
    }
    public void UpdatePlayerIconDirection(Vector3 playerDirection)
    {
        playerIconImage.rectTransform.localEulerAngles = new Vector3(0, 0, -playerDirection.y);
    }
}
