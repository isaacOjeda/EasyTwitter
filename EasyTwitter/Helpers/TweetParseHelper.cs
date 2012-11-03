using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyTwitter.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;

namespace EasyTwitter.Helpers
{
    public class TweetParseHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="twitterResponse"></param>
        /// <returns></returns>
        public static TwitterResponse<List<Tweet>> ParseTweetsCollection(TwitterResponse<string> twitterResponse)
        {
            if (twitterResponse.Status == TwitterStatus.Success)
            {
                JArray json = JArray.Parse(twitterResponse.ObjectResponse);

                List<Tweet> listFavourited =
                    (
                        from tweets in json.AsEnumerable()
                        select new Tweet
                        {
                            Id = (decimal)tweets["id"],
                            Text = (string)tweets["text"],
                            CreatedAt = DateTime.ParseExact(tweets["created_at"].ToString(), "ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture),
                            User = new TwitterUser
                            {
                                Id = (int)tweets["user"]["id"],
                                Description = (string)tweets["user"]["description"],
                                FavouritesCount = (int)tweets["user"]["favourites_count"],
                                FollowersCount = (int)tweets["user"]["followers_count"],
                                Location = (string)tweets["user"]["location"],
                                Name = (string)tweets["user"]["name"],
                                ProfileImageUrl = (string)tweets["user"]["profile_image_url"],
                                ScreenName = (string)tweets["user"]["screen_name"]
                            },
                            Entities = new TweetEntities
                            {
                                Urls =
                                (
                                        from url in tweets["entities"]["urls"].AsEnumerable()
                                        select new EntitieUrl
                                        {
                                            DisplayUrl = (string)url["url"],
                                            ExpandedUrl = (string)url["display_url"],
                                            Url = (string)url["expanded_url"]
                                        }
                                ).ToList<EntitieUrl>()
                            }
                        }
                    ).ToList<Tweet>();
                return new TwitterResponse<List<Tweet>>
                {
                    ObjectResponse = listFavourited,
                    Status = TwitterStatus.Success
                };
            }
            else
            {
                return new TwitterResponse<List<Tweet>>
                {
                    ObjectResponse = null,
                    Status = TwitterStatus.GeneralError
                };
            }
        }
    }
}
