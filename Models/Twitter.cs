using System;
using System.IO;
using System.Net;
using System.Configuration;
using System.Drawing;

using CoreTweet;

using Neptune.Enum;
using static System.Console;
using System.Windows.Media.Imaging;

namespace Neptune.Models
{
    /// <summary>
    /// Twitter ; 認証～画像取得までのプロセスを援助します。
    /// </summary>
    public class Twitter
    {
        #region 認証パラメータ
        private const int       ARY_SIZE = 4;
        private static string[] Authorize;

        private static string   consumer_key;
        private static string   consumer_key_secret;
        private static string   access_token;
        private static string   access_token_secret;
        #endregion

        // Profile画像取得APIのアドレス
        private static readonly string PROFILE_IMAGE_URL =
            "http://api.twitter.com/1/users/profile_image/";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Twitter(){ }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public void Initialize()
        {
            // 配列の初期化
            Authorize       = new string[ ARY_SIZE] { null, null, null, null };
                  
            // app.config から情報取得
            var appSettings = ConfigurationManager.AppSettings.AllKeys;

            try
            {
                // app.config : key の要素分だけループ
                for ( int keyElements = 0; keyElements < appSettings.Length; keyElements++ )
                {
                    // 各 key.value の値を配列に格納する
                    Authorize[ keyElements] = ConfigurationManager.AppSettings[ keyElements];

                    // 配列から各認証パラメータメンバに対して格納する
                    switch ( keyElements )
                    {
                        case 0: consumer_key        = Authorize[ ( int ) SwitchType.CONSUMER_KEY ];   break;
                        case 1: consumer_key_secret = Authorize[ ( int ) SwitchType.CONSUMER_KEY_S ]; break;
                        case 2: access_token        = Authorize[ ( int ) SwitchType.ACCESS_TOKEN ];   break;
                        case 3: access_token_secret = Authorize[ ( int ) SwitchType.ACCESS_TOKEN_S ]; break;
                        default: throw new Exception();
                    }
                }
            }
            catch ( Exception ex )
            {
                WriteLine( ex.StackTrace );
            }

        }

        /// <summary>
        /// Twitter に何かしら発言をしたい場合、このメソッドを経由する。
        /// </summary>
        public bool Tweet()
        {
            // API にアクセスするためのトークン群
            var tokens = Tokens.Create(
                $"{consumer_key}",
                $"{consumer_key_secret}",
                $"{access_token}",
                $"{access_token_secret}"
            );

            // Tweet 用に文字列を生成する
            string Mix = "Neptune Twitter API Access Test.";

            try
            {
                tokens.Statuses.Update(
                    new
                    {
                        status = Mix
                    }
                );

                Console.WriteLine($"{Mix}");
            }

            // ツイートが重複した際に catch する。
            catch (Exception ex)
            {
                string msg = $"Neptune is Tweet Overlapped.";

                Console.WriteLine(ex.Message);
                tokens.Statuses.Update(new { status = msg });

                return false;
            }

            return true;
        }

        /// <summary>
        /// ProfileImage を取得する
        /// ここは、まだ実装途中。
        /// </summary>
        /// <param name="screen_name">Profile Imageを取得するID(screen_name)</param>
        /// <param name="image_size">Profile Imageのサイズ</param>
        /// <returns>取得した画像</returns>
        public BitmapImage GetProfileImage( string screen_name, ImageSize image_size )
        {
            // サイズ指定テキストの定義
            string[] image_size_string = { "minimum", "normal", "bigger" };

            // 画像のURLを取得
            WebRequest api_req = WebRequest.Create( PROFILE_IMAGE_URL + 
                                                    screen_name       + 
                                                    ".xml"            +
                                                    "?size = "        + 
                                                    image_size_string[ ( int )image_size ]
                                                   );

            WebResponse api_res = api_req.GetResponse();

            // 画像を取得
            WebRequest  image_req = WebRequest.Create( api_res.ResponseUri );
            WebResponse image_res = image_req.GetResponse();

            // BITMAPに変換
            Stream stream = image_res.GetResponseStream();
            BitmapImage bitmap = new BitmapImage( stream );

            // 後始末
            stream.Close();
            image_res.Close();
            api_res.Close();

            return ( bitmap );
        }
    }
}