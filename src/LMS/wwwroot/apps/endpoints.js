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
        list: "/api/areas/?IncludeGoals=true", //GET
        add:"/api/goals/",  // POST
        edit: "/api/goals/", // PUT
        del: "/api/goals/" //DELETE
    }
}

//export endpoints;