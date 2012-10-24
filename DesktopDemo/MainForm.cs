using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EasyTwitter;
using EasyTwitter.Entities;
using System.Diagnostics;

namespace DesktopDemo
{
    public partial class MainForm : Form
    {
        public OAuthTokens Tokens { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string requestToken = OAuthUtility.GetRequestToken().Token;

            Uri authenticationUri = OAuthUtility.BuildAuthorizationUri(requestToken);
            Process.Start(authenticationUri.AbsoluteUri);

            OAuthDialog dialog = new OAuthDialog(requestToken);
            dialog.ShowDialog(this);

            if (dialog.isAuth)
            {
                //user is authenticated
                this.Tokens = dialog.Tokens;
                btnSignIn.Enabled = false;
                btnTweet.Enabled = true;
                txtTweet.Enabled = true;
            }

        }

        private void btnTweet_Click(object sender, EventArgs e)
        {
            TweetActions tweetActions = new TweetActions(this.Tokens);
            TwitterStatus status = tweetActions.UpdateStatus(txtTweet.Text);
            if (status == TwitterStatus.Success)
                MessageBox.Show("You updated your status successfully");
            else
                MessageBox.Show("Something went wrong",
                    "ERROR",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

            txtTweet.Text = "";
        }
    }
}
