using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/* EasyTwitter using's */
using EasyTwitter.Config;
using EasyTwitter;
using EasyTwitter.Entities;
/* */
using System.Net;
using System.IO;
using System.Text;




namespace Twitter4Later
{
    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                OAuthTokenResponse tokenResponse = Session["OAuthTokenResponse"] as OAuthTokenResponse ?? null;

                if (tokenResponse != null)
                {
                    string message = "<p>Now you are logged</p></br> <bold>Welcome</bold>: "+tokenResponse.ScreenName;
                    nowLogged.InnerHtml = message;
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {            
            string requestToken = OAuthUtility.GetRequestToken().Token;

            Uri authenticationUri = OAuthUtility.BuildAuthorizationUri(requestToken);
            Response.Redirect(authenticationUri.AbsoluteUri);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            OAuthTokens tokens = (OAuthTokens)Session["OAuthTokens"];

            #region TwitterUser
            TwitterUserActions twitterObject = new TwitterUserActions(tokens);
            TwitterResponse<TwitterUser> twitterResponse = twitterObject.GetUserInfo(txtUser.Text);

            if (twitterResponse.Status == TwitterStatus.Success)
            {
                TwitterUser user = twitterResponse.ObjectResponse;

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<h3>{0}</h3>", user.Name)
                    .AppendFormat("</br><img src=\"{0}\" /> @{1}", user.ProfileImageUrl, user.ScreenName)
                    .AppendFormat("</br>Description:{0}", user.Description)
                    .AppendFormat("</br>Location: {0}", user.Location)
                    .AppendFormat("</br>Followers:{0} Favorited tweets:{1}", user.FollowersCount, user.FavouritesCount);

                infoUser.InnerHtml = sb.ToString();
            }
            else
            {
                infoUser.InnerText = "No se encontro el usuario o ocurrio otra cosa";
            }
            #endregion

            #region Favourited
            FavoriteActions favourites = new FavoriteActions(tokens);
            TwitterResponse<List<Tweet>> response= favourites.GetTweets();
            
            gridFav.DataSource = response.ObjectResponse;
            gridFav.DataBind();
            #endregion

            #region homeTimeline
            TimelineActions actionsTimeline = new TimelineActions(tokens);
            TwitterResponse<List<Tweet>> responseTimeline= actionsTimeline.GetHomeTimeline();
            //max
            responseTimeline = actionsTimeline.GetHomeTimeline(10, 264858936895754240);
            //since
            responseTimeline = actionsTimeline.GetHomeTimeline(264858936895754240);
            

            #endregion
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            OAuthTokens tokens = (OAuthTokens)Session["OAuthTokens"];

            TweetActions tweet = new TweetActions(tokens);

            TwitterStatus status=tweet.UpdateStatus(txtStatus.Text);
            
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            OAuthTokens tokens = (OAuthTokens)Session["OAuthTokens"];
            FavoriteActions favorite = new FavoriteActions(tokens);
            TwitterStatus status = favorite.DestroyFavorite(txtIdTweet.Text);
            if (TwitterStatus.Success == status)
            {
                //success
            }
        }

    }
}