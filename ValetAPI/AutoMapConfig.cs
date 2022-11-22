using AutoMapper;
using ValetAPI.Models;

namespace ValetAPI;

public class AutoMapConfig
{
    public AutoMapConfig()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });
    }
}

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // CreateMap<VenueDto, Venue>().ReverseMap();

        CreateMap<AreaEntity, Area>()
            .ForMember(a => a.Sittings, opt => opt.MapFrom(x => x.AreaSittings.Select(y => y.Sitting)));

        CreateMap<SittingEntity, Sitting>()
            .ForMember(a => a.Areas, opt => opt.MapFrom(x => x.AreaSittings.Select(y => y.Area)));

        CreateMap<ReservationEntity, Reservation>()
            .ForMember(r => r.Tables, opt => opt.MapFrom(x => x.ReservationTables.Select(y => y.Table)));


        // CreateMap<Area, AreaEntity>()
        //     .ForMember(a => a.AreaSittings, opt => opt.MapFrom(x => x.AreaSittings.Select(y => y.Sitting).ToList()));

        // CreateMap<Area, AreaEntity>()
        //     .ForMember(a => a.AreaSittings, opt => opt.MapFrom(x => x.Sittings.Select(y => y.Areas).ToList()));

        CreateMap<ReservationTable, TableEntity>().ReverseMap();
        CreateMap<ReservationTable, ReservationEntity>().ReverseMap();
        CreateMap<ReservationTable, Table>().ReverseMap();
        CreateMap<ReservationTable, Reservation>().ReverseMap();
        

        CreateMap<Area, AreaEntity>();
        CreateMap<AreaSitting, AreaSittingEntity>().ReverseMap();
        CreateMap<Customer, CustomerEntity>().ReverseMap();
        CreateMap<Reservation, ReservationEntity>().ReverseMap();
        CreateMap<Sitting, SittingEntity>();
        CreateMap<Table, TableEntity>().ReverseMap();
        CreateMap<Venue, VenueEntity>().ReverseMap();

        CreateMap<Area, Models.DTO.Area>().ReverseMap();
        CreateMap<Table, Models.DTO.Table>().ReverseMap();
        CreateMap<Customer, Models.DTO.Customer>().ReverseMap();
        CreateMap<Reservation, Models.DTO.Reservation>().ReverseMap();
        CreateMap<Sitting, Models.DTO.Sitting>().ReverseMap();
        
        CreateMap<AreaEntity, Models.DTO.Area>().ReverseMap();
        CreateMap<TableEntity, Models.DTO.Table>().ReverseMap();
        CreateMap<CustomerEntity, Models.DTO.Customer>().ReverseMap();
        CreateMap<ReservationEntity, Models.DTO.Reservation>().ReverseMap();
        CreateMap<SittingEntity, Models.DTO.Sitting>().ReverseMap();

        // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
    }
}