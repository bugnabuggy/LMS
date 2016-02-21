describe('Test Areas page', function() {
    var $controller;
    var $httpBackend;
    var injector;
    var arSrv;
    var arCtrl;
    var testData = [
    {
        Id: '',
        Timestamp: '',
        UserId: '',
        Title: '',
        Priority: '',
        Color:''
    },
    {
        Id: '',
        Timestamp: '',
        UserId: '',
        Title: '',
        Priority: '',
        Color: ''
    }];

    var AddAnswer = {
         Id:'12',
         Timestamp: '',
         UserId: '',
         Title: '',
         Priority: '',
         Color: '#FFFFFF'
     };

    beforeEach(module('areasApp'));

    beforeEach(inject(function ($injector) {
        $controller = $injector.get('$controller');
        $httpBackend = $injector.get('$httpBackend');
        injector = $injector;
        var scope = $injector.get('$rootScope').$new();

        arSrv = injector.get('areasSrv');
        arCtrl = $controller('areasCtrl', { areasSrv:arSrv, $scope: scope });
    }));

    it('should work!', function () {
        assert.isOk(endpoints);
        assert.equal(endpoints.areas.list,"/api/areas/list/");
    });

    it('should have areas controller and servcie', function () {
        assert.isOk(arSrv);
        assert.isOk(arCtrl);
    });

    it('should load areas list', function () {
        var requestHandler = $httpBackend.when('GET', endpoints.areas.list).respond(testData);

        arSrv.list();
        $httpBackend.flush();

        assert.equal(arSrv.areas.length, 2);

        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should generate color for area', function () {
        var color = arCtrl.generateColor();

        assert.isOk(color);
    });

    it('should add area, post to server, and then push to array', function () {
        var requestHandler = $httpBackend.when('POST', endpoints.areas.add).respond(AddAnswer);

        var count = arCtrl.areas.length;
        arCtrl.addArea();

        $httpBackend.flush();

        assert.isTrue(arCtrl.areas.length - 1 === count);
        assert.isString(arCtrl.areas[count].Color);

        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should toggle edit sate for area and edit it when toggle back', function () {
        var area = {
            Id: '12',
            Timestamp: '',
            UserId: '',
            Title: '',
            Priority: '',
            Color: '#000000'
        };
        arSrv.areas.push(area);

        arCtrl.editArea(area);

        assert.isTrue(area.editVisible);
        assert.isFalse(area.labelVisible);

        area.Title = "Edited";

        var requestHandler = $httpBackend.when('PUT', endpoints.areas.edit).respond(AddAnswer);

        arCtrl.editArea(area);
        $httpBackend.flush();

        assert.equal(arSrv.areas[0].Color, "#FFFFFF");
        assert.equal(arSrv.areas.length, 1);

        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();

        assert.isFalse(area.editVisible);
        assert.isTrue(area.labelVisible);
    });

    it('should delete area', function() {
        var area = AddAnswer;
        arSrv.areas.push(area);

        var requestHandler = $httpBackend.when('DELETE', endpoints.areas.del).respond(AddAnswer);

        arCtrl.delArea(area);

        $httpBackend.flush();
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();

        assert.equal(arSrv.areas.length, 0);

    });
})