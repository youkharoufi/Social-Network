using Facebook.Models;
using AutoMapper;


namespace Facebook.Mapper
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {


            CreateMap<RegisterUser, User>();

        }
    }
}
