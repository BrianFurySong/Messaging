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
    public class ChatService : BaseService, IChatService
    {
        public List<Chat> GetAll()
        {
            List<Chat> list = new List<Chat>();

            DataProvider.ExecuteCmd("dbo.Chat_SelectAll",
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<Chat>.Instance.MapToObject(reader));
                });
            return list;
        }

        public Chat GetById(int id)
        {
            Chat item = new Chat();

            DataProvider.ExecuteCmd("dbo.Chat_SelectById",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                },

                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    item = DataMapper<Chat>.Instance.MapToObject(reader);
                });
            return item;
        }

        public int Insert(ChatAddRequest model)
        {
            int id = 0;
            DataProvider.ExecuteNonQuery("dbo.Chat_Insert",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@ChatName", model.ChatName);
                    paramCollection.AddWithValue("@TeamId", model.TeamId);

                    SqlParameter idOutput = new SqlParameter("@Id", SqlDbType.Int);
                    idOutput.Direction = ParameterDirection.Output;
                    paramCollection.Add(idOutput);
                },
                returnParameters: delegate (SqlParameterCollection paramCollection)
                {
                    int.TryParse(paramCollection["@Id"].Value.ToString(), out id);
                });
            return id;
        }

        public void Update(ChatUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery("dbo.Chat_Update",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@ChatName", model.ChatName);
                    paramCollection.AddWithValue("@Id", model.Id);
                });
        }

        public void Delete(int Id)
        {
            DataProvider.ExecuteNonQuery("dbo.Chat_Delete",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", Id);
                });
        }

        public List<Chat> GetByUserBaseId(int UserBaseId)
        {
            List<Chat> list = new List<Chat>();

            DataProvider.ExecuteCmd("dbo.Chat_SelectByBaseId",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", UserBaseId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<Chat>.Instance.MapToObject(reader));
                });
            return list;
        }

        public List<Chat> GetAllByChatAllMemebers(int ChatId)
        {
            List<Chat> list = new List<Chat>();

            DataProvider.ExecuteCmd("dbo.chat_SelectByIdAllMembers",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@ChatId", ChatId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<Chat>.Instance.MapToObject(reader));
                });
            return list;
        }

        public List<Chat> GetByEmail(string Email)
        {
            List<Chat> list = new List<Chat>();

            DataProvider.ExecuteCmd("dbo.chat_GetByEmail",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Email", Email);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<Chat>.Instance.MapToObject(reader));
                });
            return list;
        }

        public List<Chat> GetMembersLeftOver(int UserBaseId, int ChatId, int TeamId)
        {
            List<Chat> list = new List<Chat>();

            DataProvider.ExecuteCmd("dbo.Chat_SelectMembersLeftOver",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserBaseId", UserBaseId);
                    paramCollection.AddWithValue("@ChatId", ChatId);
                    paramCollection.AddWithValue("@TeamId", TeamId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    list.Add(DataMapper<Chat>.Instance.MapToObject(reader));
                });
            return list;
        }

    }
}
