using System.IO;
using System.Text.RegularExpressions;

using UnityEngine;

// Create texture atlas and set up UVs for textures
public class TextureAtlas
{
    // Atlas getter
    public Texture2D Atlas { get; private set; }

    // Creates Texture Atlas from all terrain textures
    public void CreateAtlas(string _path)
    {
        // Get all terrain texture names
        string[] imageNames = Directory.GetFiles($@"Assets/Textures/{_path}/", "*.png");
        // Default tile pixel size
        int tilePixelWidth = 32;
        int tilePixelHeight = 32;
        // Create an atlas big enough to hold all images
        int atlasWidth = Mathf.CeilToInt((Mathf.Sqrt(imageNames.Length) + 1) * tilePixelWidth);
        int atlasHeight = Mathf.CeilToInt((Mathf.Sqrt(imageNames.Length) + 1) * tilePixelHeight);
        Texture2D tempAtlas = new Texture2D(atlasWidth, atlasHeight);
        // Initialize image count
        int count = 0;
        // Loop through image slots for textures in atlas
        for(int x = 0; x < atlasWidth / tilePixelWidth; x++)
        {
            for(int y = 0; y < atlasHeight / tilePixelHeight; y++)
            {
                // If completed all images go to end
                if(count > imageNames.Length - 1)
                {
                    goto end;
                }
                // Load image into temporary texture
                Texture2D tempTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                tempTexture.LoadImage(File.ReadAllBytes(imageNames[count]));
                // Write image to temporary atlas
                tempAtlas.SetPixels(x * tilePixelWidth, y * tilePixelHeight, tilePixelWidth, tilePixelHeight, tempTexture.GetPixels());
                // Get UV coords for image in atlas
                float startx = x * tilePixelWidth;
                float starty = y * tilePixelHeight;
                float perPixelRatiox = 1.0f / tempAtlas.width;
                float perPixelRatioy = 1.0f / tempAtlas.height;
                startx *= perPixelRatiox;
                starty *= perPixelRatioy;
                float endx = startx + (perPixelRatiox * tilePixelWidth);
                float endy = starty + (perPixelRatioy * tilePixelHeight);
                // Get image name for UV map
                Match imageName = Regex.Match(imageNames[count], @"([ \w-]+?)(?=\.)");
                // Create a new UV map with coords for named texture on atlas
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
        end:;
        this.Atlas = tempAtlas;
        File.WriteAllBytes($@"Assets/Textures/Atlas/{_path} Atlas.png", tempAtlas.EncodeToPNG());
    }
}
