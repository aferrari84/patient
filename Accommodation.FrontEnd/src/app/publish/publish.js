(function () {
    "use strict";
    /**
     * Config the routes for Home Module
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {

        stateProvider.state('base.publish', {
            url: '/publish',
            views: {
                "main": {
                    controller: 'PublishCtrl',
                    templateUrl: 'publish/publish.tpl.html'
                }
            },
            data: {
                title: 'Publicar Aviso Gratis'
            }
        });

        stateProvider.state('base.publish_new', {
            url: '/publish/new/{type}',
            views: {
                'main': {
                    controller: 'PublishFormCtrl',
                    templateUrl: 'publish/publish_form.tpl.html'
                }
            },
            data: {
                title: 'Nueva Publicacion'
            }

        });
        stateProvider.state('base.publish_update', {
            url: '/publish/edit/{id}/:recordType',
            views: {
                'main': {
                    controller: 'PublishFormCtrl',
                    templateUrl: 'publish/publish_form.tpl.html'
                }
            },
            data: {
                title: 'Editar Publicacion'
            }

        });

    }

    /**
     * Home Controller for main view
     * @param rootScope
     * @constructor
     */
    function PublishController(scope, rootScope, state, Location, Lodging, OfferedService) {

        scope.lodgings = {};
        scope.services = {};

        Lodging.get_by_user().then(function (data) {
            scope.lodgings = data.ResponseData;
        }, function (error) {
            rootScope.$broadcast('notify', { text: "Error.", type: 'error-message' });
        });

        OfferedService.get_by_user().then(function (data) {
            scope.services = data.ResponseData;
        }, function (error) {
            rootScope.$broadcast('notify', { text: "Error.", type: 'error-message' });
        });

        var locationId = state.current.data.locationId;

        scope.publish = function (type) {

            var result = { type: type };
            state.go('base.publish_new', result);

        };

        scope.edit = function (id, recordType) {

            var result = { id: id, recordType: recordType };
            state.go('base.publish_update', result);

        };

    }


    /**
     * Office Form Controller handles the Add / Update Office
     * @param scope
     * @param rootScope
     * @param Office
     * @param stateParams
     * @param location
     * @constructor
     */
    function PublishFormController(scope, rootScope, stateParams, state, flowFactory, Location, Lodging, OfferedService, Tourism, HashTag) {
        //rootScope.state_name = 'management';
        scope.publish = {};
        scope.validation_error = false;
        scope.locations = [];
        scope.photos = {};
        scope.photosHasChanged = false;
        scope.typeNewForm = stateParams.type !== undefined;
        scope.id = stateParams.id;
        scope.hashtagValid = undefined;
        scope.hashtagList = {};

        HashTag.get_all().then(function (data) {
            scope.hashtagList = data.ResponseData;
        }, function (error) {
            rootScope.$broadcast('notify', { text: "Error.", type: 'error-message' });
        });


        if (stateParams.type !== undefined)
        {
            scope.publish.PublishType = stateParams.type;
        } else {
            if(stateParams.recordType === 'Lodging')
            {
                Lodging.get_by_id(stateParams.id).then(function (data) {
                    scope.publish = { 
                        Name: data.ResponseData.Name,
                        Body: data.ResponseData.Description,
                        State: { ID: data.ResponseData.Location.State.ID },
                        Lodging: { Capacity: data.ResponseData.Capacity, Type: {ID: data.ResponseData.AccommodationType.ID}},
                        Phone: data.ResponseData.Phone,
                        Location: { ID: data.ResponseData.Location.ID},
                        Email: data.ResponseData.Email,
                        WebPage: data.ResponseData.WebPage,
                        Type: 'Alojamiento',
                        PublishType: data.ResponseData.PublishType.ID.toString()
                    };
                    scope.photos = data.ResponseData.Photos;
                    scope.populateLocation();
                });
            } else {
                OfferedService.get_by_id(stateParams.id).then(function (data) {
                    scope.publish = {
                        Name: data.ResponseData.Name,
                        Body: data.ResponseData.Description,
                        State: { ID: data.ResponseData.Location.State.ID },
                        Service: { Type: { ID: data.ResponseData.ServiceType.ID } },
                        Phone: data.ResponseData.Phone,
                        Location: { ID: data.ResponseData.Location.ID },
                        Email: data.ResponseData.Email,
                        WebPage: data.ResponseData.WebPage,
                        Type: 'Servicio',
                        PublishType: data.ResponseData.PublishType.ID.toString()
                    };
                    scope.photos = data.ResponseData.Photos;
                    scope.populateLocation();
                });
            }
        }

        scope.publish.Body = '<p><b style="color: rgb(118, 118, 118);">Costo:</b><br></p>' +
'<p></p><ul><li><span style="color: rgb(118, 118, 118);float: none;background-color: rgb(255, 255, 255);">Temporada alta &nbsp;$0, consultar promociones.</span></li><li><span style="color: rgb(118, 118, 118);">Temporada baja $0, consultar promociones.</span><br></li></ul><div></div>' +
'<div><font color="#767676"><br></font></div>' +
'<div><font color="#767676"><b>Detalle:</b><span class="Apple-converted-space">&nbsp;</span>C<span style="color: rgb(118, 118, 118);float: none;background-color: rgb(255, 255, 255);">uentan con cocina completa, heladera, microondas<span style="color: rgb(118, 118, 118);float: none;background-color: rgb(255, 255, 255);">, calefaccion con hogar, parrilla individual y cochera cubierta, piscina con amplio solarium, juegos para chicos, parque, hamaca paraguaya.</span></span></font></div>' +
'<div><span class="Label" style="color: rgb(118, 118, 118);"><br></span></div>' +
'<div><span class="Label" style="color: rgb(118, 118, 118);"><b>Servicios:</b>&nbsp;El alojamiento cuenta con d</span><span style="color: rgb(118, 118, 118);">esayuno incluido, servicio de lavanderia, TV 29" c/ cable. Consultar por otros servicios disponibles.</span><p style="color: rgb(118, 118, 118);background-color: rgb(255, 255, 255);"><span class="Label"><br></span></p><p style="color: rgb(118, 118, 118);background-color: rgb(255, 255, 255);"><span class="Label"><b>Direccion:</b></span>&nbsp;El alojamiento se encuentra ubicado en la localidad de N a 5 minutos de N, de facil acceso a locales comerciales y lugares turisticos.</p></div><!--EndFragment-->' +
'<p><br></p>';

        // VALIDATORS
        scope.errorValidate = function (nameInput) {
            if (nameInput !== undefined) {
                if (nameInput.$invalid && !nameInput.$pristine) {
                    return true;
                } else {
                    return scope.validation_error && nameInput.$invalid;

                }
            }
        };


        scope.successValidate = function (nameInput) {
            if (angular.isDefined(nameInput)) {
                return nameInput.$valid && !nameInput.$pristine;
            }
            else {
                return false;
            }
        };


        scope.populateLocation = function () {
            Location.get_by_state(scope.publish.State.ID).then(function (data) {
                scope.locations = data.ResponseData;
            }, function (error) {
                console.info('Error', error);
                scope.alert = { type: 'danger', msg: 'No se pudo encontrar localidades.' };
            });
        };

        scope.existingFlowObject = flowFactory.create({
            target: 'api/upload'
        });

        // Save
        scope.savePublish = function (form) {

            var formValidation = form.$valid;
            var bodyValidation = scope.publish.Body !== '' && scope.publish.Body !== undefined;
            var photoValidationUpdate = (scope.existingFlowObject.files.length > 0 || (scope.typeNewForm === false && scope.photosHasChanged === false));
            var photoValidationNew = (scope.existingFlowObject.files.length > 0 && scope.typeNewForm);
            var hashtagValidation = scope.hashtagValid !== false;
            var photoValidation = photoValidationUpdate || photoValidationNew;

            if (formValidation && bodyValidation && photoValidation && hashtagValidation) {
                scope.save();
            } else {
                scope.validation_error = true;
                
                angular.forEach(form.$error.required, function (error, key) {
                    rootScope.$broadcast('notify', { text: error.$name + " es requerido.", type: 'error-message' });
                });
                if (scope.publish.Body === '' || scope.publish.Body === undefined) {
                    rootScope.$broadcast('notify', { text: "Informacion es requerido.", type: 'error-message' });
                }
                if (photoValidation === false) {
                    rootScope.$broadcast('notify', { text: "Debe subir al menos una foto.", type: 'error-message' });
                }
                if (!scope.hashtagValid) {
                    rootScope.$broadcast('notify', { text: "El nombre ya es utilizado por otra publicacion.", type: 'error-message' });
                }
            }
        };

        scope.save = function () {

            
            if (stateParams.id === undefined) {
                if (scope.publish.Type === 'Alojamiento')
                {
                    var newLodging = {
                        Name: scope.publish.Name,
                        Description: scope.publish.Body,
                        Capacity: scope.publish.Lodging.Capacity,
                        Phone: scope.publish.Phone,
                        Location: { ID: scope.publish.Location.ID },
                        AccommodationType: { ID: scope.publish.Lodging.Type.ID },
                        Email: scope.publish.Email,
                        WebPage: scope.publish.WebPage,
                        PublishType: { ID: scope.publish.PublishType }
                    };


                    Lodging.register(newLodging).then(function () {
                        scope.photoUpload("true");
                        rootScope.$broadcast('notify', { text: "La publicacion se guardo correctamente.", type: 'info-message' });
                        state.go('base.publish');
                    }, function (error) {
                        console.info('Error', error);
                        scope.alert = { type: 'danger', msg: 'No se pudo guardar la publicacion.' };
                    });
                }

                if (scope.publish.Type === 'Servicio')
                {
                    var newService = {
                        Name: scope.publish.Name,
                        Description: scope.publish.Body,
                        Phone: scope.publish.Phone,
                        Location: { ID: scope.publish.Location.ID },
                        ServiceType: { ID: scope.publish.Service.Type.ID },
                        Email: scope.publish.Email,
                        WebPage: scope.publish.WebPage,
                        PublishType: { ID: scope.publish.PublishType }
                    };

                    OfferedService.register(newService).then(function () {
                        scope.photoUpload("true");
                        rootScope.$broadcast('notify', { text: "La publicacion se guardo correctamente.", type: 'info-message' });
                        state.go('base.publish');
                    }, function (error) {
                        console.info('Error', error);
                        scope.alert = { type: 'danger', msg: 'No se pudo guardar la publicacion.' };
                    });
                }

                
            } else {
                if (scope.publish.Type === 'Alojamiento') {
                    var updateLodging = {
                        ID: scope.id,
                        Name: scope.publish.Name,
                        Description: scope.publish.Body,
                        Capacity: scope.publish.Lodging.Capacity,
                        Phone: scope.publish.Phone,
                        Location: { ID: scope.publish.Location.ID },
                        AccommodationType: { ID: scope.publish.Lodging.Type.ID },
                        Email: scope.publish.Email,
                        WebPage: scope.publish.WebPage,
                        PhotoHasChanged: scope.photosHasChanged,
                        PublishType: { ID: scope.publish.PublishType }
                    };

                    Lodging.update(updateLodging, stateParams.id).then(function () {
                        scope.photoUpload("false");
                        rootScope.$broadcast('notify', { text: "La publicacion se guardo correctamente.", type: 'info-message' });
                        state.go('base.publish');
                    }, function (error) {
                        console.info('Error', error);
                        scope.alert = { type: 'danger', msg: 'No se pudo guardar la publicacion.' };
                    });
                }

                if (scope.publish.Type === 'Servicio') {
                    var updateService = {
                        ID: scope.id,
                        Name: scope.publish.Name,
                        Description: scope.publish.Body,
                        Phone: scope.publish.Phone,
                        Location: { ID: scope.publish.Location.ID },
                        ServiceType: { ID: scope.publish.Service.Type.ID },
                        Email: scope.publish.Email,
                        WebPage: scope.publish.WebPage,
                        PhotoHasChanged: scope.photosHasChanged,
                        PublishType: { ID: scope.publish.PublishType }
                    };

                    OfferedService.update(updateService, stateParams.id).then(function () {
                        scope.photoUpload("false");
                        rootScope.$broadcast('notify', { text: "La publicacion se guardo correctamente.", type: 'info-message' });
                        state.go('base.publish');
                    }, function (error) {
                        console.info('Error', error);
                        scope.alert = { type: 'danger', msg: 'No se pudo guardar la publicacion.' };
                    });
                }
            }
        };

        scope.photoUpload = function (isNew) {

            var hashtag = scope.publish.Name.split(' ').join('').toLowerCase();

            if (isNew == "true")
            {
                scope.photosHasChanged = true;
            }
            if (scope.photosHasChanged) {

                var index = 1;

                angular.forEach(scope.existingFlowObject.files, function (file, key) {
                    
                    var extension = file.name.split('.')[file.name.split('.').length - 1];
                    file.name = hashtag + index.toString() + "." + extension;

                    index = index + 1;
                });

                scope.existingFlowObject.opts.target = "api/upload?hashtag=" + hashtag + "&isNew=" + isNew;
                scope.existingFlowObject.upload();
            }
        };

        scope.validateHashtag = function () {
            var isValid = true;
            var hashtag = scope.publish.Name.split(' ').join('').toLowerCase();

            angular.forEach(scope.hashtagList, function (value, key) {
                if(value.HashTag === hashtag && value.ID.toString() !== scope.id)
                {
                    isValid = false;
                }
            });

            scope.hashtagValid = isValid;
        };


        scope.cancel = function () {
            state.go('base.publish');
        };

        scope.cancelChangePhotos = function () {
            scope.photosHasChanged = false;
        };

        scope.changePhotos = function () {
            scope.photosHasChanged = true;
        };

        scope.getTimeStamp = function () {
            var stamp = new Date().getTime().toString();
            return stamp;
        };
    }



    //injectors
    Config.$inject = ['$stateProvider'];
    PublishController.$inject = ['$scope', '$rootScope', '$state', 'Location', 'Lodging', 'OfferedService'];
    PublishFormController.$inject = ['$scope', '$rootScope', '$stateParams', '$state', 'flowFactory', 'Location', 'Lodging', 'OfferedService', 'Tourism', 'HashTag'];

    angular.module('AccommodationApp.Publish', ['ui.router', 'AccommodationApp.Base'])
        .config(Config)
        .controller('PublishCtrl', PublishController)
        .controller('PublishFormCtrl', PublishFormController);
}());
