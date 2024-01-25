using System.Collections;
using TMPro;
using UnityEngine;

public class changePicture : MonoBehaviour
{
    public Renderer screenRenderer;
    public Texture2D pg;
    private Texture2D modificable_pg;
    private Texture2D prev_pg;
    public Texture2D inverted_pg;
    private Texture2D blackTexture;
    private FiltersRiddleButton buttonScript;
    private Material material;
    private int activeFilter;
    private float filterIntesity;
    private bool isScreenActive;
    private bool reversedColors;
    public TextMeshPro textToShow;

    private bool isProcessing = false;
    void Start()
    {
        material = screenRenderer.material;
        activeFilter = 0;
        filterIntesity = 0.0f;

        buttonScript = GetComponent<FiltersRiddleButton>();
        changeTextVisibility();

        modificable_pg = pg;
        prev_pg = pg;
        // When screen is off
        blackTexture = new Texture2D(1, 1);
        ((Texture2D)blackTexture).SetPixel(0, 0, Color.black);
        ((Texture2D)blackTexture).Apply();
        material.mainTexture = blackTexture;

        isScreenActive = false;
        reversedColors = false;
    }

    public void HandleButtonRelease(FiltersRiddleButton.FiltersRiddleButtonType buttonType)
    {
        if (isProcessing)
        {
            Debug.Log("Already processing, please wait.");
            return;
        }
        // Activate button actions
        switch (buttonType)
        {
            case FiltersRiddleButton.FiltersRiddleButtonType.OnOff:
                TurnScreenOnOff();
                changeTextVisibility();
                break;

            case FiltersRiddleButton.FiltersRiddleButtonType.Reset:
                ResetFilters();
                changeText();
                break;

            case FiltersRiddleButton.FiltersRiddleButtonType.Next:
                NextFilter();
                changeText();
                break;

            case FiltersRiddleButton.FiltersRiddleButtonType.Prev:
                PrevFilter();
                changeText();
                break;

            case FiltersRiddleButton.FiltersRiddleButtonType.Reverse:
                ReverseColors();
                break;
            case FiltersRiddleButton.FiltersRiddleButtonType.Plus:
                ChangeIntensity(true);
                changeText();
                break;
            case FiltersRiddleButton.FiltersRiddleButtonType.Minus:
                ChangeIntensity(false);
                changeText();
                break;
            default:
                Debug.LogWarning("Unknown button type: " + buttonType);
                break;
        }
    }

    // Changes displayed text
    private void changeText()
    {
        int currentValue = (int)(filterIntesity * 100.0f);
        switch (activeFilter)
        {
            case 0:
                textToShow.text = "Temperature \nIntensity: " + currentValue + "%";
                break;
            case 1:
                textToShow.text = "Grayscale \nIntensity: " + currentValue + "%";
                break;
            case 2:
                textToShow.text = "Sepia \nIntensity: " + currentValue + "%";
                break;
            case 3:
                textToShow.text = "EdgeDetection \nIntensity: " + currentValue + "%";
                break;
            case 4:
                textToShow.text = "Blur \nIntensity: " + currentValue + "%";
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

            // Make text fully transparent when screen is off
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
            Debug.LogWarning("Unable to reset picture. Check if the texture and material are assigned.");
        }
    }

    // Change active filter
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

    // Change filter intesity
    public void ChangeIntensity(bool up)
    {
        if (up && filterIntesity < 0.9f)
        {
            filterIntesity += 0.25f;
            changeDisplayedImage();
        }
        else if(!up && filterIntesity > 0.1f)
        {
            // If intesnisty is lower then reset the image to the image before applying this filter and apply weaker filter again
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
            Debug.LogWarning("Unable to reverse colors. Check if the texture and material are assigned and colors need to be reversed.");
        }
    }

    private void changeDisplayedImage()
    {
        ApplyFilter(activeFilter);
    }

    private void ApplyFilter(int filterIndex)
    {
        isProcessing = true;

        switch (filterIndex)
        {
            case 0:
                StartCoroutine(ModifyTextureTemperature(filterIntesity));
                break;
            case 1:
                StartCoroutine(ApplyGrayscaleFilter(filterIntesity));
                break;

            case 2:
                StartCoroutine(ApplySepiaFilter(filterIntesity));
                break;

            case 3:
                StartCoroutine(ApplyEdgeDetectionFilter(filterIntesity));
                break;

            case 4:
                StartCoroutine(ApplyBlurFilter(filterIntesity));
                break;

            default:
                break;
        }

        isProcessing = false;

        material.mainTexture = modificable_pg;
    }

    private IEnumerator ModifyTextureTemperature(float filterIntesity)
    {
        float temperature = 1.0f + filterIntesity;
        Color[] pixels = modificable_pg.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] *= temperature;
        }
        yield return null;
        Texture2D texture = new Texture2D(modificable_pg.width, modificable_pg.height);
        texture.SetPixels(pixels);
        texture.Apply();
        modificable_pg = texture;

        material.mainTexture = modificable_pg;
        yield return null;
    }

    private IEnumerator ApplyGrayscaleFilter(float filterIntesity)
    {
        if(filterIntesity > 0.1f)
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
            Texture2D texture = new Texture2D(modificable_pg.width, modificable_pg.height);
            texture.SetPixels(pixels);
            texture.Apply();
            modificable_pg = texture;
        }

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
        Texture2D texture = new Texture2D(modificable_pg.width, modificable_pg.height);
        texture.SetPixels(pixels);
        texture.Apply();
        modificable_pg = texture;
        material.mainTexture = modificable_pg;
        yield return null;
    }
    private IEnumerator ApplyEdgeDetectionFilter(float filterIntesity)
    {
        Color[] pixels = modificable_pg.GetPixels();
        int width = modificable_pg.width;
        int height = modificable_pg.height;

        Color[] newPixels = new Color[width * height];

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
                newPixels[index] = Color.Lerp(pixels[index], new Color(Mathf.Clamp01(sumR), Mathf.Clamp01(sumG), Mathf.Clamp01(sumB), pixels[index].a), filterIntesity);
            }
        }
        yield return null;
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(newPixels);
        texture.Apply();
        modificable_pg = texture;

        material.mainTexture = modificable_pg;
        yield return null;

    }

    private IEnumerator ApplyBlurFilter(float filterIntensity)
    {
        Color[] pixels = modificable_pg.GetPixels();
        int width = modificable_pg.width;
        int height = modificable_pg.height;

        Color[] newPixels = new Color[width * height];

        float[,] kernel = new float[,]
        {
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 }
        };

        float kernelWeight = 9; 

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
                newPixels[index] = Color.Lerp(pixels[index], new Color(sumR / kernelWeight, sumG / kernelWeight, sumB / kernelWeight, pixels[index].a), filterIntensity);
            }
        }
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(newPixels);
        texture.Apply();
        modificable_pg = texture;

        material.mainTexture = modificable_pg;
        yield return null;
    }

}
