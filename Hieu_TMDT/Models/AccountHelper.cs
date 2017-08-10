using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class AccountHelper
    {
        wthEntities db = new wthEntities();
        public bool SendMail(string ToAddress, string Subject, string Body)
        {
            try
            {
                string to = ToAddress;
                string from = "ginta15979@gmail.com";

                string subject = Subject;
                string body = Body;
                MailMessage message = new MailMessage(from, to, subject, body);
                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("ginta15979@gmail.com", "songhieu1206");
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    client.Send(message);

                }
                return true;
            }
            catch (Exception) { return false; }
        }
        //kiểm tra khớp chuỗi sau khi nhấn confirm
        public bool CheckBarCode(string username, string email, string barcode)
        {
            NGUOI_DUNG check = db.NGUOI_DUNG.SingleOrDefault(p => p.EMAIL == email && p.USERNAME == username && p.BARCODE == barcode && p.XACTHUC == 1);
            if (check != null) //khớp --> confirm
            {
                db.NGUOI_DUNG.Where(p => p.USERNAME == username).SingleOrDefault().XACTHUC = 0;
                db.SaveChanges();
                return true;
            }
            return false;
        }
        //sinh chuỗi ngẫu nhiên
        public string RandomString(int size)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            return sb.ToString().ToLower();
        }
        public bool CheckResetPass(string username, string email)
        {
            try
            {
                NGUOI_DUNG user = db.NGUOI_DUNG.Where(p => p.USERNAME == username && p.EMAIL == email).FirstOrDefault();
                if (user != null)
                {
                    string newpass = RandomString(6);
                    string newbarcode = RandomString(10);
                    var userEdited = db.NGUOI_DUNG.Where(p => p.USERNAME == username && p.EMAIL == email).FirstOrDefault();
                    userEdited.PASSWORD = newpass;
                    userEdited.BARCODE = newbarcode;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception) { return false; }
        }

        public byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }
    }
}