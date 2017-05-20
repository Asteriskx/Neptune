using System;
using System.IO;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Input;

using CoreTweet;
using static System.Console;


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
                        case 0: consumer_key        = Authorize[ keyElements]; break;
                        case 1: consumer_key_secret = Authorize[ keyElements]; break;
                        case 2: access_token        = Authorize[ keyElements]; break;
                        case 3: access_token_secret = Authorize[ keyElements]; break;
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
    }
}
