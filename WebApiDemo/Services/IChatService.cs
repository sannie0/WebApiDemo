using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo.Services
{
    public interface IChatService
    {
        void Registration(User newUser);
        void SaveUsers();
        int GetUserId();

        //Task SendMessage(string userId, string message);
        
        string LoginUser(User user);
    }
}
