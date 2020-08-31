using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RhythmsGonnaGetYou;

namespace RhythmsGonnaGetYou
{


    class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsExplicit { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int BandId { get; set; }

        public Band Band { get; set; }
    }
    class Band
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        public int NumberofMembers { get; set; }
        public string Website { get; set; }
        public string Style { get; set; }
        public bool IsSigned { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
    }



    class SuncoastRecordsContext : DbContext
    {
        public DbSet<Band> Bands { get; set; }
        public DbSet<Album> Album { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseNpgsql("server=localhost;database=SuncoastRecords");

        }

    }
    class Program
    {
        static void Main(string[] args)
        {

            var whileRunning = true;
            while (whileRunning == true)
            {
                var context = new SuncoastRecordsContext();
                var theAlbums = context.Album.Include(Album => Album.Band);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("--------Welcome to Suncoast Records!--------");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("What would you like to do?:");
                Console.WriteLine("Add band");
                Console.WriteLine("View bands");
                Console.WriteLine("Add album");
                Console.WriteLine("Let band go");
                Console.WriteLine("Sign band");
                Console.WriteLine("View band's albums");
                Console.WriteLine("View all albums");
                Console.WriteLine("View signed bands");
                Console.WriteLine("View unsigned bands");
                Console.WriteLine("Quit");
                Console.Write("Choice: ");
                var choice = Console.ReadLine();
                if (choice == "Add band")
                {
                    Console.Write("Name of the Band?(required): ");
                    var name = Console.ReadLine();
                    Console.Write("Country of Origin?(required): ");
                    var countryOfOrigin = Console.ReadLine();
                    Console.Write("How many members?: ");
                    var numberOfMembers = int.Parse(Console.ReadLine());
                    Console.Write("What's their website?: ");
                    var website = Console.ReadLine();
                    Console.Write("What style of music are they?: ");
                    var style = Console.ReadLine();
                    Console.Write("Are they signed? true or false: ");
                    var isSigned = bool.Parse(Console.ReadLine());
                    Console.Write("Who do we Contact?: ");
                    var contactName = Console.ReadLine();
                    Console.Write("What is their contact number?:  ");
                    var contactNumber = Console.ReadLine();

                    var newBand = new Band
                    {
                        Name = name,
                        CountryOfOrigin = countryOfOrigin,
                        NumberofMembers = numberOfMembers,
                        Website = website,
                        Style = style,
                        IsSigned = isSigned,
                        ContactName = contactName,
                        ContactPhoneNumber = contactNumber,
                    };
                    context.Bands.Add(newBand);
                    context.SaveChanges();

                }
                if (choice == "View bands")
                {
                    foreach (var theBand in theAlbums)

                    {
                        Console.WriteLine($"There is a band named {theBand.Band.Name}");
                    }
                }
                if (choice == "Add album")
                {
                    Console.Write("Name of Band: ");
                    var bandName = Console.ReadLine();
                    var foundBand = context.Bands.FirstOrDefault(bands => bands.Name == bandName);
                    if (foundBand != null)
                        Console.Write("Title of the album?: ");
                    var title = Console.ReadLine();
                    Console.Write("Is it explicit?  true or false: ");
                    var isExplicit = bool.Parse(Console.ReadLine());
                    Console.Write("When was it released?(YYYY-MM-DD): ");
                    var releaseDate = DateTime.Parse(Console.ReadLine());
                    var newID = context.Bands.Max(band => band.Id);

                    var newAlbum = new Album
                    {
                        Title = title,
                        IsExplicit = isExplicit,
                        ReleaseDate = releaseDate,
                        BandId = newID,

                    };

                    context.Album.Add(newAlbum);
                    context.SaveChanges();
                }
                if (choice == "Let band go")
                {
                    Console.Write("Name: ");
                    var bandName = Console.ReadLine();
                    var foundBand = context.Bands.FirstOrDefault(bands => bands.Name == bandName);
                    if (foundBand != null)
                    {
                        Console.Write("Are you sure you want to let this band go? Yes or No: ");
                        var unsignChoice = Console.ReadLine();
                        if (unsignChoice == "Yes")
                        {
                            if (foundBand.IsSigned == false)
                            {
                                Console.WriteLine($"Sorry {foundBand.Name} is already unsigned.");
                            }
                            else
                            {
                                foundBand.IsSigned = false;
                                Console.WriteLine($"{foundBand.Name} has been let go.");
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Returning to Menu...");
                        }

                    }



                }
                if (choice == "Sign band")
                {
                    Console.Write("Name: ");
                    var bandName = Console.ReadLine();
                    var foundBand = context.Bands.FirstOrDefault(bands => bands.Name == bandName);
                    if (foundBand != null)
                    {
                        Console.Write("Are you sure you want to sign ths band? Yes or No: ");
                        var signChoice = Console.ReadLine();
                        if (signChoice == "Yes")
                        {
                            if (foundBand.IsSigned == true)
                            {
                                Console.WriteLine($"Sorry {foundBand.Name} is already signed.");
                            }
                            else
                            {
                                foundBand.IsSigned = true;
                                Console.WriteLine($"{foundBand.Name} has been signed on!");
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Returning to Menu...");
                        }

                    }
                }
                if (choice == "View band's albums")
                {
                    Console.Write("Name of Band: ");
                    var bandName = Console.ReadLine();
                    var foundBand = context.Album.FirstOrDefault(album => album.Band.Name == bandName);
                    if (foundBand != null)
                    {
                        var foundAlbums = context.Album.Where(album => album.Band.Name == bandName && album.BandId == album.Band.Id);
                        foreach (var album in foundAlbums)
                        {
                            Console.WriteLine($"There is an album called {album.Title} by this band");
                        }
                    }
                }
                if (choice == "View all albums")
                {
                    var releaseDate = theAlbums.OrderBy(theBands => theBands.ReleaseDate);
                    foreach (var theBand in releaseDate)
                    {

                        Console.WriteLine($"The is an album called {theBand.Title}. It was released {theBand.ReleaseDate} ");
                    }
                }
                if (choice == "View signed bands")
                {
                    foreach (var theBand in theAlbums)
                    {
                        if (theBand.Band.IsSigned == true)
                        {
                            Console.WriteLine($"There is a signed band named {theBand.Band.Name}.");
                        }
                    }
                }
                if (choice == "View unsigned bands")
                {
                    foreach (var theBand in theAlbums)
                    {
                        if (theBand.Band.IsSigned == false)
                        {
                            Console.WriteLine($"There is an unsigned band named {theBand.Band.Name}.");
                        }

                    }
                }
                if (choice == "Quit")
                {
                    whileRunning = false;
                }
            }
            Console.WriteLine("....Have a good day....");
        }
    }
}
