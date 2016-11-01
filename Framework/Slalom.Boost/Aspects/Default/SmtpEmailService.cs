using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects.Default
{
    /// <summary>
    /// An SMTP <see cref="ISendEmail"/> implementation.
    /// </summary>
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [DefaultBinding]
    public class SmtpEmailService : ISendEmail
    {
        private static bool circuit;

        /// <summary>
        /// Sends an email to the specified recipients.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email</param>
        /// <param name="recipients">The email addresses of recipients of the email.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="subject"/> argument is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="body"/> argument is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="recipients"/> argument is null or empty.</exception>
        public void Send(string subject, string body, string[] recipients)
        {
            if (String.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("An email must have a subject.", nameof(subject));
            }

            if (String.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException("An email must have a body.", nameof(body));
            }


            if (recipients == null || recipients.Any(String.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("All of the specified recipient emails must be non-empty values.");
            }

            using (var client = new SmtpClient())
            {
                var message = new MailMessage();
                recipients.ToList().ForEach(e => message.To.Add(e));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = body.IndexOf("<body", StringComparison.OrdinalIgnoreCase) > -1;
                if (client.Host == null)
                {
                    client.Host = "127.0.0.1";
                    EnsureLocalListener();
                }
                if (message.From == null)
                {
                    message.From = new MailAddress("local@local.com");
                }
                client.Send(message);
            }
        }

        /// <summary>
        /// Checks to see if anything is listening on the local SMTP port and boots SMTP4Dev if nothing is listening.
        /// </summary>
        [DebuggerStepThrough, DebuggerNonUserCode]
        public static void EnsureLocalListener()
        {
            if (!circuit)
            {
                circuit = true;
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    try
                    {
                        socket.Connect("127.0.0.1", 25);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                        {
                            new Action(() =>
                            {
                                var path = @"C:\Program Files (x86)\smtp4dev\Smtp4dev.exe";
                                if (File.Exists(path))
                                {
                                    Process.Start(path);
                                }
                                else if (File.Exists(path = @"C:\Program Files\smtp4dev\Smtp4dev.exe"))
                                {
                                    Process.Start(path);
                                }
                            }).BeginInvoke(null, null);
                            Thread.Sleep(500);
                        }
                    }
                }
            }
        }
    }
}