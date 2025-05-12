using ConsoleProject.LayoutServices;
using ConsoleProject.Repository;
using System;

public class Program
{
    public static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            PrintHeader();
            PrintMenuBody();
            PrintFooter();

            int choice = InputStream.GetInt("Select an option: ");

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Feature: Display All Metadata");

                    var data = await FileRepository.GetData();

                    Console.WriteLine("==================================================================================================================");
                    Console.WriteLine("| No |         File Name         | File Type | File Size |   Created At   |     Modified At     |");
                    Console.WriteLine("==================================================================================================================");

                    for (int i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        Console.WriteLine($"| {i + 1,2} | {item.filename,-25} | {item.file_type,-9} | {item.size,9} | {item.created_at,-15} | {item.modified_at ?? "Data Tidak ada",-20} |");
                    }

                    Console.WriteLine("==================================================================================================================");


                    break;
                case 2:
                    Console.WriteLine("Feature: Update File Metadata");

                    Console.Write("Enter The ID to change data : ");

                    var input = InputStream.GetString(Console.ReadLine()).ToLower();

                    

                    break;
                case 3:
                    Console.WriteLine("Feature: Delete File");
                    break;
                case 4:
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void PrintHeader()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("     FILE MANAGEMENT CONSOLE APP");
        Console.WriteLine("=======================================");
    }

    static void PrintMenuBody()
    {
        Console.WriteLine("1. Display All File Metadata");
        Console.WriteLine("2. Update File Metadata");
        Console.WriteLine("3. Delete File");
        Console.WriteLine("4. Exit");
        Console.WriteLine("---------------------------------------");
    }

    static void PrintFooter()
    {
        Console.WriteLine("Note: Please enter a number from 1 to 4.");
        Console.WriteLine("---------------------------------------");
    }
}