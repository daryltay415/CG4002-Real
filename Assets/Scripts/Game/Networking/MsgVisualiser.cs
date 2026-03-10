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
    private MqttClient client;
    public string brokerIP = "10.166.207.131";
    public string p1_topic = "unity/moves/player-1";
    public string p2_topic = "unity/moves/player-2";

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
        string clientId = "Unity";
        Debug.Log("Attempting to connect to " + brokerIP + "...");
        
        // 4. Connect
        client.Connect(clientId, "iot", "hello"); 
        
        Debug.Log("SUCCESS: Unity Connected!");

        if (IsServer)
        {
            client.Subscribe(new string[] { p1_topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            Debug.Log("Successfully subscribed to" + p1_topic);
        }
        else
        {
            client.Subscribe(new string[] { p2_topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            Debug.Log("Successfully subscribed to" + p2_topic);
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
    if (e.Topic == p1_topic) {
        p1_Message = msg;
        p1_NewData = true; 
    } 
    else if (e.Topic == p2_topic) {
        p2_Message = msg;
        p2_NewData = true;
    }
}

    // Unity Main Thread (60fps)
    void Update() {
        // Only update if the flag is true
        if (p1_NewData) {
            //ProcessGesture(p1_Message, 1);
            OnInputDetected?.Invoke(p1_Message);
            Debug.Log("PLAYER1 got input");
            p1_NewData = false; // Reset flag
        }
        if (p2_NewData)
        {   
            OnInputDetected?.Invoke(p2_Message);
            //ProcessGesture(p2_Message, 2);
            Debug.Log("PLAYER2 got input");
            p2_NewData = false; // Reset flag
        }
    }

//    void ProcessGesture(string json, int playerId) {
//
//        MoveData data = JsonUtility.FromJson<MoveData>(json);
//        
//        // ============ INSERT AR CODE HERE ==============
//        Animator targetAnim = (playerId == 1) ? character1Animator : character2Animator;
//
//        if (targetAnim != null && !string.IsNullOrEmpty(data.gesture)) {
//            targetAnim.SetTrigger(data.gesture);
//            Debug.Log($"Triggering {data.gesture} for Player {playerId}");
//        }
//    }
//
//}


    [Serializable] // This attribute is mandatory for JsonUtility
    public class MoveData
    {
        public int player;
        public string type;
        public string gesture;
    }
}