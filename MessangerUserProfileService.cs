using LeaseHold.Models.Domain;
using LeaseHold.Models.Requests;
using LeaseHold.Services.Interfaces;
using LeaseHold.Services.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaseHold.Services
{
    public class ChatUserProfileService : BaseService, IChatUserProfileService
    {
        public List<ChatUserProfile> GetAll()
        {
            List<ChatUserProfile> list = new List<ChatUserProfile>();

            DataProvider.ExecuteCmd("dbo.ChatUserProfile_SelectAll",
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<ChatUserProfile>.Instance.MapToObject(reader));
                });
            return list;
        }

        public List<ChatUserProfile> GetAllByEachTeamForChat(int teamId, int userBaseId)
        {
            List<ChatUserProfile> list = new List<ChatUserProfile>();

            DataProvider.ExecuteCmd("dbo.ChatUserProfile_SelectByEachTeamForChat",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", userBaseId);
                    paramCollection.AddWithValue("@TeamId", teamId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<ChatUserProfile>.Instance.MapToObject(reader));
                });
            return list;
        }

        public ChatUserProfile GetById(int id)
        {
            ChatUserProfile item = new ChatUserProfile();

            DataProvider.ExecuteCmd("dbo.ChatUserProfile_SelectById",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                },

                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    item = DataMapper<ChatUserProfile>.Instance.MapToObject(reader);
                });
            return item;
        }

        public void Insert(ChatUserProfileAddRequest model)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatUserProfile_Insert",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", model.UserBaseId);
                    paramCollection.AddWithValue("@ChatId", model.ChatId);
                });
        }

        public void Delete(int Id, int ChatId)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatUserProfile_Delete",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", Id);
                    paramCollection.AddWithValue("@ChatId", ChatId);

                });
        }
        public void IncrementPendingMessages(ChatUserProfileAddRequest model)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatUserProfile_IncrementPendingMessages",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", model.UserBaseId);
                    paramCollection.AddWithValue("@ChatId", model.ChatId);
                });
        }
        public void ResetPendingMessages(ChatUserProfileAddRequest model)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatUserProfile_ResetPendingMessages",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", model.UserBaseId);
                    paramCollection.AddWithValue("@ChatId", model.ChatId);
                });
        }
        public void ToggleOnChat(ChatUserProfileAddRequest model)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatUserProfile_ToggleOnChat",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", model.UserBaseId);
                    paramCollection.AddWithValue("@ChatId", model.ChatId);
                    paramCollection.AddWithValue("@IsOnChat", model.IsOnChat);
                });
        }
        public void SetAllIsOnChatFalse(int UserBaseId)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatUserProfile_SetAllIsOnChatFalse",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", UserBaseId);
                });
        }
        public List<ChatUserProfile> GetByUserBaseId(int userBaseId)
        {
            List<ChatUserProfile> list = new List<ChatUserProfile>();

            DataProvider.ExecuteCmd("dbo.ChatUserProfile_SelectByUserBaseId",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", userBaseId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<ChatUserProfile>.Instance.MapToObject(reader));
                });
             return list;
        }
    }
}
