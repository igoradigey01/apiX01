using AutoMapper;
using ShopAPI.Models;


//https://code-maze.com/automapper-net-core/ 
//https://www.c-sharpcorner.com/article/integrate-automapper-in-net-core-web-api2/
namespace ShopAPI
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }

    }
}
