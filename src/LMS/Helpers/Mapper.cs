using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Areas.Api.ViewModels;
using LMS.Core.Models;

namespace LMS.Services
{
    //Unfortunately, manual mapping, because Automapper is not supports coreclr
    public static class Mapper
    {
        public static GoalVM Map(Goal goal)
        {
            return new GoalVM
            {
                Id = goal.Id,
                StateId = goal.StateId,
                Description = goal.Description,
                Priority = goal.Priority

            };
        }

        public static void Map(GoalVM model, Goal goal)
        {
            goal.Description = model.Description;
            goal.StateId = model.StateId;
        }

        public static Goal Map(GoalVM model)
        {
            return new Goal
            {
                StateId = model.StateId,
                Description = model.Description,
                Priority = model.Priority
            };
        }

        public static UserAreaVM Map(UserArea userArea)
        {
            return new UserAreaVM
            {
                Id = userArea.Id,
                Color = userArea.Color,
                Priority = userArea.Priority,
                Title = userArea.Title,
                Goals = userArea.Goals != null 
                    ? userArea.Goals.Select(Map).ToList() 
                    : new List<GoalVM>()
            };
        }

        public static UserArea Map(UserAreaVM model)
        {
            return new UserArea
            {
                Color = model.Color,
                Priority = model.Priority,
                Title = model.Title
            };
        }

        public static void Map(UserAreaVM model, UserArea item)
        {
            item.Color = model.Color;
            item.Priority = model.Priority;
            item.Title = model.Title;
        }
    }
}
