using System.Collections;
using TMPro;
using UnityEngine;
using static ButtonVR;

public class changePicture : MonoBehaviour
{
    public Renderer screenRenderer;
    public Texture2D pg;
    public Texture2D modificable_pg;
    private Texture2D prev_pg;
    public Texture2D inverted_pg;
    private Texture2D blackTexture;
    private ButtonVR buttonScript;
    public Material material;
    public int activeFilter;
    public float filterIntesity;
    public bool isScreenActive;
    public bool reversedColors;
    public TextMeshPro textToShow;

    private bool isProcessing = false;
    // Start is called before the first frame update
    void Start()
    {
        material = screenRenderer.material;
        //material.SetColor("_EmissionColor", Color.black);
        activeFilter = 0;
        filterIntesity = 0.0f;

        buttonScript = GetComponent<ButtonVR>();
        changeTextVisibility();

        modificable_pg = pg;
        prev_pg = pg;
        // Creating a 1x1 black texture
        blackTexture = new Texture2D(1, 1);
        ((Texture2D)blackTexture).SetPixel(0, 0, Color.black);
        ((Texture2D)blackTexture).Apply();
        material.mainTexture = blackTexture;

        isScreenActive = false;
        reversedColors = false;
    }

    // Handle the button release event
    public void HandleButtonRelease(ButtonVR.ButtonType buttonType)
    {
        if (isProcessing)
        {
            Debug.Log("Already processing, please wait.");
            return;
        }

        switch (buttonType)
        {
            case ButtonVR.ButtonType.OnOff:
                TurnScreenOnOff();
                changeTextVisibility();
                break;

            case ButtonVR.ButtonType.Reset:
                ResetFilters();
                changeText();
                break;

            case ButtonVR.ButtonType.Next:
                NextFilter();
                changeText();
                break;

            case ButtonVR.ButtonType.Prev:
                PrevFilter();
                changeText();
                break;

            case ButtonVR.ButtonType.Reverse:
                ReverseColors();
                break;
            case ButtonVR.ButtonType.Plus:
                ChangeIntensity(true);
                changeText();
                break;
            case ButtonVR.ButtonType.Minus:
                ChangeIntensity(false);
                changeText();
                break;
            default:
                Debug.LogWarning("Unknown button type: " + buttonType);
                break;
        }
    }
    private void changeText()
    {
        int currentValue = (int)(filterIntesity * 100.0f);
        switch (activeFilter)
        {
            case 0:
                textToShow.text = "None \nIntensity: " + currentValue + "%";
                break;
            case 1:
                textToShow.text = "Temperature \nIntensity: " + currentValue + "%";
                break;
            case 2:
                textToShow.text = "Grayscale \nIntensity: " + currentValue + "%";
                break;
            case 3:
                textToShow.text = "Sepia \nIntensity: " + currentValue + "%";
                break;
            case 4:
                textToShow.text = "EdgeDetection \nIntensity: " + currentValue + "%";
                break;
            default:
                textToShow.text = "Error";
                break;
        }
    }
    private void changeTextVisibility()
    {
        if (textToShow != null)
        {
            Color textColorTransparent = textToShow.color;
            Color textColor = textToShow.color;

            // Set the alpha component to 0 (fully transparent)
            textColorTransparent.a = 0;
            textColor.a = 1;
            if (isScreenActive)
            {
                textToShow.color = textColor;
            }
            else
            {
                textToShow.color = textColorTransparent;
            }

        }
        else
        {
            Debug.LogWarning("Text component not assigned!");
        }
    }
    public void TurnScreenOnOff()
    {
        isScreenActive = !isScreenActive;

        // Toggle the visibility of the screenObject based on isScreenActive
        if (screenRenderer != null)
        {

            if (isScreenActive)
            {
                material.mainTexture = modificable_pg;
            }
            else
            {
                material.mainTexture = blackTexture;
            }
        }
    }

    public void ResetFilters()
    {
        if (screenRenderer != null && isScreenActive && material != null && material.mainTexture != null)
        {
            filterIntesity = 0.0f;
            activeFilter = 0;
            reversedColors = false;
            modificable_pg = pg;
            material.mainTexture = modificable_pg;
        }
        else
        {
            // Handle the case where the texture or material is not available, or colors don't need to be reversed
            Debug.LogWarning("Unable to reset picture. Check if the texture and material are assigned.");
        }
    }
    public void NextFilter()
    {
        if (activeFilter > 3)
        {
            activeFilter = 0;
        }
        else
        {
            activeFilter++;
        }
        filterIntesity = 0.0f;
        prev_pg = modificable_pg;
        changeDisplayedImage();
    }
    public void PrevFilter()
    {
        if (activeFilter == 0)
        {
            activeFilter = 4;
        }
        else
        {
            activeFilter--;
        }
        filterIntesity = 0.0f;
        prev_pg = modificable_pg;
        changeDisplayedImage();
    }
    public void ChangeIntensity(bool up)
    {
        if (up && filterIntesity < 0.9f)
        {
            filterIntesity += 0.25f;
            changeDisplayedImage();
        }
        else if(!up && filterIntesity > 0.1f)
        {
            filterIntesity -= 0.25f;
            modificable_pg = prev_pg;
            changeDisplayedImage();
        }
    }
    public void ReverseColors()
    {
        if (screenRenderer != null && isScreenActive && material != null && material.mainTexture != null)
        {
            reversedColors = !reversedColors;

            // Apply the inverted texture to the material
            if (reversedColors)
            {
                modificable_pg = inverted_pg;
            }
            else
            {
                modificable_pg = pg;
            }
            material.mainTexture = modificable_pg;

        }
        else
        {
            // Handle the case where the texture or material is not available, or colors don't need to be reversed
            Debug.LogWarning("Unable to reverse colors. Check if the texture and material are assigned and colors need to be reversed.");
        }
    }

    private void changeDisplayedImage()
    {

        ApplyFilter(activeFilter);

        //material.mainTexture = modificable_pg;
    }

    private void ApplyFilter(int filterIndex)
    {
        isProcessing = true;

        switch (filterIndex)
        {
            case 1:
                // Example: Adjust Temperature
                StartCoroutine(ModifyTextureTemperature(filterIntesity));
                break;

            case 2:
                // Example: Apply Grayscale Filter
                StartCoroutine(ApplyGrayscaleFilter(filterIntesity));
                break;

            case 3:
                // Example: Apply Sepia Filter
                StartCoroutine(ApplySepiaFilter(filterIntesity));
                break;

            case 4:
                StartCoroutine(ApplyEdgeDetectionFilter(filterIntesity));
                break;

            default:
                break;
        }

        isProcessing = false;

        material.mainTexture = modificable_pg;
    }

    // Example: Adjust Temperature Filter
    private IEnumerator ModifyTextureTemperature(float filterIntesity)
    {
        float temperature = 1.0f + filterIntesity;
        Color[] pixels = modificable_pg.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] *= temperature;
        }
        yield return null;
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.Lerp(modificable_pg.GetPixel(i % modificable_pg.width, i / modificable_pg.width), pixels[i], filterIntesity);
        }
        Texture2D texture = new Texture2D(modificable_pg.width, modificable_pg.height);
        texture.SetPixels(pixels);
        texture.Apply();
        modificable_pg = texture;

        material.mainTexture = modificable_pg;
        yield return null;
    }

    private IEnumerator ApplyGrayscaleFilter(float filterIntesity)
    {
        Color[] pixels = modificable_pg.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            float luminance = pixels[i].r * 0.299f + pixels[i].g * 0.587f + pixels[i].b * 0.114f;
            pixels[i] = new Color(luminance, luminance, luminance, pixels[i].a);
        }
        yield return null;
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.Lerp(modificable_pg.GetPixel(i % modificable_pg.width, i / modificable_pg.width), pixels[i], filterIntesity);
        }
        modificable_pg.SetPixels(pixels);
        modificable_pg.Apply();

        material.mainTexture = modificable_pg;
        yield return null;
    }

    private IEnumerator ApplySepiaFilter(float filterIntesity)
    {
        Color[] pixels = modificable_pg.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            float tr = pixels[i].r * 0.393f + pixels[i].g * 0.769f + pixels[i].b * 0.189f;
            float tg = pixels[i].r * 0.349f + pixels[i].g * 0.686f + pixels[i].b * 0.168f;
            float tb = pixels[i].r * 0.272f + pixels[i].g * 0.534f + pixels[i].b * 0.131f;

            pixels[i] = new Color(Mathf.Clamp01(tr), Mathf.Clamp01(tg), Mathf.Clamp01(tb), pixels[i].a);
        }
        yield return null;
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.Lerp(modificable_pg.GetPixel(i % modificable_pg.width, i / modificable_pg.width), pixels[i], filterIntesity);
        }
        modificable_pg.SetPixels(pixels);
        modificable_pg.Apply();

        material.mainTexture = modificable_pg;
        yield return null;
    }
    private IEnumerator ApplyEdgeDetectionFilter(float filterIntesity)
    {
        Color[] pixels = modificable_pg.GetPixels();
        int width = modificable_pg.width;
        int height = modificable_pg.height;

        Color[] newPixels = new Color[width * height];

        // Define a simple 3x3 edge detection kernel
        float[,] kernel = new float[,]
        {
        { -1, -1, -1 },
        { -1,  8, -1 },
        { -1, -1, -1 }
        };

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                float sumR = 0, sumG = 0, sumB = 0;

                for (int ky = -1; ky <= 1; ky++)
                {
                    for (int kx = -1; kx <= 1; kx++)
                    {
                        int pixelX = x + kx;
                        int pixelY = y + ky;

                        Color pixelColor = pixels[pixelY * width + pixelX];

                        sumR += pixelColor.r * kernel[ky + 1, kx + 1];
                        sumG += pixelColor.g * kernel[ky + 1, kx + 1];
                        sumB += pixelColor.b * kernel[ky + 1, kx + 1];
                    }
                }

                int index = y * width + x;
                //newPixels[index] = new Color(Mathf.Clamp01(sumR), Mathf.Clamp01(sumG), Mathf.Clamp01(sumB), pixels[index].a);
                newPixels[index] = Color.Lerp(pixels[index], new Color(Mathf.Clamp01(sumR), Mathf.Clamp01(sumG), Mathf.Clamp01(sumB), pixels[index].a), filterIntesity);
            }
        }
        yield return null;
        modificable_pg.SetPixels(newPixels);
        modificable_pg.Apply();

        material.mainTexture = modificable_pg;
        yield return null;

    }
}
