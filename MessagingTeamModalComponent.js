(function () {
    "use strict";

    angular.module(AppName).component("chatTeamModal", {
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        templateUrl: "/scripts/components/views/chatTeamModalView.html",
        controller: function (requestService, $scope, $window, $filter, $uibModal, $anchorScroll, $location) { 
            var vm = this;
            vm.$onInit = _init; 
            vm.$scope = $scope;
            vm.$scope.chat = $.connection.notificationHub;
            vm.teamsYouAreOn = [];
            vm.membersInTeam = [];
            vm.chatArray = [];
            vm.newChat = {};
            vm.allChats = [];
            vm.chatName = {};
            vm.allChatsWithMembers = [];
            vm.restOfMembersIntoChat = [];
            vm.teamId = {};

            vm.closeChatTeamModalWindow = _closeChatTeamModalWindow;
            vm.getAllTeamsAndMembers = _getAllTeamsAndMembers;
            vm.pickPeople = _pickPeople;
            vm.startChatting = _startChatting;

            function _init() {
                requestService.ApiRequestService("get", "/api/Teams/TeamNameMembers")
                    .then(function (response) {
                        vm.teamsYouAreOn = response.items;
                    })
                    .catch(function (err) {
                        console.log(err);
                    })
            }

            function _getAllTeamsAndMembers(id) {
                vm.chatArray = [];
                vm.teamId = id;
                requestService.ApiRequestService("get", "/api/ChatUserProfiles/Team/" + id)
                    .then(function (response) {
                        vm.membersInTeam = response.items;
                    })
                    .catch(function (err) {
                        console.log(err);
                    })
            }

            function _pickPeople(chatModel) {
                if (chatModel.isChecked === true) {
                    vm.chatArray.push(chatModel);
                } else {
                    for (var i = vm.chatArray.length - 1; i >= 0; i--) {
                        if (vm.chatArray[i].isChecked == false) {
                            vm.chatArray.splice(i, 1);
                        }
                    }
                }
            }

            function _startChatting(teamName) {
                vm.chatName.chatName = teamName + " -" 
                for (var i = 0; i < vm.chatArray.length; i++) {
                    vm.chatName.chatName += " " + vm.chatArray[i].fullName.split(" ")[0];
                    vm.chatName.teamId = vm.teamId;
                }

                requestService.ApiRequestService("POST", "/api/Chats/", vm.chatName)
                    .then(function (response) {
                        vm.newChat.chatId = response.item;
                        vm.$scope.chat.server.addToGroup(vm.newChat.chatId);
                        vm.$scope.chat.server.addToGroup(vm.newChat.chatId);
                        requestService.ApiRequestService("POST", "/api/ChatUserProfiles", vm.newChat)
                            .then(function (response) {
                                for (var i = 0; i < vm.chatArray.length; i++) {
                                    var restOfMembersChatObj = {};
                                    restOfMembersChatObj.chatId = vm.newChat.chatId
                                    var userBaseId = vm.chatArray[i].userBaseId;
                                    restOfMembersChatObj.userBaseId = userBaseId;
                                    vm.restOfMembersIntoChat.push(restOfMembersChatObj);
                                }
                                for (var j = 0; j < vm.restOfMembersIntoChat.length; j++) {
                                    (function () {
                                        return requestService.ApiRequestService("post", "/api/ChatUserProfiles", vm.restOfMembersIntoChat[j])
                                            .then(function (response) {
                                                _closeChatTeamModalWindow();
                                            })
                                            .catch(function (err) {
                                                console.log(err);
                                            })
                                    })(vm.restOfMembersIntoChat[j]);
                                }
                            })
                            .catch(function (err) {
                                console.log(err);
                            })
                    })
                    .catch(function (err) {
                        console.log(err);
                    })
            }

            function _closeChatTeamModalWindow() {
                vm.close({  });
            }
           
        }
    })
})();
