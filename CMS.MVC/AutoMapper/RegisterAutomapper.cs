
using AutoMapper;
using EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebModels;

namespace CMS.MVC.AutoMapper
{
    public static class RegisterAutomapper
    {
        public static void Excute()
         {

             //Admin
             Mapper.CreateMap<UserModels, User>();
             Mapper.CreateMap<User, UserModels>();
      
         }
}
}