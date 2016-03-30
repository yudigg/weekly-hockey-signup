using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace HockeyLibrary
{
    public class HockeyDb
    {
        private readonly string _connectionString;
        public HockeyDb(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int CreateEvent(Event e)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "insert into Events values(@dt,@players);select @@identity";
                cmd.Parameters.AddWithValue("@dt", e.Dt);
                cmd.Parameters.AddWithValue("@players", e.Players);
                conn.Open();
                return (int)(decimal)cmd.ExecuteScalar();

            }
        }
        public int GetMaxPlayersForEvent(int eventid)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select e.Players from events e where eventid = @eventId ";
                cmd.Parameters.AddWithValue("@eventid", eventid);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public Event GetLatestEvent()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select top 1 * from events";
                conn.Open();
                Event e = new Event();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    e.EventId = (int)reader["eventId"];
                    e.Dt = (DateTime)reader["dateTime"];
                    e.Players = (int)reader["players"];
                }
                return e;
            }
        }
        public EventStatus GetEventStatus(Event e)
        {
            if(e.Dt < DateTime.Now)
            {
                return EventStatus.InThePast;
            }
            if(GetNumberofSignUps(e.EventId) < GetMaxPlayersForEvent(e.EventId))
            {
                return EventStatus.Open;
            }
            return EventStatus.Full;
        }
        public int GetNumberofSignUps(int eventid)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select COUNT(*) from SignUps s
            join Events e 
            on e.EventId = s.EventId";
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public void CreateSignUps(SignUp s)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "insert into SignUps values(@firstName,@lastName,@email,@eventId)";
                cmd.Parameters.AddWithValue("@firstName", s.FirstName);
                cmd.Parameters.AddWithValue("@lastName", s.LastName);
                cmd.Parameters.AddWithValue("@email", s.Email);
                cmd.Parameters.AddWithValue("@eventId", s.EventId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddToEmailList(string email)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "insert into Notifications(Email) values(@email)";
                cmd.Parameters.AddWithValue("@email", email);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public IEnumerable<Notifications> GetEmailList()
        {
            List<Notifications> result = new List<Notifications>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from Notifications";
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Notifications n = new Notifications
                    {
                        Email = (string)reader["email"],
                        Id = (int)reader["id"]
                    };
                    result.Add(n);
                }
                return result;
            }
        }
        public IEnumerable<SignUp> GetCurrentSignUps(int eventid)
        {
            List<SignUp> result = new List<SignUp>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from SignUps where eventId = @eventid";
                cmd.Parameters.AddWithValue("@eventid",eventid);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SignUp s= new SignUp
                    {
                       FirstName = (string)reader["FirstName"],
                       LastName = (string)reader["LastName"],
                       Email = (string)reader["email"],
                       EventId= (int)reader["eventid"]
                        };
                    result.Add(s);
                }
                return result;
            }
        }
        public void Delete(int eventid)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "Delete from signups where eventid = @eventid";
                cmd.Parameters.AddWithValue("@eventid", eventid);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void SendEmail(string body)
        {
            var fromAddress = new MailAddress("yyaagg148@gmail.com", "From Yudi");
            var toAddress = new MailAddress("ygoldgrab@gmail.com", "To Name");
            const string fromPassword = "yudyudyud";
            const string subject = "test";
             body = "Hey now!!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
