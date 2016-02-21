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
    [Authorize]
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
        public IEnumerable<UserAreaVM> Get()
        {
            return _userAreaService.List(new AreaListOptions { IncludeGoals = true, OnlyLastGoals = true });
        }

        [HttpGet("{id}")]
        public UserAreaVM Get(string id)
        {
            return _userAreaService.Get(id, new AreaListOptions { IncludeGoals = true, OnlyLastGoals = true });
        }

        [HttpPost]
        public UserAreaVM Post([FromBody]UserAreaVM userArea)
        {
            return _userAreaService.Add(userArea);
        }

        [HttpPut("{id}")]
        public UserAreaVM Put(string id, [FromBody]UserAreaVM userArea)
        {
            userArea.Id = id;
            return _userAreaService.Update(userArea);
        }

        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            _userAreaService.Delete(id);
            return id;
        }
    }
}
