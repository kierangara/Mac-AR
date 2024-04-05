//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
[Serializable]
public class DataCollection
{
    public string lobbyId;

    public DataCollection()
    {
        FullReset();
    }

    public void FullReset()
    {
        lobbyId = "";
    }
}
