using System;
using System.Collections.Generic;
using System.IO;

public class Album
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public double Duration { get; set; }
    public string Performers { get; set; }
}

public class Track
{
    public int AlbumId { get; set; }
    public string Title { get; set; }
    public double Duration { get; set; } 
    public string Composer { get; set; }
}

public class MusicLibrary
{
    public List<Album> Albums { get; set; } = new List<Album>();
    public List<Track> Tracks { get; set; } = new List<Track>();

    public void AddAlbum(Album album)
    {
        Albums.Add(album);
    }

    public void AddTrack(Track track)
    {
        Tracks.Add(track);
    }

    public void SaveToSql(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine("CREATE TABLE IF NOT EXISTS Albums (");
            writer.WriteLine("    Id INT PRIMARY KEY,");
            writer.WriteLine("    Title VARCHAR(255),");
            writer.WriteLine("    Type VARCHAR(10),");
            writer.WriteLine("    Duration DOUBLE,");
            writer.WriteLine("    Performers TEXT");
            writer.WriteLine(");");
            writer.WriteLine();
            writer.WriteLine("CREATE TABLE IF NOT EXISTS Tracks (");
            writer.WriteLine("    AlbumId INT,");
            writer.WriteLine("    Title VARCHAR(255),");
            writer.WriteLine("    Duration DOUBLE,");
            writer.WriteLine("    Composer VARCHAR(255),");
            writer.WriteLine("    FOREIGN KEY (AlbumId) REFERENCES Albums(Id)");
            writer.WriteLine(");");
            writer.WriteLine();

            foreach (var album in Albums)
            {
                writer.WriteLine($"INSERT INTO Albums (Id, Title, Type, Duration, Performers) VALUES ({album.Id}, '{album.Title}', '{album.Type}', {album.Duration}, '{album.Performers}');");
            }

            foreach (var track in Tracks)
            {
                writer.WriteLine($"INSERT INTO Tracks (AlbumId, Title, Duration, Composer) VALUES ({track.AlbumId}, '{track.Title}', {track.Duration}, '{track.Composer}');");
            }
        }
    }

    public static MusicLibrary LoadFromSql(string filename)
    {
        
        return new MusicLibrary();
    }
}

class Program
{
    static void Main(string[] args)
    {
        MusicLibrary library = new MusicLibrary();

        while (true)
        {
            Console.WriteLine("1. Add Album");
            Console.WriteLine("2. Add Track");
            Console.WriteLine("3. Display All Albums");
            Console.WriteLine("4. Display Album Details");
            Console.WriteLine("5. Display Track Details");
            Console.WriteLine("6. Save to SQL");
            Console.WriteLine("7. Load from SQL");
            Console.WriteLine("0. Exit");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddAlbum(library);
                    break;
                case 2:
                    AddTrack(library);
                    break;
                case 3:
                    DisplayAllAlbums(library);
                    break;
                case 4:
                    DisplayAlbumDetails(library);
                    break;
                case 5:
                    DisplayTrackDetails(library);
                    break;
                case 6:
                    SaveToSql(library);
                    break;
                case 7:
                    library = LoadFromSql();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }

    static void AddAlbum(MusicLibrary library)
    {
        Album album = new Album();
        Console.Write("Enter album ID: ");
        album.Id = int.Parse(Console.ReadLine());
        Console.Write("Enter album title: ");
        album.Title = Console.ReadLine();
        Console.Write("Enter album type (CD/DVD): ");
        album.Type = Console.ReadLine();
        Console.Write("Enter album duration (minutes): ");
        album.Duration = double.Parse(Console.ReadLine());
        Console.Write("Enter performers (comma-separated): ");
        album.Performers = Console.ReadLine();

        library.AddAlbum(album);
        Console.WriteLine("Album added successfully.");
    }

    static void AddTrack(MusicLibrary library)
    {
        Track track = new Track();
        Console.Write("Enter album ID: ");
        track.AlbumId = int.Parse(Console.ReadLine());
        Console.Write("Enter track title: ");
        track.Title = Console.ReadLine();
        Console.Write("Enter track duration (minutes): ");
        track.Duration = double.Parse(Console.ReadLine());
        Console.Write("Enter composer: ");
        track.Composer = Console.ReadLine();

        library.AddTrack(track);
        Console.WriteLine("Track added successfully.");
    }

    static void DisplayAllAlbums(MusicLibrary library)
    {
        var albums = library.Albums;
        if (albums.Count > 0)
        {
            foreach (var album in albums)
            {
                Console.WriteLine($"ID: {album.Id}, Title: {album.Title}, Type: {album.Type}, Duration: {album.Duration} minutes, Performers: {album.Performers}");
            }
        }
        else
        {
            Console.WriteLine("No albums found.");
        }
    }

    static void DisplayAlbumDetails(MusicLibrary library)
    {
        Console.Write("Enter album ID: ");
        int albumId = int.Parse(Console.ReadLine());
        var album = library.Albums.Find(a => a.Id == albumId);

        if (album != null)
        {
            Console.WriteLine($"Title: {album.Title}, Type: {album.Type}, Duration: {album.Duration} minutes, Performers: {album.Performers}");
        }
        else
        {
            Console.WriteLine("Album not found.");
        }
    }

    static void DisplayTrackDetails(MusicLibrary library)
    {
        Console.Write("Enter album ID: ");
        int albumId = int.Parse(Console.ReadLine());
        Console.Write("Enter track title: ");
        string trackTitle = Console.ReadLine();

        var tracks = library.Tracks.FindAll(t => t.AlbumId == albumId && t.Title == trackTitle);

        if (tracks.Count > 0)
        {
            foreach (var track in tracks)
            {
                Console.WriteLine($"Title: {track.Title}, Duration: {track.Duration} minutes, Composer: {track.Composer}");
            }
        }
        else
        {
            Console.WriteLine("Track not found.");
        }
    }

    static void SaveToSql(MusicLibrary library)
    {
        Console.Write("Enter filename to save: ");
        string filename = Console.ReadLine();
        library.SaveToSql(filename);
        Console.WriteLine("Library saved successfully.");
    }

    static MusicLibrary LoadFromSql()
    {
        
        return new MusicLibrary();
    }
}
