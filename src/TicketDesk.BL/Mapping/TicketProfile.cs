using AutoMapper;
using TicketDesk.Domain.Entities;
using TicketDesk.Shared;

namespace TicketDesk.BL.Mapping;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<Ticket, TicketDto>()
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));

        CreateMap<CreateTicketDto, Ticket>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore());

        CreateMap<UpdateTicketDto, Ticket>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore());
    }
}