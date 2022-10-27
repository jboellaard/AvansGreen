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

        //public static IEnumerable<T> GetAll<T>() where T : Canteen =>
        //    typeof(T).GetFields(BindingFlags.Public |
        //                        BindingFlags.Static |
        //                        BindingFlags.DeclaredOnly)
        //             .Select(f => f.GetValue(null))
        //             .Cast<T>();


        //public static T FromName<T>(string name) where T : Canteen
        //{
        //    var matchingItem = Parse<T, string>(name, "name", item => item.Name == name);
        //    return matchingItem;
        //}

        //private static TCanteen Parse<TCanteen, TIntOrString>(
        //TIntOrString nameOrValue,
        //string description,
        //Func<TCanteen, bool> predicate)
        //where TCanteen : Canteen
        //{
        //    var matchingItem = GetAll<TCanteen>().FirstOrDefault(predicate);

        //    if (matchingItem == null)
        //    {
        //        throw new InvalidOperationException(
        //            $"'{nameOrValue}' is not a valid {description} in {typeof(TCanteen)}");
        //    }

        //    return matchingItem;
        //}


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
        private static readonly new string City = "Breda";
        private static readonly new bool HasWarmMeals = true;
        public static readonly BredaCanteen LA5 = new(1, nameof(LA5));
        public static readonly BredaCanteen LD1 = new(2, nameof(LD1));
        public static readonly BredaCanteen HA1 = new(3, nameof(HA1));
        public BredaCanteen(int id, string name) : base(id, name, City, HasWarmMeals)
        {
        }
    }

    public class TilburgCanteen : Canteen
    {
        private static readonly new string City = "Tilburg";
        private static readonly new bool HasWarmMeals = false;
        public static readonly TilburgCanteen TH1 = new(4, nameof(TH1));
        public static readonly TilburgCanteen TH5 = new(5, nameof(TH5));
        public TilburgCanteen(int id, string name) : base(id, name, City, HasWarmMeals)
        {
        }
    }

    public class DenBoschCanteen : Canteen
    {
        private static readonly new string City = "Den Bosch";
        private static readonly new bool HasWarmMeals = true;
        public static readonly DenBoschCanteen DH1 = new(6, nameof(DH1));
        public static readonly DenBoschCanteen DH5 = new(7, nameof(DH5));
        public DenBoschCanteen(int id, string name) : base(id, name, City, HasWarmMeals)
        {
        }
    }

}
