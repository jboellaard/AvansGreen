namespace Core.Domain
{
    public enum MealTypeId : int
    {
        Bread = 1,
        WarmMeal = 2,
        Drink = 3,
        Snack = 4
    }

    public class MealType
    {
        public MealTypeId MealTypeId { get; set; }
        public string Name { get; set; } = null!;

    }

}
