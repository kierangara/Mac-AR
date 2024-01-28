using System.Collections;
using System.Collections.Generic;

interface IPuzzle
{
    int GetNumberOfPlayers();
    List<IComponent> GetComponents();
}
