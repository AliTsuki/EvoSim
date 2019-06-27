using System.IO;
using System.Text.RegularExpressions;

using UnityEngine;

// Create texture atlas and set up UVs for textures
public static class TextureAtlas
{
    // Regular expressions
    // TODO: Fix this fucking regex shit OMG UUUUGGHGHGHGHG
    public static Regex regex = new Regex(@"/[ \w-]+?(?=\.)/");

    // Atlas getter
    public static Texture2D Atlas { get; private set; }

    // Creates Texture Atlas from all terrain textures
    public static void CreateAtlas()
    {
        // Get all terrain texture names
        string[] imageNames = Directory.GetFiles("Assets/Textures/Terrain Textures/", "*.png");
        // Default tile pixel size
        int tilePixelWidth = 32;
        int tilePixelHeight = 32;
        // Create an atlas big enough to hold all images
        int atlasWidth = Mathf.CeilToInt((Mathf.Sqrt(imageNames.Length) + 1) * tilePixelWidth);
        int atlasHeight = Mathf.CeilToInt((Mathf.Sqrt(imageNames.Length) + 1) * tilePixelHeight);
        Texture2D tempAtlas = new Texture2D(atlasWidth, atlasHeight);
        // Initialize image count
        int count = 0;
        if(count <= imageNames.Length - 1)
        {
            for(int x = 0; x < atlasWidth / tilePixelWidth; x++)
            {
                for(int y = 0; y < atlasHeight / tilePixelHeight; y++)
                {
                    Texture2D tempTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                    tempTexture.LoadImage(File.ReadAllBytes(imageNames[count]));
                    tempAtlas.SetPixels(x * tilePixelWidth, y * tilePixelHeight, tilePixelWidth, tilePixelHeight, tempTexture.GetPixels());
                    float startx = x * tilePixelWidth;
                    float starty = y * tilePixelHeight;
                    float perPixelRatiox = 1.0f / tempAtlas.width;
                    float perPixelRatioy = 1.0f / tempAtlas.height;
                    startx *= perPixelRatiox;
                    starty *= perPixelRatioy;
                    float endx = startx + (perPixelRatiox * tilePixelWidth);
                    float endy = starty + (perPixelRatioy * tilePixelHeight);
                    Match imageName = regex.Match(imageNames[count]);
                    Debug.Log($@"Before REGEX: {imageNames[count]}, After REGEX: {imageName.Value}");
                    UVMap map = new UVMap(imageName.Value, new Vector2[]
                    {
                        new Vector2(startx, starty),
                        new Vector2(startx, endy),
                        new Vector2(endx, starty),
                        new Vector2(endx, endy)
                    });
                    count++;
                }
            }
        }
        Atlas = tempAtlas;
        File.WriteAllBytes("Assets/Textures/Atlas/atlas.png", tempAtlas.EncodeToPNG());
    }
}
