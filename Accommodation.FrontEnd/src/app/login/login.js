(function () {
    "use strict";
    /**
     * Configuration for Login Module - Define routes
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {
        stateProvider.state('login', {
            url: '/login?message',
            views: {
                "container": {
                    controller: 'LoginCtrl',
                    templateUrl: 'login/login.tpl.html'
                }
            },
            data: { title: 'Login' }
        });
        stateProvider.state('sessionLogin', {
            url: '/auth?access_token&token_type&expires_in&state&redirect_url',
            views: {
                "container": {
                    controller: 'SessionCtrl'
                }
            },
            data: { title: 'Authenticating' }
        });
        stateProvider.state('logout', {
            url: '/logout',
            views: {
                "container": {
                    controller: 'LogoutCtrl',
                    templateUrl: 'login/login.tpl.html'
                }
            },
            data: { title: 'Logout' }
        });
        stateProvider.state('register', {
            url: '/register',
            views: {
                "container": {
                    controller: 'RegisterCtrl',
                    templateUrl: 'login/register.tpl.html'
                }
            },
            data: { title: 'Nuevo Usuario' }
        });
    }

    /**
     * Login Controller handle redirection to log in with Google Accounts
     * @param scope
     * @param state
     * @param stateParams
     * @param window
     * @param Login
     * @param applicationConfiguration
     * @constructor
     */
    function LoginController(scope, rootScope, location, state, stateParams, window, Login, applicationConfiguration) {
        
        var redirectURL = location.url().replace('/login', '/publish').replace('=','|');

        scope.user = "";
        scope.password = "";

        if (stateParams.message) {
            scope.error = stateParams.message;
        }
        if (Login.is_authenticated()) {
            state.go('base.publish', {}, { reload: true });
            return;
        }
        //var URL = applicationConfiguration.BASE_API_URL + '/Account/ExternalLogin?provider=Google&response_type=token&client_id=self&redirect_uri=' + location.protocol() + "://" + location.host() + applicationConfiguration.BASE_URL_CALLBACK + 'redirect_url=' + redirectURL + '&state=rRxNmnfeRVPgsVtnkgHRI8lFRtPO5qIVD0L9vVOckgU1';
        //scope.go_to_google = function () {
        //    window.location.href = URL;
        //};
        scope.authenticate = function () {

            var data = 'grant_type=password&username=' + scope.user + '&password=' + scope.password;

            scope.invalid_grant = false;
            scope.not_activated = false;

            Login.auth(data).then(function (response) {
                Login.store_token(response);
                
                window.location.href = "/publish";
            }, function (error) {
                if (error.error == "not_activated") {
                    scope.not_activated = true;
                    scope.invalid_grant = false;
                }
                if (error.error == "invalid_grant" || error.error == "banned_user") {
                    scope.invalid_grant = true;
                    scope.not_activated = false;
                }
                scope.has_error = true;
            });
        };

        scope.register = function () {

            state.go('register');
        };

        scope.back = function () {

            state.go('base.home');
        };

    }

    /**
     * Logout controller handle the log out action
     * @param state
     * @param Login
     * @constructor
     */
    function LogoutController(state, window, Login) {
        if (Login.is_authenticated()) {
            Login.logout();
            state.go('base.home');
            window.location.reload();
        }
    }

    /**
     * Register controller handle the log out action
     * @param state
     * @param Login
     * @constructor
     */
    function RegisterController(scope, rootScope, state, window, Login) {

        scope.Email = '';

        scope.register_user = function () {



            //var formValidation = form.$valid;
            var emailValidation = scope.Email.split('@').length === 2 && scope.Email.split('@')[1].split('.').length === 2 && scope.Email.split('@')[1].split('.')[1] !== "";
            var passwordEqualValidation = scope.Password === scope.RepeatPassword;
            var passwordLenghtValidation = scope.Password.length > 5;
            var nameValidation = scope.Name.length > 1;
            var lastnameValidation = scope.LastName.length > 1;

            if (emailValidation && passwordEqualValidation && passwordLenghtValidation && nameValidation && lastnameValidation) {

                var data = { UserName: scope.Email, Name: scope.Name, LastName: scope.LastName, Password: scope.Password };

                Login.register(data).then(function (data) {
                    state.go('login');
                }, function (error) {
                    rootScope.$broadcast('notify', { text: "El usuario ya existe o el password no es correcta.", type: 'error-message' });
                });

            } else {
                scope.validation_error = true;

                //angular.forEach(form.$error.required, function (error, key) {
                //    rootScope.$broadcast('notify', { text: error.$name + " es requerido.", type: 'error-message' });
                //});
                if (!emailValidation) {
                    rootScope.$broadcast('notify', { text: "Formato de email incorrecto.", type: 'error-message' });
                }
                if (!passwordEqualValidation) {
                    rootScope.$broadcast('notify', { text: "Los password no son iguales.", type: 'error-message' });
                }
                if (!passwordLenghtValidation) {
                    rootScope.$broadcast('notify', { text: "El password debe seer de al menos 6 caracteres.", type: 'error-message' });
                }
                if (!nameValidation) {
                    rootScope.$broadcast('notify', { text: "Nombre no valido.", type: 'error-message' });
                }
                if (!lastnameValidation) {
                    rootScope.$broadcast('notify', { text: "Apellido no valido.", type: 'error-message' });
                }
            }

        };

        scope.register_cancel = function () {

            state.go('base.home');
        };
    }

    /**
     * Session Controller handle the storage of the token
     * @param state
     * @param location
     * @param Login
     * @constructor
     */
    function SessionController(state, location, Login, stateParams) {

        var redirect_url = stateParams.redirect_url.replace('|', '=');

        if (location.url().indexOf('#access_token') !== -1) {
            var hash_params = location.hash();
            var params = new window.GetQueryString(hash_params);
            if (params.access_token != null) {
                Login.store_token(params);
                window.location.href = redirect_url;
            } else {
                state.go('login');
            }
        } else {
            state.go('login');
        }
    }

//injectors
    Config.$inject = ['$stateProvider'];
    LoginController.$inject = ['$scope', '$rootScope', '$location', '$state', '$stateParams', '$window', 'Login', 'applicationConfiguration', '$window'];
    LogoutController.$inject = ['$state', '$window', 'Login'];
    RegisterController.$inject = ['$scope', '$rootScope', '$state', '$window', 'Login'];
    SessionController.$inject = ['$state', '$location', 'Login', '$stateParams'];

    angular.module('AccommodationApp.Login', ['ui.router', 'ui.bootstrap'])
        .config(Config)
        .controller('LogoutCtrl', LogoutController)
        .controller('RegisterCtrl', RegisterController)
        .controller('LoginCtrl', LoginController)
        .controller('SessionCtrl', SessionController);
}());


