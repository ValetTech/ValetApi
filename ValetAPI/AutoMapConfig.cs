using AutoMapper;
using ValetAPI.Models;

namespace ValetAPI;

public class AutoMapConfig
{
    public AutoMapConfig()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });
    }
}


public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // CreateMap<VenueDto, Venue>().ReverseMap();
        CreateMap<Area, AreaEntity>().ReverseMap();
        CreateMap<AreaSitting, AreaSittingEntity>().ReverseMap();
        CreateMap<Customer, CustomerEntity>().ReverseMap();
        CreateMap<Reservation, ReservationEntity>().ReverseMap();
        CreateMap<Sitting, SittingEntity>().ReverseMap();
        CreateMap<Table, TableEntity>().ReverseMap();
        CreateMap<Venue, VenueEntity>().ReverseMap();
        // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
    }
}