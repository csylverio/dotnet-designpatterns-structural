using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using WebDesignPattern.Domain.Notification;

namespace WebDesignPattern.Infra.Notification;

public class SendGridEmailService : IEmailService
{
    private readonly SendGridClient _client;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public SendGridEmailService(string apiKey, string fromEmail, string fromName)
    {
        _client = new SendGridClient(apiKey);
        _fromEmail = fromEmail;
        _fromName = fromName;
    }

    public async void SendEmail(string to, string subject, string body)
    {
        // var msg = new SendGridMessage
        // {
        //     From = new EmailAddress(_fromEmail, _fromName),
        //     Subject = subject,
        //     PlainTextContent = body,
        //     HtmlContent = $"<p>{body.Replace("\n", "<br>")}</p>"
        // };
        // msg.AddTo(new EmailAddress(to));

        try
        {
            Console.WriteLine($"Enviando e-mail para {to} com assunto '{subject}'");
            // var response = await _client.SendEmailAsync(msg);

            // if (!response.IsSuccessStatusCode)
            // {
            //     var error = await response.Body.ReadAsStringAsync();
            //     // Ideal seria disparar uma exceção ou registrar o erro em um sistema de log
            //     // Para fins didáticos, vamos apenas imprimir no console
            //     //throw new Exception($"Falha no envio: {error}");
            //     Console.WriteLine($"Falha no envio: {error}");
            // }
        }
        catch (Exception ex)
        {
            // Log do erro (ex: Serilog, Application Insights)
            Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            throw; // Re-lança para tratamento superior
        }
    }
}