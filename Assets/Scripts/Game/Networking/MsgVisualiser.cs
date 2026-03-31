using UnityEngine;
using UnityEngine.UI; // Required for UI
using TMPro;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Unity.Netcode;

public class MsgVisualiser : NetworkBehaviour{
    public static MsgVisualiser Instance;
    public event Action<string> OnInputDetected;
    public event Action<int> OnBPMDetected;
    private MqttClient client;
    public string brokerIP = "54.66.30.206";
    public string p1_move_topic = "unity/moves/player-1";
    public string p2_move_topic = "unity/moves/player-2";
    public string p1_feedback_topic = "unity/feedback/player-1";
    public string p2_feedback_topic = "unity/feedback/player-2";
    public int TopicToSub = 1;

    [Header("UI References")]
    public Image statusLight;
    public GameObject visualizerCube;

    public TextMeshProUGUI messageDisplayText;

    // --- Main Thread Bridge ---
    private string p1_Message = "";
    private bool p1_NewData = false;

    private string p2_Message = "";
    private bool p2_NewData = false;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        
    }

    public override void OnNetworkSpawn() {
    Debug.Log("Starting Connection Sequence...");

    // Load Certificate
    TextAsset certAsset = Resources.Load<TextAsset>("ca.crt"); 
    if (certAsset == null) { Debug.LogError("CRITICAL: ca.crt.txt not found in Resources!"); return; }
    X509Certificate caCert = new X509Certificate(certAsset.bytes);
    Debug.Log("Loaded CA: " + caCert.Subject);

    // This forces Unity to accept the certificate no matter what
    System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => {
        Debug.LogWarning("Bypassing SSL Error: " + sslPolicyErrors);
        return true; // FORCE SUCCESS
    };

    UnityEngine.Debug.unityLogger.logEnabled = true;

    // Initialize Client
    // Pass 'null' for the callback here because we handled it globally above
    client = new MqttClient(brokerIP, 8883, true, caCert, null, MqttSslProtocols.TLSv1_2, RemoteCertificateValidationCallback);
    
    client.MqttMsgPublishReceived += OnMessageReceived;

    try {

        //Added this line to distiniguish host and client (not sure if it matters)
        string clientId = "Unity-" + TopicToSub.ToString();
        Debug.Log("Attempting to connect to " + brokerIP + "...");
        
        // 4. Connect
        client.Connect(clientId, "iot", "hello"); 
        
        Debug.Log("SUCCESS: Unity Connected!");

        if (TopicToSub == 1)
        {
            client.Subscribe(new string[] { p1_move_topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            Debug.Log("Successfully subscribed to" + p1_move_topic);
        }
        else
        {
            client.Subscribe(new string[] { p2_move_topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            Debug.Log("Successfully subscribed to" + p2_move_topic);
        }
        
    } catch (Exception e) {
        // Print the INNER exception for debugging
        Debug.LogError("CONNECTION FAILED!");
        Debug.LogError("Main Error: " + e.Message);
        if (e.InnerException != null) {
            Debug.LogError("REAL REASON: " + e.InnerException.Message);
        }

        if (e.InnerException?.InnerException != null)
        Debug.LogError("Deep Inner Exception: " + e.InnerException.InnerException.Message);
        }
    }

    bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
        // Allow if there are no errors
        if (sslPolicyErrors == SslPolicyErrors.None) return true;

        // Allow Self-signed certificate
        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0) { 
            Debug.LogWarning("Allow Self Signed Certificates");
            return true;
        }
        // Allow "Name Mismatch" (IP address mismatch)
        // This ensures that even if you switch networks, the encryption holds but the name check is skipped.
        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0) {
            Debug.LogWarning("Certificate Name Mismatch (Ignoring for Demo): " + sslPolicyErrors);
            return true; 
        }

        Debug.LogError("Security Blocked: " + sslPolicyErrors);
        return false;
    }

    // MQTT Background Thread
    void OnMessageReceived(object sender, MqttMsgPublishEventArgs e) {
    string msg = System.Text.Encoding.UTF8.GetString(e.Message);

    // Identify which topic the message belongs to
    if (e.Topic == p1_move_topic) {
        p1_Message = msg;
        p1_NewData = true; 
    } 
    else if (e.Topic == p2_move_topic) {
        p2_Message = msg;
        p2_NewData = true;
    }
}

    // Unity Main Thread (60fps)
    void Update() {
        // Only update if the flag is true
        if (p1_NewData) {
            var(gesture, bpm) = ParseSensorData(p1_Message);
            Debug.Log("Player 1: " + bpm);
            OnInputDetected?.Invoke(gesture);
            OnBPMDetected?.Invoke(bpm);
            Debug.Log("PLAYER1 got input");
            p1_NewData = false; // Reset flag
        }
        if (p2_NewData)
        {   
            var(gesture, bpm) = ParseSensorData(p1_Message);
            Debug.Log("Player 2: " + bpm);
            OnInputDetected?.Invoke(gesture);
            OnBPMDetected?.Invoke(bpm);
            Debug.Log("PLAYER2 got input");
            p2_NewData = false; // Reset flag
        }
    }

    public void sendFeedback(String msg, ulong clientID) {
        string pub_topic = "";
        if (clientID == 0) {
            pub_topic = p1_feedback_topic;
            Debug.Log("P1 got feedback");

        } else if (clientID == 1) {
            pub_topic = p2_feedback_topic;
            Debug.Log("P2 got feedback");
        } else {
            Debug.Log("Wrong client ID");
            return;
        }

        if (client != null && client.IsConnected) {
        // Convert string directly to UTF8 bytes
            byte[] payload = System.Text.Encoding.UTF8.GetBytes(msg);
            
            // Publish with QoS 1 to ensure delivery
            client.Publish(pub_topic, payload, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            
            Debug.Log($"[MQTT] Sent to {pub_topic}: {msg}");
        } else {
            Debug.LogError("MQTT Client is offline!");
        }
    }

    (string gesture, int bpm) ParseSensorData(string jsonString) {
        try {
            PlayerData incomingData = JsonUtility.FromJson<PlayerData>(jsonString);

            if (incomingData != null) 
            {
                // Return the two values directly
                return (incomingData.gesture, incomingData.bpm);
            }
        }
        catch (Exception e) 
        {
            Debug.LogError($"Parse Error: {e.Message}");
        }

        // Return default/fallback values if parsing fails
        return ("UNKNOWN", 0);
    }


    [Serializable] // This attribute is mandatory for JsonUtility
    public class PlayerData
    {
        public string gesture;
        public int bpm;
    }
}