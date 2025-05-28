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

                    Console.Write("Apakah kamu ingin mencari data? (y/n): ");
                    string confirm1 = Console.ReadLine().Trim().ToLower();
                    if (confirm1 == "y")
                    {
                        Console.WriteLine("------------------------------");

                        Console.Write("Enter keyword to search: ");
                        var searchKeyword = InputStream.GetString(Console.ReadLine());

                        await FileRepository.SearchData(searchKeyword);

                        Console.WriteLine("------------------------------");
                    }
                    else
                    {
                        var data = await FileRepository.GetData();

                        if (data.Count != 0)
                        {
                            Console.WriteLine("==================================================================================================================");
                            Console.WriteLine("| No |         File Name         | File Type | File Size |   Created At   |     Modified At     |");
                            Console.WriteLine("==================================================================================================================");

                            for (int i = 0; i < data.Count; i++)
                            {
                                var item = data[i];
                                Console.WriteLine($"| {i + 1,2} | {item.filename,-25} | {item.file_type,-9} | {item.size,9} | {item.created_at,-15} | {item.modified_at ?? "Data Tidak ada",-20} |");
                            }

                            Console.WriteLine("==================================================================================================================");
                        } else
                        {
                            Console.WriteLine("------------------------------");

                            Console.WriteLine("Data Kosong");

                            Console.WriteLine("------------------------------");
                        }

                        
                    }

                    break;
                case 2:
                    Console.WriteLine("Feature: Update File Metadata");

                    var updateId = await FileRepository.SearchDataForId();
                    if (updateId != null)
                    {
                        Console.WriteLine($"Ready to update file with ID: {updateId}");
                        await FileRepository.RenameFileMetadata(updateId);
                    }

                    break;

                case 3:
                    Console.WriteLine("Feature: Delete File");

                    var deleteId = await FileRepository.SearchDataForId();

                    if (deleteId != null)
                    {
                        Console.WriteLine($"Ready to delete file with ID: {deleteId}");

                        Console.Write("Are you sure you want to delete this file? (y/n): ");
                        string confirm2 = Console.ReadLine().Trim().ToLower();
                        if (confirm2 == "y")
                        {
                            await FileRepository.DeleteFileById(deleteId);
                        }
                        else
                        {
                            Console.WriteLine("Delete operation cancelled.");
                        }
                    }

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