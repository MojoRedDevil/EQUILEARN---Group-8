using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using UnityEngine.InputSystem;
using TMPro;

public class WSClient : MonoBehaviour
{
    WebSocket ws;
    [SerializeField] private TextMeshProUGUI MessageText;
    public RawImage background;
    private Texture2D newBackgroundTexture;

    private static WebSocket _ws;
    private bool _messageLogged; // Add this variable to track whether the message has been logged

    private void Start()
    {
        if (_ws == null)
        {
            _ws = new WebSocket("ws://localhost:8765");
            _ws.OnMessage += OnMessageReceived;
            _ws.Connect();
        }

        ws = _ws;

        if (ws != null) // Check if ws is not null
        {
            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket connection is open.");
            };
        }
    }

    private static Texture2D ConvertToTexture2D(Texture sourceTexture)
    {
        // Create a RenderTexture the size of the sourceTexture
        RenderTexture renderTex = RenderTexture.GetTemporary(
            sourceTexture.width,
            sourceTexture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        // Copy the sourceTexture to the RenderTexture
        Graphics.Blit(sourceTexture, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        // Create a new Texture2D and read the RenderTexture contents into it
        Texture2D resultTex = new Texture2D(sourceTexture.width, sourceTexture.height);
        resultTex.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        resultTex.Apply();

        // Clean up
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        return resultTex;
    }

    public static byte[] ConvertRawImageToPNG(RawImage rawImage)
    {
        Texture texture = rawImage.texture;
        Texture2D texture2D = texture as Texture2D;

        // If the texture is not directly a Texture2D (e.g., RenderTexture), convert it
        if (texture2D == null)
        {
            texture2D = ConvertToTexture2D(texture);
        }

        // Encode the texture into PNG format
        byte[] pngData = texture2D.EncodeToPNG();
        return pngData;
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);

        MessageText.text = e.Data;
        MessageText.enabled = false;
        MessageText.enabled = true;

    }

    void Update()
    {
        if (ws == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (ws.IsAlive)
            {
                Debug.Log("WebSocket connection is open.");
                byte[] pngData = ConvertRawImageToPNG(background);
                ws.Send(pngData);
            }

        }



        if (ws.IsAlive)
        {
            if (!_messageLogged) // Check if the message has already been logged
            {
                Debug.Log("WebSocket connection is alive.");
                _messageLogged = true; // Set the variable to true to indicate that the message has been logged
            }
        }
        else
        {
            _messageLogged = false; // Reset the variable to false when the connection is not alive
        }
    }

    private void OnDestroy() { }
}