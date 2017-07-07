describe('Shared module', function () {
	describe('Check Controller', function () {
		var LeavingPageCtrl, $scope, modalServiceMock;

		beforeEach(module('AccommodationApp'));

		beforeEach(inject(function ($controller, $rootScope) {
			$scope = $rootScope.$new();

			// fake promise
			var modalResult = {
				then: function (callback) {
					callback();
				}
			};

			// mock version of anuglar-ui $modal service
			modalServiceMock = {
				open: function (options) {

				},
				close: function (item) {
					//The user clicked OK on the modal dialog, call the stored confirm callback with the selected item
				},
				dismiss: function (type) {
					//The user clicked cancel on the modal dialog, call the stored cancel callback

				}
			};

			// set up fake methods
			spyOn(modalServiceMock, "open").andReturn({ result: modalResult });
			spyOn(modalServiceMock, "dismiss").andReturn({ result: modalResult });
			spyOn(modalServiceMock, "close").andReturn({ result: modalResult });
			LeavingPageCtrl = $controller('LeavingPageCtrl', {
				$scope: $scope,
				$modalInstance: modalServiceMock
			});
		}));

		

	});
});