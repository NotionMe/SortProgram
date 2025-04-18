﻿using Avalonia;
using System;
using System.Globalization; // Додаємо для роботи з CultureInfo
using System.Threading;    // Додаємо для доступу до Thread
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Установлюємо мову за замовчуванням на en-US
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        
        // Конвертуємо паролі у файлі users.json з відкритого тексту на хеші
        ConvertExistingPasswordsToHashed();

        // Запускаємо Avalonia-додаток
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Метод для конвертації існуючих паролів в хешовані
    private static void ConvertExistingPasswordsToHashed()
    {
        try
        {
            Console.WriteLine("Checking for passwords that need to be hashed...");
            UserRepository userRepository = new UserRepository();
            userRepository.UpdatePasswordsToHashed();
            Console.WriteLine("Password migration completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during password migration: {ex.Message}");
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
