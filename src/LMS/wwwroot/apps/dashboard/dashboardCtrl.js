(function(app) {
    var ctrl = function($http,scope) {
        var ctrl = this;
        ctrl.loadAll = false;

        //hate this way!:
        ctrl.lastArea = null;
        ctrl.lastGoal = null;

        ctrl.goals = new Array();

        // get list
        var goodList = function (response) {
            ctrl.goals = response.data;
        }

        var badList = function (response) {
            if (typeof testinEnviroment == "undefined") {
                alert('Can`t get list:' + response.status + " " + response.statusText);
            }
        }

        ctrl.refresh = function (loadAll) {
            if (!loadAll)
                $http.get(endpoints.dashboard.list).then(goodList, badList);
            else
                $http.get(endpoints.dashboard.listAll).then(goodList, badList);
        };

        ctrl.showAll = function (flag) {
            ctrl.loadAll = !ctrl.loadAll;
            ctrl.refresh(flag);
        }

        // Check

        var goodChk = function (response) {
            ctrl.refresh(ctrl.loadAll);
        }

        var badChk = function (response) {
            if (typeof testEnviroment == "undefined") {
                alert('Can`t check goal: ' + response.status + " " + response.statusText);
            }
        }

        ctrl.check = function (event, area, goal, task) {
            event.target.disabled = true;
            if (task) {
                $http.delete(endpoints.dashboard.del + task.Id).then(goodChk, badChk);
            } else {
                $http.post(endpoints.dashboard.add + goal.Id + "/calendartasks", { TimeSpentMin :60}).then(goodChk, badChk);    
            }
            
        }

        ctrl.refresh(ctrl.loadAll);
    }

    app.controller('dashboardCtrl', ['$http','$rootScope',ctrl]);
})(angular.module('dashboardApp'))