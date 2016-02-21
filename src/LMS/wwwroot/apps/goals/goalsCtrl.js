(function(app) {
    var ctrl = function(gSrv,scope) {
        ctrl = this;
        ctrl.goals = gSrv.goals;

        ctrl.refresh = function() {
            gSrv.list();
        };
        
    }

    app.controller('goalsCtrl', ['goalsSrv','$scope',ctrl]);

})(angular.module('goalsApp'))