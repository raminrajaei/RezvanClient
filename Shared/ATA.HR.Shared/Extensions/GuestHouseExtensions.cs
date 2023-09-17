using ATA.HR.Shared.Enums.GuestHouse;

namespace ATA.HR.Shared.Extensions;

public static class GuestHouseExtensions
{
    public static int GetRoomBookingType(this int roomBedCounts, int occupiedBedCounts)
    {
        if (occupiedBedCounts == 0)
        {
            return (int)RoomBookingStatus.Empty;
        }
        else if (roomBedCounts == occupiedBedCounts)
        {
            return (int)RoomBookingStatus.FullCapacity;
        }
        else
        {
            return (int)RoomBookingStatus.HasCapacity;
        }
    }
}
