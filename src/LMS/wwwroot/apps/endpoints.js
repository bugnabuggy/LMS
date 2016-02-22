endpoints = {
    areas: {
        page:"/Areas/",
        list:"/api/areas/", // GET
        add:"/api/areas/", // POST
        edit:"/api/areas/", // PUT
        del:"/api/areas/" //DELETE 
    },
    goals: {
        page:"/Planning",
        list: "/api/areas/?IncludeGoals=true&onlyLastGoals=false", //GET
        add: "/api/areas/",  // POST
        edit: "/api/goals/", // PUT
        del: "/api/goals/" //DELETE
    },
    dashboard: {
        page: "/Dashboard",
        list: "/api/areas/?IncludeGoals=true", //GET
        listAll: "/api/areas/?IncludeGoals=true&onlyLastGoals=false", //GET
        add: "/api/goals/",  // POST
        edit: "/api/goals/", // PUT
        del: "/api/calendartasks/" //DELETE
    }
}

//export endpoints;