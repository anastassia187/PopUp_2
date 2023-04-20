using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace PopUp_2
{
    public partial class MainPage : ContentPage
    {
        private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(); // массив букв алфавита

        public MainPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.LightCoral;
        }

        async Task SaveDataToFileAsync(string fileName, string data)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, fileName);

            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                await sw.WriteAsync(data).ConfigureAwait(false);
            }
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, "MultiplicationTable.txt");

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    await DisplayAlert("Успешно", $"Файл успешно удален", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", $"Ошибка при удалении файла: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл не найден", "OK");
            }
        }

        async void OnDeleteAlphabetTestButtonClicked(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, "AlphabetTest.txt");

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    await DisplayAlert("Успешно", $"Файл успешно удален", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", $"Ошибка при удалении файла: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл не найден", "OK");
            }
        }



        async void OnCheckButtonClicked(object sender, EventArgs e)
        {
            // Получаем имя пользователя
            string name = await DisplayPromptAsync("Введите ваше имя", "");

            // Получаем выбранный множитель
            string[] pages = { "умножаем на 1", "умножаем на 2", "умножаем на 3", "умножаем на 4", "умножаем на 5", "умножаем на 6", "умножаем на 7", "умножаем на 8", "умножаем на 9" };
            string page = await DisplayActionSheet("Выберите множитель", "Отмена", null, pages);

            // Получаем введенное пользователем число
            string input = await DisplayPromptAsync("Введите число от 1 до 10", "");

            // Проверяем правильность ввода и отображаем соответствующую страницу
            if (int.TryParse(input, out int number) && number >= 1 && number <= 10)
            {
                int multiplier = Array.IndexOf(pages, page) + 1;
                int result = number * multiplier;
                await DisplayAlert("Ответ", $"{number} x {multiplier} = {result}", "OK");

                await SaveDataToFileAsync("MultiplicationTable.txt", $"{number} x {multiplier} = {result}\n");
            }
            else
            {
                await DisplayAlert("Ошибка", "Введите число от 1 до 10", "OK");
                return; // выходим из функции, если введенное число не подходит
            }
            // Предлагаем что-либо выполнить
            bool answer = await DisplayAlert("Повторить?", "Хотите повторить тест?", "Да", "Нет");
            if (answer)
            {
                // Если пользователь хочет повторить тест
                OnCheckButtonClicked(sender, e);
            }
        }

        async void OnCheckAlphabetClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Проверка знания алфавита", "Нажмите OK, чтобы начать", "OK");

            bool repeat = true;
            while (repeat)
            {
                // Вывод случайной буквы алфавита
                Random random = new Random();
                char letter = alphabet[random.Next(alphabet.Length)];

                // Получение ввода пользователя
                string input = await DisplayPromptAsync($"Введите букву после {letter}", "");

                // Проверка правильности ввода
                if (string.IsNullOrEmpty(input))
                {
                    await DisplayAlert("Ошибка", "Вы не ввели букву", "OK");
                }
                else if (char.ToUpper(input[0]) == letter + 1)
                {
                    await DisplayAlert("Правильно", $"Буква {input.ToUpper()} следует за буквой {letter}", "OK");
                    await SaveDataToFileAsync("AlphabetTest.txt", $"Правильно: Буква {input.ToUpper()} следует за буквой {letter}\n");
                }
                else
                {
                    await DisplayAlert("Неправильно", $"Буква {input.ToUpper()} не следует за буквой {letter}", "OK");
                    await SaveDataToFileAsync("AlphabetTest.txt", $"Неправильно: Буква {input.ToUpper()} не следует за буквой {letter}\n");
                }

                // Предложение повторить тест
                repeat = await DisplayAlert("Повторить?", "Хотите повторить тест?", "Да", "Нет");
            }
        }

        async void OnShowMultiplicationTableResults(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, "MultiplicationTable.txt");

            if (File.Exists(fullPath))
            {
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    string content = await sr.ReadToEndAsync();
                    await DisplayAlert("Результаты таблицы умножения", content, "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл с результатами не найден", "OK");
            }
        }
        async void OnShowAlphabetResults(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, "AlphabetTest.txt");

            if (File.Exists(fullPath))
            {
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    string content = await sr.ReadToEndAsync();
                    await DisplayAlert("Результаты проверки знания алфавита", content, "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл с результатами не найден", "OK");
            }
        }
        
    }
    }

