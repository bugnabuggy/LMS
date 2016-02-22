(function(app) {
    var srv = function($http,scope) {
        var srv = this;
        srv.areas = new Array();
        

        // get Areas
        var success = function (response) {
            srv.areas = response.data;
        }
        var fail = function (response) {
            if (typeof testEnviroment == "undefined") {
                alert("Status code: " + response.status + "  " + response.statusText);
            }
            
                
        }

        srv.list = function () {
            $http.get(endpoints.areas.list).then(success, fail);
            return srv.areas;
            
        }

        // add Area
        var goodPost = function(response) {
            srv.areas.push(response.data);
        }

        var badPost = function (response) {
            if (typeof testEnviroment == "undefined")
            alert("Can`t add Area: " + response.statusText);
        }

        srv.add = function(area) {
            $http.post(endpoints.areas.add, area).then(goodPost, badPost);
        }

        // edit Area
        var goodPut = function (response) {
            var item = _.findWhere(srv.areas, { Id: response.data.Id });
            var index = _.indexOf(srv.areas, item);
            srv.areas[index] = response.data;
        }

        var badPut = function (response) {
            if (typeof testEnviroment == "undefined")
            alert("Cant update Area: " + response.statusText);
        }

        srv.edit = function (area) {
            $http.put(endpoints.areas.edit+area.Id, area).then(goodPut, badPut);
        }

        // del Area
        var goodDel = function (response) {
            var item = _.findWhere(srv.areas, { Id: response.data.Id });
            var index = _.indexOf(srv.areas, item);
            srv.areas.splice(index,1);
            
        }

        var badDel = function (response) {
            if (typeof testEnviroment == "undefined")
            alert("Cant delete Area: " + response.statusText);
        }

        srv.del = function (area) {
            $http.delete(endpoints.areas.del+area.Id, area).then(goodDel, badDel);
        }

        srv.list();
    }

    app.service('areasSrv',['$http','$rootScope',srv]);

})(angular.module('areasApp'))