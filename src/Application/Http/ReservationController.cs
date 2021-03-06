using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CQS.Reservation;
using Application.CQS.Reservation.Command;
using Application.CQS.Reservation.Query;
using Microsoft.AspNetCore.Mvc;

namespace Application.Http
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController
    {
        [HttpGet]
        public async Task<IEnumerable<ReservationsOutput>> GetReservations(
            [FromServices] GetAllReservationsQuery query,
            [FromQuery] ReservationsFilter filter
        )
        {
            return await query.ExecuteAsync(filter);
        }

        [HttpGet("{roomId:guid}/dates")]
        public async Task<IEnumerable<ReservedDateOutput>> GetAllReservedDatesAsync(
            [FromServices] GetAllReservedDatesQuery query,
            [FromRoute] Guid roomId
        )
        {
            return await query.ExecuteAsync(roomId);
        }

        [HttpPost]
        public async Task CreateReservation([FromServices] CreateReservationCommand command, [FromBody] ReservationInput input)
        {
            await command.ExecuteAsync(input);
        }

        [HttpDelete("reservationId:guid")]
        public async Task DeleteReservation([FromServices] DeleteReservationCommand command, [FromRoute] Guid reservationId)
        {
            await command.ExecuteAsync(reservationId);
        }
    }
}
