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
                ShowLoginPageEvent.Invoke();
                ShowMessageEvent.Invoke("Ошибка", "Ваша должность не позволяет совершить данную операцию!");
            }
            else if (Ex.Message == "INVALID_DATA_ERROR")
            {
                ShowMessageEvent.Invoke("Ошибка", "Введенные данные не прошли валидацию!");
            }
            else if (Ex.Message == "INVALID_ID")
            {
                ShowMessageEvent.Invoke("Ошибка", "Неверный ID!");
            }
            else if (Ex.Message == "INVALID_POSITION_ERROR")
            {
                ShowMessageEvent.Invoke("Ошибка", "Указана недопустимая должность!");
            }
            else if (Ex.Message == "NOT_ENOUGH_PRODUCT")
            {
                ShowMessageEvent.Invoke("Ошибка", "Продуктов на складе не хватает!");
            }
            else if (Ex.Message == "AMOUNT_TOO_BIG")
            {
                ShowMessageEvent.Invoke("Ошибка", "Количество возвращаемых продуктов не должно превышать количество заказанных!");
            }
            else if (Ex.Message == "ALREADY_TAKEN_LOGIN_ERROR")
            {
                ShowMessageEvent.Invoke("Ошибка", "Этот логин уже занят!");
            }
            else
            {
                ShowMessageEvent.Invoke("Ошибка", "Операция не была выполнена. Возможные причины: \n 1. Нет подключения к базе данных. \n 2. Выполнение операции вызвало нарушение ограничений в базе данных. \n 3. Запись, с которой вы работаете, устарела.");
            }
        }
    }
}
