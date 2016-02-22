(function(module) {
    var ctrl = function(areasSrv,scope) {
        var ctrl = this;
        
        ctrl.areas = areasSrv.areas;

        ctrl.refresh = function () {
            areasSrv.list();
            ctrl.areas = areasSrv.areas;
        };

        ctrl.addArea = function() {
            var newArea = {
                Color: "",
                Title:""
            }
            newArea.Color = ctrl.generateColor();
            areasSrv.add(newArea);
            ctrl.areas = areasSrv.areas;
            //areasSrv.areas.push(newArea);
        }

        ctrl.editArea = function(area) {
            if (!area.editVisible) {
                area.labelVisible = true;
                area.editVisible = true;
            } else {
                areasSrv.edit(area);

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
        
        window.setTimeout(function (scope) {
            ctrl.areas = areasSrv.areas;
            scope.$apply();
        }, 1000, scope);
    }

    module.controller('areasCtrl',['areasSrv','$scope', ctrl]);

})(angular.module('areasApp'));