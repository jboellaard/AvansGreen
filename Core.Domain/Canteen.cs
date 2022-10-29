using System.Reflection;

namespace Core.Domain
{
    public class Canteen : CanteenEnumerable
    {
        private static readonly string CityBreda = "Breda";
        private static readonly bool HasWarmMealsBreda = true;
        public static readonly Canteen LA5 = new(1, nameof(LA5), CityBreda, HasWarmMealsBreda);
        public static readonly Canteen LD1 = new(2, nameof(LD1), CityBreda, HasWarmMealsBreda);
        public static readonly Canteen HA1 = new(3, nameof(HA1), CityBreda, HasWarmMealsBreda);

        private static readonly string CityTilburg = "Tilburg";
        private static readonly bool HasWarmMealsTilburg = false;
        public static readonly Canteen TH1 = new(4, nameof(TH1), CityTilburg, HasWarmMealsTilburg);
        public static readonly Canteen TH5 = new(5, nameof(TH5), CityTilburg, HasWarmMealsTilburg);

        private static readonly string CityDenBosch = "Den Bosch";
        private static readonly bool HasWarmMealsDenBosch = true;
        public static readonly Canteen DH1 = new(6, nameof(DH1), CityDenBosch, HasWarmMealsDenBosch);
        public static readonly Canteen DH5 = new(7, nameof(DH5), CityDenBosch, HasWarmMealsDenBosch);

        public Canteen(int id, string name, string city, bool hasWarmMeals) : base(id, name, city, hasWarmMeals)
        {
        }
    }

    public abstract class CanteenEnumerable : IComparable
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string City { get; private set; }

        public bool HasWarmMeals { get; private set; }

        protected CanteenEnumerable(int id, string name, string city, bool hasWarmMeals)
            => (Id, Name, City, HasWarmMeals) = (id, name, city, hasWarmMeals);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : CanteenEnumerable =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public static T FromId<T>(int id) where T : CanteenEnumerable
        {
            var matchingItem = Parse<T, int>(id, "Id", item => item.Id == id);
            return matchingItem;
        }

        public static T FromName<T>(string name) where T : CanteenEnumerable
        {
            var matchingItem = Parse<T, string>(name, "name", item => item.Name == name);
            return matchingItem;
        }

        private static TCanteen Parse<TCanteen, TIntOrString>(
        TIntOrString nameOrValue,
        string description,
        Func<TCanteen, bool> predicate)
        where TCanteen : CanteenEnumerable
        {
            var matchingItem = GetAll<TCanteen>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                throw new InvalidOperationException(
                    $"'{nameOrValue}' is not a valid {description} in {typeof(TCanteen)}");
            }

            return matchingItem;
        }


#pragma warning disable CS8765 // Nullability of type parameter doesn't match overridden member (possibly because of nullability attributes).
        public override bool Equals(object obj)
        {
            if (obj is not CanteenEnumerable otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }
#pragma warning restore CS8765 // Nullability of type parameter doesn't match overridden member (possibly because of nullability attributes).


#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).           
        public int CompareTo(object other)
        {
            if (City.Equals(((CanteenEnumerable)other).City))
                return Id.CompareTo(((CanteenEnumerable)other).Id);
            else return City.CompareTo(((CanteenEnumerable)other).City);
        }
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

        public override int GetHashCode() => City.GetHashCode() ^ Id.GetHashCode();
    }



}
