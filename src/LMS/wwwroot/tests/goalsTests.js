describe('Goals tests',function() {
    var $controller;
    var $httpBackend;
    var injector;
    var gSrv;
    var gCtrl;
    var testData = [
        {
            Area: {},
            Goals:[{}, {}]
        },
        {
            Area: {},
            Goals: [{}, {}]
        }
    ];

    beforeEach(module('goalsApp'));

    beforeEach(inject(function ($injector) {
        $controller = $injector.get('$controller');
        $httpBackend = $injector.get('$httpBackend');
        injector = $injector;
        var scope = $injector.get('$rootScope').$new();

        gSrv = injector.get('goalsSrv');
        gCtrl = $controller('goalsCtrl', { areasSrv: gSrv, $scope: scope });
    }));

    it('should work with goals!',function() {
        assert.isOk(gSrv);
        assert.isOk(gCtrl);
    });

    it('should load areas with goals', function() {
        var requestHandler = $httpBackend.when('GET', endpoints.goals.list).respond(testData);

        gSrv.refresh();

        gSrv.goals.length;

    });

})