(function(app) {
    var goalsSrv = function($http) {
        var srv = this;
        srv.goals = new Array();

        // get Goals with Areas
        var success = function (response) {
            srv.goals = response.data;
        }
        var fail = function (response) {
            if (!testEnviroment)
                alert("Status code: " + response.status + "  " + response.statusText);
        }

        srv.list = function () {
            $http.get(endpoints.goals.list).then(success, fail);
        }

        // add Area
        var goodPost = function (response) {
            srv.goals.push(response.data);
        }

        var badPost = function (response) {
            if (!testEnviroment)
                alert("Can`t add Area: " + response.statusText);
        }

        srv.add = function (area) {
            $http.post(endpoints.goals.add, area).then(goodPost, badPost);
        }

        // edit Area
        var goodPut = function (response) {
            var item = _.findWhere(srv.goals, { Id: response.data.Id });
            var index = _.indexOf(srv.goals, item);
            srv.goals[index] = response.data;
        }

        var badPut = function (response) {
            if (!testEnviroment)
                alert("Cant update Area: " + response.statusText);
        }

        srv.edit = function (area) {
            $http.put(endpoints.goals.edit, area).then(goodPut, badPut);
        }

        // del Area
        var goodDel = function (response) {
            var item = _.findWhere(srv.goals, { Id: response.data.Id });
            var index = _.indexOf(srv.goals, item);
            srv.goals.splice(index, 1);

        }

        var badDel = function (response) {
            if (!testEnviroment)
                alert("Cant delete Area: " + response.statusText);
        }

        srv.del = function (area) {
            $http.delete(endpoints.goals.del, area).then(goodDel, badDel);
        }
    }

    app.service('goalsSrv', ['$http', goalsSrv]);

})(angular.module('goalsApp'));