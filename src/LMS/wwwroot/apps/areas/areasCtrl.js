(function(module) {
    var ctrl = function(areasSrv,scope) {
        var ctrl = this;

        ctrl.areas = areasSrv.areas;

        ctrl.refresh = function() {
            areasSrv.list();
        };

        ctrl.refresh();

        ctrl.addArea = function() {
            var newArea = {
                Color: "",
                Title:""
            }
            newArea.Color = ctrl.generateColor();
            areasSrv.add(newArea);

            //areasSrv.areas.push(newArea);
        }

        ctrl.editArea = function(area) {
            if (!area.editVisible) {
                area.labelVisible = false;
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

    }

    module.controller('areasCtrl',['areasSrv','$scope', ctrl]);

})(angular.module('areasApp'));