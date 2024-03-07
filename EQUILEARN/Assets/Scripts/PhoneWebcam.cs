using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using UnityEngine.InputSystem;
using System.IO;
using TMPro;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture webcamTexture;
    private Texture defaultBackground;
    public RawImage background;
    public AspectRatioFitter fit;
    private Texture2D currentFrameTexture;
    private static WebSocket ws;

    public TextMeshProUGUI Display;


    void Start()
    {
        // Ensure UI elements are assigned
        if (background == null || fit == null)
        {
            Debug.LogError("UI components are not assigned.");
            return;
        }

        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No Camera Detected");
            camAvailable = false;
            return;
        }

        // Try to find the highest resolution
        int maxWidth = 640; // Default fallback resolution width
        int maxHeight = 480; // Default fallback resolution height
        if (devices[0].availableResolutions != null && devices[0].availableResolutions.Length > 0)
        {
            foreach (var res in devices[0].availableResolutions)
            {
                if (res.width > maxWidth || res.height > maxHeight)
                {
                    maxWidth = res.width;
                    maxHeight = res.height;
                }
            }
        }

        webcamTexture = new WebCamTexture(devices[0].name, maxWidth, maxHeight);

        if (webcamTexture == null)
        {
            Debug.LogError("Unable to find webcam.");
            return;
        }

        webcamTexture.Play();
        background.texture = webcamTexture;
        camAvailable = true;
        currentFrameTexture = new Texture2D(webcamTexture.width, webcamTexture.height);

        InitializeWebSocket();
    }

    void InitializeWebSocket()
    {
        if (ws == null)
        {
            ws = new WebSocket("ws://localhost:8765");
            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Message received: " + e.Data);
                Display.text = e.Data;
                
            };
            ws.Connect();
        }
    }

    void Update()
    {
        if (!camAvailable || ws == null)
        {
            return;
        }

        AdjustCameraImage();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SaveCurrentFrame();
            SendCurrentFrame();
        }
    }

    private void AdjustCameraImage()
    {
        float ratio = (float)webcamTexture.width / (float)webcamTexture.height;
        fit.aspectRatio = ratio;
        float scaleY = webcamTexture.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        int orient = webcamTexture.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        UpdateCurrentFrame();
    }

    void UpdateCurrentFrame()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            currentFrameTexture.SetPixels(webcamTexture.GetPixels());
            currentFrameTexture.Apply();
        }
    }

    private void SaveCurrentFrame()
    {
        Debug.Log("Saving current frame.");
        byte[] jpgData = currentFrameTexture.EncodeToJPG(100); // Adjust quality here (0-100)
        string filePath = Path.Combine("/Users/amandarodrigo/Documents/Pictures Unity/", "CapturedFrame.jpg");
        File.WriteAllBytes(filePath, jpgData);
        Debug.Log("Image saved at: " + filePath);
    }

    private void SendCurrentFrame()
    {
        Debug.Log("Sending current frame.");
        Texture2D tempTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
        tempTexture.SetPixels(webcamTexture.GetPixels());
        tempTexture.Apply();
        byte[] jpgData = tempTexture.EncodeToJPG(100);
        ws.Send(jpgData);
        Destroy(tempTexture);
        Debug.Log("Frame sent.");
    }
}

