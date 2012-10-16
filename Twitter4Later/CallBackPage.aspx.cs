using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EasyTwitter;


namespace Twitter4Later
{
    public partial class CallBackPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            OAuthTokens tokens = new OAuthTokens();
            OAuthTokenResponse accessTokenResponse = OAuthUtility.GetAccessToken();

            if (accessTokenResponse != null)
            {
                tokens.AccessToken = accessTokenResponse.Token;
                tokens.AccessTokenSecret = accessTokenResponse.TokenSecret;

                Session.Add("OAuthTokenResponse", accessTokenResponse);
                Session.Add("OAuthTokens", tokens);

                Response.Redirect("Default.aspx?action=logged");
            }
            else
            {
                Response.Redirect("Default.aspx?action=noLogged");
            }

        }
    }
}