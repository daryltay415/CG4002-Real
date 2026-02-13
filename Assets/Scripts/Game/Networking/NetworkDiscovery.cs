using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkDiscovery : MonoBehaviour
{
    private UnityTransport _transport;

    void Awake()
    {
        _transport = GetComponent<UnityTransport>();
    }

    // Call this for the Host instead of just StartHost()
    public void StartHosting()
    {
        // Get local IP (simplest way for testing)
        string ip = GetLocalIPAddress();
        _transport.SetConnectionData(ip, 7777); 
        NetworkManager.Singleton.StartHost();
        Debug.Log($"Host started on: {ip}");
    }

    // Call this for the Client to target a specific IP
    public void JoinGame(string targetIP)
    {
        _transport.SetConnectionData(targetIP, 7777);
        NetworkManager.Singleton.StartClient();
    }

    private string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }
}
