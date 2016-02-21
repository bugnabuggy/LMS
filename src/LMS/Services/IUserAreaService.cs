using System.Collections.Generic;
using LMS.Areas.Api.ViewModels;

namespace LMS.Services
{
    public interface IUserAreaService
    {
        List<UserAreaVM> List(AreaListOptions options);
        UserAreaVM Get(string areaId, AreaListOptions options);
        UserAreaVM Add(UserAreaVM userArea);
        UserAreaVM Update(UserAreaVM userArea);
        void Delete(string areaId);
    }
}