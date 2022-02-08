using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum FruitItems
{
    apple,
    pear
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
    biscuits,
    sugar
}

enum DrinkItems
{
    water,
    tea
}

enum TempItems
{
    Bottiglietta,
    FlaconeShampoo,
    Lattina
}

static class SupermarketItems
{
    public static List<string> getItemsList<T> () {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(e => "Prefabs/" + e.ToString()).ToList();
    }
}
