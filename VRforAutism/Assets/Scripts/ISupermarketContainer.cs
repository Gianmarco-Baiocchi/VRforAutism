using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISupermarketContainer
{
    /// <summary>
    /// Sets the object contained in the shelves and it instances all possible units.
    /// </summary>
    /// <param name="obj">Object to fill the shelves</param>
    void SetObject(UnityEngine.GameObject obj);

    /// <summary>
    /// Gets the container's length.
    /// </summary>
    /// <returns>the container's length</returns>
    float GetLength();
}
