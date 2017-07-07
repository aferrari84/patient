(function () {
    "use strict";
    /**
     * Define the global configuration parameters
     * @returns {{getItem: getItem, $get: $get}}
     * @constructor
     */
    function AccommodationAppGlobalConfiguration() {

        var globals = {
            BASE_URL_CALLBACK: '/auth?',
            BASE_API_URL: '/api',
            DEFAULT_CONTENT_TYPE: 'application/json; charset=utf-8',
            DEFAULT_PAGE_SIZE: 21,
            COOKIE_NAME: 'api_token',
            EMPLOYEE_UPLOAD_IMAGES_SERVICE: '/api/employees/upload-images',
            PROJECT_BILLING_REPORT_EVIDENCE_SERVICE: '/api/project-billing-report/upload-evidence',
            IMPORT_WORKTIMES: '/api/worktimes/ImportWorktime',
            PROJECT_BILLING_REPORT_EVIDENCE_MAX_FILE_UPLOAD: 6,
            MAX_SIZE_FILE_SIZE_KB: 5000,
            UPLOAD_EMPLOYEE_IMAGES_PATH: '/api/Uploads/',
            GENDER: [
                { ID: 1, NAME: 'Male'},
                { ID: 2, NAME: 'Female'}
            ],
            CIVIL_STATUS: [
                {ID: 1, NAME: 'Married'},
                {ID: 2, NAME: 'Divorced'},
                {ID: 3, NAME: 'Widow'},
                {ID: 4, NAME: 'Single'}
            ],
            EMPLOYEE_TYPE: [
                {ID: 1, NAME: 'Staff'},
                {ID: 2, NAME: 'Productive'},
                {ID: 3, NAME: 'Contractor'}
            ],
            DATE_FORMAT: 'MM/dd/yyyy',
            TRACKING_CONCEPTS: {
                'Holidays': 11,
                'Vacations': 12,
                'PTO': 13
            },
            OPERATION_TYPE: [
                {ID: 1, NAME: 'Insert'},
                {ID: 2, NAME: 'Update'},
                {ID: 3, NAME: 'Delete'}
            ],
            VACATIONS_EXPORT_FILE_NAME: '-Vacations.csv',
            EMPLOYEE_EXPORT_FILE_NAME: '-Employees.csv',
            AVAILABILITY_EXPORT_FILE_NAME: '-EmployeesAvailability.csv',
            PROJECT_EXPORT_FILE_NAME: '-Projects.csv',
            EMPLOYEE_ENTRIES_LEAVINGS_EXPORT_FILE_NAME: '-Entries-Leaving-Employees.csv',
            VACATION_PERIOD_COUNTER: 5,
            TICKET_TYPE: {
                'VacationRequest': 1,
                'BillingReport': 2
            },
            REPORT_START_YEAR: 2010
            
        };
        return {
            getItem: function (key) {
                return globals[key];
            },
            $get: function () {
                return globals;
            }
        };
    }

//angular module
    angular.module('AccommodationApp.Config', []).provider('applicationConfiguration', AccommodationAppGlobalConfiguration);
}());