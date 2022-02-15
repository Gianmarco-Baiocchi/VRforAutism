using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum FruitItems
{
    Apple,
    Orange,
    Peach,
    Ananas,
}

enum VegetablesItems
{
    salad,
    tomato
}

enum FridgeItems
{
    milk,
    cheese,
    yogurt
}

enum SweetItems
{
    Biscuit,
}

enum DrinkItems
{
    Water,
    Tea,
    Soda,
    Lattina,
}

enum BeautyItems
{
    FlaconeShampoo,
}

enum TempItems
{
    Bottiglietta,
    //FlaconeShampoo,
    //Lattina,
    //Biscuit,
    //Orange,
    //Water,
    //Apple,
    //Soda
}

static class SupermarketItems
{
    /// <summary>
    /// Gets the objects list of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of objects.</typeparam>
    /// <returns>the list of objects' path of a certain type.</returns>
    public static List<string> getItemsList<T> () {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(e => "Prefabs/Items/" + e.ToString()).ToList();
    }
}
