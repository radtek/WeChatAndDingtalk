using Beisen.AppConnect.Infrastructure;
using Beisen.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    internal static class MobileVerificationDao
    {
        internal static void Add(string mobile, int code, int type)
        {
            SafeProcedure.ExecuteNonQuery(Database.GetDatabase(DatabaseName.AppConnect),
                "dbo.MobileVerification_Add",
                parameterMapper =>
                {
                    parameterMapper.AddWithValue("@Mobile", mobile);
                    parameterMapper.AddWithValue("@Code", code);
                    parameterMapper.AddWithValue("@Type", type);
                });
        }

        internal static bool Verify(string mobile, int code, DateTime time, int type, out int codestate)
        {
            int result = 0;
            int state = 0;
            SafeProcedure.ExecuteNonQuery(Database.GetDatabase(DatabaseName.AppConnect),
                                          "dbo.MobileVerification_Verify", parameterMapper =>
                                          {
                                              parameterMapper.AddWithValue("@Mobile", mobile);
                                              parameterMapper.AddWithValue("@Code", code);
                                              parameterMapper.AddWithValue("@Time", time);
                                              parameterMapper.AddWithValue("@Type", type);
                                              parameterMapper.AddTypedDbNull("@Result", ParameterDirectionWrap.Output, DbType.Int32);
                                              parameterMapper.AddTypedDbNull("@State", ParameterDirectionWrap.Output, DbType.Int32);
                                          }, p =>
                                          {
                                              result = (int)p.GetValue("@Result");
                                              state = (int)p.GetValue("@State");
                                          });
            codestate = state;
            return result > 0;
        }

        internal static int Get(string mobile, int type, DateTime dateTime)
        {
            int result = 0;
            SafeProcedure.ExecuteNonQuery(Database.GetDatabase(DatabaseName.AppConnect),
                                          "dbo.MobileVerification_Get", parameterMapper =>
                                          {
                                              parameterMapper.AddWithValue("@Mobile", mobile);
                                              parameterMapper.AddWithValue("@Type", type);
                                              parameterMapper.AddWithValue("@Time", dateTime);
                                              parameterMapper.AddTypedDbNull("@Result", ParameterDirectionWrap.Output, DbType.Int32);
                                          }, p => result = (int)p.GetValue("@Result"));

            return result;
        }

        internal static void Abate(string mobile, int code, int type)
        {
            SafeProcedure.ExecuteNonQuery(Database.GetDatabase(DatabaseName.AppConnect),
                "dbo.MobileVerification_Abate",
                parameterMapper =>
                {
                    parameterMapper.AddWithValue("@Mobile", mobile);
                    parameterMapper.AddWithValue("@Code", code);
                    parameterMapper.AddWithValue("@Type", type);
                });
        }

        internal static bool Check(string mobile, int code, int type)
        {
            int result = 0;
            SafeProcedure.ExecuteNonQuery(Database.GetDatabase(DatabaseName.AppConnect),
                                          "dbo.MobileVerification_Check", parameterMapper =>
                                          {
                                              parameterMapper.AddWithValue("@Mobile", mobile);
                                              parameterMapper.AddWithValue("@Code", code);
                                              parameterMapper.AddWithValue("@Type", type);
                                              parameterMapper.AddTypedDbNull("@Result", ParameterDirectionWrap.Output, DbType.Int32);
                                          }, p => result = (int)p.GetValue("@Result"));

            return result > 0;
        }
    }
}
