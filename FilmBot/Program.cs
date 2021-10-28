using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace FilmBot
{
    class Program
    {
        private static string token { get; set; } = "2034602500:AAE_S3cs8E3ZTxglygveyevYGXg3N-ATapg";
        private static TelegramBotClient client;

        static string FilmName;

        static string answerOnFilm;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();

        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if ((msg.Text != null) & (msg.Type == Telegram.Bot.Types.Enums.MessageType.Text))
            {
                Console.WriteLine($"Message come: {msg.Text}");

                DB FilmData = new DB();

                string sql;

                switch (msg.Text)
                {
                    case "Поиск фильма":

                        var stick = client.SendStickerAsync(
                        chatId: msg.Chat.Id,
                        sticker: "https://tlgrm.ru/_/stickers/8a1/9aa/8a19aab4-98c0-37cb-a3d4-491cb94d7e12/3.webp",
                        replyToMessageId: msg.MessageId);

                        answerOnFilm = "Напишите название фильма";
                        await client.SendTextMessageAsync(msg.Chat.Id, answerOnFilm, replyMarkup: GetButtons());
                        
                        break;

                    case "Год выпуска":

                        sql = $"SELECT `name`,`year` FROM `filmdescription` WHERE name LIKE '{FilmName}%'";
                        answerOnFilm = FilmData.SelectFromBD(sql) + " года выпуска";
                        await client.SendTextMessageAsync(msg.Chat.Id, answerOnFilm, replyMarkup: GetButtons2());

                        break;

                    case "Оценка кинопоиск":

                        sql = $"SELECT `name`,`starskinopoisk` FROM `evaluation` WHERE name LIKE '{FilmName}%'";
                        answerOnFilm = FilmData.SelectFromBD(sql);
                        await client.SendTextMessageAsync(msg.Chat.Id, answerOnFilm, replyMarkup: GetButtons2());

                        break;

                    case "Оценка IMDb":

                        sql = $"SELECT `name`,`starsIMDb` FROM `evaluation` WHERE name LIKE '{FilmName}%'";
                        answerOnFilm = FilmData.SelectFromBD(sql);
                        await client.SendTextMessageAsync(msg.Chat.Id, answerOnFilm, replyMarkup: GetButtons2());

                        break;

                    default:
                        try
                        {
                            sql = $"SELECT `name`,`descript` FROM `filmdescription` WHERE name LIKE '{msg.Text}%'";
                            answerOnFilm = FilmData.SelectFromBD(sql);
                            FilmName = msg.Text;
                            await client.SendTextMessageAsync(msg.Chat.Id, answerOnFilm, replyMarkup: GetButtons2());
                        }
                        catch (Exception)
                        {
                            answerOnFilm = "Данного фильма нет в БД или возникла проблема подключения к БД";
                            await client.SendTextMessageAsync(msg.Chat.Id, answerOnFilm, replyMarkup: GetButtons2());
                            return;
                        }
                        break;

                }
                
                
                
            }
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Гнев человеческий" }, new KeyboardButton { Text = "Дюна" } },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Джентльмены" }, new KeyboardButton { Text = "Майор Гром: Чумной Доктор" } }
                }
            };
        }

        private static IReplyMarkup GetButtons2()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Поиск фильма" }, new KeyboardButton { Text = "Год выпуска" } },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Оценка кинопоиск" }, new KeyboardButton { Text = "Оценка IMDb" } }
                }
            };
        }
    }
}

