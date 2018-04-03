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
    [RoutePrefix("api/ChatMessages")]
    public class ChatMessageController : ApiController
    {
        IChatMessageService _chatMessageService;
        IUserService _userService;

        public ChatMessageController(IChatMessageService chatMessageService, IUserService userService)
        {
            _chatMessageService = chatMessageService;
            _userService = userService;
        }

        public ChatMessageController()
        {
        }

        [Route(), HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                ItemsResponse<ChatMessage> response = new ItemsResponse<ChatMessage>();
                response.Items = _chatMessageService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                ItemResponse<ChatMessage> response = new ItemResponse<ChatMessage>();
                response.Item = _chatMessageService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [Route("Chat/{chatId:int}"), HttpGet]
        public HttpResponseMessage GetByChatId(int chatId)
        {
            try
            {
                ItemsResponse<ChatMessage> response = new ItemsResponse<ChatMessage>();
                response.Items = _chatMessageService.GetByChatId(chatId);
                for (int i = 0; i < response.Items.Count; i++)
                {
                    if (response.Items[i].UserBaseId == _userService.GetCurrentUserId())
                    {
                        response.Items[i].IsUser = true;
                    }
                    else
                    {
                        response.Items[i].IsUser = false;
                    }
                    response.Items[i].UserProfileId = 0;
                    response.Items[i].UserBaseId = 0;
                }
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route(), HttpPost]
        public HttpResponseMessage Post(ChatMessageAddRequest model)
        {
            try
            {
                model.UserBaseId = _userService.GetCurrentUserId();
                ItemResponse<int> response = new ItemResponse<int>();
                response.Item = _chatMessageService.Insert(model);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Put(ChatMessageUpdateRequest model)
        {
            try
            {
                model.UserBaseId = _userService.GetCurrentUserId();
                _chatMessageService.Update(model);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _chatMessageService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
