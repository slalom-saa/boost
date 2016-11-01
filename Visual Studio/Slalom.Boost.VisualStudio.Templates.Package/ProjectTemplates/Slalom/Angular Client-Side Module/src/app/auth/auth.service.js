(function (angular) {
    var refreshing = false;
    angular.module('auth').factory('auth.service', [
        '$http', '$q', '$localStorage', '$rootScope', 'config', function ($http, $q, storage, $rootScope, config) {

            var getToken = function () {
                var token = storage['authToken'];
                if (token != null && new Date(token.expires) < new Date()) {
                    token = storage['authToken'] = null;
                }
                return token || {
                    authenticated: false
                };
            };

            var setToken = function (value) {
                if (value != null && value.authenticated !== false) {
                    value.authenticated = true;
                } else {
                    value = {
                        authenticated: false
                    };
                }
                storage['authToken'] = value;
                $rootScope.user = value;
            };

            var parseToken = function (token) {
                return {
                    token: token.access_token,
                    userName: token.userName,
                    roles: JSON.parse(token.roles || '[]'),
                    refreshToken: token.refresh_token,
                    expires: new Date(token['.expires']),
                    firstName: token.firstName,
                    lastName: token.lastName
                };
            };

            var user = {
                authenticated: false,
                userName: ""
            };

            var signIn = function (signInData) {

                if (config.secure === false) {
                    var empty = $q.defer();
                    empty.resolve({
                        data: {

                        }
                    });
                    return empty.promise;
                }

                var deferred = $q.defer();
                var url = config.settings.apiUrl + 'identity/token';
                var content = 'grant_type=password&username=' + signInData.userName + '&password=' + signInData.password + '&client_id=' + config.settings.clientId + '&client_secret=' + config.settings.clientSecret + '';
                $http({
                    method: 'POST',
                    url: url,
                    data: content,
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).success(function (response) {
                    setToken(parseToken(response));
                    deferred.resolve(response);
                }, function () {
                    signOut();
                }).error(function (data) {
                    deferred.reject(data);
                });
                return deferred.promise;
            };

            var signOut = function () {
                setToken(null);
            };

            var refreshToken = function () {
                if (!refreshing) {
                    refreshing = true;
                    var deferred = $q.defer();

                    var authData = getToken();

                    if (authData && authData.refreshToken && Math.round((((new Date(authData.expires) - new Date()) % 86400000) % 3600000) / 60000) < 5) {

                        var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + '&scope=full&client_id=' + config.settings.clientId + '&client_secret=' + config.settings.clientSecret + '';

                        storage['authToken'] = null;

                        $http.post(config.settings.apiUrl + 'identity/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                            setToken(parseToken(response));
                            deferred.resolve(response);

                        }).error(function (err, status) {
                            signOut();
                            deferred.reject(err);
                        });

                        return deferred.promise;
                    }
                    refreshing = false;
                }
                return null;
            };


            function register(data) {
                return $http.post(config.settings.apiUrl + 'identity/users/actions/register', data).then(function (data) {
                    return data.data;
                });
            }





            if (config.secure === false) {
                getToken = function () {
                    return {

                        token: 'none',
                        userName: 'test.user',
                        roles: [],
                        refreshToken: 'none',
                        expires: new Date('1/1/2020'),
                        firstName: 'Test',
                        lastName: 'User'
                    }
                }
            }

            setToken(getToken());

            return {
                signIn: signIn,
                signOut: signOut,
                user: user,
                refreshToken: refreshToken,
                register : register
            };
        }
    ]);
}(angular));