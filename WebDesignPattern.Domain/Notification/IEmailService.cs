using System;

namespace WebDesignPattern.Domain.Notification;

public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
}
