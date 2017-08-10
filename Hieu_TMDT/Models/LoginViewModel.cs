using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class LoginViewModel
    {
        AccountHelper acc = new AccountHelper();
        wthEntities db = new wthEntities();
        public NGUOI_DUNG user { set; get; }

        //kiểm tra tài khoản khi Login
        public int CheckAccount(NGUOI_DUNG x) //0 : chưa có tài khoản, 1: chưa xác thực, 2: đã xác thực
        {
            try
            {
                string password = acc.md5(x.PASSWORD);
                //kiểm tra là đúng username, pass
                var user = db.NGUOI_DUNG.Where(p => p.USERNAME == x.USERNAME && p.PASSWORD == password).SingleOrDefault();
                if (user != null)
                {
                    if (user.XACTHUC == 1) { return 1; } //chưa xác thực
                    this.user.USERNAME = x.USERNAME;
                    this.user.IDQH = user.IDQH;
                    this.user.PASSWORD = x.PASSWORD;
                    return 2;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        
    }
}