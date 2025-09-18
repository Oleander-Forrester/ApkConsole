using System;
using System.Collections.Generic;

public abstract class KontenAudio
{
    private int jumlahPemutaran;

    public string Judul { get; private set; }
    public int DurasiDetik { get; private set; }
    public int JumlahPemutaran
    {
        get { return jumlahPemutaran; }
    }

    public KontenAudio(string judul, int durasiDetik)
    {
        this.Judul = judul;
        this.DurasiDetik = durasiDetik;
        this.jumlahPemutaran = 0; //Jumlah pemutaran awal 0
    }

    protected void IncrementJumlahPemutaran()
    {
        jumlahPemutaran++;
    }

    public abstract void Putar();
}

public class Lagu : KontenAudio
{
    public Artis Artis { get; private set; }
    public Album Album { get; private set; }

    public Lagu(string judul, int durasiDetik, Artis artis, Album album)
        : base(judul, durasiDetik)
    {
        this.Artis = artis;
        this.Album = album;
    }

    public override void Putar()
    {
         string namaArtis = this.Artis.NamaArtis;
    
    // Cek dulu apakah albumnya ada atau tidak (untuk kasus lagu single)
    string namaAlbum = (this.Album != null) ? this.Album.NamaAlbum : "Single";

    // Gabungkan semuanya ke dalam string output
    Console.WriteLine($"Memutar lagu: '{this.Judul}' oleh {namaArtis} dari album '{namaAlbum}'.");
    
    IncrementJumlahPemutaran();
    }

    public void Putar(bool tampilkanLirik)
    {
        Putar();
        if (tampilkanLirik)
        {
            Console.WriteLine($"Menampilkan lirik untuk lagu: {Judul}");
        }
    }
}

public class Podcast : KontenAudio
{
    public string NamaHost { get; private set; }
    public int Episode { get; private set; }

    public Podcast(string judul, int durasi, string host, int episode) : base(judul, durasi)
    {
        this.NamaHost = host;
        this.Episode = episode;
    }

    public override void Putar()
    {
        Console.WriteLine($"🎙️ Memutar podcast: '{this.Judul}' - Ep. {this.Episode} oleh {this.NamaHost}");
        IncrementJumlahPemutaran();
    }
}

public class Artis
{
    public string NamaArtis { get; private set; }
    public List<Album> DaftarAlbum { get; private set; } // COMPOSITION

    public Artis(string nama)
    {
        this.NamaArtis = nama;
        this.DaftarAlbum = new List<Album>();
    }

    public void TambahAlbum(Album album)
    {
        DaftarAlbum.Add(album);
    }
}

public class Album
{
    public string NamaAlbum { get; private set; }
    public int TahunRilis { get; private set; }
    public List<Lagu> DaftarLagu { get; private set; } // COMPOSITION

    public Album(string nama, int tahun)
    {
        this.NamaAlbum = nama;
        this.TahunRilis = tahun;
        this.DaftarLagu = new List<Lagu>();
    }

    public void TambahLagu(Lagu lagu)
    {
        DaftarLagu.Add(lagu);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== Selamat Datang di Console Music Player by Jeki =====\n");

        // 1. ABSTRACTION
        Artis queen = new Artis("Queen");
        Album aNightAtTheOpera = new Album("A Night at the Opera", 1975);
        queen.TambahAlbum(aNightAtTheOpera);
        Album theGame = new Album("The Game", 1979);
        queen.TambahAlbum(theGame);

        Artis firehouse = new Artis("Firehouse");

        // 2. CONSTRUCTOR
        Lagu bohemianRhapsody = new Lagu("Bohemian Rhapsody", 355, queen, aNightAtTheOpera);
        Lagu anotherOneBitesTheDust = new Lagu("Another One Bites the Dust", 215, queen, theGame);
        Lagu loveOfaLifetime = new Lagu("Love of a Lifetime", 215, firehouse, null); // Lagu single
        Podcast techTalk = new Podcast("Tech Talk Weekly", 1800, "Jeki", 42);

        // Menambahkan lagu ke dalam album (Composition)
        aNightAtTheOpera.TambahLagu(bohemianRhapsody);

        // Membuat 'playlist' untuk mendemonstrasikan Polymorphism
        List<KontenAudio> playlist = new List<KontenAudio>
        {
            bohemianRhapsody,
            techTalk,
            anotherOneBitesTheDust,
            loveOfaLifetime
        };

        // 5. POLYMORPHISM (OVERRIDING)
        Console.WriteLine("--- Memutar Semua Konten di Playlist ---");
        foreach (var konten in playlist)
        {
            konten.Putar();
            Console.WriteLine(); // Spasi
        }

        // 5. POLYMORPHISM (OVERLOADING)
        // Memanggil versi lain dari method Putar() yang ada di class Lagu
        Console.WriteLine("\n--- Memutar Lagu dengan Opsi Lirik (Overloading) ---");
        bohemianRhapsody.Putar(true);
        
        // 3. ENCAPSULATION & DATA HIDING
        Console.WriteLine("\n\n--- Mengecek Jumlah Pemutaran (Encapsulation) ---");
        Console.WriteLine($"'{bohemianRhapsody.Judul}' telah diputar sebanyak: {bohemianRhapsody.JumlahPemutaran} kali.");
        Console.WriteLine($"'{techTalk.Judul}' telah diputar sebanyak: {techTalk.JumlahPemutaran} kali.");
        Console.WriteLine($"'{anotherOneBitesTheDust.Judul}' telah diputar sebanyak: {anotherOneBitesTheDust.JumlahPemutaran} kali.");
        Console.WriteLine($"'{loveOfaLifetime.Judul}' telah diputar sebanyak: {loveOfaLifetime.JumlahPemutaran} kali.");
    }
}