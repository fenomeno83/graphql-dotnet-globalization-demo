using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class DateTimeExtensions
    {

        public static DateTime? SetKind(this DateTime? DT, DateTimeKind DTKind)
        {
            if (DT == null)
                return null;

            return SetKind(DT.Value, DTKind);

        }

        public static DateTime SetKind(this DateTime DT, DateTimeKind DTKind)
        {
            var NewDT = new DateTime(DT.Year, DT.Month, DT.Day, DT.Hour, DT.Minute, DT.Second, DT.Millisecond, DTKind);
            return NewDT;
        }

        public static DateTime? ConvertDate(this DateTime? DT, DateTimeKind DestinationKind, DateTimeKind DefaultUnspecifiedKind = DateTimeKind.Local)
        {
            if (DT == null)
                return null;

            return ConvertDate(DT.Value, DestinationKind, DefaultUnspecifiedKind);

        }

        public static DateTime ConvertDate(this DateTime DT, DateTimeKind DestinationKind, DateTimeKind DefaultUnspecifiedKind = DateTimeKind.Local)
        {
            if (DestinationKind == DateTimeKind.Unspecified)
                return DT;

            if (DT.Kind == DateTimeKind.Unspecified) //set default kind
                DT = DT.SetKind(DefaultUnspecifiedKind == DateTimeKind.Unspecified ? DateTimeKind.Local : DefaultUnspecifiedKind);

            if (DestinationKind == DateTimeKind.Local) //local
            {
                if (DT.Kind == DateTimeKind.Local)
                    return DT;
                else
                    return DT.ToLocalTime();

            }
            else //utc
            {
                if (DT.Kind == DateTimeKind.Local)
                    return DT.ToUniversalTime();
                else
                    return DT;
            }
        }
    }
}
