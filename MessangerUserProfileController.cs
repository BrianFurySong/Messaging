using LeaseHold.Models.Domain;
using LeaseHold.Models.Requests;
using LeaseHold.Models.Responses;
using LeaseHold.Services;
using LeaseHold.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LeaseHold.Web.Controllers.Api
{
    [Authorize(Roles = "Admin, Subscriber, Broker, Client")]
    [RoutePrefix("api/ChatUserProfiles")]
    public class ChatUserProfileController : ApiController
    {
        IChatUserProfileService _chatUserProfileService;
        IUserService _userService;

        public ChatUserProfileController(IChatUserProfileService chatUserProfileService, IUserService userService)
        {
            _chatUserProfileService = chatUserProfileService;
            _userService = userService;
        }

        [Route(), HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                ItemsResponse<ChatUserProfile> response = new ItemsResponse<ChatUserProfile>();
                response.Items = _chatUserProfileService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex; 

            }
        }

        [Route("Team/{teamId:int}"), HttpGet]
        public HttpResponseMessage GetAllByEachTeamForChat(int teamId)
        {
            try
            {
                ItemsResponse<ChatUserProfile> response = new ItemsResponse<ChatUserProfile>();
                response.Items = _chatUserProfileService.GetAllByEachTeamForChat(teamId, _userService.GetCurrentUserId());
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                ItemResponse<ChatUserProfile> response = new ItemResponse<ChatUserProfile>();
                response.Item = _chatUserProfileService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route(), HttpPost]
        public HttpResponseMessage Post(ChatUserProfileAddRequest model)
        {
            try
            {
                if (model.UserBaseId == 0)
                {
                    model.UserBaseId = _userService.GetCurrentUserId();
                }
                _chatUserProfileService.Insert(model);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("{chatId:int}"), HttpDelete]
        public HttpResponseMessage Delete(int chatId)
        {
            try
            {
                _chatUserProfileService.Delete(_userService.GetCurrentUserId(), chatId);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("{chatId:int}"), HttpPut]
        public HttpResponseMessage ToggleOnChat(ChatUserProfileAddRequest model)
        {
            try
            {
                model.UserBaseId = _userService.GetCurrentUserId();
                _chatUserProfileService.ToggleOnChat(model);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [Route("Reset/{chatId:int}"), HttpPut]
        public HttpResponseMessage ResetPendingMessages(int chatId)
        {
            try
            {
                ChatUserProfileAddRequest model = new ChatUserProfileAddRequest
                {
                    ChatId = chatId,
                    UserBaseId = _userService.GetCurrentUserId()
                };
                _chatUserProfileService.ResetPendingMessages(model);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("User"), HttpGet]
        public HttpResponseMessage GetByUserBaseId()
        {
            try
            {
                ItemsResponse<ChatUserProfile> response = new ItemsResponse<ChatUserProfile>();
                response.Items = _chatUserProfileService.GetByUserBaseId(_userService.GetCurrentUserId());
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("Increment/{chatId:int}"), HttpPut]
        public HttpResponseMessage IncrementPendingMessages(int chatId)
        {
            try
            {
                ChatUserProfileAddRequest model = new ChatUserProfileAddRequest
                {
                    ChatId = chatId,
                    UserBaseId = _userService.GetCurrentUserId()
                };
                _chatUserProfileService.IncrementPendingMessages(model);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
