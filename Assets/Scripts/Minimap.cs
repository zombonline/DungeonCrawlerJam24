using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] Image minimapImage, playerIconImage;  

    [SerializeField] float maxZoom = 1, minZoom = 0.1f;
    float currentZoom = 1f;
    [SerializeField] Vector2 mapImageSize;
    [SerializeField] Vector3 lastDiscoveredTile;
    [SerializeField] Color mapColor1, mapColor2, trailColor;
    void Awake()
    {
        // Create a new texture with the same dimensions as the original texture
        Texture2D newTexture = new Texture2D(201, 201, TextureFormat.RGBA32, false);
        newTexture.filterMode = FilterMode.Point;

        for(int i = 0; i < newTexture.width; i++)
        {
            for(int j = 0; j < newTexture.height; j++)
            {
                if(i%2 == j%2)
                    newTexture.SetPixel(i, j, mapColor1);
                else
                    newTexture.SetPixel(i, j, mapColor2);
            }
        }
        newTexture.Apply();
        // Replace the original texture with the new texture
        minimapImage.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            currentZoom += 0.1f;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            UpdateMapZoom(currentZoom);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            currentZoom -= 0.1f;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            UpdateMapZoom(currentZoom);
        }
    }

    public void UpdateMinimap(Vector3 discoveredTile)
    {
        lastDiscoveredTile = discoveredTile;
        int x = (int)discoveredTile.x + 100;
        int y = (int)discoveredTile.z + 100;

        minimapImage.sprite.texture.SetPixel(x, y, trailColor);
        minimapImage.sprite.texture.Apply();

        minimapImage.rectTransform.anchoredPosition = 
        new Vector2(-discoveredTile.x*(minimapImage.rectTransform.rect.width/minimapImage.sprite.texture.width),
         -discoveredTile.z* (minimapImage.rectTransform.rect.height/minimapImage.sprite.texture.height)); 
    }
    public void UpdatePlayerIconDirection(Vector3 playerDirection)
    {
        playerIconImage.rectTransform.localEulerAngles = new Vector3(0, 0, -playerDirection.y);
    }
    public void UpdateMapZoom(float zoomValue)
    {
        minimapImage.rectTransform.sizeDelta = mapImageSize * zoomValue;
        UpdateMinimap(lastDiscoveredTile);
    }
}
