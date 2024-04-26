using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo.Services
{
    public interface IChatService
    {
        int Registration(User newUser);
        void SaveUsers();
        string GetUserName(int user_id);

        int GetUserId();
        //Task SendMessage(string userId, string message);
        void AddMessage(ChatMessage newMessage);
        string LoginUser(User user);

        //public string GetUserById(int userId);
    }
}
