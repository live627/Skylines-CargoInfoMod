using System;
using System.Linq;

namespace CargoInfoMod.Data
{
    [Flags]
    public enum CarFlags
    {
        None                = 0x0000,
        Previous            = 0x0001,
        Sent                = 0x0002,
        Imported            = 0x0004,
        Exported            = 0x0008,

        Resource            = 0x01F0,

        Oil                 = 0x0000,
        Petrol              = 0x0010,
        Ore                 = 0x0020,
        Coal                = 0x0030,
        Logs                = 0x0040,
        Lumber              = 0x0050,
        Grain               = 0x0060,
        Food                = 0x0070,
        Goods               = 0x0080,
        Mail                = 0x0090,
        UnsortedMail        = 0x00A0,
        SortedMail          = 0x00B0,
        OutgoingMail        = 0x00C0,
        IncomingMail        = 0x00D0,
        AnimalProducts      = 0x00E0,
        Flours              = 0x00F0,
        Paper               = 0x0100,
        PlanedTimber        = 0x0110,
        Petroleum           = 0x0120,
        Plastics            = 0x0130,
        Glass               = 0x0140,
        Metals              = 0x0150,
        LuxuryProducts      = 0x0160
    }

    [Serializable]
    public class CargoStats2
    {
        public int[] CarsCounted;

        public CargoStats2()
        {
            CarsCounted = new int[(int)Enum.GetValues(typeof(CarFlags)).Cast<CarFlags>().Aggregate((v, agg) => agg | v) + 1];
        }

        // Syntactic sugar fluff
        public int GetTotalWhere(Func<CarFlags, bool> pred)
        {
            return CarsCounted.Where((t, idx) => pred((CarFlags)idx)).Sum();
        }

        public int CarsSentLastTime => GetTotalWhere(f => (f & CarFlags.Sent) != 0 && (f & CarFlags.Previous) != 0);
        public int CarsReceivedLastTime => GetTotalWhere(f => (f & CarFlags.Sent) == 0 && (f & CarFlags.Previous) != 0);
        public int CarsSent => GetTotalWhere(f => (f & CarFlags.Sent) != 0 && (f & CarFlags.Previous) == 0);
        public int CarsReceived => GetTotalWhere(f => (f & CarFlags.Sent) == 0 && (f & CarFlags.Previous) == 0);

        // In case the number of flags changes between versions
        public CargoStats2 Upgrade()
        {
            var upgradedStats = new CargoStats2();
            CarsCounted.CopyTo(upgradedStats.CarsCounted, 0);
            return upgradedStats;
        }
    }

    // Used only in v1.1 and below
    //[Serializable]
    //public class CargoStats
    //{
    //    public int carsReceivedLastTime = 0;
    //    public int carsSentLastTime = 0;
    //}
}
