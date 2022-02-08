using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ISupermarketContainer
{
    /// <summary>
    /// Sets the object contained in the shelves and it instances all possible units.
    /// </summary>
    /// <param name="obj">Object to fill the shelves.</param>
    /// <param name="index">Shelf index</param>
    void SetObject(GameObject obj, int index);

    /// <summary>
    /// Sets a list of objects in the shelves and it instances all possible units.
    /// </summary>
    /// <param name="objectsList">An ordered list of objects to put on shelves.</param>
    void SetObjects(List<GameObject> objectsList);

    /// <summary>
    /// Gets the number of shelves.
    /// </summary>
    /// <returns>The number of shelves.</returns>
    int GetShelvesNumber();

    /// <summary>
    /// Gets the container's length.
    /// </summary>
    /// <returns>the container's length.</returns>
    float GetLength();
}
