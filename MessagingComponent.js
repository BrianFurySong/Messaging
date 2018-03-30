(function () {
    "use strict";
    angular.module(AppName).component("chat", {
        bindings: {},
        templateUrl: "/scripts/components/views/chat.html",
        controller: function (requestService, $scope, $window, $filter, $uibModal) {
            var vm = this;
            vm.$onInit = _init;
            vm.$scope = $scope;
            vm.$scope.chat = $.connection.notificationHub;
            vm.chats = [];
            vm.chatId = [], vm.messageCount = [];
            vm.membersInTeamForPics = [];
            vm.finalMembersPics = [];
            vm.chatThread = [];
            vm.chatUsers = [];
            vm.chatMessage = {};
            vm.chatIndex = true;
            vm.openChat = _openChat;
            vm.sendMessage = _sendMessage;
            vm.launchChatTeamModal = _launchChatTeamModal;
            vm.back = _back;
            vm.leaveChat = _leaveChat;
            vm.addMemberModal = _addMemberModal;
            vm.teamId = {};

            function _init() {
                $.connection.hub.stop();
                $.connection.hub.start();
                requestService.ApiRequestService("GET", "/api/Chats/user").then(function (response) {
                    vm.chats = response.items;
                    for (var i = 0; i < vm.chats.length; i++) {
                        vm.chatId.push(vm.chats[i].id);
                        (function () {
                            return requestService.ApiRequestService("get", "/api/Chats/AllMembersInChat/" + vm.chatId[i])
                                .then(function (response) {
                                    vm.membersInTeamForPics = response.items;
                                    var chatObj = { chatId: vm.membersInTeamForPics[0].chatId, chatName: vm.membersInTeamForPics[0].chatName, urlArray: []};
                                    for (var x = 0; x < vm.membersInTeamForPics.length; x++) {
                                        chatObj.urlArray.push(vm.membersInTeamForPics[x].imageURL);
                                    }
                                    vm.finalMembersPics.push(chatObj);
                                })
                                .catch(function (err) {
                                    console.log(err);
                                })
                        })(vm.membersInTeamForPics, vm.messageCount);
                    }
                }).catch(function (err) {
                    console.log(err);
                });
            }

            vm.$scope.chat.client.getChat = function () {
                requestService.ApiRequestService("GET", "/api/ChatMessages/Chat/" + parseInt(vm.chatMessage.chatId, 10)).then(function (response) {
                    for (var i = 0; i < response.items.length; i++) {
                        response.items[i].createdDate = moment(response.items[i].createdDate).format('MMMM Do YYYY, h:mm:ss a').toString();
                    }
                    vm.chatThread = response.items;
                    vm.chatIndex = false;
                }).catch(function (err) {
                    console.log(err);
                });
            }

            function _launchChatTeamModal() {
                var modalInstance = $uibModal.open({
                    animation: vm.animationsEnabled,
                    component: 'chatTeamModal',
                    size: 'md',
                    resolve: {
                    }
                });

                modalInstance.result.then(function (close) {
                    vm.valueFromModal = close;
                    vm.finalMembersPics = [];
                    vm.chats = [];
                    vm.membersInTeamForPics = [];
                    vm.chatUsers = [];
                    vm.chatId = [];
                    _init();

                }, function () {
                    vm.finalMembersPics = [];
                    vm.chats = [];
                    vm.membersInTeamForPics = [];
                    vm.chatUsers = [];
                    vm.chatId = [];
                    _init();
                });
            }

            function _openChat(chatId) {
                vm.chatMessage.chatId = chatId;
                vm.chatMessage.isOnChat = true;
                requestService.ApiRequestService("PUT", "/api/ChatUserProfiles/" + chatId, vm.chatMessage).then(function (response) {
                    requestService.ApiRequestService("PUT", "/api/ChatUserProfiles/Reset/" + chatId, chatId).then(function (res) {
                    }).catch(function (err) {

                    })
                }).catch(function (err) {
                    console.log(err);
                })
                vm.$scope.chat.client.getChat();
                _getUsersInChat()
            }

            function _sendMessage() {
                vm.chatMessage.createdDate = moment().format();
                vm.$scope.chat.server.send(vm.chatMessage);
                vm.chatMessage.message = "";
            }

            function _getUsersInChat() {
                requestService.ApiRequestService("GET", "/api/Chats/AllMembersInChat/" + vm.chatMessage.chatId).then(function (response) {
                    vm.chatUsers = response.items;
                }).catch(function (err) {
                    console.log(err);
                })
            }

            function _back() {
                vm.chatMessage.isOnChat = false;
                requestService.ApiRequestService("PUT", "/api/ChatUserProfiles/" + vm.chatMessage.chatId, vm.chatMessage).then(function (response) {
                }).catch(function (err) {
                    console.log(err);
                })
                vm.chatIndex = true;
            }

            function _leaveChat() {
                swal({
                    title: "Are you sure you want to leave this chat?",
                    type: "error",
                    showCancelButton: true,
                    confirmButtonColor: "#FF0000",
                    confirmButtonText: "Okay",
                    closeOnCancel: true,
                    closeOnConfirm: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            requestService.ApiRequestService("delete", "/api/ChatUserProfiles/" + vm.chatMessage.chatId)
                                .then(function (response) {
                                    _back();
                                    vm.finalMembersPics = [];
                                    vm.chats = [];
                                    vm.membersInTeamForPics = [];
                                    vm.chatId = [];
                                    _init();
                                })
                                .catch(function (err) {
                                    console.log(err);
                                }); 
                        }
                    });
            }

            function _addMemberModal() {
                for (var i = 0; i < vm.chatUsers.length; i++) {
                    vm.teamId = vm.chatUsers[0].teamId;
                }

                var modalInstance = $uibModal.open({
                    animation: vm.animationsEnabled,
                    component: 'addMemberToChat',
                    size: 'md',
                    resolve: {
                        chatId: function () { return vm.chatMessage.chatId; },
                        teamId: function () { return vm.teamId; }
                    }
                });

                modalInstance.result.then(function (close) {
                    vm.valueFromModal = close;
                    vm.chatUsers = [];
                    _getUsersInChat();
                }, function () {
                    vm.chatUsers = [];
                    _getUsersInChat();
                });
            }
        }
    })
})();
