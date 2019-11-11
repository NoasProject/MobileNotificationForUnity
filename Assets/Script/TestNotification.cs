using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noa.LocalMobileNotification;

public class TestNotification : MonoBehaviour
{
    private int cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start -- Test ---- ");

        this.Send(5, 1);
        this.Send(7, 5);
        this.Send(9, 15);
        this.Send(12, 10);
        this.Send(14, 10);

        Debug.Log("End -- Test ---- ");
    }

    public void Send(int second, int channelId)
    {
        this.cnt += 1;
        MobileNotificationManager.Ins.SendMessage("Test", $"Test -- Desc - {channelId}\nCount: {this.cnt}", second, channelId);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            MobileNotificationManager.Ins.SendMessage("Test", "SubTest", $"バックグラウンドに行ってから、10秒後", 10, 110);
            MobileNotificationManager.Ins.SendMessage("Test", "SubTest", $"バックグラウンドに行ってから、30秒後", 30, 130);
            MobileNotificationManager.Ins.SendMessage("Test", "SubTest", $"バックグラウンドに行ってから、1分後", 60, 160);
        }
    }

}
