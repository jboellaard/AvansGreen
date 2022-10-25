using System.Reflection;

namespace Core.Domain
{
    public abstract class Canteen : IComparable
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string City { get; private set; }

        public bool HasWarmMeals { get; private set; }

        protected Canteen(int id, string name, string city, bool hasWarmMeals)
            => (Id, Name, City, HasWarmMeals) = (id, name, city, hasWarmMeals);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Canteen =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();


#pragma warning disable CS8765 // Nullability of type parameter doesn't match overridden member (possibly because of nullability attributes).
        public override bool Equals(object obj)
        {
            if (obj is not Canteen otherValue)
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
            if (City.Equals(((Canteen)other).City))
                return Id.CompareTo(((Canteen)other).Id);
            else return City.CompareTo(((Canteen)other).City);
        }
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

        public override int GetHashCode() => City.GetHashCode() ^ Id.GetHashCode();

    }

    public class BredaCanteen : Canteen
    {
        private readonly static new string City = "Breda";
        private readonly static new bool HasWarmMeals = true;
        public static BredaCanteen LA5 = new(1, nameof(LA5));
        public static BredaCanteen LD1 = new(2, nameof(LD1));
        public static BredaCanteen HA1 = new(3, nameof(HA1));
        public BredaCanteen(int id, string name) : base(id, name, City, HasWarmMeals)
        {
        }
    }

    public class TilburgCanteen : Canteen
    {
        private readonly static new string City = "Tilburg";
        private readonly static new bool HasWarmMeals = false;
        public static TilburgCanteen TH1 = new(4, nameof(TH1));
        public static TilburgCanteen TH5 = new(5, nameof(TH5));
        public TilburgCanteen(int id, string name) : base(id, name, City, HasWarmMeals)
        {
        }
    }

    public class DenBoschCanteen : Canteen
    {
        private readonly static new string City = "Den Bosch";
        private readonly static new bool HasWarmMeals = true;
        public static DenBoschCanteen DH1 = new(6, nameof(DH1));
        public static DenBoschCanteen DH5 = new(7, nameof(DH5));
        public DenBoschCanteen(int id, string name) : base(id, name, City, HasWarmMeals)
        {
        }
    }

}
