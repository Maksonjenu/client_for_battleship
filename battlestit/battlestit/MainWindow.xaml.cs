using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace battlestit
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

       


        int n = 0;

        public string message;
        const int port = 25565;
        const string address = "127.0.0.1";
        //объявление TCP клиента
        TcpClient client = null;
        NetworkStream stream;
        Thread myThread1;
        public void Btn_Click(object sender, RoutedEventArgs e)
        {

            //userName = "p1";
            //получение значения лежащего в Tag
             n = (int)((Button)sender).Tag;
            //установка фона нажатой кнопки, цвета и размера шрифта
            ((Button)sender).Background = Brushes.Red;
            ((Button)sender).Foreground = Brushes.Black;
            ((Button)sender).FontSize = 8;
            //запись в нажатую кнопку её номера
            ((Button)sender).Content = n.ToString();
            //  Thread myThread1 = new Thread(new ThreadStart(Count1));  тред на получение данных 
            //  myThread1.Start();
           client = new TcpClient(address, port);
            //получение потока для обмена сообщениями
            stream = client.GetStream();

              myThread1 = new Thread(new ThreadStart(Count1));
              myThread1.Start();


            message = n.ToString();

            //преобразование сообщение в массив байтов
            byte[] data = Encoding.Unicode.GetBytes(message);
            //отправка сообщения
            stream.Write(data, 0, data.Length);

        }

        public MainWindow()
        {
            InitializeComponent();
           
            ugr.Rows = 10;
            ugr.Columns = 10;
           
            //указываются размеры сетки (число ячеек * (размер кнопки в ячейки + толщина её границ))
            // ugr.Width = 10 * (40 + 4);
            // ugr.Height = 10 * (40 + 4);
            //толщина границ сетки
            // ugr.Margin = new Thickness(-456, 10, 458, -2);
            for (int i = 1; i < 101; i++)
            {
                //создание кнопки
                Button btn = new Button();
                //запись номера кнопки
                btn.Tag = i;
                //установка размеров кнопки
                btn.Width = 40;
                btn.Height = 40;
                //текст на кнопке
                btn.Content = i;
                //толщина границ кнопки
                btn.Margin = new Thickness(1);
                //при нажатии кнопки, будет вызываться метод Btn_Click
                btn.Click += Btn_Click;
                //добавление кнопки в сетку
                ugr.Children.Add(btn);

            }
            ugr1.Rows = 10;
            ugr1.Columns = 10;
            //указываются размеры сетки (число ячеек * (размер кнопки в ячейки + толщина её границ))
            // ugr1.Width = 10 * (40 + 4);
            // ugr1.Height = 10 * (40 + 4);
            //толщина границ сетки
            //  ugr1.Margin = new Thickness(2, 2, 2, 2);
            for (int i = 1; i < 101; i++)
            {
                //создание кнопки
                Button btn1 = new Button();
                //запись номера кнопки
                btn1.Tag = i ;
                //установка размеров кнопки
                btn1.Width = 40;
                btn1.Height = 40;
                //текст на кнопке
                btn1.Content = i;
                //толщина границ кнопки
                btn1.Margin = new Thickness(1);
                //при нажатии кнопки, будет вызываться метод Btn_Click
                btn1.Click += Btn_Click;
                //добавление кнопки в сетку
                ugr1.Children.Add(btn1);

            }
        }
        //private void ReceiveMessages()
        //{
        //    alive = true;
        //    try
        //    {
        //        while (alive)
        //        {
        //            IPEndPoint remoteIp = null;
        //            byte[] data = client.Receive(ref remoteIp);
        //            string message = Encoding.Unicode.GetString(data);

        //            // добавляем полученное сообщение в текстовое поле

        //        }
        //    }
        //    catch (ObjectDisposedException)
        //    {
        //        if (!alive)
        //            return;
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        private void Count1()
        {

            try
            {
                while (true)
                {
                    //буфер для получаемых данных
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    //до тех пор пока есть данные в потоке
                    do
                    {
                        //получение 64 байт
                        bytes = stream.Read(data, 0, data.Length);
                        //формирование строки
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    message = builder.ToString();
                    //stream.Close();
                }
            }

            catch (Exception ex)
            {
                stream.Close();
            }
            finally
            {

                client.Close();
            }

        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            message = "fkthisshtimout";

            //преобразование сообщение в массив байтов
            byte[] data = Encoding.Unicode.GetBytes(message);
            //отправка сообщения
            stream.Write(data, 0, data.Length);



            if (client != null)
                client.Close();
            if (stream != null)
                stream.Close();
        }
    }
}
