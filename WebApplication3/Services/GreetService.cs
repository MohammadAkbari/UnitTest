using System;

namespace WebApplication3.Services
{
    public class GreetService
    {
        public string Greet()
        {
            if (DateTime.Now.Hour < 12)
                return "Good morning!";
            else
                return "Have a great day!";
        }
    }
}
