(function(module) {
    var ctrl = function(scope,$http) {
        var ctrl = this;

        ctrl.areas = new Array();//areasSrv.areas;

         // get Areas
        var success = function (response) {
            ctrl.areas = response.data;
        }
        var fail = function (response) {
            if (typeof testEnviroment == "undefined") {
                alert("Status code: " + response.status + "  " + response.statusText);
            }
        }

        ctrl.list = function () {
            $http.get(endpoints.areas.list).then(success, fail);
        }

        // add Area
        var goodPost = function(response) {
            ctrl.areas.push(response.data);
        }

        var badPost = function (response) {
            if (typeof testEnviroment == "undefined")
            alert("Can`t add Area: " + response.statusText);
        }

        ctrl.add = function (area) {
            $http.post(endpoints.areas.add, area).then(goodPost, badPost);
        }

        // edit Area
        var goodPut = function (response) {
            var item = _.findWhere(ctrl.areas, { Id: response.data.Id });
            var index = _.indexOf(ctrl.areas, item);
            ctrl.areas[index] = response.data;
        }

        var badPut = function (response) {
            if (typeof testEnviroment == "undefined")
            alert("Cant update Area: " + response.statusText);
        }

        ctrl.edit = function (area) {
            $http.put(endpoints.areas.edit+area.Id, area).then(goodPut, badPut);
        }

        // del Area
        var goodDel = function (response) {
            var item = _.findWhere(ctrl.areas, { Id: response.data.Id });
            var index = _.indexOf(ctrl.areas, item);
            ctrl.areas.splice(index, 1);
            
        }

        var badDel = function (response) {
            if (typeof testEnviroment == "undefined")
            alert("Cant delete Area: " + response.statusText);
        }

        ctrl.del = function (area) {
            $http.delete(endpoints.areas.del+area.Id, area).then(goodDel, badDel);
        }

        ctrl.refresh = function () {
            ctrl.list();
        };

        ctrl.addArea = function() {
            var newArea = {
                Color: "",
                Title:""
            }
            newArea.Color = ctrl.generateColor();
            ctrl.add(newArea);
            ctrl.list();
   
        }

        ctrl.editArea = function(area) {
            if (!area.editVisible) {
                area.labelVisible = true;
                area.editVisible = true;
            } else {
                ctrl.edit(area);

                area.editVisible = false;
                window.setTimeout(function (scope) {
                    area.labelVisible = true;
                    scope.$apply();
                },200,scope);

            }
            
        };

        ctrl.delArea = function(area) {
            areasSrv.del(area);
        };

        ctrl.key = function (area, key) {
            if (key.keyCode == 13) {
                ctrl.editArea(area);
            }
        };

        ctrl.generateColor = function() {
            var letters = '0123456789ABCDEF'.split('');
            var color = '#';
            for (var i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * 16)];
            }

            return color;
        }

        ctrl.list();
       
    }

    module.controller('areasCtrl',['$scope','$http', ctrl]);

})(angular.module('areasApp'));