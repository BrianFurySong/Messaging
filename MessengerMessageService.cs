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
    public class ChatMessageService : BaseService, IChatMessageService
    {
        public List<ChatMessage> GetAll()
        {
            //creating a new instance of a chatmessage list
            List<ChatMessage> list = new List<ChatMessage>();
            //running the sql stored procedures
            DataProvider.ExecuteCmd("dbo.ChatMessage_SelectAll",
                //not going to be sending anything to the db
                inputParamMapper: null,
                //this is what were recieving
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<ChatMessage>.Instance.MapToObject(reader));
                });
            //return the array of items
            return list;
        }
        public ChatMessage GetById(int id)
        {
                
            ChatMessage item = new ChatMessage();
            DataProvider.ExecuteCmd("dbo.ChatMessage_SelectById",
                inputParamMapper: delegate(SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", id);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    item = DataMapper<ChatMessage>.Instance.MapToObject(reader);
                });
            return item;
        }
        public List<ChatMessage> GetByChatId(int chatId)
        {

            List<ChatMessage> list = new List<ChatMessage>();
            DataProvider.ExecuteCmd("dbo.ChatMessage_SelectByChatId",
                inputParamMapper: delegate(SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@ChatId", chatId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<ChatMessage>.Instance.MapToObject(reader));
                });
            return list;
        }
        public int Insert(ChatMessageAddRequest model)
        {
            int id = 0;
            DataProvider.ExecuteNonQuery("dbo.ChatMessage_Insert",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", id);
                    parameterCollection.AddWithValue("@ChatId", model.ChatId);
                    parameterCollection.AddWithValue("@CreatedDate", model.CreatedDate);
                    parameterCollection.AddWithValue("@UserBaseId", model.UserBaseId);
                    parameterCollection.AddWithValue("@Message", model.Message);
                },
                returnParameters: delegate (SqlParameterCollection parameterCollection)
                {
                    int.TryParse(parameterCollection["@Id"].Value.ToString(), out id);
                });
            return id;
        }
        public void Update(ChatMessageUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatMessage_Update",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", model.Id);
                    parameterCollection.AddWithValue("@ChatId", model.ChatId);
                    parameterCollection.AddWithValue("@UserBaseId", model.UserBaseId);
                    parameterCollection.AddWithValue("@Message", model.Message);
                });
        }
        public void Delete(int id)
        {
            DataProvider.ExecuteNonQuery("dbo.ChatMessage_Delete",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                });

        }
    }
}
