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
