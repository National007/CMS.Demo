
using AutoMapper;
using EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebModels;

namespace Application.AutoMapper
{
    public static class RegisterAutomapper
    {
        public static void Excute()
         {

            //Admin
            Mapper.CreateMap<User, UserModels>()
                .ForMember(m=>m.SexName,o=>o.MapFrom(d=>d.Sex==1?"A":"B"));
            Mapper.CreateMap<UserModels, User>();
             
      
         }
}
}