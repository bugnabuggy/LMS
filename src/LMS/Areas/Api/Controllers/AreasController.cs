using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Areas.Api.ViewModels;
using LMS.Services;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace LMS.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    public class AreasController : AController
    {
        private IUserAreaService _userAreaService;

        public AreasController(IAppContext appContext, IUserAreaService userAreaService)
            : base(appContext)
        {
            _userAreaService = userAreaService;
        }

        [HttpGet]
        public IEnumerable<UserAreaVM> Get(bool includeGoals = true, bool onlyLastGoals = true)
        {
            return _userAreaService.List(new AreaListOptions { IncludeGoals = includeGoals, OnlyLastGoals = onlyLastGoals });
        }

        [HttpGet("{id}")]
        public UserAreaVM Get(string id, bool includeGoals = true, bool onlyLastGoals = true)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Area id is not set");
            }
            return _userAreaService.Get(id, new AreaListOptions { IncludeGoals = includeGoals, OnlyLastGoals = onlyLastGoals });
        }

        [HttpPost]
        public UserAreaVM Post([FromBody]UserAreaVM userArea)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException();
            }
            return _userAreaService.Add(userArea);
        }

        [HttpPut("{id}")]
        public UserAreaVM Put(string id, [FromBody]UserAreaVM userArea)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Area id is not set");
            }
            if (!ModelState.IsValid)
            {
                throw new ArgumentException();
            }
            userArea.Id = id;
            return _userAreaService.Update(userArea);
        }

        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Area id is not set");
            }
            _userAreaService.Delete(id);
            return id;
        }
    }
}
