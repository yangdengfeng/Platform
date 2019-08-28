using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
    public class XUtils
    {
        public static void Assign<T, S>(T target, S source)
        {
            var typeFrom = target.GetType();
            var typeTo = source.GetType();
            var propsFrom = typeFrom.GetProperties();
            var propsTo = typeTo.GetProperties().ToList();
            foreach (var s in propsFrom)
            {
                var t = propsTo.Find(r=>r.Name == s.Name);
                if (t == null)
                    continue;
                t.SetValue(target, s.GetValue(source));
            }
        }

        public const double EARTH_RADIUS = 6378137;

        private static double toRad(double arc)
        {
            return arc * Math.PI / 180;
        }

        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            lng1 = toRad(lng1);
            lat1 = toRad(lat1);
            lng2 = toRad(lng2);
            lat2 = toRad(lat2);

            double lat = lat1 - lat2;
            double lng = lng1 - lng2;

            double dis = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(lat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(lng / 2), 2)));
            dis = dis * EARTH_RADIUS;
            dis = Math.Round(dis * 10) / 10;

            return dis;
        }
    }
}
