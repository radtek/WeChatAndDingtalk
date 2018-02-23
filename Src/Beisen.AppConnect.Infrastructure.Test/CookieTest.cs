using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beisen.AppConnect.Infrastructure.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.Infrastructure.Test
{
    [TestClass]
    public class CookieTest
    {
        [TestMethod]
        public void Test()
        {
            var value = CryptographyHelper.AESDecrypt("DV+d4Ll+6pehQ7mEDlIq2174CkMuoBAszuSfngRh47QnM09eHcXyDgW01k16C9tfDaFsCzU1FVpnIZSptRmGknM8A2P5uutpZpitsG1O30kJ1UDlFKM0o+YVaITBMCu9aVbWMIT/e7/tR2Fu+w2NneU1I+9thmiLyKmH1S3jiPHdxbs/xJvcGYfjYGhJJ5vT7fNMphu5wrnfMRD0fUSyXg==");
            var userCookie = SerializeHelper.Deserialize<UserCookie>(value);
        }
    }
}
