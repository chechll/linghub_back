using AutoMapper;
using linghub.Dto;

namespace linghub.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Word, WordDto>();
            CreateMap<WordDto, Word>();
            CreateMap<Text, TextDto>();
            CreateMap<TextDto, Text>();
            CreateMap<Calendar, CalendarDto>();
            CreateMap<CalendarDto, Calendar>();
            CreateMap<UText, UtextDto>();
            CreateMap<UtextDto, UText>();
            CreateMap<UWord, UwordDto>();
            CreateMap<UwordDto, UWord>();
        }
    }
}
