using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum FruitItems
{
    Apple,
    //Orange,
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
    Bottiglietta,
    //Soda,
    //Lattina,
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
    public static List<string> getItemsList<T> () {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(e => "Prefabs/Items/" + e.ToString()).ToList();
    }
}
