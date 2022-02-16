using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum FruitItems
{
    AppleRed,
    AppleYellow,
    AppleGreen,
    Orange,
    OrangeRed,
    Peach,
    Ananas,
    Avocado,
    Lime,
    Lemon,
    Pear,
    Banana,
}

enum VegetablesItems
{
    Bean,
    Peas,
    Broccoli,
}

enum FridgeItems
{
    Milk,
    Cheese,
    Tuna,
}

enum SweetItems
{
    Biscuit,
    Chips,
    Pasta,
    Cracker,
}

enum DrinkItems
{
    Water,
    Tea,
    Soda,
    Cola,
}

enum BeautyItems
{
    Shampoo,
    FaceCream,
    Balsamo,
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
