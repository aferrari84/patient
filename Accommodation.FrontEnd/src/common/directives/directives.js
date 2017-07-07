(function () {
    "use strict";

    function Navigation() {
        return {
            restrict: 'A',
            link: function (scope, ele, attrs, ctrl) {
                $(document).ready(function () {
                    $('#nav-accordion').dcAccordion({
                        eventType: 'click',
                        autoClose: true,
                        saveState: true,
                        disableLink: false,
                        speed: 'fast',
                        showCount: false,
                        autoExpand: true,
                        classExpand: 'dcjq-current-parent'
                    });

                    //$(".leftside-navigation .sub-menu > a").click(function () {
                    //    var o = ($(this).offset());
                    //    var diff = 80 - o.top;
                    //    if (diff > 0) {
                    //        $(".leftside-navigation").scrollTo("-=" + Math.abs(diff), 100);
                    //    } else {
                    //        $(".leftside-navigation").scrollTo("+=" + Math.abs(diff), 100);
                    //    }
                    //});

                    $('.sidebar-toggle-box .fa-bars').unbind("click");
                    $('.sidebar-toggle-box .fa-bars').click(function (e) {

                        $(".leftside-navigation").niceScroll({
                            cursorcolor: "#1FB5AD",
                            cursorborder: "0px solid #fff",
                            cursorborderradius: "0px",
                            cursorwidth: "3px"
                        });

                        $('#sidebar').toggleClass('hide-left-bar');
                        if ($('#sidebar').hasClass('hide-left-bar')) {
                            $(".leftside-navigation").getNiceScroll().hide();
                        }
                        $(".leftside-navigation").getNiceScroll().show();
                        $('#main-content').toggleClass('merge-left');
                        e.stopPropagation();
                        if ($('#container').hasClass('open-right-panel')) {
                            $('#container').removeClass('open-right-panel');
                        }
                        if ($('.right-sidebar').hasClass('open-right-bar')) {
                            $('.right-sidebar').removeClass('open-right-bar');
                        }

                        if ($('.header').hasClass('merge-header')) {
                            $('.header').removeClass('merge-header');
                        }


                    });
                });
            }
        };
    }

    function CaptureHttpErrors($rootScope) {
        return {
            link: function (scope, element, attrs, ctrl) {
                $rootScope.$on('error_request', function (params) {
                    $.gritter.add({
                        // (string | mandatory) the heading of the notification
                        title: 'An Error Occurred',
                        // (string | mandatory) the text inside the notification
                        text: params.Exception,
                        sticky: false,
                        // (int | optional) the time you want it to be alive for before fading out
                        time: 5000
                    });
                });
            }
        };
    }

    function SetLoginViewClass() {
        return {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                $('body').addClass('lock-screen');
            }
        };
    }

    function ShowMessage(rootScope) {
        return {
            restrict: 'A',
            link: function (scope) {
                rootScope.$on('notify', function (event, params) {
                    $.gritter.add({
                        title: "Mensaje",
                        text: params.text,
                        class_name: params.type,
                        sticky: false,
                        time: 5000
                    });
                });
            }
        };
    }

    function HasPermission(rootScope) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var value = attrs.hasPermission.trim();
                var invertLogic = value[0] === '!';
                var ele = $(element);

                ele.hide();

                if (invertLogic) {
                    value = value.substring(1, value.length);
                }

                try {
                    var getPermission = function (currentUser, perm) {
                        var permissions = currentUser.ResponseData.Employee.Permissions;
                        var elementPermission = perm.split('-')[0];
                        var elementModule = perm.split('-')[1];
                        var allowElement = false;
                        for (var i = 0; i < permissions.length; i++) {
                            if (permissions[i].ModuleName.toLowerCase() === elementModule.toLowerCase()) {
                                switch (elementPermission.toLowerCase()) {
                                    case "create":
                                        allowElement = permissions[i].CanCreate;
                                        break;
                                    case "edit":
                                        allowElement = permissions[i].CanEdit;
                                        break;
                                    case "delete":
                                        allowElement = permissions[i].CanDelete;
                                        break;
                                    case "view":
                                        allowElement = permissions[i].CanView;
                                        break;
                                }
                            }
                        }

                        if (invertLogic) {
                            allowElement = !allowElement;
                        }

                        return allowElement;
                    };


                    rootScope.$watch('global_user', function (user) {
                        if (user !== undefined) {
                            if (getPermission(user, value)) {
                                ele.show();
                            }
                        }
                    });
                    
                }
                catch (err) {
                    $(element).hide();
                    rootScope.$broadcast('notify', { text: "There was an error checking permissions.", type: 'error-message' });
                }


            }
        };

    }

    function ShowTab($rootScope) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.showTab = function (tabName) {
                    var tab = $("#" + tabName + " .ng-binding")[0];
                    if (tab !== undefined) {
                        tab.click();
                    }
                };
            }
        };
    }

    function OnlyNumbers() {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                if (!ngModelCtrl) {
                    return;
                }

                ngModelCtrl.$parsers.push(function (val) {
                    if (val !== undefined) {
                        var clean = val.replace(/[^0-9\s]+/g, '');
                        if (val !== clean) {
                            ngModelCtrl.$setViewValue(clean);
                            ngModelCtrl.$render();
                        }
                        return clean;
                    }
                });

                element.bind('keypress', function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                    }
                });
            }
        };
    }
       
    function setFocus($timeout) {
        return {
            link: function (scope, element, attrs) {
                $timeout(function () {
                    element[0].focus();
                }, 750);
            }
        };
    }

    function ResizeWindows($window) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                var w = $(angular.element($window));
                scope.getWindowDimensions = function () {
                    return {
                        'h': w.height(),
                        'w': w.width()
                    };
                };
                scope.$watch(scope.getWindowDimensions, function (newValue, oldValue) {
                    scope.windowHeight = newValue.h;
                    scope.windowWidth = newValue.w;

                    scope.style = function () {
                        return {
                            'height': (newValue.h - 100) + 'px',
                            'width': (newValue.w - 100) + 'px'
                        };
                    };

                }, true);

                w.bind('resize', function () {
                    scope.$apply();
                    if (scope.getWindowDimensions().w < 767) {
                        $('#sidebar').addClass('hide-left-bar');
                    }
                    else {
                        $('#sidebar').removeClass('hide-left-bar');
                    }
                });
            }
        };
    }
    
    function DatepickerLocaldate($parse) {
        return {
            restrict: 'A',
            require: ['ngModel'],
            link: function link(scope, element, attr, ngModel) {
                var converted = false;
                scope.$watch(
                    function () {
                        return ngModel[0].$modelValue;
                    },
                    function (modelValue) {
                        if (!converted && modelValue) {
                            converted = true;

                            var dt = new Date(modelValue);
                            if (dt.getTimezoneOffset() > 0) {
                                dt.setMinutes(dt.getMinutes() + dt.getTimezoneOffset());
                            }

                            ngModel[0].$modelValue = dt;
                            ngModel[0].$render();

                        }
                    });
            }
        };
    }

    function LoadHtml($compile) {
        return {
            restrict: 'A',
            template: '',
            scope: {
                htmlToBind: '=',
                variables: '='
            },
            link: function (scope, element, attrs) {
                var content = angular.element(scope.htmlToBind);
                var compiled = $compile(content);
                element.append(content);
                compiled(scope);
            }
        };
    }

    /**
    * Directive to load spinner
    * @param $rootScope
    * @returns {{restrict: string, link: link}}
    * @constructor
    */
    function LoadingSpinner($rootScope) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                $rootScope.$on('start_request', function () {
                    $(element).removeClass('ng-hide');
                });
                $rootScope.$on('end_request', function () {
                    $(element).addClass('ng-hide');
                });
            }
        };
    }

    function MapArea($state) {
        return {
            restrict: 'A',
            template: '',
            link: function (scope, element, attrs) {
                var options =
                {
                    onClick: function (e) {
                        $('.mapster_tooltip').remove();
                        $state.go('base.location', { id: e.key });
                        return true;
                    },
                    mapKey: 'data-key',
                    showToolTip: true,
                    toolTipClose: ["area-mouseout"],
                    fillColor: '026aa7',
                    fillOpacity: 0.3,
                    areas: [
                        { key: "1", toolTip: $('<div>C&#243;rdoba</div>') },
                        { key: "2", toolTip: $('<div>Buenos Aires</div>') },
                        { key: "20", toolTip: $('<div>Santa Fe</div>') },
                        { key: "18", toolTip: $('<div>San Luis</div>') },
                        { key: "22", toolTip: $('<div>Tierra del Fuego</div>') },
                        { key: "19", toolTip: $('<div>Santa Cruz</div>') },
                        { key: "5", toolTip: $('<div>Chubut</div>') },
                        { key: "15", toolTip: $('<div>Rio Negro</div>') },
                        { key: "4", toolTip: $('<div>Chaco</div>') },
                        { key: "18", toolTip: $('<div>San Luis</div>') },
                        { key: "14", toolTip: $('<div>Neuquen</div>') },
                        { key: "13", toolTip: $('<div>Misiones</div>') },
                        { key: "6", toolTip: $('<div>Corrientes</div>') },
                        { key: "7", toolTip: $('<div>Entre Rios</div>') },
                        { key: "8", toolTip: $('<div>Formosa</div>') },
                        { key: "12", toolTip: $('<div>Mendoza</div>') },
                        { key: "23", toolTip: $('<div>tucuman</div>') },
                        { key: "9", toolTip: $('<div>Jujuy</div>') },
                        { key: "16", toolTip: $('<div>Salta</div>') },
                        { key: "21", toolTip: $('<div>Santiago del Estero</div>') },
                        { key: "3", toolTip: $('<div>Catamarca</div>') },
                        { key: "11", toolTip: $('<div>La Rioja</div>') },
                         { key: "17", toolTip: $('<div>San Juan</div>') },
                        { key: "10", toolTip: $('<div>La Pampa</div>') }]
                };

                
                var map = $('#argentine_map');


                map.mapster(options)
                .mapster('set', false, '1,2,10');
            }
        };
    }

    // injectors
    CaptureHttpErrors.$inject = ['$rootScope'];
    LoadingSpinner.$inject = ['$rootScope'];
    ShowMessage.$inject = ['$rootScope'];
    HasPermission.$inject = ['$rootScope'];
    ResizeWindows.$inject = ['$window'];
    DatepickerLocaldate.$inject = ['$parse'];
    LoadHtml.$inject = ['$compile'];
    MapArea.$inject = ['$state'];
    //ng module
    angular.module('AccommodationApp.Directives', [])
        .directive('navigation', Navigation)
        .directive('captureHttpErrors', CaptureHttpErrors)
        .directive('loginView', SetLoginViewClass)
        .directive('showMessage', ShowMessage)
        .directive('hasPermission', HasPermission)
        .directive('showtab', ShowTab)
        .directive('onlyNumbers', OnlyNumbers)
        .directive('loadHtml', LoadHtml)
        .directive('mapArea', MapArea)
        .directive('loadingSpinner', LoadingSpinner)
        .directive('setFocus', setFocus)
        .directive('datepickerLocaldate', DatepickerLocaldate)
        .directive('resize', ResizeWindows);
}());