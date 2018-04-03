using AutoMapper;
using EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebModels;

namespace CMS.MVC.AutoMapper
{
    public static class MapperExtention
    {
        #region User
 
         public static UserModels ToModel(this User entity)
          {
              return Mapper.Map<User, UserModels>(entity);
         }

         public static User ToEntity(this UserModels model)
         {
             return Mapper.Map<UserModels, User>(model);
         }
 
         public static User ToEntity(this UserModels model, User entity)
         {
             return Mapper.Map<UserModels, User>(model, entity);
         }
        #endregion
    }
}