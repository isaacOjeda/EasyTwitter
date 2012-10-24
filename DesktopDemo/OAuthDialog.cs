using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EasyTwitter;
namespace DesktopDemo
{
    public partial class OAuthDialog : Form
    {
        public string RequestToken { get; set; }
        public bool isAuth { get; set; }
        public OAuthTokens Tokens { get; set; }

        public OAuthDialog(string requestToken)
        {
            this.RequestToken = requestToken;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(txtPIN.Text))
            {
                OAuthTokens tokens = new OAuthTokens();
                OAuthTokenResponse accessTokenResponse = OAuthUtility.GetAccessToken(this.RequestToken,txtPIN.Text);

                if (accessTokenResponse != null)
                {
                    tokens.AccessToken = accessTokenResponse.Token;
                    tokens.AccessTokenSecret = accessTokenResponse.TokenSecret;
                    this.Tokens = tokens;
                    this.isAuth = true;

                    this.Close();
                }
                else
                    this.isAuth = false;
            }
        }
    }
}
