﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Configuration;

namespace Login
{
    public partial class LoginForm : Form
    {
        public string ftpHost { get; set; }
        public string ftpUserName { get; set; }
        public string ftpPassword { get; set; }
        private System.Configuration.Configuration config;

        public LoginForm()
        {
            InitializeComponent();
            btn_login.Image = Properties.Resources.login;

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var setting = config.AppSettings.Settings;
            if(setting["FtpServer"] == null)
            {
                setting.Add("FtpServer", "10.87.10.4:2121");
            }
            if (setting["FtpUsername"] == null)
            {
                setting.Add("FtpUsername", "sanmeng");
            }
            if (setting["FtpPassword"] == null)
            {
                setting.Add("FtpPassword", "hero198891");
            }
            ftpHost = config.AppSettings.Settings["FtpServer"].Value;
            ftpUserName = config.AppSettings.Settings["FtpUsername"].Value;
            ftpPassword = config.AppSettings.Settings["FtpPassword"].Value;

            tb_user.Text = ftpUserName;
            tb_pwd.Text = ftpPassword;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            ftpHost = "10.87.10.4:2121";
            ftpUserName = tb_user.Text;
            ftpPassword = tb_pwd.Text;
            lb_statue.Text = "登录中 ...";

            if (Login())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                config.AppSettings.Settings["FtpUsername"].Value = ftpUserName;
                config.AppSettings.Settings["FtpPassword"].Value = ftpPassword;
                config.Save();
            }
            btn_login.Image = Properties.Resources.login;
        }

        private void btn_cls_Click(object sender, EventArgs e)
        {
            label1.Image = Properties.Resources.btn_close_pressed;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        private bool Login()
        {
            bool ResultValue = true;
            try
            {
                FtpWebRequest ftprequest = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost);//创建FtpWebRequest对象
                ftprequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);//设置FTP登陆信息
                ftprequest.Method = WebRequestMethods.Ftp.ListDirectory;//发送一个请求命令
                ftprequest.Proxy = null;
                ftprequest.KeepAlive = false;
                FtpWebResponse ftpResponse = (FtpWebResponse)ftprequest.GetResponse();//响应一个请求
                ftpResponse.Close();//关闭请求
            }
            catch (System.Security.SecurityException sse)
            {
                lb_statue.Text = sse.Message;
                ResultValue = false;
            }
            catch (System.UriFormatException)
            {
                lb_statue.Text = "主机名不正确";
                ResultValue = false;
            }
            catch (System.Net.WebException we)
            {
                lb_statue.Text = we.Message;
                ResultValue = false;
            }
            return ResultValue;
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            label1.Image = Properties.Resources.btn_close_hover;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.Image = Properties.Resources.btn_close_normal;
        }

        private void btn_login_MouseHover(object sender, EventArgs e)
        {
            btn_login.Image = Properties.Resources.loginhover;
        }

        private void btn_login_MouseLeave(object sender, EventArgs e)
        {
            btn_login.Image = Properties.Resources.login;
        }

        private void tb_pwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\r')
            {
                btn_login.Image = Properties.Resources.loginhover;
                btn_login.PerformClick();
            }
        }
    }
}
