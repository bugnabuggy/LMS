(function(app) {
    var ctrl = function($http,$scope) {
        ctrl = this;
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

        ctrl.refresh = function () {
            $http.get(endpoints.goals.list).then(goodList, badList);
        };

        // add goal
        var goodAdd = function (response) {
            ctrl.refresh();
        }

        var badAdd = function (response) {
            if (typeof testinEnviroment == "undefined") {
                alert('Can`t add goal:' + response.status + " " + response.statusText);
            }
        }
        ctrl.add = function (area, desc) {
            if (typeof desc == "undefined") {
                var element = document.getElementById(area.Id);
                element.focus();
                return;
            }
            $http.post(endpoints.goals.add + area.Id + "/goals/", { Description: desc }).then(goodAdd, badAdd);
        }


        // edit goal
        var goodEdit = function (response) {
            ctrl.refresh();
        }

        var badEdit = function (response) {
            if (typeof testinEnviroment == "undefined") {
                alert('Can`t edit goal:' + response.status + " " + response.statusText);
            }
        }

        ctrl.edit = function(area, goal) {
            if (!goal.editVisible) {
                goal.labelVisible = true;
                goal.editVisible = true;
            } else {
                if (goal.Description.length < 1) {
                    alert('Fill goal description');
                    return;
                }
                $http.put(endpoints.goals.edit + goal.Id, goal).then(goodEdit, badEdit);
            }
        }

        ctrl.key = function (area,goal, key) {
            if (key.keyCode == 13) {
                ctrl.edit(area, goal);
            }
        };

        // del goal
        var goodDel = function (response) {
            ctrl.refresh();
        }

        var badDel = function (response) {
            if (typeof testinEnviroment == "undefined") {
                alert('Can`t delete goal:' + response.status + " " + response.statusText);
            }
        }

        ctrl.del = function (goal) {
                $http.delete(endpoints.goals.del + goal.Id).then(goodDel, badDel);
        }

        ctrl.refresh();
    }

    app.controller('goalsCtrl', ['$http','$scope',ctrl]);

})(angular.module('goalsApp'))