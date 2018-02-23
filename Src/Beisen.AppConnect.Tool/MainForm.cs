using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Constants;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.Tool.Common;
using Beisen.AppConnect.Tool.RepairMethod;
using Beisen.SearchV3;
using Beisen.SearchV3.DSL.Filters;
using Beisen.SearchV3.DSL.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Beisen.UpaasPortal.ImportTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void startRepairButton_Click(object sender, EventArgs e)
        {
            var x = SqlHelper.GetAppIdByType();
            var y = SqlHelper.GetInfoByAppId("wx27b047a32d81ba29,ww05f97ebb171b4a8f");

            var message = "";
            logTB.Text = message;
            var metaObjectName = metaObjectTB.Text.Trim();
            startRepairButton.Enabled = false;
            var tenantIds = GetTenantId(tenantTB.Text.Trim());

            switch (metaObjectName)
            {
                case "AppConnect.UserInfoMapping":
                    UserInfoMappingRepair.RepairUserInfoMapping(tenantIds, metaObjectName, fieldsTB.Text.Trim(), ref message);
                    break;
                case AppUserConstants.MetaName:
                    AppUserRepair.RunRepair_AppUserRepair(new List<int>(), metaObjectName, ref message);
                    break;
                default:
                    MessageBox.Show("对象名错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    startRepairButton.Enabled = true;
                    return;
            }
            if (!string.IsNullOrEmpty(message))
            {
                logTB.Text = message;
                MessageBox.Show("修复完成！");
            }
            startRepairButton.Enabled = true;
        }

        private List<int> GetTenantId(string str)
        {
            string message = "";
            List<int> tenantIds = new List<int>();
            var tenantIdstr = str.Replace("，", ",").Trim();
            tenantIds = BeisenUserDao.GetTenantIdList(tenantIdstr, ref message);

            return tenantIds;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transBT_Click(object sender, EventArgs e)
        {
            //input
            var inputUrl = textBox3.Text.Trim();
            if (string.IsNullOrEmpty(inputUrl))
            {
                MessageBox.Show("原始Url不能为空！");
                return; 
            }
            if (!(inputUrl.StartsWith("http://")) && !(inputUrl.StartsWith("https://")))
            {
                MessageBox.Show("Url协议不正确");
                return; 
            }
           
            string[] redirectArray = inputUrl.Split(new string[] { "redirect_url=" }, StringSplitOptions.RemoveEmptyEntries);
            if (redirectArray.Length != 2)
            {
                MessageBox.Show("Url不能缺少redirect_url参数！");
                return;
            }

            string[] returnArray = redirectArray[1].Split(new string[] { "return_url=" }, StringSplitOptions.RemoveEmptyEntries);

            string encodeUrl = "";
            if (returnArray.Length == 2)
            {
                var encodeReturnUrl = UrlEncode(returnArray[1]);
                encodeUrl = UrlEncode(returnArray[0] + "return_url=" + encodeReturnUrl);
            }
            else
            {
               encodeUrl = UrlEncode(redirectArray[1]);
            }
            textBox1.Text = redirectArray[0] + "redirect_url=" + encodeUrl;
        }

        public string UrlEncode(string str)
        {
            //StringBuilder sb = new StringBuilder();
            //byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            //for (int i = 0; i < byStr.Length; i++)
            //{
            //    sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            //}
            //return (sb.ToString());
            return System.Web.HttpUtility.UrlEncode(str);
        }
    }
}