using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class DoubleExtensions
    {
        public static decimal? TryRoundNumber(this double? value, int decimalToRound)
        {
            if (value == null)
                return null;

            return TryRoundNumber(value.Value, decimalToRound);

        }

        public static decimal TryRoundNumber(this double value, int decimalToRound)
        {

            return TryRoundNumber((decimal)(value), decimalToRound);

        }

        public static decimal? TryRoundNumber(this decimal? value, int decimalToRound)
        {
            if (value == null)
                return null;

            return TryRoundNumber(value.Value, decimalToRound);

        }

        public static decimal TryRoundNumber(this decimal value, int decimalToRound)
        {

            try
            {
                return Decimal.Round(value, decimalToRound);
            }
            catch
            {
                return value;
            }
        }
    }
}
