using ValetAPI.Models;

namespace ValetAPI.Hubs;

/// <summary>
/// 
/// </summary>
public interface IReservationClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reservation"></param>
    /// <returns></returns>
    Task ReceiveReservation(Reservation reservation);
}