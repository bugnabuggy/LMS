using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Areas.Api.ViewModels;
using LMS.Core.Models;
using LMS.Services;
using Xunit;

namespace LMS.Tests
{
    public class UserAreaServiceTest : AUnitTest
    {

        [Fact]
        public void AddUserArea()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            var area = new UserAreaVM
            {
                Title = "My new area",
                Color = "#FFFFFF"
            };

            var service = CreateUserAreaService();
            var addedAreaModel = service.Add(area);
            var addedArea = _userAreaRepository.Source.Last();

            Assert.Equal(2, _userAreaRepository.Source.Count);
            Assert.NotNull(addedAreaModel.Id);
            Assert.Equal(area.Title, addedArea.Title);
            Assert.Equal(area.Title, addedAreaModel.Title);
            Assert.Equal(area.Color, addedArea.Color);
            Assert.Equal(area.Color, addedAreaModel.Color);
        }

        private UserAreaService CreateUserAreaService()
        {
            return new UserAreaService(_userAreaRepository,
                _goalRepository,
                new TimeConverter(_context),
                new UnitOfWorkFactoryFake(),
                _context);
        }

        [Fact]
        public void UpdateExistsUserArea()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            var area2 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
                area2
            };

            var area = new UserAreaVM
            {
                Id = area1.Id,
                Title = "My new area",
                Color = "#FFFFFF"
            };

            var service = CreateUserAreaService();
            service.Update(area);
            var addedArea = _userAreaRepository.Source[0];

            Assert.Equal(2, _userAreaRepository.Source.Count);
            Assert.Equal(area.Title, addedArea.Title);
            Assert.Equal(area.Color, addedArea.Color);
        }

        [Fact]
        public void UpdateNotExistsUserAreaThrowException()
        {
            var area = new UserAreaVM
            {
                Id = "area id"
            };

            var service = CreateUserAreaService();
            Assert.Throws<ItemNotFountException>(() => service.Update(area));
        }

        [Fact]
        public void RemoveExistsArea()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            var area2 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
                area2
            };

            var service = CreateUserAreaService();
            service.Delete(area1.Id);

            Assert.Equal(1, _userAreaRepository.Source.Count);
            Assert.NotEqual(area1.Id, _userAreaRepository.Source[0].Id);
        }

        [Fact]
        public void RemoveNotExistsAreaDontThrowException()
        {
            var service = CreateUserAreaService();
            service.Delete("Some goal id");
        }

        [Fact]
        public void RemoveAnotherUserAreaDontRemoveTheArea()
        {
            var user1 = new User();
            var user2 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user2, UserId = user2.Id };
            var area2 = new UserArea { User = user2, UserId = user2.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
                area2
            };

            var service = CreateUserAreaService();
            service.Delete(area1.Id);

            Assert.Equal(2, _userAreaRepository.Source.Count);
        }

        [Fact]
        public void GetAreaByIdReturnsCorrectGoal()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            var area2 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1, area2
            };

            var service = CreateUserAreaService();
            var model = service.Get(area2.Id, new AreaListOptions { IncludeGoals = false });

            Assert.Equal(area2.Id, model.Id);
            Assert.Equal(area2.Title, model.Title);
            Assert.Equal(area2.Color, model.Color);
        }

        [Fact]
        public void GetAnotherUserAreaThrowException()
        {
            var user1 = new User();
            var user2 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            var area2 = new UserArea { User = user2, UserId = user2.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
                area2
            };

            var service = CreateUserAreaService();
            Assert.Throws<ItemNotFountException>(() => service.Get(area2.Id, new AreaListOptions()));
        }

        [Fact]
        public void GetNotExistsAreaByIdThrowsException()
        {
            var service = CreateUserAreaService();
            Assert.Throws<ItemNotFountException>(() => service.Get("Goal id", new AreaListOptions()));
        }
    }
}
