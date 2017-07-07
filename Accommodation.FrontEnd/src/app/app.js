(function () {
    "use strict";
    /**
     * Configuration function for the AccommodationApp app
     * @param httpProvider
     * @param locationProvider
     * @param urlRouterProvider
     * @param applicationConfigurationProvider
     * @constructor
     */
    function Config(stateProvider, httpProvider, locationProvider, urlRouterProvider, applicationConfigurationProvider) {
        locationProvider.html5Mode(true).hashPrefix('!');
        httpProvider.defaults.headers.common["Content-Type"] = applicationConfigurationProvider.getItem('DEFAULT_CONTENT_TYPE');
        httpProvider.defaults.useXDomain = true;
        httpProvider.interceptors.push('$responseObserver');
        delete httpProvider.defaults.headers.common['X-Requested-With'];
        urlRouterProvider.when('/', '/home');
        urlRouterProvider.when('', '/home');
        //urlRouterProvider.otherwise('/error_404');
        
    }



    /**
     * Main Controller for the AccommodationApp App
     * @param scope
     * @param state
     * @param rootScope
     * @param stateParams
     * @param Login
     * @param User
     * @constructor
     */
    function AppCtrl(scope, state, rootScope, stateParams, Login, User,modal) {
        scope.name = 'AccommodationApp';

        scope.$on('$stateChangeStart', function (event, toState) {            
            if (state.current.data && state.current.data.isDirty) {
                event.preventDefault();
                var modalInstance = modal.open({
                    templateUrl: 'shared/leaving_page_modal.tpl.html',
                    backdrop: 'static',
                    keyboard: false,                    
                    controller: "LeavingPageCtrl"
                });
                modalInstance.result.then(function (data) {
                    if (data) {
                        state.current.data.isDirty = false;
                        state.go(toState.name,toState.params);
                    }
                        
                });
                
            }

            rootScope.state_name = toState.name.replace('base.', '');
            if (angular.isDefined(toState.data.title)) {
                scope.pageTitle = toState.data.title;
            }
            
        });

        scope.$on('$stateChangeSuccess', function (event, toState) {
            if (rootScope.state_name === "publish") {
                rootScope.is_logged_in = Login.is_authenticated();
                if (!rootScope.is_logged_in) {
                    event.preventDefault();
                    state.go('login', stateParams);
                }
            }
            if (Login.is_authenticated() && rootScope.global_user === undefined) {
                User.me().then(function (response) {
                    rootScope.global_user = response;
                });
            }
            if (angular.isDefined(toState.data.title)) {
                scope.pageTitle = toState.data.title;
            }
        });

        scope.$on('$stateChangeError', function (event) {
            state.go('base.static_404');
        });

        rootScope.$watch('global_user', function (value) {
            scope.logged_user = value;
        });

        rootScope.$watch('page_title', function (value) {
            scope.page_title = value;
        });
    }

    //Injections
    Config.$inject = ['$stateProvider', '$httpProvider', '$locationProvider', '$urlRouterProvider', 'applicationConfigurationProvider'];
    AppCtrl.$inject = ['$scope', '$state', '$rootScope', '$stateParams', 'Login', 'User','$modal'];

    //angular module
    angular.module('AccommodationApp', [
        'templates-app',
        'templates-common',
        'ui.router',
        'LocalStorageModule',
        'AccommodationApp.Config',
        'AccommodationApp.Helpers',
        'AccommodationApp.Directives',
        'AccommodationApp.Services',
        'AccommodationApp.Home',
        'AccommodationApp.Publish',
        'AccommodationApp.Accommodation',
        'AccommodationApp.OfferedService',
        'AccommodationApp.Tourism',
        'AccommodationApp.Location',
        'AccommodationApp.Login',
        'AccommodationApp.Shared',
        'jkuri.gallery',
        'flow',
        'textAngular'
    ])
        .config(Config)
        .controller('AppCtrl', AppCtrl);
}());

