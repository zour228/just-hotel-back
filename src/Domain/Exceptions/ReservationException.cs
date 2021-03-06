using System;

namespace Domain.Exceptions
{
    public class ReservationException : Exception
    {
        public ReservationException(string message): base(message)
        {
        }

        public static void AssertDatesValid(DateTime from, DateTime to)
        {
            if (to < DateTime.Now)
            {
                throw new ReservationException("Reservation date 'from' can't be in past.");
            }

            if (from >= to)
            {
                throw new ReservationException("Reservation date 'from' must be greater than date 'to'.");
            }
        }
    }
}
