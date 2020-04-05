using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PingLog : MonoBehaviour
{
    private Text text;
    private string ip;
    StringBuilder log;
    Ping ping;

    void Start()
    {
        ip = NetManager.Instance.GetIP();
        log = new StringBuilder();
        text = this.gameObject.GetComponent<Text>();       
        Ping();
    }

    void Update()
    {
        if (ping != null && ping.isDone)
        {
            log.Remove(0, log.Length);
            log.Append(ping.time);
            log.Append("MS");
            text.text = log.ToString();
            if (ping.time <= 100)
            {
                text.color = Color.white;
            }
            else if (ping.time < 300)
            {
                text.color = new Color(1f, 0.5f, 0f, 1f);
            }
            else
            {
                text.color = Color.red;
            }
            ping.DestroyPing();
            ping = null;
            Invoke("Ping", 1);
        }
    }

    private void Ping()
    {
        ping = new Ping(ip);
    }
}
