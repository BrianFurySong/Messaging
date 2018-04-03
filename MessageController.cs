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
    [RoutePrefix("api/Chats")]
    public class ChatController : ApiController
    {
        IChatService _chatService;
        IUserService _userService;
        public ChatController(IChatService chatService, IUserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }

        [Route(), HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                ItemsResponse<Chat> response = new ItemsResponse<Chat>();
                response.Items = _chatService.GetAll();
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
                ItemResponse<Chat> response = new ItemResponse<Chat>();
                response.Item = _chatService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route(), HttpPost]
        public HttpResponseMessage Post(ChatAddRequest model)
        {
            try
            {
                ItemResponse<int> response = new ItemResponse<int>();
                response.Item = _chatService.Insert(model);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Put(ChatUpdateRequest model)
        {
            try
            {
                _chatService.Update(model);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _chatService.Delete(id);
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
                int id = _userService.GetCurrentUserId();
                ItemsResponse<Chat> response = new ItemsResponse<Chat>();
                response.Items = _chatService.GetByUserBaseId(id);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("AllMembersInChat/{ChatId:int}"), HttpGet]
        public HttpResponseMessage GetAllByChatAllMemebers(int ChatId)
        {
            try
            {
                ItemsResponse<Chat> response = new ItemsResponse<Chat>();
                response.Items = _chatService.GetAllByChatAllMemebers(ChatId);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex; 

            }
        }

        [Route("GetMembersLeftOver/{ChatId:int}/{TeamId:int}"), HttpGet]
        public HttpResponseMessage GetMembersLeftOver(int ChatId, int TeamId)
        {
            try
            {
                int UserBaseId = _userService.GetCurrentUserId();
                ItemsResponse<Chat> response = new ItemsResponse<Chat>();
                response.Items = _chatService.GetMembersLeftOver(_userService.GetCurrentUserId(), ChatId, TeamId);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
