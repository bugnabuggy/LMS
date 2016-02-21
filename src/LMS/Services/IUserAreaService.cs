using System.Collections.Generic;
using LMS.Areas.Api.ViewModels;

namespace LMS.Services
{
    public interface IUserAreaService
    {
        List<UserAreaVM> List(GoalListOptions options);
        UserAreaVM Get(GoalListOptions options);
        void Add(UserAreaVM userArea);
        void Update(UserAreaVM userArea);
        void Delete(string areaId);
    }
}