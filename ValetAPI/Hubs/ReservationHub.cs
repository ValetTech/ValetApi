using Microsoft.AspNetCore.SignalR;
using ValetAPI.Models;

namespace ValetAPI.Hubs;

/// <summary>
/// </summary>
public class ReservationHub : Hub<IReservationClient>
{
    /// <summary>
    /// </summary>
    /// <param name="reservation"></param>
    public async Task NewReservation(Reservation reservation)
    {
        await Clients.All.ReceiveReservation(reservation);
    }
}