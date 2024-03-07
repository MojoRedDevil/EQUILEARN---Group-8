using UnityEngine;
using TMPro;
using WebSocketSharp;

public class WebSocketConnection : MonoBehaviour
{
    WebSocket ws;
    public TextMeshProUGUI dataText;

    private static WebSocket _ws;

    private void Start()
    {
        if (_ws == null)
        {
            _ws = new WebSocket("ws://localhost:8765");
            _ws.OnMessage += OnMessageReceived;
            _ws.Connect();
        }

        ws = _ws;

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connection is open.");
        };
    }

    void OnMessageReceived(object sender, MessageEventArgs e)
    {
        Debug.Log("Message received");
        dataText.text = e.Data;
        Debug.Log("Message set");
    }

    void OnDestroy() { }
}