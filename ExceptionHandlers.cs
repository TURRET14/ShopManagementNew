using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement
{
    internal class ExceptionHandlers
    {
        static public void SqlExceptionHandler(SqlException Ex, Events.ShowMessageDelegate ShowMessageEvent, Events.ShowLoginPageDelegate ShowLoginPageEvent)
        {
            if (Ex.Message == "AUTHORIZATION_ERROR")
            {
                ShowMessageEvent.Invoke("ERROR", "AUTHORIZATION_ERROR");
                ShowLoginPageEvent.Invoke();
            }
            else if (Ex.Message == "INVALID_ID")
            {
                ShowMessageEvent.Invoke("ERROR", "INVALID_ID");
                ShowLoginPageEvent.Invoke();
            }
            else if (Ex.Message == "INVALID_POSITION_ERROR")
            {
                ShowMessageEvent.Invoke("ERROR", "INVALID_POSITION_ERROR");
                ShowLoginPageEvent.Invoke();
            }
            else if (Ex.Message == "NOT_ENOUGH_PRODUCT")
            {
                ShowMessageEvent.Invoke("ERROR", "NOT_ENOUGH_PRODUCT");
                ShowLoginPageEvent.Invoke();
            }
            else if (Ex.Message == "AMOUNT_TOO_BIG")
            {
                ShowMessageEvent.Invoke("ERROR", "AMOUNT_TOO_BIG");
                ShowLoginPageEvent.Invoke();
            }
            else
            {
                ShowMessageEvent.Invoke("ERROR", "UNKNOWN_SQL_SERVER_ERROR. Error Code: " + Ex.ErrorCode);
            }
        }
    }
}
