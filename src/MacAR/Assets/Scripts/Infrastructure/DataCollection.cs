using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
